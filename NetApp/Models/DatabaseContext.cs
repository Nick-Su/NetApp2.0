using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NetApp.Models;

namespace NetApp.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DefaultConnection")
        { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Tact> Tacts { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<TablePlayer> TablePlayers { get; set; }
        public DbSet<ConnectionType> ConnectionTypes { get; set; }
        public DbSet<NeedType> NeedTypes { get; set; }
        public DbSet<BenefitType> BenefitTypes { get; set; }
        public DbSet<ConnectionRequest> ConnectionRequests { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<NetApp.Models.Connection> Connections { get; set; }
    }
}