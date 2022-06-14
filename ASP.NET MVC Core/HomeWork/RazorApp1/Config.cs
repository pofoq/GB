
using Microsoft.Extensions.Configuration;

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
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
#if DEBUG
            builder.AddUserSecrets("535c5d29-6e79-4041-96ab-b3702656356c");
#endif
            builder.AddEnvironmentVariables();
        }
    }
}
