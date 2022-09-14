using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.Data.Models
{
    public enum ContainerUserActions
    {
        DOWNLOADED_FILES,
        UPLOADED_FILES,
        DELETED_FILES,
        PAGE_VISIT
    }
    public partial class ContainerHubAccessLogs
    {
        public ContainerHubAccessLogs()
        {

        }

        public ContainerHubAccessLogs(string visitorId, ContainerUserActions action)
        {
            VisitorId = visitorId;
            AccessedOn = DateTime.UtcNow;
            Action = action;
        }

        public int Id { get; set; }
        public string VisitorId { get; set; } = string.Empty;
        public DateTime AccessedOn { get; set; }
        public ContainerUserActions Action { get; set; }

        public virtual ContainerHubs? ContainerHub { get; set; }
    }
}
