using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly BlogContext _context;

    public BlogController(BlogContext context)
    {
        _context = context;
    }

    // GET: api/blog
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlogPost>>> GetAll()
    {
        return await _context.BlogPosts.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    // GET: api/blog/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<BlogPost>> GetById(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);

        if (post == null)
            return NotFound(new { message = "Blog post not found" });

        return post;
    }

    // POST: api/blog
    [HttpPost]
    public async Task<ActionResult<BlogPost>> Create(BlogPost post)
    {
        post.CreatedAt = DateTime.UtcNow;
        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    // PUT: api/blog/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BlogPost updatedPost)
    {
        if (id != updatedPost.Id)
            return BadRequest(new { message = "ID mismatch" });

        var existingPost = await _context.BlogPosts.FindAsync(id);
        if (existingPost == null)
            return NotFound(new { message = "Blog post not found" });

        existingPost.Title = updatedPost.Title;
        existingPost.Content = updatedPost.Content;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/blog/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        if (post == null)
            return NotFound(new { message = "Blog post not found" });

        _context.BlogPosts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
