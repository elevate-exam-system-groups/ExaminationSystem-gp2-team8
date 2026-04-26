using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using Microsoft.Net.Http.Headers;

namespace ExaminationSystem.API.Middlewares
{
    public class JsonContentTypeMiddleware
    {
        private static readonly HashSet<string> WriteMethods = new(StringComparer.OrdinalIgnoreCase)
        {
            HttpMethods.Post,
            HttpMethods.Put,
            HttpMethods.Patch,
            HttpMethods.Delete
        };

        private readonly RequestDelegate _next;

        public JsonContentTypeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (WriteMethods.Contains(context.Request.Method) && !HasApplicationJsonContentType(context.Request))
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.FailureResponse(
                    "Content-Type must be application/json.",
                    StatusCodes.Status415UnsupportedMediaType.ToString());

                await context.Response.WriteAsJsonAsync(response);
                return;
            }

            await _next(context);
        }

        private static bool HasApplicationJsonContentType(HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ContentType))
            {
                return false;
            }

            return MediaTypeHeaderValue.TryParse(request.ContentType, out var contentType)
                && string.Equals(contentType.MediaType.Value, "application/json", StringComparison.OrdinalIgnoreCase);
        }
    }
}
