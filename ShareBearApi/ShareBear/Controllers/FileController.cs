using Microsoft.AspNetCore.Mvc;
using ShareBear.Data.Requests;
using ShareBear.Filters;
using ShareBear.Helpers;
using ShareBear.Services;

namespace ShareBear.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileAccessService fileAccessService;

        public FileController(IFileAccessService fileAccessService)
        {
            this.fileAccessService = fileAccessService;
        }


        [HttpGet("storage")]
        public async Task<IActionResult> GetStorageStatistics()
        {
            try
            {
                var container = await fileAccessService.GetStorageStatistics();

                return Ok(container);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("container")]
        public async Task<IActionResult> GetContainerFilesShort([FromQuery] string? shortRequestCode, [FromQuery] string? longRequestCode)
        {
            try
            {
                var visitorId = "";

                if ((shortRequestCode is null && longRequestCode is null) || 
                    (shortRequestCode is not null && longRequestCode is not null))
                    return BadRequest(new { Message = "You need to provide either a short code or long code." });

                if (shortRequestCode is not null)
                {
                    return Ok(await fileAccessService.GetContainerFiles(visitorId, shortRequestCode, true));
                }

                if(longRequestCode is not null)
                {
                    return Ok(await fileAccessService.GetContainerFiles(visitorId, longRequestCode, true));
                }

                return BadRequest(new { Message = "Invalid request" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new {Message = ex.Message});
            }
        }

        [TypeFilter(typeof(VisitorAuthorizeFilter))]
        [HttpPost("container/create")]
        public async Task<IActionResult> GenerateContainer([FromForm]GenerateContainerRequest request)
        {
            try
            {
                if(request.FormFiles.Count == 0)
                    return BadRequest(new {Message = "You need to upload at least 1 file to create a container."});

                var visitorId = HttpContext.GetVisitorId();

                var container = await fileAccessService.GenerateContainer(visitorId, request.FormFiles);

                return Ok(container);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
