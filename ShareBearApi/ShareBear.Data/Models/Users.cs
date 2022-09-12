using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.Data.Models
{
    public partial class Users
    {
        public Users()
        {
            RefreshTokens = new HashSet<RefreshTokens>();
        }

        public int Id { get; set; }
        public string UserGlobalIdentifier { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime LastLoggedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<RefreshTokens> RefreshTokens { get; set; }
    }
}
