using Microsoft.EntityFrameworkCore;

namespace EF_Spike.DatabaseContext
{
    public partial class RegistryContext : DbContext
    {
        public RegistryContext(DbContextOptions<RegistryContext> options) : base(options)
        {

        }

        public virtual DbSet<TblEvent> TblEvent { get; set; }
        public virtual DbSet<TblEventSource> TblEventSource { get; set; }
        public virtual DbSet<TblEventType> TblEventType { get; set; }
        public virtual DbSet<TblEventTypeGroup> TblEventTypeGroup { get; set; }
        public virtual DbSet<TblLevyTagType> TblLevyTagType { get; set; }
        public virtual DbSet<TblMembership> TblMembership { get; set; }
        public virtual DbSet<TblMembershipAverageAgeBasis> TblMembershipAverageAgeBasis { get; set; }
        public virtual DbSet<TblMembershipAverageAgeBasisType> TblMembershipAverageAgeBasisType { get; set; }
        public virtual DbSet<TblMembershipBenefitType> TblMembershipBenefitType { get; set; }
        public virtual DbSet<TblMembershipDetails> TblMembershipDetails { get; set; }
        public virtual DbSet<TblMembershipType> TblMembershipType { get; set; }
        public virtual DbSet<TblScheme> TblScheme { get; set; }
        public virtual DbSet<TblSection> TblSection { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblEvent>(entity =>
            {
                entity.HasKey(e => e.EventReference);

                entity.ToTable("tbl_Event");

                entity.HasIndex(e => new { e.EventType, e.Psrnumber, e.CreateDateTime })
                    .HasName("NC_IDX02");

                entity.HasIndex(e => new { e.EventType, e.Psrnumber, e.NotificationDate })
                    .HasName("idx_EventsByDate");

                entity.HasIndex(e => new { e.Psrnumber, e.EventSourceReference, e.EventType })
                    .HasName("NC_IDX01");

                entity.Property(e => e.CreateDateTime)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EventSourceReference).HasDefaultValueSql("(0)");

                entity.Property(e => e.NotificationDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Psrnumber).HasColumnName("PSRNumber");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.EventSourceReferenceNavigation)
                    .WithMany(p => p.TblEvent)
                    .HasForeignKey(d => d.EventSourceReference)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Event_tbl_EventSource");

                entity.HasOne(d => d.EventTypeNavigation)
                    .WithMany(p => p.TblEvent)
                    .HasForeignKey(d => d.EventType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Event_EventType");
            });

            modelBuilder.Entity<TblEventSource>(entity =>
            {
                entity.HasKey(e => e.EventSourceReference);

                entity.ToTable("tbl_EventSource");

                entity.Property(e => e.EventSourceDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEventType>(entity =>
            {
                entity.HasKey(e => e.EventType);

                entity.ToTable("tbl_EventType");

                entity.Property(e => e.EventTypeDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventTypeGroupReference).HasDefaultValueSql("(0)");

                entity.Property(e => e.ScoreUivisible)
                    .HasColumnName("ScoreUIVisible")
                    .HasDefaultValueSql("(1)");

                entity.HasOne(d => d.EventTypeGroupReferenceNavigation)
                    .WithMany(p => p.TblEventType)
                    .HasForeignKey(d => d.EventTypeGroupReference)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_EventType_tbl_EventTypeGroup");
            });

            modelBuilder.Entity<TblEventTypeGroup>(entity =>
            {
                entity.HasKey(e => e.EventTypeGroupReference);

                entity.ToTable("tbl_EventTypeGroup");

                entity.Property(e => e.EventTypeGroupDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblLevyTagType>(entity =>
            {
                entity.HasKey(e => e.LevyTagTypeReference);

                entity.ToTable("tbl_LevyTagType");

                entity.Property(e => e.LevyTagDescription)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.LevyYearEndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.LevyYearStartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Sryear).HasColumnName("SRYear");
            });

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

                entity.HasOne(d => d.LevyTagTypeReferenceNavigation)
                    .WithMany(p => p.TblMembership)
                    .HasForeignKey(d => d.LevyTagTypeReference)
                    .HasConstraintName("FK_Membership_LevyTagType");

                entity.HasOne(d => d.TblSection)
                    .WithMany(p => p.TblMembership)
                    .HasForeignKey(d => new { d.Psrnumber, d.SectionNumber })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Membership_Section");
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

            modelBuilder.Entity<TblScheme>(entity =>
            {
                entity.HasKey(e => e.Psrnumber);

                entity.ToTable("tbl_Scheme");

                entity.Property(e => e.Psrnumber)
                    .HasColumnName("PSRNumber")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<TblSection>(entity =>
            {
                entity.HasKey(e => new { e.Psrnumber, e.SectionNumber });

                entity.ToTable("tbl_Section");

                entity.Property(e => e.Psrnumber).HasColumnName("PSRNumber");

                entity.HasOne(d => d.PsrnumberNavigation)
                    .WithMany(p => p.TblSection)
                    .HasForeignKey(d => d.Psrnumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Section_Scheme");
            });
        }
    }
}
