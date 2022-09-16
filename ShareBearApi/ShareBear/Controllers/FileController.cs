using Microsoft.AspNetCore.Mvc;
using ShareBear.Services;

namespace ShareBear.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IAzureStorageService fileService;
        private readonly IFileAccessService fileAccessService;

        public FileController(IAzureStorageService fileService, IFileAccessService fileAccessService)
        {
            this.fileService = fileService;
            this.fileAccessService = fileAccessService;
        }

        [Route("uploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files)
        {
            try
            {
                await fileAccessService.GenerateContainer("demovisitorid", files);

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
