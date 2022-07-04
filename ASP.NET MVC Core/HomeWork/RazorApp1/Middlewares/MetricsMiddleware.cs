
namespace RazorApp1.Middlewares
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MetricsMiddleware> _logger;
        private readonly Data _data;

        public MetricsMiddleware(RequestDelegate next, ILogger<MetricsMiddleware> logger, Data data)
        {
            _logger = logger;
            _next = next;
            _data = data;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context?.Request?.Path != null)
            {
                string path = context.Request.Path.ToString();

                switch (path)
                {
                    case var _ when path.StartsWith("/lib/"):
                    case var _ when path.StartsWith("/css/"):
                    case var _ when path.StartsWith("/js/"):
                    case var _ when path.StartsWith("/RazorApp1"):
                        break;
                    default:
                        if (_data.Metrics.ContainsKey(path))
                        {
                            int i = _data.Metrics[path];
                            _data.Metrics.TryUpdate(path, i + 1, i);
                        }
                        else
                        {
                            _data.Metrics.TryAdd(path, 1);
                        }
                        break;
                }
                await _next.Invoke(context);
            }
        }
    }
}
