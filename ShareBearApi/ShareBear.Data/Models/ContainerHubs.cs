using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.Data.Models
{
    public partial class ContainerHubs
    {
        public ContainerHubs()
        {
            ContainerFiles = new HashSet<ContainerFiles>();
            ContainerHubAccessLogs = new HashSet<ContainerHubAccessLogs>();
        }

        public ContainerHubs(string createdByVisitorId, bool isProduction)
        {
            ContainerFiles = new HashSet<ContainerFiles>();
            ContainerHubAccessLogs = new HashSet<ContainerHubAccessLogs>();

            CreatedByVisitorId = createdByVisitorId;

            ShortCodeString = new string(Enumerable.Repeat("0123456789", 6)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());

            FullCodeString = Guid.NewGuid().ToString("N");

            // Visitor ID + Random string + Environment
            ContainerName = $"{createdByVisitorId}-{Guid.NewGuid()}-{(isProduction ? "production" : "development")}";

            CreatedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddSeconds(20);

            IsActive = true;
        }
        public int Id { get; set; }
        public string ShortCodeString { get; set; } = string.Empty;
        public string FullCodeString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
        public string CreatedByVisitorId { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<ContainerFiles> ContainerFiles { get; set; }
        public virtual ICollection<ContainerHubAccessLogs> ContainerHubAccessLogs { get; set; }


        // If already deleted / access forbiden
        public bool IsActive { get; set; }
    }
}
