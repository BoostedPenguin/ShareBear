using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.Data.Models
{
    public partial class ContainerFiles
    {
        public ContainerFiles()
        {

        }

        public ContainerFiles(IFormFile formFile)
        {
            FileName = formFile.FileName;
            FileType = Path.GetExtension(formFile.FileName);
            FileSize = formFile.Length;
        }

        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        // In bytes
        public long FileSize { get; set; }

        public virtual ContainerHubs ContainerHub { get; set; }
    }
}
