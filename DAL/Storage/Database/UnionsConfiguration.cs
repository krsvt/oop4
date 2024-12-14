using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Storage.Database;

public class UnionsConfiguration : IEntityTypeConfiguration<Union>
{
    public void Configure(EntityTypeBuilder<Union> builder)
    {
        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(u => u.Partner1Id);

        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(u => u.Partner2Id);

        builder.HasKey(u => new { u.Partner1Id, u.Partner2Id, u.ChildId });
    }
}
