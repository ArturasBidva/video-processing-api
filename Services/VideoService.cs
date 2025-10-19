using Microsoft.EntityFrameworkCore;
using video_processing_api.Data;
using video_processing_api.Models;

namespace video_processing_api.Services
{
	public class VideoService
	{
		private readonly AppDbContext _context;

		public VideoService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Video> UploadVideoAsync(string fileName, int userId)
		{
			var video = new Video
			{
				FileName = fileName,
				UserId = userId,
				Status = VideoStatus.Uploaded,
				UploadedAt = DateTime.UtcNow
			};

			_context.Videos.Add(video);
			await _context.SaveChangesAsync();
			return video;
		}

		public async Task<List<Video>> GetUserVideosAsync(int userId)
		{
			return await _context.Videos
				.Where(v => v.UserId == userId)
				.OrderByDescending(v => v.UploadedAt)
				.ToListAsync();
		}

		public async Task<Video> GetVideoAsync(int videoId, int userId)
		{
			return await _context.Videos
				.FirstOrDefaultAsync(v => v.Id == videoId && v.UserId == userId);
		}

		public async Task<bool> CanProcessVideoAsync(int videoId, int userId)
		{
			var video = await GetVideoAsync(videoId, userId);
			return video != null && (video.Status == VideoStatus.Uploaded|| video.Status == VideoStatus.Failed);
		}

		public async Task UpdateVideoStatusAsync(int videoId, VideoStatus status)
		{
			var video = await _context.Videos.FindAsync(videoId);
			if (video != null)
			{
				video.Status = status;
				await _context.SaveChangesAsync();
			}
		}
	}
}