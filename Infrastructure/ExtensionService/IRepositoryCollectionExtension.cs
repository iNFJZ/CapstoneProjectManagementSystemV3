using Infrastructure.Repositories;
using Infrastructure.Repositories.AffiliateAccountRepository;
using Infrastructure.Repositories.ChangeFinalGroupRequestRepository;
using Infrastructure.Repositories.ChangeTopicRequestRepository;
using Infrastructure.Repositories.ConfigurationRepository;
using Infrastructure.Repositories.FinalGroupRepository;
using Infrastructure.Repositories.GroupIdeaOfSupervisorProfessionRepository;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.NewsRepository;
using Infrastructure.Repositories.NotificationRepository;
using Infrastructure.Repositories.PasswordHash;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.RegisteredRepository;
using Infrastructure.Repositories.RoleRepository;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Repositories.SpecialtyRepository;
using Infrastructure.Repositories.StaffRepository;
using Infrastructure.Repositories.Student_FavoriteGroupIdeaRepository;
using Infrastructure.Repositories.StudentFavoriteGroupIdeaRepository;
using Infrastructure.Repositories.StudentGroupIdeaRepository;
using Infrastructure.Repositories.StudentRepository;
using Infrastructure.Repositories.Supervisor_GroupIdeaReporitory;
using Infrastructure.Repositories.SupervisorProfessionRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Repositories.SupportRepository;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.Repositories.WithRepository;
using Infrastructure.Services.CommonServices.NotificationService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExtensionService
{
    public static class IRepositoryCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAffiliateAccountRepository, AffiliateAccountRepository>();
            services.AddTransient<IChangeFinalGroupRequestRepository, ChangeFinalGroupRequestRepository>();
            services.AddTransient<IChangeTopicRequestRepository, ChangeTopicRequestRepository>();
            services.AddTransient<IConfigurationRepository, ConfigurationRepository>();
            services.AddTransient<IFinalGroupRepository, FinalGroupRepository>();
            services.AddTransient<IGroupIdeaOfSupervisorProfessionRepository, GroupIdeaOfSupervisorProfessionRepository>();
            services.AddTransient<IGroupIdeaRepository, GroupIdeaRepository>();
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IPasswordHash, PasswordHash>();
            services.AddTransient<IProfessionRepository, ProfessionRepository>();
            services.AddTransient<IRegisteredRepository, RegisteredRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddTransient<ISemesterRepository, SemesterRepository>();
            services.AddTransient<ISpecialtyRepository, SpecialtyRepository>();
            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<IStudentFavoriteGroupIdeaRepository, StudentFavoriteGroupIdeaRepository>();
            services.AddTransient<IStudentGroupIdeaRepository, StudentGroupIdeaRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ISupervisorGroupIdeaReporitory, SupervisorGroupIdeaReporitory>();
            services.AddTransient<ISupervisorProfessionRepository, SupervisorProfessionRepository>();
            services.AddTransient<ISupervisorRepository, SupervisorRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISupportRepository, SupportRepository>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IWithRepository, WithRepository>(); 
            return services;
        }

    }
}
