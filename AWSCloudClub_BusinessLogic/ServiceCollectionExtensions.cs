using Microsoft.Extensions.DependencyInjection;
using AWSCloudClub_BusinessLogic;
namespace AWSCloudClub_BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<AdminBusinessLogic>();
            services.AddScoped<MemberBusinessLogic>();

            // Register Event business logic
            services.AddScoped<EventBusinessLogic>();

            // Register Ticket business logic
            services.AddScoped<ITicketBusinessLogic, TicketBusinessLogic>();
            return services;
        }
    }
}
