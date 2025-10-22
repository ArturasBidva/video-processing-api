
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using video_processing_api.Data;
using video_processing_api.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace video_processing_api.Services
{
    public class VideoProcessingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VideoProcessingService> _logger;

        public VideoProcessingService(IServiceProvider serviceProvider, ILogger<VideoProcessingService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessVideosAsync();
                await Task.Delay(5000, stoppingToken); // Check every 5 seconds
            }
        }

        private async Task ProcessVideosAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Find videos queued for processing
            var queuedVideos = await dbContext.Videos
                .Where(v => v.Status == VideoStatus.QueuedForProcessing)
                .ToListAsync();

            foreach (var video in queuedVideos)
            {
                await ProcessSingleVideoAsync(video, dbContext);
            }
        }

        private async Task ProcessSingleVideoAsync(Video video, AppDbContext dbContext)
        {
            try
            {
                _logger.LogInformation($"Starting processing for video {video.Id}");

               
                video.Status = VideoStatus.Processing;
                await dbContext.SaveChangesAsync();

               
                await Task.Delay(20000); // 20 seconds

                if (new Random().Next(0, 10) < 9)
                {
                    video.Status = VideoStatus.Completed;
                    _logger.LogInformation($"Video {video.Id} processing completed");
                }
                else
                {
                    video.Status = VideoStatus.Failed;
                    _logger.LogWarning($"Video {video.Id} processing failed");
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing video {video.Id}");
                video.Status = VideoStatus.Failed;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}