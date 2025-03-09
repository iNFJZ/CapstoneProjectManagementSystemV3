using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.RoleDto;
using Infrastructure.Entities.Dto.UserDto;
using Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.RegisteredRepository;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Repositories.SupervisorProfessionRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.Services.CommonServices.ExcelService;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SupervisorService
{
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IMailService _mailService;
        private readonly IProfessionRepository _professionRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IExcelService _excelService;
        private readonly IRegisteredRepository _registeredRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISupervisorProfessionRepository _supervisorProfessionRepository;
        public SupervisorService(ISupervisorRepository supervisorRepository,
            IMailService mailService,
            IProfessionRepository professionRepository,
            ISemesterRepository semesterRepository,
            IExcelService excelService,
            IRegisteredRepository registeredRepository,
            IUserRepository userRepository,
            ISupervisorProfessionRepository supervisorProfessionRepository)
        {
            _supervisorRepository = supervisorRepository;
            _mailService = mailService;
            _professionRepository = professionRepository;
            _semesterRepository = semesterRepository;
            _excelService = excelService;
            _registeredRepository = registeredRepository;
            _userRepository = userRepository;
            _supervisorProfessionRepository = supervisorProfessionRepository;
        }

        public async Task<ApiResult<bool>> checkDuplicateFEEduEmail(string feEduEmail)
        {
            var reult = await _supervisorRepository.GetById(s => s.FeEduEmail == feEduEmail && s.DeletedAt == null);
            if (reult != null)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<(List<SupervisorDto>, List<string>, List<int>)>> CreateSupervisorListBasedOnWorkSheet(IXLWorksheet worksheet, int startRow, List<ProfessionDto> professions)
        {
            int rowCount = worksheet.RowCount();
            SupervisorDto supervisor;
            var configuration = new ConfigurationBuilder()
                                   .AddJsonFile("appsettings.json")
                                   .Build();
            var emailPattern = configuration.GetValue<string>("emailOrEmptyStringPattern");
            var fptEduEmailPattern = configuration.GetValue<string>("fptEduEmailStringPattern");
            var feEduEmailPattern = configuration.GetValue<string>("feEduEmailStringPattern");
            List<SupervisorDto> supervisors = new List<SupervisorDto>();
            List<string> errorMessages = new List<string>();
            List<int> errorLineMessage = new List<int>();
            string temp = "";
            for (int row = startRow; row <= rowCount;) // Assuming the data starts from the second row
            {
                string supervisorId = worksheet.Row(row).Cell(1).Value.ToString().Trim().ToLower();
                User user = await _userRepository.GetById(u => u.UserId == supervisorId + "@fpt.edu.vn");
                if (user != null && user.Role.RoleId != 2)
                {
                    errorMessages.Add($"This Fpt Email was already exsisted in the system with the role is not supervisor");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(supervisorId) || string.IsNullOrEmpty(supervisorId)) break;
                string feEduEmail = worksheet.Row(row).Cell(2).Value.ToString().Trim().ToLower();
                string personalEmail = worksheet.Row(row).Cell(3).Value.ToString().Trim().ToLower();
                Supervisor supervisorById = await _supervisorRepository.GetById(s => s.SupervisorId == supervisorId + "@fpt.edu.vn");
                string phoneNumber = worksheet.Row(row).Cell(4)?.GetRichText().ToString().Trim();
                Semester currentSemester = await _semesterRepository.GetById(cs => cs.StatusCloseBit == true && cs.DeletedAt == null);
                bool isDevHead = false;
                bool isValidEmail = false;
                if (supervisorById != null)
                {
                    foreach (SupervisorProfession profession in supervisorById.SupervisorProfessions)
                    {
                        if (profession.IsDevHead.Value)
                        {
                            isDevHead = true;
                            break;
                        }
                    }
                }

                if (supervisorId.Length > 100)
                {
                    errorMessages.Add("@fpt.edu.vn account must be less than or equal to 100 characters");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }

                if (personalEmail != null && personalEmail.Length > 100)
                {
                    errorMessages.Add("Personal email must be less than or equal to 100 characters");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }

                if (feEduEmail != null && feEduEmail.Length > 100)
                {
                    errorMessages.Add("@fe.edu.vn account must be less than or equal to 100 characters");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }

                if (phoneNumber.Length == 0)
                {
                    errorMessages.Add("Phone number is not allowed to be empty");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }
                if (isDevHead)
                {
                    errorMessages.Add("@fpt.edu.vn account is department leader");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }

                if ((string.IsNullOrWhiteSpace(personalEmail) || string.IsNullOrEmpty(personalEmail)) && (string.IsNullOrWhiteSpace(feEduEmail) || string.IsNullOrEmpty(feEduEmail)))
                {
                    errorMessages.Add("One of two @fe.edu.vn account and personal email must be not empty");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(supervisorId) || string.IsNullOrEmpty(supervisorId) || supervisorId.Contains("@") || feEduEmail.Contains("@"))
                {
                    errorMessages.Add("@fpt.edu.vn account and @fe.edu.vn account only require filling in the information before the @ symbol");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }
                isValidEmail = (personalEmail == null || personalEmail.Length == 0 || Regex.IsMatch(personalEmail, emailPattern))
                        && (feEduEmail == null || feEduEmail.Length == 0 || Regex.IsMatch(feEduEmail + "@fe.edu.vn", feEduEmailPattern))
                        && (supervisorId != null && Regex.IsMatch(supervisorId + "@fpt.edu.vn", fptEduEmailPattern));
                if (!isValidEmail)
                {
                    errorMessages.Add("@fpt.edu.vn account or @fe.edu.vn account or personal email is not an valid email");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }


                temp = worksheet.Row(row).Cell(1).Value.ToString();
                supervisor = new SupervisorDto();
                supervisor.SupervisorID = supervisorId;
                supervisor.FeEduEmail = worksheet.Row(row).Cell(2).Value.ToString().Trim().Length != 0 ? worksheet.Row(row).Cell(2).Value.ToString().Trim().ToLower() : null;
                supervisor.PersonalEmail = (worksheet.Row(row).Cell(3).Value.ToString().Trim().Length != 0 ? worksheet.Row(row).Cell(3).Value.ToString().Trim().ToLower() : null);
                supervisor.PhoneNumber = (bool)(phoneNumber[0] != '0' && phoneNumber.Length == 9) ? "0" + phoneNumber : phoneNumber;
                string regexPattern = @"^(0\d{9})$";

                if (!Regex.IsMatch(supervisor.PhoneNumber, regexPattern))
                {
                    errorMessages.Add("Phone number is not valid (10 characters and start from 0 character)");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }

                try
                {
                    supervisor.IsActive = bool.Parse(worksheet.Row(row).Cell(6).Value.ToString());
                }
                catch (SystemException exception)
                {
                }
                Regex regex = new Regex(@"\s +");
                string fullName = regex.Replace(worksheet.Row(row).Cell(7).Value.ToString(), " ");
                if (fullName.Length == 0)
                {
                    errorMessages.Add("Fullname must not be empty");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }
                supervisor.SupervisorNavigation = new UserDto()
                {
                    UserID = supervisor.SupervisorID,
                    UserName = supervisor.SupervisorNavigation.UserName,
                    FullName = fullName,
                    CreatedAt = DateTime.Now,
                    FptEmail = supervisor.SupervisorNavigation.FptEmail,
                    Gender = (bool)worksheet.Row(row).Cell(8).Value.ToString().Equals("Female") ? 0 : (bool)worksheet.Row(row).Cell(8).Value.ToString().Equals("Male") ? 1 : 2,
                };
                supervisor.FieldSetting = JsonSerializer.Serialize(new SupervisorFieldSetting());
                supervisor.SupervisorProfessions = new List<SupervisorProfessionDto>();
                var sprofession = await _professionRepository.GetById(p => p.ProfessionFullName == worksheet.Row(row).Cell(9).Value.ToString() && p.SemesterId == currentSemester.SemesterId);
                SupervisorProfessionDto supervisorProfession = new SupervisorProfessionDto()
                {
                    Supervisor = supervisor,
                    Profession = new ProfessionDto()
                    {
                        ProfessionID = sprofession.ProfessionId,
                        ProfessionFullName = sprofession.ProfessionFullName,
                        ProfessionAbbreviation = sprofession.ProfessionAbbreviation
                    },

                };
                try
                {
                    int maxGroup = Int32.Parse(worksheet.Row(row).Cell(5).Value.ToString());
                    if (maxGroup < 0) maxGroup = 0;
                    supervisorProfession.MaxGroup = maxGroup;
                }
                catch (Exception exception)
                {

                }
                if (supervisorProfession.Profession == null)
                {
                    errorMessages.Add("Profession name is updated by staff , please get the new template and then re-import");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }
                bool isDevheadProfession = false;
                if (professions != null)
                {
                    foreach (ProfessionDto profession in professions)
                    {
                        if (profession.ProfessionFullName.Replace(" ", "").Trim().ToLower().Equals(supervisorProfession.Profession.ProfessionFullName.Replace(" ", "").Trim().ToLower()))
                        {
                            isDevheadProfession = true;
                            break;
                        }
                    }
                }
                if (!isDevheadProfession)
                {
                    errorMessages.Add("Profession does not belong to the options in the select box");
                    errorLineMessage.Add(row);
                    row++;
                    continue;
                }


                supervisor.SupervisorProfessions.Add(supervisorProfession);
                supervisor.SupervisorNavigation.Role = new RoleDto()
                {
                    Role_ID = 2
                };
                supervisors.Add(supervisor);
                string supervisorIdOfNextRow = worksheet.Row(row + 1).Cell(1).Value.ToString();
                if (supervisorIdOfNextRow == null || supervisorIdOfNextRow.Trim().Length == 0 || temp.Equals(supervisorIdOfNextRow))
                {
                    row++;
                    string professionRaw = worksheet.Row(row).Cell(9).Value.ToString();
                    bool isContainProfession = false;
                    while (temp != null && temp.Equals(worksheet.Row(row).Cell(1).Value.ToString())
                        && !string.IsNullOrWhiteSpace(professionRaw)
                            && !string.IsNullOrEmpty(professionRaw))
                    {
                        isContainProfession = supervisor.SupervisorProfessions.Any(supervisorProfession =>
                        {
                            return supervisorProfession.Profession.ProfessionFullName.Equals(professionRaw.Trim().Replace(" ", "").ToUpper());
                        });
                        if (isContainProfession)
                        {
                            row++;
                            continue;
                        };
                        supervisorProfession = new SupervisorProfessionDto()
                        {
                            Supervisor = supervisor,
                            Profession = new ProfessionDto()
                            {
                                ProfessionID = sprofession.ProfessionId,
                                ProfessionFullName = sprofession.ProfessionFullName,
                                ProfessionAbbreviation = sprofession.ProfessionAbbreviation
                            },
                            MaxGroup = string.IsNullOrEmpty(worksheet.Row(row).Cell(5).Value.ToString()) ? 3 : Int32.Parse(worksheet.Row(row).Cell(5).Value.ToString())
                        };
                        supervisor.SupervisorProfessions.Add(supervisorProfession);
                        temp = worksheet.Row(row).Cell(1).Value.ToString();
                        row++;
                    }
                }
                else
                {
                    row++;
                }
            }
            return new ApiSuccessResult<(List<SupervisorDto>, List<string>, List<int>)>((supervisors, errorMessages, errorLineMessage));
        }

        public async Task<ApiResult<XLWorkbook>> CreateWorkBookBasedOnSupervisorList(List<SupervisorDto> supervisors, int currentRow, string supervisorId)
        {
            int initRow = currentRow + 1;
            var workbook = new XLWorkbook();
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();
            var emailPattern = configuration.GetValue<string>("emailOrEmptyStringPattern");
            var worksheet = workbook.Worksheets.Add("Supervisor List");
            if (supervisors == null || supervisors.Count == 0)
            {
                worksheet.Cell(currentRow, 1).Value = "There's No Supervisor";
                return new ApiSuccessResult<XLWorkbook>(workbook);
            }
            // Add introuction image
            _excelService.setWorkSheetIntroduction(worksheet, "*Notes : The columns @fpt.edu.vn and @fe.edu.vn accounts only require filling in the information before the @ symbol, and the system will automatically add the corresponding domain.\n It is required to fill in the column @fpt.edu.vn Account and either one of the two columns @fe.edu.vn Account or personal email.\n In the case where one supervisor belongs to multiple professions, please add a row with the @fpt.edu.vn account is the same as that supervisor's @fpt.edu.vn account then modify the profession");
            Semester currentSemester = await _semesterRepository.GetById(s => s.StatusCloseBit == true && s.DeletedAt == null);
            var supervisorProfession = await _supervisorProfessionRepository.GetByCondition(sp => sp.SupervisorId == supervisorId && sp.IsDevHead == true);
            var professionIds = supervisorProfession.Select(sp => sp.ProfessionId).ToList();
            List<Profession> professions = await _professionRepository.GetByCondition(p => professionIds.Contains(p.ProfessionId) && p.SemesterId == currentSemester.SemesterId && p.DeletedAt == null);
            if (professions.Count == 0)
            {
                worksheet.Cell(currentRow, 1).Value = "You have no profession";
                return new ApiSuccessResult<XLWorkbook>(workbook);
            }
            worksheet.Cell(currentRow, 1).Value = "@fpt.edu.vn Account";
            worksheet.Cell(currentRow, 2).Value = "@fe.edu.vn Account";
            worksheet.Cell(currentRow, 3).Value = "Personal Email";
            worksheet.Cell(currentRow, 4).Value = "Phone Number";
            worksheet.Cell(currentRow, 5).Value = "Max Group";
            worksheet.Cell(currentRow, 6).Value = "Active";
            worksheet.Cell(currentRow, 7).Value = "Full Name";
            worksheet.Cell(currentRow, 8).Value = "Gender";
            worksheet.Cell(currentRow, 9).Value = "Profession";
            // Set column width
            worksheet.Column(1).Width = 19;
            worksheet.Column(2).Width = 19;
            worksheet.Column(3).Width = 19;
            worksheet.Column(4).Width = 13;
            worksheet.Column(5).Width = 11;
            worksheet.Column(7).Width = 19;
            worksheet.Column(9).Width = 19;
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 9)).Style.Fill.BackgroundColor = XLColor.FromHtml("#f7caac");
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 9)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 9)).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 9)).Style.Font.Bold = true;
            foreach (SupervisorDto supervisor in supervisors)
            {
                var firstSupervisour = supervisor.SupervisorProfessions.FirstOrDefault();
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = supervisor.SupervisorID;
                worksheet.Cell(currentRow, 2).Value = supervisor.FeEduEmail;
                worksheet.Cell(currentRow, 3).Value = supervisor.PersonalEmail;
                worksheet.Cell(currentRow, 4).CreateRichText().AddText(supervisor.PhoneNumber != null && supervisor.PhoneNumber.Length != 0 ? supervisor.PhoneNumber : "None");
                worksheet.Cell(currentRow, 5).Value = supervisor.SupervisorProfessions != null && supervisor.SupervisorProfessions.Count != 0 ? firstSupervisour.MaxGroup : 3;
                worksheet.Cell(currentRow, 6).Value = supervisor.IsActive;
                worksheet.Cell(currentRow, 7).Value = supervisor.SupervisorNavigation == null || supervisor.SupervisorNavigation.FullName == null || supervisor.SupervisorNavigation.FullName.Length == 0 ? "None" : supervisor.SupervisorNavigation.FullName;
                worksheet.Cell(currentRow, 8).Value = supervisor.SupervisorNavigation.Gender == 0 ? "Female" : supervisor.SupervisorNavigation.Gender == 1 ? "Male" : "Other";
                worksheet.Cell(currentRow, 9).Value = firstSupervisour.Profession == null ? "None" : firstSupervisour.Profession.ProfessionFullName;
            }
            // Init options data
            string[] selectGenders = new string[] { "Female", "Male", "Other" };
            string[] selectBit = new string[] { "TRUE", "FALSE" };
            string[] selectProfessions = professions.Select(profession => profession.ProfessionFullName).ToArray();
            // Set options to the column
            int lastRow = worksheet.LastRowUsed().RowNumber();
            var optionsColumn = _excelService.setOptionsToColumn(worksheet.Column(25), selectGenders);
            worksheet.Range(worksheet.Cell(initRow, 8), worksheet.Cell(lastRow, 8)).SetDataValidation().List(worksheet.Range(optionsColumn.Cell(1), optionsColumn.Cell(selectGenders.Length)));
            optionsColumn = _excelService.setOptionsToColumn(worksheet.Column(26), selectProfessions);
            worksheet.Range(worksheet.Cell(initRow, 9), worksheet.Cell(lastRow, 9)).SetDataValidation().List(worksheet.Range(optionsColumn.Cell(1), optionsColumn.Cell(selectProfessions.Length)));
            optionsColumn = _excelService.setOptionsToColumn(worksheet.Column(27), selectBit);
            worksheet.Range(worksheet.Cell(2, 6), worksheet.Cell(lastRow, 6)).SetDataValidation().List(worksheet.Range(optionsColumn.Cell(1), optionsColumn.Cell(selectBit.Length)));
            return new ApiSuccessResult<XLWorkbook>(workbook);
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<SupervisorDto>>> GetAllSupervisor()
        {
            var supervisors = (await _supervisorRepository.GetAll()).ToList();

            // Lấy danh sách User tương ứng
            var supervisorIds = supervisors.Select(s => s.SupervisorId).ToList();
            var users = await _userRepository.GetByCondition(u => supervisorIds.Contains(u.UserId));

            // Gán dữ liệu User vào Supervisor
            foreach (var supervisor in supervisors)
            {
                supervisor.SupervisorNavigation = users.FirstOrDefault(u => u.UserId == supervisor.SupervisorId);
            }

            return new ApiSuccessResult<List<SupervisorDto>>(supervisors);
        }

        public async Task<ApiResult<List<SupervisorDto>>> GetDevHeadByProfessionID(int professionID)
        {
            var supervisorProfessions = await _supervisorProfessionRepository.GetByCondition(sp => sp.ProfessionId == professionID && sp.IsDevHead == true);
            var supervisorIds = supervisorProfessions.Select(sp => sp.SupervisorId).ToList();
            var supervisors = await _supervisorRepository.GetByCondition(s => supervisorIds.Contains(s.SupervisorId));
            var users = await _userRepository.GetByCondition(u => supervisorIds.Contains(u.UserId));
            foreach (var supervisor in supervisors)
            {
                supervisor.SupervisorNavigation = users.FirstOrDefault(u => u.UserId == supervisor.SupervisorId);
            }

            return new ApiSuccessResult<List<SupervisorDto>>(supervisors);
        }

        public async Task<ApiResult<SupervisorDto>> GetDevheadDetailForStaff(string devheadId, int semesterId)
        {
            var supervisor = await _supervisorRepository.GetById(s => s.SupervisorId == devheadId);

            if (supervisor == null)
            {
                return new ApiErrorResult<SupervisorDto>("Supervisor not found.");
            }

            // Lấy thông tin User tương ứng
            var user = await _userRepository.GetById(u => u.UserId == devheadId);
            if (user != null)
            {
                supervisor.SupervisorNavigation = user;
            }

            // Lấy danh sách Supervisor_Profession liên quan đến Supervisor
            var supervisorProfessions = await _supervisorProfessionRepository
                .GetByCondition(sp => sp.SupervisorId == devheadId);

            if (supervisorProfessions.Any())
            {
                supervisor.SupervisorProfessions = supervisorProfessions.ToList();
            }

            // Lọc danh sách Professions dựa trên điều kiện Deleted_At IS NULL và Semester_ID
            var professionIds = supervisorProfessions.Select(sp => sp.ProfessionId).ToList();
            var professions = await _professionRepository
                .GetByCondition(p => professionIds.Contains(p.ProfessionId) && p.DeletedAt == null && p.SemesterId == semesterId);

            // Gán thông tin Profession vào từng Supervisor_Profession
            foreach (var sp in supervisor.SupervisorProfessions)
            {
                sp.Profession = professions.FirstOrDefault(p => p.ProfessionId == sp.ProfessionId);
            }
            var supervisorProfessionDtos = new List<SupervisorProfessionDto>();
            foreach (var sp in supervisor.SupervisorProfessions)
            {
                supervisorProfessionDtos.Add(new SupervisorProfessionDto()
                {
                    Profession = new ProfessionDto()
                    {
                        ProfessionID = sp.ProfessionId
                    },
                    IsDevHead = sp.IsDevHead.Value,
                    MaxGroup = sp.MaxGroup.Value
                });
            }
            var result = new SupervisorDto()
            {
                SupervisorID = supervisor.SupervisorId,
                User = new UserDto()
                {
                    FptEmail = supervisor.SupervisorNavigation.FptEmail,
                    FullName = supervisor.SupervisorNavigation.FullName,
                    Gender = supervisor.SupervisorNavigation.Gender,

                },
                FeEduEmail = supervisor.FeEduEmail,
                IsActive = supervisor.IsActive.Value,
                SupervisorProfessions = supervisorProfessionDtos
            };
            return new ApiSuccessResult<SupervisorDto>(result);
        }

        public async Task<ApiResult<(int, int, List<SupervisorDto>)>> GetListDevheadForStaffPaging(int pageNumber, string search, int professionId)
        {
            Expression<Func<User, bool>> filter = u => u.RoleId == 4 && u.Supervisor != null && u.Supervisor.DeletedAt == null && (string.IsNullOrEmpty(search) || u.FptEmail.Contains(search) || u.FullName.Contains(search)) &&
            (professionId == 0 || u.Supervisor.SupervisorProfessions.Any(sp => sp.ProfessionId == professionId));
            // Đếm tổng số bản ghi
            var totalRecord = (await _userRepository.GetByCondition(filter)).Count();
            if (totalRecord == 0)
            {
                return new ApiSuccessResult<(int, int, List<SupervisorDto>)>((0, 0, new List<SupervisorDto>()));
            }
            // Xử lý phân trang
            int[] pagingQueryResult = PagingQuery(totalRecord, pageNumber);
            int recordSkippedBefore = pagingQueryResult[2];
            int recordSkippedLater = pagingQueryResult[3];

            // Lấy danh sách Supervisor theo trang sử dụng GetAll()
            var users = await _userRepository.GetAll(recordSkippedLater - recordSkippedBefore, recordSkippedBefore, filter);

            var supervisors = users.Select(u => new SupervisorDto
            {
                SupervisorNavigation = new UserDto
                {
                    UserID = u.UserId,
                    FptEmail = u.FptEmail,
                    FullName = u.FullName ?? ""
                },
                IsActive = u.Supervisor.IsActive.Value
            }).ToList();

            return new ApiSuccessResult<(int, int, List<SupervisorDto>)>((pagingQueryResult[0], pagingQueryResult[1], supervisors));
        }
        public int[] PagingQuery(int totalRecord, int pageNumber, int pageSize = 10)
        {
            if (totalRecord == 0)
            {
                return new int[] { 0, 0, 0, 0 };
            }

            int totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages)); // Giới hạn pageNumber trong khoảng hợp lệ

            int recordSkippedBefore = (pageNumber - 1) * pageSize;
            int recordSkippedLater = Math.Min(recordSkippedBefore + pageSize, totalRecord);

            return new int[] { totalPages, totalRecord, recordSkippedBefore, recordSkippedLater };
        }

        public async Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorForPaging(int page, string search, string userId)
        {
            var supervisorProfession = await _supervisorProfessionRepository.GetById(sp => sp.SupervisorId == userId);
            var supervisorResult = await _supervisorProfessionRepository.GetByCondition(sp => sp.ProfessionId == supervisorProfession.ProfessionId);
            var supervisorIds = supervisorResult.Select(sp => sp.SupervisorId).Distinct().ToList();

            if (!supervisorIds.Any())
            {
                return new ApiSuccessResult<(int, int, List<SupervisorWithRowNum>)>((0, 0, new List<SupervisorWithRowNum>()));
            }

            // Đếm số lượng Supervisor hợp lệ
            var users = await _userRepository
                 .GetByCondition(u => supervisorIds.Contains(u.UserId) &&
                                      u.Supervisor != null &&
                                      u.Supervisor.DeletedAt == null &&
                                      u.Supervisor.IsActive == true &&
                                      (string.IsNullOrEmpty(search) ||
                                       u.Supervisor.FeEduEmail.Contains(search) ||
                                       u.FullName.Contains(search)));
            int totalRecord = users.Count;
            if (totalRecord == 0)
            {
                return new ApiSuccessResult<(int, int, List<SupervisorWithRowNum>)>((0, 0, new List<SupervisorWithRowNum>()));
            }

            // Phân trang
            int[] pagingQueryResult = PagingQuery(totalRecord, page);
            int recordSkippedBefore = pagingQueryResult[2];
            int recordSkippedLater = pagingQueryResult[3];

            // Lấy danh sách Supervisor theo trang
            var result = await _userRepository
                .GetByCondition(u => supervisorIds.Contains(u.UserId) && u.Supervisor != null && u.Supervisor.DeletedAt == null && u.Supervisor.IsActive == true &&
                                     (string.IsNullOrEmpty(search) || u.Supervisor.FeEduEmail.Contains(search) || u.FullName.Contains(search)));

            var supervisors = result.OrderBy(u => u.UserId).Skip(recordSkippedBefore).Take(recordSkippedLater - recordSkippedBefore)
                .Select(u => new SupervisorWithRowNum
                {
                    RowNum = 0, // Giá trị rownum có thể được tính khi cần
                    SupervisorID = u.UserId,
                    PhoneNumber = u.Supervisor.PhoneNumber ?? "",
                    SelfDescription = u.Supervisor.SelfDescription ?? "",
                    FeEduEmail = u.Supervisor.FeEduEmail ?? "",
                    PersonalEmail = u.Supervisor.PersonalEmail ?? "",
                    User = new User
                    {
                        FullName = u.FullName ?? ""
                    },
                    SupervisorProfession = new SupervisorProfession
                    {
                        Profession = new Profession
                        {
                            ProfessionId = 0 // Nếu cần lấy giá trị thực tế, có thể truy vấn lại từ Supervisor_Profession
                        }
                    }
                }).ToList();
            return new ApiSuccessResult<(int, int, List<SupervisorWithRowNum>)>((pagingQueryResult[0], pagingQueryResult[1], supervisors));
        }

        public async Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorForPagingForStudent(int pageNumber, int professionId, string search)
        {
            var supervisorLists = await _supervisorRepository.GetByCondition(s => s.IsActive == true && s.DeletedAt == null && s.SupervisorProfessions.Any(sp => sp.ProfessionId == professionId) &&
            (string.IsNullOrEmpty(search) || s.FeEduEmail.Contains(search) || s.SupervisorNavigation.FullName.Contains(search)));
            int totalRecord = supervisorLists.Count();
            if (totalRecord == 0)
            {
                return new ApiSuccessResult<(int, int, List<SupervisorWithRowNum>)>((0, 0, new List<SupervisorWithRowNum>()));
            }

            // Lấy danh sách Supervisor theo trang
            var supervisors = await _supervisorRepository.GetAll(10, pageNumber, s => s.IsActive == true && s.DeletedAt == null && s.SupervisorProfessions.Any(sp => sp.ProfessionId == professionId) &&
                (string.IsNullOrEmpty(search) || s.FeEduEmail.Contains(search) || s.SupervisorNavigation.FullName.Contains(search))
            );

            var supervisorList = supervisors.Select(s => new SupervisorWithRowNum
            {
                SupervisorID = s.SupervisorId,
                PhoneNumber = s.PhoneNumber ?? "",
                SelfDescription = s.SelfDescription ?? "",
                FeEduEmail = s.FeEduEmail ?? "",
                PersonalEmail = s.PersonalEmail ?? "",
                User = new User
                {
                    FullName = s.SupervisorNavigation.FullName ?? "",
                },
                SupervisorProfession = new SupervisorProfession
                {
                    Profession = new Profession
                    {
                        ProfessionId = s.SupervisorProfessions.FirstOrDefault()?.ProfessionId ?? 0,
                    }
                }
            }).ToList();

            int totalPages = (int)Math.Ceiling((double)totalRecord / 10);

            return new ApiSuccessResult<(int, int, List<SupervisorWithRowNum>)>((pageNumber, totalPages, supervisorList));
        }

        public async Task<ApiResult<List<SupervisorDto>>> getListSupervisorForRegistration(int professionID, int semesterId)
        {
            var supervisors = await _supervisorRepository.GetByCondition(s => s.IsActive == true && s.DeletedAt == null &&
            s.SupervisorProfessions.Any(sp => sp.ProfessionId == professionID && sp.Profession.SemesterId == semesterId));

            if (!supervisors.Any())
            {
                return new ApiSuccessResult<List<SupervisorDto>>(new List<SupervisorDto>());
            }

            return new ApiSuccessResult<List<SupervisorDto>>(supervisors);
        }

        public async Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorPagingForDevHead(int pageNumber, string search, string userId, int status)
        {
            var professionIds = (await _supervisorProfessionRepository.GetByCondition(sp => sp.SupervisorId == userId)).Select(sp => sp.ProfessionId).ToList();

            if (!professionIds.Any())
            {
                return new ApiSuccessResult<(int, int, List<SupervisorWithRowNum>)>((0, 0, new List<SupervisorWithRowNum>()));
            }

            // Lọc danh sách supervisor theo professionIds đã lấy được
            var query = await _supervisorRepository.GetByCondition(s =>
                s.DeletedAt == null &&
                s.SupervisorId != userId &&
                s.SupervisorProfessions.Any(sp => professionIds.Contains(sp.ProfessionId)) &&
                (string.IsNullOrEmpty(search) || s.FeEduEmail.Contains(search) || s.SupervisorNavigation.FullName.Contains(search)) &&
                (status == -1 || s.IsActive == (status == 1)));

            int totalRecord = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecord / 10);

            var supervisors = await _supervisorRepository.GetAll(10, pageNumber, s =>
                s.DeletedAt == null &&
                s.SupervisorId != userId &&
                s.SupervisorProfessions.Any(sp => professionIds.Contains(sp.ProfessionId)) &&
                (string.IsNullOrEmpty(search) || s.FeEduEmail.Contains(search) || s.SupervisorNavigation.FullName.Contains(search)) &&
                (status == -1 || s.IsActive == (status == 1))
            );

            var supervisorList = supervisors.Select(s => new SupervisorWithRowNum
            {
                SupervisorID = s.SupervisorId,
                PhoneNumber = s.PhoneNumber ?? "",
                SelfDescription = s.SelfDescription ?? "",
                FeEduEmail = s.FeEduEmail ?? "",
                PersonalEmail = s.PersonalEmail ?? "",
                IsActive = s.IsActive,
                User = new User
                {
                    FullName = s.SupervisorNavigation.FullName ?? "",
                }
            }).ToList();

            return new ApiSuccessResult<(int, int, List<SupervisorWithRowNum>)>((pageNumber, totalPages, supervisorList));
        }

        public async Task<ApiResult<List<SupervisorProfessionDto>>> GetListSupervisorProfessionBySupervisorId(string supervisorId)
        {
            var supervisorProfessions = await _supervisorProfessionRepository.GetByCondition(sp => sp.SupervisorId == supervisorId);

            if (supervisorProfessions == null || !supervisorProfessions.Any())
            {
                return new ApiSuccessResult<List<SupervisorProfessionDto>>(new List<SupervisorProfessionDto>());
            }
            var result = new List<SupervisorProfessionDto>();
            foreach (var profession in supervisorProfessions)
            {
                result.Add(new SupervisorProfessionDto()
                {
                    Profession = new ProfessionDto()
                    {
                        ProfessionID = profession.ProfessionId,
                        ProfessionFullName = profession.Profession.ProfessionFullName,
                    }
                });
            }
            return new ApiSuccessResult<List<SupervisorProfessionDto>>(result);
        }

        public async Task<ApiResult<SupervisorDto>> GetProfileOfSupervisorByUserId(string userId)
        {
            var supervisor = await _supervisorRepository.GetById(s => s.SupervisorId == userId && s.DeletedAt == null);

            if (supervisor == null)
            {
                return new ApiErrorResult<SupervisorDto>("Không tìm thấy thông tin giảng viên.");
            }

            var user = await _userRepository.GetById(u => u.UserId == userId);

            if (user == null)
            {
                return new ApiErrorResult<SupervisorDto>("Không tìm thấy thông tin người dùng.");
            }

            // Trả về thông tin Supervisor kèm theo User
            return new ApiSuccessResult<SupervisorDto>(new SupervisorDto
            {
                SupervisorID = supervisor.SupervisorId,
                PhoneNumber = supervisor.PhoneNumber ?? "",
                SelfDescription = supervisor.SelfDescription ?? "",
                FeEduEmail = supervisor.FeEduEmail ?? "",
                PersonalEmail = supervisor.PersonalEmail ?? "",
                FieldSetting = supervisor.FieldSetting,
                SupervisorNavigation = new UserDto
                {
                    UserID = user.UserId,
                    Avatar = user.Avatar ?? "",
                    FullName = user.FullName ?? "",
                    Gender = user.Gender ?? 0,
                    FptEmail = user.FptEmail ?? "",
                    RoleID = user.RoleId.Value,
                }
            });
        }

        public async Task<ApiResult<SupervisorDto>> GetProfileOfSupervisorByUserIdFullPro(string userid)
        {
            var supervisor = await _supervisorRepository.GetById(s => s.SupervisorId == userid && s.DeletedAt == null);

            if (supervisor == null)
            {
                return new ApiErrorResult<SupervisorDto>("Không tìm thấy thông tin giảng viên.");
            }

            var user = await _userRepository.GetById(u => u.UserId == userid);

            if (user == null)
            {
                return new ApiErrorResult<SupervisorDto>("Không tìm thấy thông tin người dùng.");
            }

            // Chuyển đổi FieldSetting từ JSON
            SupervisorFieldSetting fieldSetting = string.IsNullOrEmpty(supervisor.FieldSetting) ? new SupervisorFieldSetting() : JsonSerializer.Deserialize<SupervisorFieldSetting>(supervisor.FieldSetting);

            // Nếu không có FeEduEmail, thì sử dụng PersonalEmail
            if (string.IsNullOrEmpty(supervisor.FeEduEmail))
            {
                fieldSetting.PersonalEmail = true;
                supervisor.FieldSetting = JsonSerializer.Serialize(fieldSetting);
            }

            // Lấy danh sách Profession
            var supervisorProfessions = await _supervisorProfessionRepository.GetByCondition(sp => sp.SupervisorId == userid);
            var professions = supervisorProfessions.Select(sp => new Profession
            {
                ProfessionId = sp.Profession.ProfessionId,
                ProfessionFullName = sp.Profession.ProfessionFullName,
            }).ToList();

            return new ApiSuccessResult<SupervisorDto>(new SupervisorDto
            {
                SupervisorID = supervisor.SupervisorId,
                SupervisorNavigation = new UserDto
                {
                    UserID = user.UserId,
                    Avatar = user.Avatar ?? "",
                    FullName = user.FullName ?? "",
                    Gender = user.Gender ?? 0
                },
                PhoneNumber = supervisor.PhoneNumber ?? "",
                SelfDescription = supervisor.SelfDescription ?? "",
                FeEduEmail = supervisor.FeEduEmail ?? "",
                PersonalEmail = supervisor.PersonalEmail ?? "",
                FieldSetting = supervisor.FieldSetting
            });
        }

        public async Task<ApiResult<SupervisorDto>> GetSupervisorById(string supervisorID)
        {
            var supervisorData = await _supervisorRepository.GetById(s => s.SupervisorId == supervisorID && s.DeletedAt == null);

            if (supervisorData == null)
            {
                return new ApiErrorResult<SupervisorDto>("Không tìm thấy thông tin giảng viên.");
            }

            // Deserialize FieldSetting
            SupervisorFieldSetting fieldSetting = new SupervisorFieldSetting();
            if (!string.IsNullOrEmpty(supervisorData.FieldSetting))
            {
                fieldSetting = JsonSerializer.Deserialize<SupervisorFieldSetting>(supervisorData.FieldSetting);
            }

            // Nếu FeEduEmail rỗng, bật cờ PersonalEmail
            if (string.IsNullOrEmpty(supervisorData.FeEduEmail))
            {
                fieldSetting.PersonalEmail = true;
            }

            // Lấy dữ liệu User
            var userData = await _userRepository.GetById(u => u.UserId == supervisorID);

            if (userData == null)
            {
                return new ApiErrorResult<SupervisorDto>("Không tìm thấy thông tin người dùng.");
            }

            // Gán dữ liệu Supervisor
            SupervisorDto supervisor = new SupervisorDto
            {
                SupervisorID = supervisorData.SupervisorId,
                FeEduEmail = supervisorData.FeEduEmail ?? "",
                FieldSetting = supervisorData.FieldSetting ?? "",
                PhoneNumber = fieldSetting.PhoneNumber ? supervisorData.PhoneNumber ?? "" : null,
                PersonalEmail = fieldSetting.PersonalEmail ? supervisorData.PersonalEmail ?? "" : null,
                SelfDescription = fieldSetting.SelfDescription ? supervisorData.SelfDescription ?? "" : null,
                SupervisorNavigation = new UserDto
                {
                    Avatar = userData.Avatar ?? "",
                    FullName = userData.FullName ?? "",
                    Gender = userData.Gender ?? 0
                }
            };

            return new ApiSuccessResult<SupervisorDto>(supervisor);
        }

        public async Task<ApiResult<SupervisorDto>> GetSupervisorByUserId(string userId)
        {
            var supervisorData = (await _supervisorRepository.GetByCondition(s => s.SupervisorId == userId && s.DeletedAt == null)).FirstOrDefault();

            if (supervisorData == null)
            {
                return new ApiErrorResult<SupervisorDto>("Không tìm thấy thông tin giảng viên.");
            }

            // Deserialize FieldSetting
            SupervisorFieldSetting fieldSetting = new SupervisorFieldSetting();
            if (!string.IsNullOrEmpty(supervisorData.FieldSetting))
            {
                fieldSetting = JsonSerializer.Deserialize<SupervisorFieldSetting>(supervisorData.FieldSetting);
            }

            // Nếu FeEduEmail rỗng, bật cờ PersonalEmail
            if (string.IsNullOrEmpty(supervisorData.FeEduEmail))
            {
                fieldSetting.PersonalEmail = true;
            }

            // Danh sách ngành nghề của Supervisor
            var supervisorProfessions = supervisorData.SupervisorProfessions.Select(sp => new SupervisorProfessionDto
            {
                Profession = new ProfessionDto
                {
                    ProfessionID = sp.Profession.ProfessionId,
                    ProfessionFullName = sp.Profession.ProfessionFullName
                },
                IsDevHead = sp.IsDevHead.Value,
            }).ToList();
            // Gán dữ liệu Supervisor
            SupervisorDto supervisor = new SupervisorDto
            {
                SupervisorID = supervisorData.SupervisorId,
                FeEduEmail = supervisorData.FeEduEmail ?? "",
                FieldSetting = JsonSerializer.Serialize(fieldSetting),
                PhoneNumber = fieldSetting.PhoneNumber ? supervisorData.PhoneNumber ?? "" : null,
                PersonalEmail = fieldSetting.PersonalEmail ? supervisorData.PersonalEmail ?? "" : null,
                SelfDescription = fieldSetting.SelfDescription ? supervisorData.SelfDescription ?? "" : null,
                SupervisorNavigation = new UserDto
                {
                    Avatar = supervisorData.SupervisorNavigation.Avatar ?? "",
                    FullName = supervisorData.SupervisorNavigation.FullName ?? "",
                    Gender = supervisorData.SupervisorNavigation.Gender ?? 0
                },
                SupervisorProfessions = supervisorProfessions
            };

            return new ApiSuccessResult<SupervisorDto>(supervisor);
        }

        public Task<ApiResult<List<SupervisorForAssigning>>> GetSupervisorsForAssigning(int[] professions, int professionOfGroupIdea, int registerGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<Dictionary<string, List<SupervisorDto>>>> ImportSupervisorList(List<SupervisorDto> supervisors, string devHeadId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateDevhead(SupervisorDto devhead)
        {
            throw new NotImplementedException();
        }
        #region Hàm lỗi
        //Hàm này không dùng
        public Task<ApiResult<bool>> UpdateInforProfileOfSupervisor(Supervisor supervisor)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Task<ApiResult<bool>> UpdateStatusForSupervisor(bool status, string supervisorId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateInforProfileOfSupervisor(Entities.SupervisorDto supervisor)
        {
            throw new NotImplementedException();
        }
    }
}