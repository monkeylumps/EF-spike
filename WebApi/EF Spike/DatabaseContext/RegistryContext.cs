using Microsoft.EntityFrameworkCore;

namespace EF_Spike.DatabaseContext
{
    public partial class RegistryContext : DbContext
    {
        public virtual DbSet<TblMembership> TblMembership { get; set; }

        public RegistryContext(DbContextOptions<RegistryContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblMembership>(entity =>
            {
                entity.HasKey(e => e.MembershipReference);

                entity.ToTable("tbl_Membership");

                entity.HasIndex(e => e.Psrnumber)
                    .HasName("nc_idx");

                entity.HasIndex(e => new { e.LevyTagTypeReference, e.EndEventReference })
                    .HasName("idx_TaggedMembership");

                entity.HasIndex(e => new { e.Psrnumber, e.EndDate, e.EndEventReference, e.LevyTagTypeReference, e.MembershipReference, e.EffectiveDate })
                    .HasName("nc_idx_tbl_Membership_FKs");

                entity.Property(e => e.EffectiveDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Psrnumber).HasColumnName("PSRNumber");
            });
        }
    }
}
