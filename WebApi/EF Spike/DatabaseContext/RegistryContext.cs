using Microsoft.EntityFrameworkCore;

namespace EF_Spike.DatabaseContext
{
    public partial class RegistryContext : DbContext
    {
        public RegistryContext(DbContextOptions<RegistryContext> options) : base(options)
        { }

        public virtual DbSet<TblMembership> TblMembership { get; set; }
        public virtual DbSet<TblMembershipAverageAgeBasis> TblMembershipAverageAgeBasis { get; set; }
        public virtual DbSet<TblMembershipAverageAgeBasisType> TblMembershipAverageAgeBasisType { get; set; }
        public virtual DbSet<TblMembershipBenefitType> TblMembershipBenefitType { get; set; }
        public virtual DbSet<TblMembershipDetails> TblMembershipDetails { get; set; }
        public virtual DbSet<TblMembershipType> TblMembershipType { get; set; }
        // Unable to generate entity type for table 'dbo.tbl_MembershipValuationBasis'. Please see the warning messages.

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

            modelBuilder.Entity<TblMembershipAverageAgeBasis>(entity =>
            {
                entity.HasKey(e => new { e.MembershipReference, e.StartEventReference });

                entity.ToTable("tbl_MembershipAverageAgeBasis");

                entity.HasOne(d => d.MembershipAverageAgeBasisNavigation)
                    .WithMany(p => p.TblMembershipAverageAgeBasis)
                    .HasForeignKey(d => d.MembershipAverageAgeBasis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_MembershipAverageAgeBasis_tbl_MembershipAverageAgeBasisType");

                entity.HasOne(d => d.MembershipReferenceNavigation)
                    .WithMany(p => p.TblMembershipAverageAgeBasis)
                    .HasForeignKey(d => d.MembershipReference)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_MembershipAverageAgeBasis_tbl_Membership");
            });

            modelBuilder.Entity<TblMembershipAverageAgeBasisType>(entity =>
            {
                entity.HasKey(e => e.MembershipAverageAgeBasis);

                entity.ToTable("tbl_MembershipAverageAgeBasisType");

                entity.Property(e => e.MembershipAverageAgeBasis).ValueGeneratedNever();

                entity.Property(e => e.MembershipAverageAgeBasisDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMembershipBenefitType>(entity =>
            {
                entity.HasKey(e => e.MembershipBenefitTypeReference);

                entity.ToTable("tbl_MembershipBenefitType");

                entity.Property(e => e.MembershipBenefitTypeDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMembershipDetails>(entity =>
            {
                entity.HasKey(e => new { e.MembershipReference, e.MembershipBenefitTypeReference, e.MembershipTypeReference });

                entity.ToTable("tbl_MembershipDetails");

                entity.HasOne(d => d.MembershipBenefitTypeReferenceNavigation)
                    .WithMany(p => p.TblMembershipDetails)
                    .HasForeignKey(d => d.MembershipBenefitTypeReference)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_MembershipDetails_tbl_MembershipBenefitType");

                entity.HasOne(d => d.MembershipReferenceNavigation)
                    .WithMany(p => p.TblMembershipDetails)
                    .HasForeignKey(d => d.MembershipReference)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_MembershipDetails_tbl_Membership");

                entity.HasOne(d => d.MembershipTypeReferenceNavigation)
                    .WithMany(p => p.TblMembershipDetails)
                    .HasForeignKey(d => d.MembershipTypeReference)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_MembershipDetails_tbl_MembershipType");
            });

            modelBuilder.Entity<TblMembershipType>(entity =>
            {
                entity.HasKey(e => e.MembershipTypeReference);

                entity.ToTable("tbl_MembershipType");

                entity.Property(e => e.MembershipTypeDescription)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });
        }
    }
}
