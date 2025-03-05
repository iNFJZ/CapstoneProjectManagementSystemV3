using CapstoneProjectManagementSystemV3UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CapstoneProjectManagementSystemV3UI.Controllers.Staff_Controlller
{
    public class StaffHomeController : Controller
    {
        private readonly ILogger<StaffHomeController> _logger;

        public StaffHomeController(ILogger<StaffHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                // Lưu giá trị cấu hình vào Web.config hoặc appsettings.json
                // Thực hiện các bước khác nếu cần thiết
                return View("/Views/Staff_View/StaffHome/Index.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Index1 error");
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
