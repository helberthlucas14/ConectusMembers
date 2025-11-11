using Conectus.Members.Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Conectus.Members.Infra.Data.EF.Configurations
{
    internal class MemberConfiguration
        : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(member => member.Id);

            builder.Property(m => m.FirstName)
                   .HasMaxLength(50)
                   .HasColumnName("FirstName")
                   .IsRequired();

            builder.Property(m => m.LastName)
                   .HasMaxLength(50)
                   .HasColumnName("LastName")
                   .IsRequired();

            builder.Property(m => m.Gender)
                   .HasColumnName("Gender")
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(m => m.DateOfBirth)
                   .HasColumnName("DateOfBirth")
                   .IsRequired();


            builder.OwnsOne(member => member.PhoneNumber,
            phoneNumber => phoneNumber.Property(p => p.Value)
                   .HasColumnName("PhoneNumber"));

            builder.OwnsOne(member => member.Document, document =>
            {
                document.Property(doc => doc.Type)
                        .IsRequired()
                        .HasColumnName("TypeDocument")
                        .HasConversion<string>();

                document.Property(doc => doc.Document)
                    .IsRequired()
                    .HasColumnName("IdentifierDocument")
                    .HasMaxLength(11);
                
                document.HasIndex(doc => doc.Document).IsUnique();
            });

            builder.OwnsOne(member => member.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").IsRequired();
                address.Property(a => a.Number).HasColumnName("Number").IsRequired();
                address.Property(a => a.City).HasColumnName("City").IsRequired();
                address.Property(a => a.District).HasColumnName("District").IsRequired();
                address.Property(a => a.Complement).HasColumnName("Complement").IsRequired();
                address.Property(a => a.State).HasColumnName("State").IsRequired();
                address.Property(a => a.ZipCode).HasColumnName("ZipCode").IsRequired();
                address.Property(a => a.Latitude).HasColumnName("Latitude");
                address.Property(a => a.Longitude).HasColumnName("Longitude");
            });

            builder.HasOne(c => c.Responsible)
             .WithMany()
             .HasForeignKey(c => c.ResponsibleId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Property<DateTime>("LastUpdated")
                   .ValueGeneratedOnAdd();

            builder.Ignore(member => member.Events);
        }
    }
}
