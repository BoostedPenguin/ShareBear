﻿using Microsoft.AspNetCore.Mvc;
using ShareBear.Filters;
using ShareBear.Helpers;
using ShareBear.Services;

namespace ShareBear.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [TypeFilter(typeof(VisitorAuthorizeFilter))]
    public class FileController : ControllerBase
    {
        private readonly IFileAccessService fileAccessService;

        public FileController(IFileAccessService fileAccessService)
        {
            this.fileAccessService = fileAccessService;
        }



        [HttpGet("container")]
        public async Task<IActionResult> GetContainerFilesShort([FromQuery] string shortRequestCode)
        {
            try
            {

                var visitorId = HttpContext.GetVisitorId();

                var container = await fileAccessService.GetContainerFiles(visitorId, shortRequestCode);

                return Ok(container);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPost("container/create")]
        public async Task<IActionResult> GenerateContainer(List<IFormFile> files)
        {
            try
            {
                if(files.Count == 0)
                    return BadRequest(new {Message = "You need to upload at least 1 file to create a container."});

                var visitorId = HttpContext.GetVisitorId();

                var container = await fileAccessService.GenerateContainer(visitorId, files);

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
