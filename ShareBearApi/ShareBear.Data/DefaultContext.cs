﻿using Microsoft.EntityFrameworkCore;
using ShareBear.Data.Models;

namespace ShareBear.Data
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public virtual DbSet<ContainerHubs> ContainerHubs { get; set; }
        public virtual DbSet<ContainerHubAccessLogs> ContainerHubAccessLogs { get; set; }
        public virtual DbSet<ContainerFiles> ContainerFiles { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}