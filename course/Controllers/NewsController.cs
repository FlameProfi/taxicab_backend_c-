//using course.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace course.Controllers;

//[ApiController]
//[AllowAnonymous]
//[Route("api/[controller]")]

//public class NewsController : ControllerBase
//{
//    private readonly AppDbContext _context;
    
//    public NewsController(AppDbContext context)
//    {
//        _context = context;
//    }
    
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<News>>> GetNewses()
//    {
//        return await _context.News.ToListAsync();
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<News>> GetNew(string id)     
//    {
//        var news = await _context.News.FindAsync(id);

//        if (news == null)
//        {
//            return NotFound();
//        }

//        return news;
//    }
//    [HttpPost]
//    public async Task<ActionResult<News>> CreateUser(News news)
//    {
//        news.Id = Guid.NewGuid()
//            .ToString();
//        _context.News.Add(news);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetNew), new { id = news.Id }, news);
//    }
//}