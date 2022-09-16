using Microsoft.AspNetCore.Mvc;
using ShareBear.Services;

namespace ShareBear.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IAzureStorageService fileService;

        public FileController(IAzureStorageService fileService)
        {
            this.fileService = fileService;
        }

        [Route("uploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files)
        {
            try
            {
                await fileService.GetTotalSizeContainers();

                return Ok("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Ok();
            }
        }

        [Route("test")]
        [HttpPost]
        public async Task<IActionResult> Test()
        {
            try
            {
                await fileService.GetTotalSizeContainers();

                return Ok("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Ok();
            }
        }
    }
}
