using ShareBear.Data.Models;
using ShareBear.Services;

namespace ShareBear.Dtos
{
    public class ContainerHubsDto
    {
        public string ShortCodeString { get; set; } = string.Empty;
        public string FullCodeString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
        public string CreatedByVisitorId { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public ContainerSASItems? ContainerFilesLinks { get; set; } 
    }
}
