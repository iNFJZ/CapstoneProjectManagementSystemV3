using Infrastructure.Custom;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.ViewModel.StaffViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.NewsService
{
    public interface INewsService
    {
        Task<ApiResult<List<News>>> getNewsWithTypeSupport(int semesterID);
        Task<ApiResult<int>> AddNewsThenReturnId(News news, int semester);
        Task<ApiResult<bool>>UpdateNews(int id, string Title, string Content, IFormFile file, string exsistedFileName);
        Task<ApiResult<News>> getNewsById(int id);
        Task<ApiResult<bool>> DeleteNews(int id);
        Task<ApiResult<List<News>>> getNews(int semesterID);
        Task<ApiResult<(int, int, List<NewsWithRowNum>)>> GetListNewsForPaging(int pageNumber, int semesterID);
        Task<ApiResult<bool>> UpdatePinOfNews(int id, bool pin);

        Task<ApiResult<News>> getNewsPin(int semesterID);
        Task<ApiResult<FileAttachDto>> DownloadFile(int id);
    }
}
