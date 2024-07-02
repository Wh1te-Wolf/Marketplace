
using Events;
using Events.Options;
using Events.Services;
using Events.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.Serialization.Json;
using UserService.AutoMapper;
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

            //// Регистрация событий жизненного цикла приложения
            //var lifetime = app.Lifetime;

            //lifetime.ApplicationStarted.Register(() =>
            //{
            //    // Здесь можно добавить логику при запуске приложения
            //    Console.WriteLine("Application started");
            //});

            //lifetime.ApplicationStopping.Register(() =>
            //{
            //    // Здесь можно добавить логику при остановке приложения
            //    Console.WriteLine("Application stopping");
            //    rebusBus.Dispose();
            //});

            app.Run();
        }
    }
}
