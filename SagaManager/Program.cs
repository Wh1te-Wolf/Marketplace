
using Events.Options;
using Events.Services;
using Events.Services.Interfaces;
using SagaManager.Handlers.CustomerCreateSaga;
using SagaManager.Handlers.CustomerRemoveSaga;
using SagaManager.Services;
using SagaManager.Services.Hosted;
using SagaManager.Services.Interfaces;

namespace SagaManager
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

            builder.Services.AddSingleton<IEventManager, EventManager>();
            builder.Services.AddSingleton<IHandlerRepository, HandlerRepository>();
            builder.Services.AddHostedService<SagaManagerHostedService>();
            builder.Services.AddSingleton<ISagaProcessManager, SagaProcessManager>();
            builder.Services.AddScoped<CustomerRemoveRollbackHandler>();
            builder.Services.AddScoped<ProductCartRemoveCommitedHandler>();
            builder.Services.AddScoped<CustomerCreatedRollbackHandler>();
            builder.Services.AddScoped<ProductCartCreatedCommitedHandler>();

            IConfigurationSection? eventManagerConfigSection = builder.Configuration.GetSection(nameof(EventManagerConnectionOptions));
            builder.Services.Configure<EventManagerConnectionOptions>(eventManagerConfigSection);

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

            app.Run();
        }
    }
}
