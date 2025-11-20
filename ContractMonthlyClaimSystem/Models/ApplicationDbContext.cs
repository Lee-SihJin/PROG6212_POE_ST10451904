using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<ProgrammeCoordinator> ProgrammeCoordinators { get; set; }
        public DbSet<AcademicManager> AcademicManagers { get; set; }
        public DbSet<MonthlyClaim> MonthlyClaims { get; set; }
        public DbSet<ClaimItem> ClaimItems { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Oracle table names
            ConfigureTableNames(modelBuilder);

            // Configure all entities
            ConfigureLecturer(modelBuilder);
            ConfigureProgrammeCoordinator(modelBuilder);
            ConfigureAcademicManager(modelBuilder);
            ConfigureMonthlyClaim(modelBuilder);
            ConfigureClaimItem(modelBuilder);
            ConfigureSupportingDocument(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigureRole(modelBuilder); // ADDED: Role configuration
            ConfigureUserRole(modelBuilder); // ADDED: Separate UserRole configuration

            // Configure indexes
            ConfigureIndexes(modelBuilder);
        }

        private void ConfigureTableNames(ModelBuilder modelBuilder)
        {
            // Configure table names for Oracle
            modelBuilder.Entity<Lecturer>().ToTable("LECTURERS");
            modelBuilder.Entity<ProgrammeCoordinator>().ToTable("PROGRAMME_COORDINATORS");
            modelBuilder.Entity<AcademicManager>().ToTable("ACADEMIC_MANAGERS");
            modelBuilder.Entity<MonthlyClaim>().ToTable("MONTHLY_CLAIMS");
            modelBuilder.Entity<ClaimItem>().ToTable("CLAIM_ITEMS");
            modelBuilder.Entity<SupportingDocument>().ToTable("SUPPORTING_DOCUMENTS");
            modelBuilder.Entity<User>().ToTable("USERS");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("ROLES");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("USER_ROLES");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("USER_CLAIMS");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("USER_LOGINS");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("USER_TOKENS");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("ROLE_CLAIMS");
        }

        private void ConfigureLecturer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.HasKey(l => l.LecturerId);
                entity.Property(l => l.LecturerId)
                    .HasColumnName("LECTURERID")
                    .ValueGeneratedOnAdd();

                entity.Property(l => l.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("FIRSTNAME");

                entity.Property(l => l.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("LASTNAME");

                entity.Property(l => l.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("EMAIL");

                entity.Property(l => l.PhoneNumber)
                    .HasMaxLength(15)
                    .HasColumnName("PHONENUMBER");

                entity.Property(l => l.HourlyRate)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("HOURLYRATE");

                entity.Property(l => l.ContractStartDate)
                    .HasColumnName("CONTRACTSTARTDATE");

                entity.Property(l => l.ContractEndDate)
                    .HasColumnName("CONTRACTENDDATE");

                // FIX: Use NUMBER(1) for boolean with proper conversion
                entity.Property(l => l.IsActive)
                    .HasColumnName("ISACTIVE");

                // Ignore computed property
                entity.Ignore(l => l.FullName);
            });
        }

        private void ConfigureProgrammeCoordinator(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgrammeCoordinator>(entity =>
            {
                entity.HasKey(pc => pc.CoordinatorId);
                entity.Property(pc => pc.CoordinatorId)
                    .HasColumnName("COORDINATOR_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(pc => pc.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("FIRST_NAME");

                entity.Property(pc => pc.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("LAST_NAME");

                entity.Property(pc => pc.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("EMAIL");

                entity.Property(pc => pc.Department)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("DEPARTMENT");

                // FIX: Use NUMBER(1) for boolean with proper conversion
                entity.Property(pc => pc.IsActive)
                    .HasColumnName("IS_ACTIVE");

                // Ignore computed property
                entity.Ignore(pc => pc.FullName);
            });
        }

        private void ConfigureAcademicManager(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicManager>(entity =>
            {
                entity.HasKey(am => am.ManagerId);
                entity.Property(am => am.ManagerId)
                    .HasColumnName("MANAGER_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(am => am.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("FIRST_NAME");

                entity.Property(am => am.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("LAST_NAME");

                entity.Property(am => am.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("EMAIL");

                // FIX: Use NUMBER(1) for boolean with proper conversion
                entity.Property(am => am.IsActive)
                    .HasColumnName("IS_ACTIVE");

                // Ignore computed property
                entity.Ignore(am => am.FullName);
            });
        }

        private void ConfigureMonthlyClaim(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonthlyClaim>(entity =>
            {
                entity.HasKey(mc => mc.ClaimId);
                entity.Property(mc => mc.ClaimId)
                    .HasColumnName("CLAIM_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(mc => mc.LecturerId)
                    .IsRequired()
                    .HasColumnName("LECTURER_ID");

                entity.Property(mc => mc.ClaimMonth)
                    .IsRequired()
                    .HasColumnName("CLAIM_MONTH");

                entity.Property(mc => mc.SubmissionDate)
                    .IsRequired()
                    .HasColumnName("SUBMISSION_DATE");

                entity.Property(mc => mc.CoordinatorReviewDate)
                    .HasColumnName("COORDINATOR_REVIEW_DATE");

                entity.Property(mc => mc.ManagerReviewDate)
                    .HasColumnName("MANAGER_REVIEW_DATE");

                entity.Property(mc => mc.CoordinatorApprovalDate)
                    .HasColumnName("COORDINATOR_APPROVAL_DATE");

                entity.Property(mc => mc.ManagerApprovalDate)
                    .HasColumnName("MANAGER_APPROVAL_DATE");

                entity.Property(mc => mc.PaymentDate)
                    .HasColumnName("PAYMENT_DATE");

                entity.Property(mc => mc.TotalHours)
                    .HasColumnName("TOTAL_HOURS");

                entity.Property(mc => mc.TotalAmount)
                    .HasColumnName("TOTAL_AMOUNT");

                entity.Property(mc => mc.Status)
                    .IsRequired()
                    .HasConversion<int>()
                    .HasColumnName("STATUS");

                entity.Property(mc => mc.CoordinatorComments)
                    .HasMaxLength(500)
                    .HasColumnName("COORDINATOR_COMMENTS");

                entity.Property(mc => mc.ManagerComments)
                    .HasMaxLength(500)
                    .HasColumnName("MANAGER_COMMENTS");

                entity.Property(mc => mc.CoordinatorId)
                    .HasColumnName("COORDINATOR_ID");

                entity.Property(mc => mc.ManagerId)
                    .HasColumnName("MANAGER_ID");

                // Relationships
                entity.HasOne(mc => mc.Lecturer)
                    .WithMany(l => l.MonthlyClaims)
                    .HasForeignKey(mc => mc.LecturerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(mc => mc.Coordinator)
                    .WithMany(pc => pc.ApprovedClaims)
                    .HasForeignKey(mc => mc.CoordinatorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(mc => mc.Manager)
                    .WithMany(am => am.FinalApprovedClaims)
                    .HasForeignKey(mc => mc.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Ignore computed properties
                entity.Ignore(mc => mc.DisplayMonth);
                entity.Ignore(mc => mc.CanBeEdited);
                entity.Ignore(mc => mc.CanBeSubmitted);
                entity.Ignore(mc => mc.RequiresCoordinator);
                entity.Ignore(mc => mc.RequiresManager);
            });
        }

        private void ConfigureClaimItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimItem>(entity =>
            {
                entity.HasKey(ci => ci.ItemId);
                entity.Property(ci => ci.ItemId)
                    .HasColumnName("ITEM_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(ci => ci.ClaimId)
                    .IsRequired()
                    .HasColumnName("CLAIM_ID");

                entity.Property(ci => ci.WorkDate)
                    .IsRequired()
                    .HasColumnName("WORK_DATE");

                entity.Property(ci => ci.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(ci => ci.HoursWorked)
                    .HasColumnType("NUMBER(4,2)")
                    .HasColumnName("HOURS_WORKED");

                entity.Property(ci => ci.HourlyRate)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("HOURLY_RATE");

                // Relationship
                entity.HasOne(ci => ci.MonthlyClaim)
                    .WithMany(mc => mc.ClaimItems)
                    .HasForeignKey(ci => ci.ClaimId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Ignore computed property
                entity.Ignore(ci => ci.TotalAmount);
            });
        }

        private void ConfigureSupportingDocument(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportingDocument>(entity =>
            {
                entity.HasKey(sd => sd.DocumentId);
                entity.Property(sd => sd.DocumentId)
                    .HasColumnName("DOCUMENT_ID")
                    .ValueGeneratedOnAdd();

                entity.Property(sd => sd.ClaimId)
                    .IsRequired()
                    .HasColumnName("CLAIM_ID");

                entity.Property(sd => sd.FileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("FILE_NAME");

                entity.Property(sd => sd.OriginalFileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("ORIGINAL_FILE_NAME");

                entity.Property(sd => sd.DocumentType)
                    .IsRequired()
                    .HasConversion<int>()
                    .HasColumnName("DOCUMENT_TYPE");

                // Configure BLOB for Oracle
                entity.Property(sd => sd.FileData)
                    .HasColumnName("FILE_DATA")
                    .HasColumnType("BLOB");

                entity.Property(sd => sd.FileSize)
                    .IsRequired()
                    .HasColumnName("FILE_SIZE");

                entity.Property(sd => sd.ContentType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("CONTENT_TYPE");

                entity.Property(sd => sd.UploadDate)
                    .IsRequired()
                    .HasColumnName("UPLOAD_DATE");

                entity.Property(sd => sd.Description)
                    .HasMaxLength(500)
                    .HasColumnName("DESCRIPTION");

                // Relationship
                entity.HasOne(sd => sd.MonthlyClaim)
                    .WithMany(mc => mc.SupportingDocuments)
                    .HasForeignKey(sd => sd.ClaimId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Ignore computed property
                entity.Ignore(sd => sd.FileSizeDisplay);
            });
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // === IDENTITY BOOLEAN CONVERSIONS WITH DEFAULTS ===
                entity.Property(u => u.EmailConfirmed)
                    .HasConversion<int>()
                    .HasDefaultValue(0)  // Critical: sets default to false
                    .IsRequired()        // Make sure it's required
                    .HasColumnName("EmailConfirmed");

                entity.Property(u => u.PhoneNumberConfirmed)
                    .HasConversion<int>()
                    .HasDefaultValue(0)
                    .IsRequired()
                    .HasColumnName("PhoneNumberConfirmed");

                entity.Property(u => u.TwoFactorEnabled)
                    .HasConversion<int>()
                    .HasDefaultValue(0)
                    .IsRequired()
                    .HasColumnName("TwoFactorEnabled");

                entity.Property(u => u.LockoutEnabled)
                    .HasConversion<int>()
                    .HasDefaultValue(1)  // Usually true by default
                    .IsRequired()
                    .HasColumnName("LockoutEnabled");

                entity.Property(u => u.AccessFailedCount)
                    .HasDefaultValue(0)
                    .IsRequired()
                    .HasColumnName("AccessFailedCount");

                entity.Property(u => u.PasswordHash)
                    .HasColumnType("NVARCHAR2(4000)")
                    .HasColumnName("PasswordHash");

                entity.Property(u => u.SecurityStamp)
                    .HasColumnType("NVARCHAR2(4000)")
                    .HasColumnName("SecurityStamp");

                entity.Property(u => u.ConcurrencyStamp)
                    .HasColumnType("NVARCHAR2(4000)")
                    .HasColumnName("ConcurrencyStamp");

                entity.Property(u => u.PhoneNumber)
                    .HasColumnType("NVARCHAR2(20)")
                    .HasColumnName("PhoneNumber");

                // === YOUR EXISTING CONFIGURATIONS ===
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("FirstName");

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("LastName");

                entity.Property(u => u.User_Type)
                    .IsRequired()
                    .HasConversion<int>()
                    .HasColumnName("User_Type");

                entity.Property(u => u.LecturerId)
                    .HasColumnName("LecturerId");

                entity.Property(u => u.CoordinatorId)
                    .HasColumnName("CoordinatorId");

                entity.Property(u => u.ManagerId)
                    .HasColumnName("ManagerId");

                entity.Property(u => u.CreatedDate)
                    .IsRequired()
                    .HasColumnName("CreatedDate");;

                // Relationships
                entity.HasOne(u => u.Lecturer)
                    .WithOne(l => l.User)
                    .HasForeignKey<User>(u => u.LecturerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Coordinator)
                    .WithOne(pc => pc.User)
                    .HasForeignKey<User>(u => u.CoordinatorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Manager)
                    .WithOne(am => am.User)
                    .HasForeignKey<User>(u => u.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Ignore computed property
                entity.Ignore(u => u.FullName);
            });
        }

        // ADDED: Configure Role entity to fix NCLOB issue
        private void ConfigureRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole<int>>(entity =>
            {
                // Fix NCLOB datatype for ConcurrencyStamp
                entity.Property(r => r.ConcurrencyStamp)
                    .HasColumnType("NVARCHAR2(4000)")
                    .HasColumnName("ConcurrencyStamp");

                // Configure other role properties
                entity.Property(r => r.Name)
                    .HasMaxLength(256)
                    .HasColumnName("Name");

                entity.Property(r => r.NormalizedName)
                    .HasMaxLength(256)
                    .HasColumnName("NormalizedName");
            });
        }

        // ADDED: Separate configuration for UserRole
        private void ConfigureUserRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("USER_ROLES");

                entity.Property(ur => ur.UserId)
                    .HasColumnName("UserId")
                    .HasColumnType("NUMBER(10)");

                entity.Property(ur => ur.RoleId)
                    .HasColumnName("RoleId")
                    .HasColumnType("NUMBER(10)");

                // Define composite primary key
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            });
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Lecturer indexes
            modelBuilder.Entity<Lecturer>()
                .HasIndex(l => l.Email)
                .IsUnique()
                .HasDatabaseName("IX_LECTURERS_EMAIL");

            // ProgrammeCoordinator indexes
            modelBuilder.Entity<ProgrammeCoordinator>()
                .HasIndex(pc => pc.Email)
                .IsUnique()
                .HasDatabaseName("IX_COORDINATORS_EMAIL");

            // AcademicManager indexes
            modelBuilder.Entity<AcademicManager>()
                .HasIndex(am => am.Email)
                .IsUnique()
                .HasDatabaseName("IX_MANAGERS_EMAIL");

            // MonthlyClaim indexes
            modelBuilder.Entity<MonthlyClaim>()
                .HasIndex(mc => mc.LecturerId)
                .HasDatabaseName("IX_MONTHLY_CLAIMS_LECTURER");

            modelBuilder.Entity<MonthlyClaim>()
                .HasIndex(mc => mc.CoordinatorId)
                .HasDatabaseName("IX_MONTHLY_CLAIMS_COORDINATOR");

            modelBuilder.Entity<MonthlyClaim>()
                .HasIndex(mc => mc.ManagerId)
                .HasDatabaseName("IX_MONTHLY_CLAIMS_MANAGER");

            modelBuilder.Entity<MonthlyClaim>()
                .HasIndex(mc => mc.Status)
                .HasDatabaseName("IX_MONTHLY_CLAIMS_STATUS");

            modelBuilder.Entity<MonthlyClaim>()
                .HasIndex(mc => mc.ClaimMonth)
                .HasDatabaseName("IX_MONTHLY_CLAIMS_MONTH");

            // ClaimItem indexes
            modelBuilder.Entity<ClaimItem>()
                .HasIndex(ci => ci.ClaimId)
                .HasDatabaseName("IX_CLAIM_ITEMS_CLAIM");

            modelBuilder.Entity<ClaimItem>()
                .HasIndex(ci => ci.WorkDate)
                .HasDatabaseName("IX_CLAIM_ITEMS_WORKDATE");

            // SupportingDocument indexes
            modelBuilder.Entity<SupportingDocument>()
                .HasIndex(sd => sd.ClaimId)
                .HasDatabaseName("IX_SUPPORTING_DOCS_CLAIM");

            modelBuilder.Entity<SupportingDocument>()
                .HasIndex(sd => sd.DocumentType)
                .HasDatabaseName("IX_SUPPORTING_DOCS_TYPE");

            // User indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.User_Type)
                .HasDatabaseName("IX_USERS_TYPE");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.LecturerId)
                .HasDatabaseName("IX_USERS_LECTURER");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.CoordinatorId)
                .HasDatabaseName("IX_USERS_COORDINATOR");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.ManagerId)
                .HasDatabaseName("IX_USERS_MANAGER");
        }
    }
}