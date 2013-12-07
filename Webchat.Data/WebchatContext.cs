namespace Webchat.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Webchat.Models;

    public class WebchatContext : DbContext
    {
        private const string ConnectionStringName = "Webchat";

        public WebchatContext()
            : base(ConnectionStringName)
        {
        }

        public WebchatContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Image> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMappings());
            modelBuilder.Configurations.Add(new ImageMappings());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
