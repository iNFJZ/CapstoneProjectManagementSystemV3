using CapstoneProjectManagementSystemV3.Controllers.AdminController;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Infrastructure.Services.PrivateService.RoleService;

namespace CapstoneProjectManagementSystemV3Tests.AdminControllerTests
{
    [TestFixture]
    public class AdminListUserControllerTests
    {
        private Mock<ILogger<AdminListUserController>> _loggerMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IDataRetrievalService> _dataRetrievalServiceMock;
        private Mock<ISupervisorService> _supervisorServiceMock;
        private Mock<IRoleService> _roleService;
        private AdminListUserController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<AdminListUserController>>();
            _userServiceMock = new Mock<IUserService>();
            _dataRetrievalServiceMock = new Mock<IDataRetrievalService>();
            _supervisorServiceMock = new Mock<ISupervisorService>();
            _roleService = new Mock<IRoleService>();
            _controller = new AdminListUserController(
                _loggerMock.Object,
                _userServiceMock.Object,
                _dataRetrievalServiceMock.Object,
                _supervisorServiceMock.Object,
                _roleService.Object
            );
        }     
    }
}