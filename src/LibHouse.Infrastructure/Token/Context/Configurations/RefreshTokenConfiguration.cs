using LibHouse.Infrastructure.Authentication.Token.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibHouse.Infrastructure.Authentication.Context.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token).HasColumnType("char(71)").IsRequired();

            builder.Property(r => r.UserId).HasColumnType("nvarchar(450)").IsRequired();

            builder.Property(r => r.JwtId).HasColumnType("varchar(max)").IsRequired();

            builder.Property(r => r.IsUsed).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();

            builder.Property(r => r.IsRevoked).HasColumnType("bit").HasDefaultValueSql("0").IsRequired();

            builder.Property(r => r.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();

            builder.Property(r => r.ExpiresIn).HasColumnType("datetime").IsRequired();

            builder.HasOne(r => r.User);

            builder.HasIndex(r => r.Token).HasDatabaseName("idx_refresh_token").IsUnique();
        }
    }
}