using Microsoft.EntityFrameworkCore;
using ShareBear.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBear.UnitTests
{
    public class TestDbContextFactory : IDbContextFactory<DefaultContext>
    {
        private DbContextOptions<DefaultContext> _options;

        public TestDbContextFactory()
        {
            _options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        public DefaultContext CreateDbContext()
        {
            return new DefaultContext(_options);
        }
    }
}
