using Financial.Transaction.API.Services.Interfaces;

namespace Financial.Transaction.API.Services
{
    public static class DependencyInjectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            AddTransactionServices(services);
        }

        private static void AddTransactionServices(IServiceCollection services)
        {
            services.AddScoped<ITransactionValidatorService, TransactionValidatorService>();
            services.AddSingleton<ITransactionService, TransactionService>();
        }
    }
}
