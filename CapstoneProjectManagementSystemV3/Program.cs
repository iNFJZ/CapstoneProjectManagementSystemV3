using Infrastructure.Entities.DBContext;
using Infrastructure.Repositories.AffiliateAccountRepository;
using Infrastructure.Repositories.ChangeFinalGroupRequestRepository;
using Infrastructure.Repositories.ChangeTopicRequestRepository;
using Infrastructure.Repositories.FinalGroupRepository;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.StaffRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Entities.DBContext;
using Infrastructure.Repositories.AffiliateAccountRepository;
using Infrastructure.Repositories.ChangeFinalGroupRequestRepository;
using Infrastructure.Repositories.ChangeTopicRequestRepository;
using Infrastructure.Repositories.FinalGroupRepository;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.PasswordHash;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Repositories.StaffRepository;
using Infrastructure.Repositories.StudentGroupIdeaRepository;
using Infrastructure.Repositories.StudentRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.Services.CommonServices.AffiliateAccountService;
using Infrastructure.Services.CommonServices.ExcelService;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.PasswordHasherService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.StudentService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure.ExtensionService;
using Infrastructure.ExtentionService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Infrastructure.Repositories.RegisteredRepository;
using Infrastructure.Repositories.SupervisorProfessionRepository;
using Infrastructure.Services.PrivateService.RoleService;
using Infrastructure.Repositories.RoleRepository;

namespace CapstoneProjectManagementSystemV3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7012/")
                                            .AllowAnyHeader()
                                            .AllowAnyOrigin()
                                            .AllowAnyMethod();
                    });
            });
            builder.Services.AddDistributedMemoryCache();  // Đăng ký dịch vụ bộ nhớ đệm phân tán trong bộ nhớ
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // Đăng ký IHttpContextAccessor trong DI container
            builder.Services.AddHttpContextAccessor();

            // Add services to the container.

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
            // Đăng ký DBContext
            builder.Services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectstr")));

            builder.Services.AddServices();
            builder.Services.AddScoped<IStaffRepository, StaffRepository>(); // Nếu sử dụng Scoped

            builder.Services.AddRepositories();


            ////Repo
            //builder.Services.AddTransient<IUserRepository, UserRepository>();
            //builder.Services.AddTransient<IStaffRepository, StaffRepository>();
            //builder.Services.AddTransient<IAffiliateAccountRepository, AffiliateAccountRepository>();
            //builder.Services.AddTransient<IChangeFinalGroupRequestRepository, ChangeFinalGroupRequestRepository>();
            //builder.Services.AddTransient<IChangeTopicRequestRepository, ChangeTopicRequestRepository>();
            //builder.Services.AddTransient<IFinalGroupRepository, FinalGroupRepository>();
            //builder.Services.AddTransient<IGroupIdeaRepository, GroupIdeaRepository>();
            //builder.Services.AddTransient<ISupervisorRepository, SupervisorRepository>();
            //builder.Services.AddTransient<IStudentRepository, StudentRepository>();
            //builder.Services.AddTransient<ISemesterRepository, SemesterRepository>();
            //builder.Services.AddTransient<IStudentGroupIdeaRepository, StudentGroupIdeaRepository>();
            //builder.Services.AddTransient<IProfessionRepository, ProfessionRepository>();
            ////builder.Services.AddTransient<IAffiliateAccountRepository, AffiliateAccountRepository>();
            ////builder.Services.AddTransient<IAffiliateAccountRepository, AffiliateAccountRepository>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IDataRetrievalService, DataRetrievalService>();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Nhập token vào đây: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
                c.AddSecurityDefinition("X-Connection-String", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-Connection-String",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Nhập connection string của bạn vào đây"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-Connection-String"
                            }
                        },
                        new List<string>()
                    }
                });
            });
            var app = builder.Build();


            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            app.UseSession();

            app.UseAuthorization();


            app.MapControllers();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.Run();
        }
    }
}
