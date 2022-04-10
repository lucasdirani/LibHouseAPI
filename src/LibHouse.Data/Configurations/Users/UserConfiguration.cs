using LibHouse.Business.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LibHouse.Data.Configurations.Users
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name).HasColumnType("varchar").HasMaxLength(40).IsRequired();

            builder.Property(u => u.LastName).HasColumnType("varchar").HasMaxLength(40).IsRequired();

            builder.Property(u => u.BirthDate).HasColumnType("date").IsRequired();

            builder.Property(u => u.Gender).HasConversion(new EnumToStringConverter<Gender>()).HasColumnType("varchar").HasMaxLength(11).IsRequired();

            builder.Property(u => u.Phone).HasColumnType("char").HasMaxLength(11).IsRequired();

            builder.Property(u => u.Email).HasColumnType("varchar").HasMaxLength(60).IsRequired();

            builder.Property(u => u.UserType).HasConversion(new EnumToStringConverter<UserType>()).HasColumnType("varchar").HasMaxLength(8).IsRequired();

            builder.Property(u => u.Active).HasColumnType("bit").IsRequired();

            builder.Property(u => u.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();

            builder.Property(u => u.UpdatedAt).HasColumnType("datetime").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAddOrUpdate().IsRequired();

            builder.OwnsOne(u => u.CPF).Property(c => c.Value).HasColumnType("char").HasMaxLength(11).HasColumnName("Cpf").IsRequired();
           
            builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("idx_user_email");
        }
    }
}