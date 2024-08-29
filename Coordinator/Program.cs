
using Events.Options;
using Events.Services.Interfaces;
using Events.Services;
using Coordinator.Services.Hosted;

namespace Coordinator
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
            builder.Services.AddHostedService<CoordinatorHostedService>();
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
