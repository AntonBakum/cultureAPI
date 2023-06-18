using CultureAPI.Controllers.DTO;
using CultureAPI.Domain.DataLayerAbstractions;
using CultureAPI.Domain.Models;
using CultureAPI.Domain.ServiceAbstractions;
using CultureAPI.Infrastructure.DataLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CultureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly INewsRepository _newsRepository;

        public NewsController (IFileService fileService, INewsRepository newsRepository)
        {
            _fileService = fileService;
            _newsRepository = newsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetNews([FromQuery] NewsPaginationParameters parameters,[FromQuery] string? searchString = null)
        {
            var news = await _newsRepository.GetNews(parameters, searchString);
            return Ok(news);
        }

        [HttpPost("create-news")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<int>>  CreateNews ([FromForm] NewsDTO news)
        {
            string imagePath = String.Empty;
            if (news.Image != null)
            {
                await _fileService.UploadFile(news.Image, news.Title, news.UserId.ToString());
                imagePath = _fileService.CreateUrl(news.Title, news.UserId.ToString());
            }
            NewsModel insertedNews = new NewsModel()
            { Title = news.Title,
                Content = news.Content,
                PublicationDate = news.PublicationDate,
                UserId = news.UserId,
                Image = imagePath,
            };
            int newsId = await _newsRepository.CreateNews(insertedNews);
            return Ok(newsId);
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<PostDTO>> GetPost([FromRoute] int id)
        {
           PostDTO post = await _newsRepository.GetPostById(id);
           if(post == null)
           {
                return NotFound();
           }
           return Ok(post);
        }
    }
}
