using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Custom;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.StaffViewModel;
using Infrastructure.Repositories.NewsRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.NewsService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<ApiResult<int>> AddNewsThenReturnId(News news, int semester)
        {
            var newNews = new News()
            {
                SemesterId = semester,
                Content = news.Content,
                Title = news.Title,
                StaffId = news.StaffId,
                Pin = news.Pin,
                TypeSupport = news.TypeSupport,
                CreatedAt = DateTime.Now,
                AttachedFile = news.AttachedFile,
                FileName = news.FileName,
            };
            await _newsRepository.CreateAsync(newNews);
            return new ApiSuccessResult<int>(newNews.NewsId);
        }

        public async Task<ApiResult<bool>> DeleteNews(int id)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            expressions.Add(n => n.NewsId == id);
            expressions.Add(n => n.DeletedAt == null);
            var result = await _newsRepository.GetByConditionId(expressions);
            result.DeletedAt = DateTime.Now;
            await _newsRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<FileAttachDto>> DownloadFile(int id)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            expressions.Add(n => n.NewsId == id);
            expressions.Add(n => n.DeletedAt == null);
            var result = await _newsRepository.GetByConditionId(expressions);
            var fileAttachDto = new FileAttachDto()
            {
                Id = result.NewsId,
                FileName = result.FileName,
                FileContent = result.AttachedFile,
                FileSize = result.AttachedFile.Length
            };
            return new ApiSuccessResult<FileAttachDto>(fileAttachDto);
        }

        public async Task<ApiResult<(int, int, List<NewsWithRowNum>)>> GetListNewsForPaging(int pageNumber, int semesterID)
        {
            int pageSize = 10;
            Expression<Func<News, bool>> filter = news =>
            news.DeletedAt == null &&
            news.SemesterId == semesterID &&
            news.TypeSupport == false;
            var listNews = await _newsRepository.GetByCondition(filter);
            int totalRecords = listNews.Count();
            // 🔹 Tính toán số trang
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            // 🔹 Lấy danh sách News với phân trang
            List<News> newsList = await _newsRepository.GetAll(pageSize, pageNumber, filter);
            List<NewsWithRowNum> listnews = newsList.Select((news, index) => new NewsWithRowNum
            {
                RowNum = (pageNumber - 1) * pageSize + index + 1,
                NewID = news.NewsId,
                Title = news.Title ?? "",
                Content = news.Content ?? "",
                Pin = news.Pin,
                TypeSupport = news.TypeSupport,
                CreatedAt = news.CreatedAt,
                Filename = news.FileName ?? "",
                AttachedFile = news.AttachedFile,
                Staff = new Staff { StaffId = news.StaffId }
            }).ToList();

            return new ApiSuccessResult<(int, int, List<NewsWithRowNum>)>((totalPages, totalRecords, listnews));
        }

        public async Task<ApiResult<List<NewsDto>>> getNews(int semesterID)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            expressions.Add(n => n.SemesterId == semesterID);
            expressions.Add(n => n.DeletedAt == null);
            var newsList = await _newsRepository.GetByConditions(expressions);
            var result = new List<NewsDto>();
            foreach (var news in newsList)
            {
                result.Add(new NewsDto()
                {
                    NewID = news.NewsId,
                    Title = news.Title,
                    Content = news.Content,
                    Pin = news.Pin.Value,
                    TypeSupport = news.TypeSupport.Value,
                    CreatedAt = news.CreatedAt,
                    Staff = new StaffDto() { StaffID = news.StaffId }
                });
            }
            return new ApiSuccessResult<List<NewsDto>>(result);
        }

        public async Task<ApiResult<NewsDto>> getNewsById(int id)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            expressions.Add(n => n.NewsId == id);
            expressions.Add(n => n.DeletedAt == null);
            var news = await _newsRepository.GetByConditionId(expressions);
            var result = new NewsDto()
            {
                NewID = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                CreatedAt = news.CreatedAt,
                //AttachedFile = news.AttachedFile,
                FileName = news.FileName
            };
            return new ApiSuccessResult<NewsDto>(result);
        }

        public async Task<ApiResult<NewsDto>> getNewsPin(int semesterID)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            expressions.Add(n => n.SemesterId == semesterID);
            expressions.Add(n => n.TypeSupport == false);
            expressions.Add(n => n.Pin == true);
            expressions.Add(n => n.DeletedAt == null);
            var news = await _newsRepository.GetByConditionId(expressions);
            var result = new NewsDto()
            {
                NewID = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                CreatedAt = news.CreatedAt
            };
            return new ApiSuccessResult<NewsDto>(result);
        }

        public async Task<ApiResult<List<NewsDto>>> getNewsWithTypeSupport(int semesterID)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            expressions.Add(n => n.SemesterId == semesterID);
            expressions.Add(n => n.TypeSupport == false);
            expressions.Add(n => n.DeletedAt == null);
            var newsList = await _newsRepository.GetByConditions(expressions);
            var result = new List<NewsDto>();
            foreach (var news in newsList)
            {
                result.Add(new NewsDto()
                {
                    NewID = news.NewsId,
                    Title = news.Title,
                    Content = news.Content,
                    Pin = news.Pin.Value,
                    TypeSupport = news.TypeSupport.Value,
                    CreatedAt = news.CreatedAt,
                    Staff = new StaffDto()
                    {
                        StaffID = news.StaffId,
                    },
                    //AttachedFile = news.AttachedFile,
                    FileName = news.FileName,
                });
            };
            return new ApiSuccessResult<List<NewsDto>>(result);
        }

        public async Task<ApiResult<bool>> UpdateNews(int id, string Title, string Content, IFormFile file, string exsistedFileName)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            expressions.Add(n => n.NewsId == id);
            expressions.Add(n => n.DeletedAt == null);
            News result = await _newsRepository.GetByConditionId(expressions);

            result.Title = Title;
            result.Content = Content;
            result.UpdatedAt = DateTime.Now;
            result.FileName = exsistedFileName;
            if (file != null && file.Length > 0)
            {
                result.FileName = file.FileName;
                result.AttachedFile = await ConvertToByteArrayAsync(file);
            }
            await _newsRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }
        private async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        public Task<ApiResult<bool>> UpdatePinOfNews(int id, bool pin)
        {
            throw new NotImplementedException();
        }
    }
}
