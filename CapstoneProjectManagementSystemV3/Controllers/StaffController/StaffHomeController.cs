using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quartz.Impl;
using Quartz;
//using CapstoneProjectManagementSystem.Controllers.DevHead_Controller;
using Microsoft.Extensions.Logging;
//using CapstoneProjectManagementSystem.Services;
using System;
using Infrastructure.Entities.Common.ApiResult;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffHomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        //private readonly ILogger<EditSupervisorController> _logger;
        public StaffHomeController(
              IConfiguration configuration)
        //ILogger<EditSupervisorController> logger)
        {
            _configuration = configuration;
            //_logger = logger;
        }
        [HttpPost]
        public IActionResult Index1(string jobSchedule)
        {
            try
            {
                //_logger.LogInformation("Index1");
                // Lưu giá trị cấu hình vào Web.config hoặc appsettings.json
                _configuration["JobSchedule"] = jobSchedule;
                // Thực hiện các bước khác nếu cần thiết
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                IScheduler scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.Start();
                return Ok(new ApiSuccessResult<dynamic>(new
                {
                    status = true,
                    Path = "/Home/Index.cshtml"
                }));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex,"Index1 error");
                return Ok(new ApiSuccessResult<dynamic>(new
                {
                    status = false,
                    mess = "Email or Password invalid",
                    Path = "/Views/Common_View/404NotFound.cshtml"
                }));
            }

        }
    }
}
