using BlazorApp1.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Server.Controllers
{
    public class DeleteFileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeleteFileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpDelete("filedelete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _context.UploadResults.FirstOrDefaultAsync(r => r.id == id);

                if (result == null)
                {
                    return NotFound();
                }

                // Remove the file from the server path
                var filePath = Path.Combine("Development", "unsafe_uploads", result.StoredFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    // Remove the record from the database
                    _context.UploadResults.Remove(result);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the error
                // ...

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


    }
}
