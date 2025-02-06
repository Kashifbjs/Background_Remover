using System.Diagnostics;

namespace Background_Remover.Services
{
    public class BackgroundRemovalService : IBackgroundRemovalService
    {
        private readonly string _pythonExecutable = "python";
        private readonly string _scriptPath;

        public BackgroundRemovalService()
        {
            // Get project root directory instead of bin path
            string projectRoot = Directory.GetCurrentDirectory();
            _scriptPath = Path.Combine(projectRoot, "Assets", "script.py");
        }

        public async Task<byte[]> RemoveBackgroundAsync(IFormFile imageFile)
        {
            // Ensure the script exists
            if (!File.Exists(_scriptPath))
            {
                throw new FileNotFoundException("Python script not found.", _scriptPath);
            }

            // Generate temporary input and output file paths
            string tempDir = Path.GetTempPath();
            string inputPath = Path.Combine(tempDir, Guid.NewGuid() + Path.GetExtension(imageFile.FileName));
            string outputPath = Path.Combine(tempDir, Guid.NewGuid() + ".png");

            // Save uploaded file to temp location
            using (var stream = new FileStream(inputPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Call Python script
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _pythonExecutable,
                    Arguments = $"\"{_scriptPath}\" \"{inputPath}\" \"{outputPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string errorOutput = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            // Check for errors
            if (!File.Exists(outputPath))
            {
                throw new Exception($"Python script failed: {errorOutput}");
            }

            // Read the output image and return as byte array
            byte[] resultImage = await File.ReadAllBytesAsync(outputPath);

            // Cleanup temporary files
            File.Delete(inputPath);
            File.Delete(outputPath);

            return resultImage;
        }
    }
}