using Background_Remover.Services;
using Microsoft.AspNetCore.Mvc;

namespace Background_Remover.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IBackgroundRemovalService _backgroundRemovalService;

        public ImageController(IBackgroundRemovalService backgroundRemovalService)
        {
            _backgroundRemovalService = backgroundRemovalService;
        }

        [HttpPost("remove-background")]
        public async Task<IActionResult> RemoveBackground(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Invalid image file.");

            byte[] resultImage = await _backgroundRemovalService.RemoveBackgroundAsync(imageFile);

            if (resultImage == null)
                return StatusCode(500, "Failed to process the image.");

            return File(resultImage, "image/png");
        }
    }
}
