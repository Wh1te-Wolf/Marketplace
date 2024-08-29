
using Events;
using Events.Options;
using Events.Services;
using Events.Services.Interfaces;
using MicroServiceBase.Interfaces;
using MicroServiceBase.Options;
using MicroServiceBase.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;
using RemoteRESTClients.Interfaces;
using RemoteRESTClients.RESTClients;
using UserService.AutoMapper;
using UserService.EventHandlers.CustomerCreateSaga;
using UserService.EventHandlers.CustomerRemoveSaga;
using UserService.Repositories;
using UserService.Repositories.EF;
using UserService.Repositories.Interfaces;
using UserService.Services;
using UserService.Services.Hosted;
using UserService.Services.Interfaces;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            string connectionString = builder.Configuration.GetConnectionString("Postgres");
            builder.Services.AddDbContext<UserServiceContext>(options => options.UseNpgsql(connectionString));
            builder.Services.AddSingleton<IEventManager, EventManager>();
            builder.Services.AddSingleton<IHandlerRepository, HandlerRepository>();
            builder.Services.AddHostedService<UserHostedService>();
            IConfigurationSection? eventManagerConfigSection = builder.Configuration.GetSection(nameof(EventManagerConnectionOptions));
            builder.Services.Configure<EventManagerConnectionOptions>(eventManagerConfigSection);
            builder.Services.AddScoped<CreateUserHandler>();
            builder.Services.AddScoped<RemoveUserHandler>();
            builder.Services.AddScoped<CreateUserSagaHandler>();
            builder.Services.AddScoped<RemoveUserSagaHandler>();
            builder.Services.AddSingleton<IRestClient, RestClient>();
            builder.Services.AddHttpClient(nameof(RestClient));

            IConfigurationSection? serviceLocationConfigSection = builder.Configuration.GetSection(nameof(ServiceLocationOptions));
            builder.Services.Configure<ServiceLocationOptions>(serviceLocationConfigSection);

            builder.Services.AddScoped<ISagaRemoteProcessManager, SagaRemoteProcessManagerRESTClient>();

            //builder.Services.AddRebus(configure =>
            //{
            //    var configurer = configure
            //        .Logging(l => l.ColoredConsole())
            //        //.Routing(r =>
            //        //{
            //        //    r.TypeBased().Map<CustomerCreatedEvent>("default-queue");
            //        //})
            //        .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", "default-queue"))
            //        .Serialization(x => x.UseNewtonsoftJson(JsonInteroperabilityMode.PureJson));

            //return configurer;
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            //var rebusBus = app.Services.GetRequiredService<IBus>();

            //// ����������� ������� ���������� ����� ����������
            //var lifetime = app.Lifetime;

            //lifetime.ApplicationStarted.Register(() =>
            //{
            //    // ����� ����� �������� ������ ��� ������� ����������
            //    Console.WriteLine("Application started");
            //});

            //lifetime.ApplicationStopping.Register(() =>
            //{
            //    // ����� ����� �������� ������ ��� ��������� ����������
            //    Console.WriteLine("Application stopping");
            //    rebusBus.Dispose();
            //});

            app.Run();
        }
    }
}
