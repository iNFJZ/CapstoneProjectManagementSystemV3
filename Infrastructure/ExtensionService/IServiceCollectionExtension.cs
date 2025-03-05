using Infrastructure.Services;
using Infrastructure.Services.CommonServices.AffiliateAccountService;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.ExcelService;
using Infrastructure.Services.CommonServices.FinalGroupService;
using Infrastructure.Services.CommonServices.GroupIdeaDisplayFormService;
using Infrastructure.Services.CommonServices.GroupIdeaService;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.PasswordHasherService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.ChangeFinalGroupRequestService;
using Infrastructure.Services.PrivateService.ChangeTopicRequestService;
using Infrastructure.Services.PrivateService.ConfigurationService;
using Infrastructure.Services.PrivateService.NewsService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.RegisteredService;
using Infrastructure.Services.PrivateService.RoleService;
using Infrastructure.Services.PrivateService.SpecialtyService;
using Infrastructure.Services.PrivateService.StaffService;
using Infrastructure.Services.PrivateService.Student_FavoriteGroupIdeaService;
using Infrastructure.Services.PrivateService.StudentGroupIdeaService;
using Infrastructure.Services.PrivateService.StudentService;
using Infrastructure.Services.PrivateService.SupervisorGroupIdeaService;
using Infrastructure.Services.PrivateService.SupervisorProfessionService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Infrastructure.Services.PrivateService.SupportService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExtentionService
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddTransient<IStaffService, StaffService>();
            services.AddTransient<IAffiliateAccountService, AffiliateAccountService>();
            services.AddTransient<IDataRetrievalService, DataRetrievalService>();
            services.AddTransient<IExcelService, ExcelService>();
            services.AddTransient<IFinalGroupService, FinalGroupService>();
            services.AddTransient<IGroupIdeaDisplayFormService, GroupIdeaDisplayFormService>();
            services.AddTransient<IGroupIdeaService, GroupIdeaService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IPasswordHasherService, PasswordHasherService>();
            services.AddTransient<ISemesterService, SemesterService>();
            services.AddTransient<ISessionExtensionService, SessionExtensionService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IChangeFinalGroupRequestService, ChangeFinalGroupRequestService>();
            services.AddTransient<IChangeTopicRequestService, ChangeTopicRequestService>();
            services.AddTransient<IConfigurationService, ConfigurationService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IProfessionService, ProfessionService>();
            //services.AddTransient<IRegisteredService, RegisteredService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISpecialtyService, SpecialtyService>();
            
            services.AddTransient<IStudentFavoriteGroupIdeaService, StudentFavoriteGroupIdeaService>();
            services.AddTransient<IStudentGroupIdeaService, StudentGroupIdeaService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<ISupervisorGroupIdeaService, SupervisorGroupIdeaService>();
            services.AddTransient<ISupervisorProfessionService, SupervisorProfessionService>();
            services.AddTransient<ISupervisorService, SupervisorService>();
            //services.AddTransient<ISupportService, SupportService>();
            services.AddSignalR();
            services.AddSingleton<RealTimeHub>();

            return services;
        }
    }
}
