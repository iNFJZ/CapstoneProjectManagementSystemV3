﻿using ClosedXML.Excel;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SupervisorService
{
    public interface ISupervisorService
    {
        Task<ApiResult<XLWorkbook>> CreateWorkBookBasedOnSupervisorList(List<SupervisorDto> supervisors, int currentRow, string supervisorId);

        Task<ApiResult<(List<SupervisorDto>, List<string>, List<int>)>> CreateSupervisorListBasedOnWorkSheet(IXLWorksheet worksheet, int startRow, List<ProfessionDto> professions);

        Task<ApiResult<Dictionary<string, List<SupervisorDto>>>> ImportSupervisorList(List<SupervisorDto> supervisors, string devHeadId);

        Task<ApiResult<SupervisorDto>> GetSupervisorById(string supervisorID);

        Task<ApiResult<List<SupervisorDto>>> GetAllSupervisor();

        Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorForPaging(int page, string search, string userId);

        Task<ApiResult<bool>> UpdateInforProfileOfSupervisor(SupervisorDto supervisor);

        Task<ApiResult<SupervisorDto>> GetProfileOfSupervisorByUserId(string userId);

        Task<ApiResult<SupervisorDto>> GetSupervisorByUserId(string userId);

        Task<ApiResult<bool>> checkDuplicateFEEduEmail(string feEduEmail);

        Task<ApiResult<List<SupervisorDto>>> GetDevHeadByProfessionID(int professionID);

        Task<ApiResult<List<SupervisorDto>>> getListSupervisorForRegistration(int professionID, int semesterId);


        Task<ApiResult<List<SupervisorForAssigning>>> GetSupervisorsForAssigning(int[] professions, int professionOfGroupIdea, int registerGroupId);

        Task<ApiResult<(int, int, List<SupervisorDto>)>> GetListDevheadForStaffPaging(int pageNumber, string search, int professionId);

        Task<ApiResult<SupervisorDto>> GetDevheadDetailForStaff(string devheadId, int semesterId);

        Task<ApiResult<bool>> UpdateDevhead(SupervisorDto devhead);

        Task<ApiResult<List<SupervisorProfessionDto>>> GetListSupervisorProfessionBySupervisorId(string supervisorId);

        Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorForPagingForStudent
                                                (int pageNumber, int professionId, string search);

        Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorPagingForDevHead
                                             (int pageNumber, string search, string userId, int status);

        Task<ApiResult<bool>> UpdateStatusForSupervisor(bool status, string supervisorId);

        Task<ApiResult<SupervisorDto>> GetProfileOfSupervisorByUserIdFullPro(string userid);
    }
}