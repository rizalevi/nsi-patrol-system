using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Dashboard;
using NSI.Domain.Interfaces.Inspection;
using NSI.Domain.Interfaces.InspectionResult;
using NSI.Domain.Interfaces.Logger;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Interfaces.Mobile;
using NSI.Domain.Interfaces.Report;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Interfaces.Tasklist;
using NSI.Infra.Datas;

using NSI.Infra.Repositories;
using NSI.Infra.Repositories.Dashboard;
using NSI.Infra.Repositories.Inspection;
using NSI.Infra.Repositories.InspectionResult;
using NSI.Infra.Repositories.Logger;
using NSI.Infra.Repositories.Master;
using NSI.Infra.Repositories.Mobile;
using NSI.Infra.Repositories.Report;
using NSI.Infra.Repositories.Security;
using NSI.Infra.Repositories.Tasklist;
using NSI.Infra.Services;

namespace NSI.Infra.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // =========================
            // Database
            // =========================
            services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<DbExecutor>();
            // =========================
            // Services
            // =========================
            services.AddScoped<JwtService>();
            // =========================
            // Repositories
            // =========================
            
            services.AddScoped<IHashService, HashService>();

            //refresh token and login attempt repos
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IDashboardRepository, DashboardRepository>();

            //master data repos
            services.AddScoped<IMSClusterRepository, MSClusterRepository>();
            services.AddScoped<IMSPlantRepository, MSPlantRepository>();
            services.AddScoped<IMSCourseRepository, MSCourseRepository>();
            services.AddScoped<IMSEquipmentGroupRepository,MSEquipmentGroupRepository>();
            services.AddScoped<IMSUnitClassificationRepository, MSUnitClassificationRepository>();
            services.AddScoped<IMSInspectionPointRepository, MSInspectionPointRepository>();
            services.AddScoped<IMSShiftRepository, MSShiftRepository>();
            services.AddScoped<IMSDeviceRepository, MSDeviceRepository>();

            //transaction data repos
            services.AddScoped<IInspectionDataDDLRepository, InspectionDataDDLRepository>();
            services.AddScoped<IInspectionDataUploadRepository, InspectionDataUploadRepository>();
            services.AddScoped<IInspectionDataLeaderRepository, InspectionDataLeaderRepository>();
            services.AddScoped<IInspectionDataApprovalRepository, InspectionDataApprovalRepository>();
            services.AddScoped<IInspectionDataInquiryRepository, InspectionDataInquiryRepository>();

            services.AddScoped<IInspectionResultAnalysisRepository, InspectionResultAnalysisRepository>();
            services.AddScoped<IInspectionResultSummaryByItemRepository, InspectionResultSummaryByItemRepository>();
            services.AddScoped<IInspectionResultSummaryByGroupRepository, InspectionResultSummaryByGroupRepository>();

            //tasklist repos
            services.AddScoped<ITaskListRemainingRepository, TaskListRemainingRepository>();
            services.AddScoped<ITopPlanAbnormalityRepository, TopPlanAbnormalityRepository>();
            services.AddScoped<IMSTraceLogRepository,MSTraceLogRepository>();


            //user setup & group data repos
            services.AddScoped<IUserSetupRepository, UserSetupRepository>();
            services.AddScoped<IGroupSetupRepository, GroupSetupRepository>();
            services.AddScoped<IGroupSettingRepository, GroupSettingRepository>();
            services.AddScoped<IChangePasswordRepository, ChangePasswordRepository>();

            //notif
            services.AddScoped<INotificationRepository, NotificationRepository>();

            //permission
            services.AddScoped<IPermissionUserRepository, PermissionUserRepository>();

            //error loger
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();


            //mobile
            services.AddScoped<IMobileCheckConnectionRepository, MobileCheckConnectionRepository>();
            services.AddScoped<IMobileInspectionExportRepository, MobileInspectionExportRepository>();
            services.AddScoped<IMobileLastVersionRepository, MobileLastVersionRepository>();
            services.AddScoped<IMobileSyncRepository, MobileSyncRepository>();
            services.AddScoped<IMSMobileRepository, MSMobileRepository>();

            return services;
        }
    }
}