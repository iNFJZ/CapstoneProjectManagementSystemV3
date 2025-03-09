using CapstoneProjectManagementSystemV3.Controllers.AdminController;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CapstoneProjectManagementSystemV3Tests.AdminControllerTests
{
    [TestFixture]
    public class AdminCreateUserControllerTests
    {
        private Mock<ILogger<AdminCreateUserController>> _loggerMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IDataRetrievalService> _dataRetrievalServiceMock;
        private Mock<ISupervisorService> _supervisorServiceMock;
        private AdminCreateUserController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<AdminCreateUserController>>();
            _userServiceMock = new Mock<IUserService>();
            _dataRetrievalServiceMock = new Mock<IDataRetrievalService>();
            _supervisorServiceMock = new Mock<ISupervisorService>();

            _controller = new AdminCreateUserController(
                _loggerMock.Object,
                _userServiceMock.Object,
                _dataRetrievalServiceMock.Object,
                _supervisorServiceMock.Object
            );
        }
        [Test]
        public async Task CreateStaffSuccess()
        {
            var user = new User { FptEmail = "hainahe176549@fpt.edu.vn" };
            _userServiceMock.Setup(x => x.checkDuplicateUser("hainahe176549@fpt.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });
            _userServiceMock.Setup(x => x.CreateStaffForAdmin(user))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = true});
            
            var result = await _controller.CreateStaff(user);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            var apiResult = okResult.Value as ApiSuccessResult<dynamic>;
            Assert.IsNotNull(apiResult);
            Assert.IsTrue(apiResult.IsSuccessed);
            Console.WriteLine("aaa"+apiResult.Message);
        }
        [Test]
        public async Task CreateStaffFail_EmailExisted()
        {
            var user = new User { FptEmail = "hainahe176549@fpt.edu.vn" };
            _userServiceMock.Setup(x => x.checkDuplicateUser("hainahe176549@fpt.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = true });
            _userServiceMock.Setup(x => x.CreateStaffForAdmin(user))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });

            var result = await _controller.CreateStaff(user);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            var apiResult = okResult.Value as ApiSuccessResult<dynamic>;

            Console.WriteLine(apiResult.IsSuccessed);

            Assert.IsNotNull(apiResult);
            Assert.IsFalse(apiResult.IsSuccessed);
        }
        [Test]
        public async Task CreateDepartmentLeaderSuccess()
        {
            var user = new User { FptEmail = "hainahe176549@fpt.edu.vn" };
            var departmentLeader = new Supervisor { SupervisorNavigation = user, FeEduEmail = "hainahe176549@fe.edu.vn" };
            _userServiceMock.Setup(x => x.checkDuplicateUser("hainahe176549@fpt.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });          
            _supervisorServiceMock.Setup(x => x.checkDuplicateFEEduEmail("hainahe176549@fe.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });
            _userServiceMock.Setup(x => x.CreateSupervisorLeaderForAdmin(departmentLeader))
                 .ReturnsAsync(new ApiResult<bool> { IsSuccessed = true });

            var result = await _controller.CreateDepartmentLeader(departmentLeader);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            var apiResult = okResult.Value as ApiSuccessResult<dynamic>;
            Assert.IsNotNull(apiResult);
            Assert.IsTrue(apiResult.IsSuccessed);
        }
        [Test]
        public async Task CreateDepartmentLeaderFail_FPTEmailExisted()
        {
            var user = new User { FptEmail = "hainahe176549@fpt.edu.vn" };
            var departmentLeader = new Supervisor { SupervisorNavigation = user, FeEduEmail = "hainahe176549@fe.edu.vn" };
            _userServiceMock.Setup(x => x.checkDuplicateUser("hainahe176549@fpt.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = true});
            _supervisorServiceMock.Setup(x => x.checkDuplicateFEEduEmail("hainahe176549@fe.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });
            _userServiceMock.Setup(x => x.CreateSupervisorLeaderForAdmin(departmentLeader))
                 .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });

            var result = await _controller.CreateDepartmentLeader(departmentLeader);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            var apiResult = okResult.Value as ApiSuccessResult<dynamic>;
            Assert.IsNotNull(apiResult);
            Assert.IsFalse(apiResult.IsSuccessed);
        }

        [Test]
        public async Task CreateDepartmentLeaderFail_FEEdulExisted()
        {
            var user = new User { FptEmail = "hainahe176549@fpt.edu.vn" };
            var departmentLeader = new Supervisor { SupervisorNavigation = user, FeEduEmail = "hainahe176549@fe.edu.vn" };
            _userServiceMock.Setup(x => x.checkDuplicateUser("hainahe176549@fpt.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });
            _supervisorServiceMock.Setup(x => x.checkDuplicateFEEduEmail("hainahe176549@fe.edu.vn"))
                .ReturnsAsync(new ApiResult<bool> { IsSuccessed = true });
            _userServiceMock.Setup(x => x.CreateSupervisorLeaderForAdmin(departmentLeader))
                 .ReturnsAsync(new ApiResult<bool> { IsSuccessed = false });

            var result = await _controller.CreateDepartmentLeader(departmentLeader);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            var apiResult = okResult.Value as ApiSuccessResult<dynamic>;
            Assert.IsNotNull(apiResult);
            Assert.IsFalse(apiResult.IsSuccessed);
        }
    }
}