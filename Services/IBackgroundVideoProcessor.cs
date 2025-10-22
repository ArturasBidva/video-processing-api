using System.Threading.Tasks;
namespace video_processing_api.Services
{
    public interface IBackgroundVideoProcessor
    {
        Task QueueVideoProcessingAsync(int videoId);
        Task ProcessVideoAsync(int videoId);
    }
}