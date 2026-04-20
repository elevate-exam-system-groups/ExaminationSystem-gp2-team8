using ExaminationSystem.BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.BuildingBlocks.Helpers
{
    public static class ControllerHelper
    {
        public static async Task<IActionResult> ExecuteAsync<TResult>(
            Func<Task<TResult>> action,
            Func<TResult, IActionResult> onSuccess)
        {
            try
            {
                var result = await action();
                return onSuccess(result);
            }
            catch (NotFoundException ex)
            {
                return new NotFoundObjectResult(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return new ConflictObjectResult(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return new UnprocessableEntityObjectResult(new { message = ex.Message });
            }
            catch (ForbiddenException ex)
            {
                return new ObjectResult(new { message = ex.Message })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
