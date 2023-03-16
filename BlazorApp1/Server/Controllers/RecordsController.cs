using BlazorApp1.Server.Data;
using BlazorApp1.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

[ApiController]
[Route("[controller]")]
public class RecordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RecordsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UploadResult>>> GetRecords()
    {
        return await _context.UploadResults.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var uploadResult = await _context.UploadResults.FindAsync(id);

        if (uploadResult == null)
            return NotFound();

        var filePath = Path.Combine("Development", "unsafe_uploads", uploadResult.StoredFileName);
        try
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            // code to download file
            return File(fileStream, uploadResult.FileType, uploadResult.FileName);
        }
        catch (Exception ex)
        {
            // log error message or display error to user
            Console.WriteLine(uploadResult.FileName);
            Console.WriteLine(filePath);
            Console.WriteLine($"Error opening file: {ex.Message}");
            return null;
        }

        
    }
}
