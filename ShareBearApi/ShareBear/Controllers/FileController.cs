using Microsoft.AspNetCore.Mvc;
using ShareBear.Services;

namespace ShareBear.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [Route("uploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return BadRequest();

                //await fileService.UploadFile(files[0]);

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
