using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.Data.Requests
{
    public class GenerateContainerRequest
    {
        public List<IFormFile> FormFiles { get; set; }
    }
}
