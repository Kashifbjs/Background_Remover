namespace Background_Remover.Services
{
    public interface IBackgroundRemovalService
    {
        Task<byte[]> RemoveBackgroundAsync(IFormFile imageFile);
    }
}
