using Events.Options;
using Events.Services;
using Events.Services.Interfaces;
using MicroServiceBase.Interfaces;
using MicroServiceBase.Options;
using MicroServiceBase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCartService.AutoMapper;
using ProductCartService.EventHandlers;
using ProductCartService.EventHandlers.CustomerRemoveSaga;
using ProductCartService.EventHandlers.CustomerCreateSaga;
using ProductCartService.Repositories;
using ProductCartService.Repositories.EF;
using ProductCartService.Repositories.Interfaces;
using ProductCartService.Services;
using ProductCartService.Services.Hosted;
using ProductCartService.Services.Interfaces;
using Rebus.Config;
using RemoteRESTClients.Interfaces;
using RemoteRESTClients.RESTClients;

namespace ProductCartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IProductCartRepository, ProductCartRepository>();
            builder.Services.AddScoped<IBillRepository, BillRepository>();
            builder.Services.AddScoped<IBillService, BillService>();
            builder.Services.AddScoped<IProductCartService, Services.ProductCartService>();
            builder.Services.AddSingleton<IRestClient, RestClient>();
            builder.Services.AddHttpClient(nameof(RestClient));

            builder.Services.AddSingleton<IProductRemoteService, ProductRemoteService>();

            IConfigurationSection? serviceLocationConfigSection = builder.Configuration.GetSection(nameof(ServiceLocationOptions));
            builder.Services.Configure<ServiceLocationOptions>(serviceLocationConfigSection);

            string connectionString = builder.Configuration.GetConnectionString("Postgres");
            builder.Services.AddDbContext<ProductCartServiceContext>(options => options.UseNpgsql(connectionString));

            IConfigurationSection? eventManagerConfigSection = builder.Configuration.GetSection(nameof(EventManagerConnectionOptions));
            builder.Services.Configure<EventManagerConnectionOptions>(eventManagerConfigSection);

            builder.Services.AddSingleton<IEventManager, EventManager>();
            builder.Services.AddSingleton<IHandlerRepository, HandlerRepository>();
            builder.Services.AddHostedService<ProductCartHostedService>();

            builder.Services.AddScoped<CustomerCreatedEventHandler>();
            builder.Services.AddScoped<CustomerRemovedEventHandler>();
            builder.Services.AddScoped<RemoveProductCardHandler>();
            builder.Services.AddScoped<CreateProductCartHandler>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


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

            builder.Services.AutoRegisterHandlersFromAssemblyOf<CustomerCreatedEventHandler>();
            //builder.Services.AddRebusHandler<CustomerCreatedEventHandler>();

            //using var activator = new BuiltinHandlerActivator();
            //activator.Register(() => new CustomerCreatedEventHandler());

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
            //rebusBus.Advanced.Topics.Subscribe("default-queue").Wait();

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
