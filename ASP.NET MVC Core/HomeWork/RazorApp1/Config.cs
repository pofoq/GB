
namespace RazorApp1
{
    public static class Config
    {
        public static IConfiguration Build()
        {
            var builder = new ConfigurationBuilder();
            Setup(builder);

            return builder.Build();
        }

        private static void Setup(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
        }
    }
}
