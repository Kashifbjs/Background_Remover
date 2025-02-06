namespace Background_Remover.Services
{
    public static class ServiceRegistrations
    {
        public static void RegisterServices(this IServiceCollection service)
        {
            service.AddScoped<IBackgroundRemovalService, BackgroundRemovalService>();
        }
    }
}
