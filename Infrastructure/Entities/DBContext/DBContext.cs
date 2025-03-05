using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Entities.DBContext
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }
        public const string SessionKeyCampus = "campus";
        public const string ConnectstrEmpty = "connectstrEmpty";
        public const string ConnectstrQA = "connectstrQA";
        public const string ConnectstrHL = "connectstr";
        public const string ConnectstrCT = "connectstrCT";
        public const string ConnectstrHCM = "connectstrHCM";
        public const string ConnectstrDN = "connectstrDN";
        public const string ConnectstrQN = "connectstrQN";
        public virtual DbSet<AffiliateAccount> AffiliateAccounts { get; set; } = null!;
        public virtual DbSet<ChangeFinalGroupRequest> ChangeFinalGroupRequests { get; set; } = null!;
        public virtual DbSet<ChangeTopicRequest> ChangeTopicRequests { get; set; } = null!;
        public virtual DbSet<DefenceSchedule> DefenceSchedules { get; set; } = null!;
        public virtual DbSet<FinalGroup> FinalGroups { get; set; } = null!;
        public virtual DbSet<GroupIdea> GroupIdeas { get; set; } = null!;
        public virtual DbSet<GroupIdeaOfSupervisorProfession> GroupIdeaOfSupervisorProfessions { get; set; } = null!;
        public virtual DbSet<GroupIdeasOfSupervisor> GroupIdeasOfSupervisors { get; set; } = null!;
        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Profession> Professions { get; set; } = null!;
        public virtual DbSet<RegisterdGroupSupervisor> RegisterdGroupSupervisors { get; set; } = null!;
        public virtual DbSet<RegisteredGroup> RegisteredGroups { get; set; } = null!;
        public virtual DbSet<ReportMaterial> ReportMaterials { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<Specialty> Specialties { get; set; } = null!;
        public virtual DbSet<Staff> Staffs { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentFavoriteGroupIdea> StudentFavoriteGroupIdeas { get; set; } = null!;
        public virtual DbSet<StudentGroupIdea> StudentGroupIdeas { get; set; } = null!;
        public virtual DbSet<Supervisor> Supervisors { get; set; } = null!;
        public virtual DbSet<SupervisorProfession> SupervisorProfessions { get; set; } = null!;
        public virtual DbSet<Support> Supports { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserGuide> UserGuides { get; set; } = null!;
        public virtual DbSet<With> Withs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            string campus = GetCampus();
            Console.WriteLine("Campus: ", campus);
            var strConn = config["ConnectionStrings:" + campus];
            optionsBuilder.UseSqlServer(strConn);
        }
        public static string GetCampus()
        {
            try
            {
                //return ConnectstrHL;
                IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
                var campus = _httpContextAccessor.HttpContext.Request.Headers["X-Connection-String"];
                var databaseStr = ConnectstrEmpty;
                if (!string.IsNullOrEmpty(campus))
                {
                    switch (campus)
                    {
                        case "CT":
                            databaseStr = ConnectstrCT;
                            break;
                        case "HCM":
                            databaseStr = ConnectstrHCM;
                            break;
                        case "DN":
                            databaseStr = ConnectstrDN;
                            break;
                        case "QN":
                            databaseStr = ConnectstrQN;
                            break;
                        case "HL":
                            databaseStr = ConnectstrHL;
                            break;
                        case "QA":
                            databaseStr = ConnectstrQA;
                            break;
                        default: break;
                    }

                }

                return databaseStr;
            }
            catch (Exception)
            {

                return ConnectstrEmpty;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
        .Navigation(u => u.Role)
        .AutoInclude();
            modelBuilder.Entity<AffiliateAccount>(entity =>
            {
                entity.Property(e => e.AffiliateAccountId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("AffiliateAccount_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.IsVerifyEmail).HasDefaultValueSql("((0))");

                entity.Property(e => e.OneTimePassword)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("One_Time_Password");

                entity.Property(e => e.OtpRequestTime)
                    .HasColumnType("datetime")
                    .HasColumnName("OTP_Request_Time");

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.AffiliateAccountNavigation)
                    .WithOne(p => p.AffiliateAccount)
                    .HasForeignKey<AffiliateAccount>(d => d.AffiliateAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AffiliateAccounts_Users");
            });

            modelBuilder.Entity<ChangeFinalGroupRequest>(entity =>
            {
                entity.ToTable("Change_FinalGroup_Requests");

                entity.Property(e => e.ChangeFinalGroupRequestId).HasColumnName("Change_FinalGroup_Request_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.FromStudentId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("FromStudent_ID");

                entity.Property(e => e.StaffComment).HasMaxLength(500);

                entity.Property(e => e.StatusOfStaff).HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusOfTo).HasDefaultValueSql("((0))");

                entity.Property(e => e.ToStudentId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ToStudent_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.FromStudent)
                    .WithMany(p => p.ChangeFinalGroupRequestFromStudents)
                    .HasForeignKey(d => d.FromStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Change_FinalGroup_Requests_FromStudents");

                entity.HasOne(d => d.ToStudent)
                    .WithMany(p => p.ChangeFinalGroupRequestToStudents)
                    .HasForeignKey(d => d.ToStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Change_FinalGroup_Requests_ToStudent");
            });

            modelBuilder.Entity<ChangeTopicRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK__ChangeTo__E9C5B293559DB007");

                entity.Property(e => e.RequestId).HasColumnName("Request_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.EmailSuperVisor)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FinalGroupId).HasColumnName("FinalGroup_ID");

                entity.Property(e => e.NewAbbreviation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OldAbbreviation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReasonChangeTopic)
                    .HasMaxLength(3000)
                    .HasColumnName("Reason_Change_Topic");

                entity.Property(e => e.StaffComment)
                    .HasMaxLength(500)
                    .HasColumnName("Staff_Comment");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.FinalGroup)
                    .WithMany(p => p.ChangeTopicRequests)
                    .HasForeignKey(d => d.FinalGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChangeTopicRequest_FinalGroup");
            });

            modelBuilder.Entity<DefenceSchedule>(entity =>
            {
                entity.Property(e => e.DefenceScheduleId).HasColumnName("DefenceSchedule_ID");

                entity.Property(e => e.ConcilInfor)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("Concil_Infor");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDefenceschedule).HasColumnType("date");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.FinalGroupId).HasColumnName("FinalGroup_ID");

                entity.Property(e => e.RoomDefenceschedule)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.FinalGroup)
                    .WithMany(p => p.DefenceSchedules)
                    .HasForeignKey(d => d.FinalGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DefenceSchedules_FinalGroups");
            });

            modelBuilder.Entity<FinalGroup>(entity =>
            {
                entity.Property(e => e.FinalGroupId).HasColumnName("FinalGroup_ID");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_ID");

                entity.Property(e => e.SupervisorId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Supervisor_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.FinalGroups)
                    .HasForeignKey(d => d.ProfessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FinalGroups_Professions");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.FinalGroups)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FinalGroups_Semesters");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.FinalGroups)
                    .HasForeignKey(d => d.SpecialtyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FinalGroups_Specialties");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.FinalGroups)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("FK_FinalGroups_Supervisors");
            });

            modelBuilder.Entity<GroupIdea>(entity =>
            {
                entity.Property(e => e.GroupIdeaId).HasColumnName("GroupIdea_ID");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.ProjectTags).HasMaxLength(200);

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.GroupIdeas)
                    .HasForeignKey(d => d.ProfessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupIdeas_Professions");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.GroupIdeas)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupIdeas_Semesters");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.GroupIdeas)
                    .HasForeignKey(d => d.SpecialtyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupIdeas_Specialties");
            });

            modelBuilder.Entity<GroupIdeaOfSupervisorProfession>(entity =>
            {
                entity.HasKey(e => new { e.GroupIdeaId, e.ProfessionId });

                entity.ToTable("GroupIdeaOfSupervisor_Profession");

                entity.Property(e => e.GroupIdeaId).HasColumnName("GroupIdea_ID");

                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_ID");

                entity.HasOne(d => d.GroupIdea)
                    .WithMany(p => p.GroupIdeaOfSupervisorProfessions)
                    .HasForeignKey(d => d.GroupIdeaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupIdeaOfSupervisor_Profession_GroupIdeasOfSupervisor");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.GroupIdeaOfSupervisorProfessions)
                    .HasForeignKey(d => d.ProfessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupIdeaOfSupervisor_Profession_Professions");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.GroupIdeaOfSupervisorProfessions)
                    .HasForeignKey(d => d.SpecialtyId)
                    .HasConstraintName("FK_GroupIdeaOfSupervisor_Profession_Specialties");
            });

            modelBuilder.Entity<GroupIdeasOfSupervisor>(entity =>
            {
                entity.HasKey(e => e.GroupIdeaId);

                entity.ToTable("GroupIdeasOfSupervisor");

                entity.Property(e => e.GroupIdeaId).HasColumnName("GroupIdea_ID");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.ProjectTags).HasMaxLength(200);

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.SupervisorId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Supervisor_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.GroupIdeasOfSupervisors)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupIdeasOfSupervisor_Semesters");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.GroupIdeasOfSupervisors)
                    .HasForeignKey(d => d.SupervisorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupIdeasOfSupervisor_Supervisors");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsId).HasColumnName("News_ID");

                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.FileName).HasMaxLength(100);

                entity.Property(e => e.Pin).HasDefaultValueSql("((0))");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.StaffId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Staff_ID");

                entity.Property(e => e.Title).HasMaxLength(300);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_News_Semesters");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_News_Staffs");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasColumnName("Notification_ID");

                entity.Property(e => e.AttachedLink)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Attached_Link");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.NotificationContent)
                    .HasMaxLength(500)
                    .HasColumnName("Notification_Content");

                entity.Property(e => e.Readed).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.Property(e => e.UserId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_Users");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.PermissionId).HasColumnName("Permission_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.PermissionName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");
            });

            modelBuilder.Entity<Profession>(entity =>
            {
                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.ProfessionAbbreviation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Profession_Abbreviation");

                entity.Property(e => e.ProfessionFullName)
                    .HasMaxLength(100)
                    .HasColumnName("Profession_FullName");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Professions)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_Professions_Semesters");
            });

            modelBuilder.Entity<RegisterdGroupSupervisor>(entity =>
            {
                entity.HasKey(e => new { e.RegisteredGroupId, e.SupervisorId });

                entity.ToTable("RegisterdGroup_Supervisor");

                entity.Property(e => e.RegisteredGroupId).HasColumnName("RegisteredGroup_ID");

                entity.Property(e => e.SupervisorId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Supervisor_ID");

                entity.Property(e => e.GroupIdeaOfSupervisorId).HasColumnName("GroupIdeaOfSupervisor_ID");

                entity.Property(e => e.IsAssigned).HasColumnName("isAssigned");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.GroupIdeaOfSupervisor)
                    .WithMany(p => p.RegisterdGroupSupervisors)
                    .HasForeignKey(d => d.GroupIdeaOfSupervisorId)
                    .HasConstraintName("FK_RegisterdGroup_Supervisor_GroupIdeasOfSupervisor");

                entity.HasOne(d => d.RegisteredGroup)
                    .WithMany(p => p.RegisterdGroupSupervisors)
                    .HasForeignKey(d => d.RegisteredGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisterdGroup_Supervisor_RegisteredGroups");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.RegisterdGroupSupervisors)
                    .HasForeignKey(d => d.SupervisorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisterdGroup_Supervisor_Supervisors");
            });

            modelBuilder.Entity<RegisteredGroup>(entity =>
            {
                entity.Property(e => e.RegisteredGroupId).HasColumnName("RegisteredGroup_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.GroupIdeaId).HasColumnName("GroupIdea_ID");

                entity.Property(e => e.RegisteredSupervisorEmail1)
                    .HasMaxLength(100)
                    .HasColumnName("Registered_Supervisor_Email_1");

                entity.Property(e => e.RegisteredSupervisorEmail2)
                    .HasMaxLength(100)
                    .HasColumnName("Registered_Supervisor_Email_2");

                entity.Property(e => e.RegisteredSupervisorName1)
                    .HasMaxLength(100)
                    .HasColumnName("Registered_Supervisor_Name_1");

                entity.Property(e => e.RegisteredSupervisorName2)
                    .HasMaxLength(100)
                    .HasColumnName("Registered_Supervisor_Name_2");

                entity.Property(e => e.RegisteredSupervisorPhone1)
                    .HasMaxLength(15)
                    .HasColumnName("Registered_Supervisor_Phone_1");

                entity.Property(e => e.RegisteredSupervisorPhone2)
                    .HasMaxLength(100)
                    .HasColumnName("Registered_Supervisor_Phone_2");

                entity.Property(e => e.StaffComment)
                    .HasMaxLength(400)
                    .HasColumnName("Staff_Comment");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.StudentComment)
                    .HasMaxLength(150)
                    .HasColumnName("Student_Comment");

                entity.Property(e => e.StudentsRegistraiton)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Students_Registraiton");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.GroupIdea)
                    .WithMany(p => p.RegisteredGroups)
                    .HasForeignKey(d => d.GroupIdeaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisteredGroups_GroupIdeas");
            });

            modelBuilder.Entity<ReportMaterial>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__ReportMa__30FA9DB1986B04B5");

                entity.Property(e => e.ReportId).HasColumnName("Report_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.FinalGroupId).HasColumnName("FinalGroup_ID");

                entity.Property(e => e.ReportContent).HasMaxLength(500);

                entity.Property(e => e.ReportTile)
                    .HasMaxLength(100)
                    .HasColumnName("Report_Tile");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.SubmissionAttachment)
                    .IsUnicode(false)
                    .HasColumnName("Submission_Attachment");

                entity.Property(e => e.SubmissionComment)
                    .HasMaxLength(500)
                    .HasColumnName("Submission_Comment");

                entity.Property(e => e.SupervisorId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Supervisor_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.FinalGroup)
                    .WithMany(p => p.ReportMaterials)
                    .HasForeignKey(d => d.FinalGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReportMaterials_FinalGroups");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.ReportMaterials)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("FK_ReportMaterials_Supervisors");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Role_Permission");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.PermissionId).HasColumnName("Permission_ID");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Permission)
                    .WithMany()
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Permission_Permissions");

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_Permission_Role");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.BodyMailTemplate).HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeadlineChangeIdea).HasColumnType("date");

                entity.Property(e => e.DeadlineRegisterGroup).HasColumnType("date");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.EndTime)
                    .HasColumnType("date")
                    .HasColumnName("End_Time");

                entity.Property(e => e.IsConfirmationOfDevHeadNeeded).HasDefaultValueSql("((1))");

                entity.Property(e => e.SemesterCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Semester_Code");

                entity.Property(e => e.SemesterName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Semester_Name");

                entity.Property(e => e.ShowGroupName)
                    .HasColumnName("showGroupName")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StartTime)
                    .HasColumnType("date")
                    .HasColumnName("Start_Time");

                entity.Property(e => e.StatusCloseBit).HasDefaultValueSql("((1))");

                entity.Property(e => e.SubjectMailTemplate)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_ID");

                entity.Property(e => e.CodeOfGroupName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.SpecialtyAbbreviation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Specialty_Abbreviation");

                entity.Property(e => e.SpecialtyFullName)
                    .HasMaxLength(100)
                    .HasColumnName("Specialty_FullName");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.Specialties)
                    .HasForeignKey(d => d.ProfessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Specialties_Professions");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Specialties)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_Specialties_Semesters");

                entity.HasMany(d => d.Withs)
                    .WithMany(p => p.Specialties)
                    .UsingEntity<Dictionary<string, object>>(
                        "ConfigurationSpecandPro",
                        l => l.HasOne<With>().WithMany().HasForeignKey("WithId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ConfigurationSpecandPro_With"),
                        r => r.HasOne<Specialty>().WithMany().HasForeignKey("SpecialtyId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ConfigurationSpecandPro_Specialties"),
                        j =>
                        {
                            j.HasKey("SpecialtyId", "WithId");

                            j.ToTable("ConfigurationSpecandPro");

                            j.IndexerProperty<int>("SpecialtyId").HasColumnName("Specialty_ID");

                            j.IndexerProperty<int>("WithId").HasColumnName("With_ID");
                        });
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.StaffId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Staff_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.StaffNavigation)
                    .WithOne(p => p.Staff)
                    .HasForeignKey<Staff>(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staffs_Users");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Student_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Curriculum)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ExpectedRoleInGroup).HasMaxLength(50);

                entity.Property(e => e.FinalGroupId).HasColumnName("FinalGroup_ID");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsEligible).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsLeader).HasDefaultValueSql("((0))");

                entity.Property(e => e.LinkFacebook)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.RollNumber)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.SelfDiscription).HasMaxLength(3000);

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.Property(e => e.WantToBeGrouped).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.FinalGroup)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.FinalGroupId)
                    .HasConstraintName("FK_Students_FinalGroups");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ProfessionId)
                    .HasConstraintName("FK_Professions_Students");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_Semesters_Students");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SpecialtyId)
                    .HasConstraintName("FK_Specialties_Students");

                entity.HasOne(d => d.StudentNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Students_Users");
            });

            modelBuilder.Entity<StudentFavoriteGroupIdea>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Student_FavoriteGroupIdea");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.GroupIdeaId).HasColumnName("GroupIdea_ID");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Student_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.GroupIdea)
                    .WithMany()
                    .HasForeignKey(d => d.GroupIdeaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_FavoriteGroupIdea_GroupIdeas");

                entity.HasOne(d => d.Student)
                    .WithMany()
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_FavoriteGroupIdea_Students");
            });

            modelBuilder.Entity<StudentGroupIdea>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Student_GroupIdea");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.GroupIdeaId).HasColumnName("GroupIdea_ID");

                entity.Property(e => e.Message).HasMaxLength(500);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Student_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.GroupIdea)
                    .WithMany()
                    .HasForeignKey(d => d.GroupIdeaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_GroupIdea_GroupIdeas");

                entity.HasOne(d => d.Student)
                    .WithMany()
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_GroupIdea_Students");
            });

            modelBuilder.Entity<Supervisor>(entity =>
            {
                entity.Property(e => e.SupervisorId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Supervisor_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.FeEduEmail)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FieldSetting)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalEmail)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SelfDescription).HasMaxLength(3000);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.SupervisorNavigation)
                    .WithOne(p => p.Supervisor)
                    .HasForeignKey<Supervisor>(d => d.SupervisorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supervisors_Users");
            });

            modelBuilder.Entity<SupervisorProfession>(entity =>
            {
                entity.HasKey(e => new { e.SupervisorId, e.ProfessionId });

                entity.ToTable("Supervisor_Profession");

                entity.Property(e => e.SupervisorId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Supervisor_ID");

                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.IsDevHead).HasDefaultValueSql("((0))");

                entity.Property(e => e.MaxGroup).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.SupervisorProfessions)
                    .HasForeignKey(d => d.ProfessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supervisor_Profession_Professions");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.SupervisorProfessions)
                    .HasForeignKey(d => d.SupervisorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supervisor_Profession_Supervisors");
            });

            modelBuilder.Entity<Support>(entity =>
            {
                entity.Property(e => e.SupportId).HasColumnName("Support_ID");

                entity.Property(e => e.Attachment).IsUnicode(false);

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReplyAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Reply_At");

                entity.Property(e => e.ReplyMessage).HasMaxLength(2000);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.StudentId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Student_ID");

                entity.Property(e => e.SupportMessage).HasMaxLength(2000);

                entity.Property(e => e.Title).HasMaxLength(2000);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Supports)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supports_Students");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("User_ID");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('https://tse2.mm.bing.net/th?id=OIP.gHmt_-48RFhIluX7nT5zBwHaHa&pid=Api&P=0&h=180')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.FptEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasDefaultValueSql("((2))");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UserGuide>(entity =>
            {
                entity.Property(e => e.UserGuideId).HasColumnName("UserGuide_ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Deleted_At");

                entity.Property(e => e.StaffId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Staff_ID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_At");

                entity.Property(e => e.UserGuideLink)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("UserGuide_Link");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.UserGuides)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGuide_Staff");
            });

            modelBuilder.Entity<With>(entity =>
            {
                entity.ToTable("With");

                entity.Property(e => e.WithId).HasColumnName("With_ID");

                entity.Property(e => e.ProfessionId).HasColumnName("Profession_ID");

                entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_ID");

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.Withs)
                    .HasForeignKey(d => d.ProfessionId)
                    .HasConstraintName("FK_With_Professions");

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.WithsNavigation)
                    .HasForeignKey(d => d.SpecialtyId)
                    .HasConstraintName("FK_With_Specialties");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
