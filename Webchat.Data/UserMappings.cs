namespace Webchat.Data
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using Webchat.Models;

    internal class UserMappings: EntityTypeConfiguration<User>
    {
        public UserMappings()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Nickname).HasMaxLength(50).IsRequired();

            this.Property(x => x.Password).HasMaxLength(40).IsFixedLength().IsRequired();

            this.Property(x => x.Email).HasMaxLength(75).IsRequired();

            this.Property(x => x.ImageUrl).IsOptional();
        }
    }
}
