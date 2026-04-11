using ExaminationSystem.BuildingBlocks.ExceptionHandling;
using ExaminationSystem.BuildingBlocks.Exceptions;

namespace ExaminationSystem.API.Middlewares
{
    public class CustomMiddleWareHandler
    {
        private readonly ILogger<CustomMiddleWareHandler> _logger;
        private readonly RequestDelegate _next;

        public CustomMiddleWareHandler(RequestDelegate next,ILogger<CustomMiddleWareHandler> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
               await _next.Invoke(httpContext);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "someThing Wrong");
                //1-set Code

                var (statusCode, code) = ex switch
                {
                    NotFoundException => (404, "404"),   
                    ForbiddenException => (403, "403"),
                    UnauthorizedException => (401, "401"),
                    _ => (500, "INTERNAL_SERVER_ERROR")
                };
                httpContext.Response.StatusCode =statusCode;



                //2-response type to change
                httpContext.Response.ContentType = "application/json";


                //3-response object
                var ResponseObject = ApiResponse<object>.FailureResponse(
                    message: ex.Message,
                    code: code
                );

                //4-return response obeject as Json
                await httpContext.Response.WriteAsJsonAsync(ResponseObject);

            }
        }
    }
}
