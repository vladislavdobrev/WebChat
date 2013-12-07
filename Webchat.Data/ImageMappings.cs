namespace Webchat.Data
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using Webchat.Models;

    internal class ImageMappings : EntityTypeConfiguration<Image>
    {
        public ImageMappings()
        {
            this.HasKey(x => x.Id);
            this.Property(x => x.UserId).IsRequired();
            this.Property(x => x.Title).HasMaxLength(50).IsRequired();
            this.Property(x => x.Url).HasMaxLength(300).IsRequired();
        }
    }
}
