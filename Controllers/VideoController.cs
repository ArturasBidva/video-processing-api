using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using video_processing_api.Services;
using video_processing_api.Dto.Requests;
using video_processing_api.Dto.Responses;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VideoController : ControllerBase
{
    private readonly VideoService _videoService;
    //private readonly IBackgroundVideoProcessor _videoProcessor;

    public VideoController(VideoService videoService)
    {
        _videoService = videoService;
        //_videoProcessor = videoProcessor;
    }

    [HttpPost]
    public async Task<IActionResult> UploadVideo(UploadVideoRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID in token");
        }
        var video = await _videoService.UploadVideoAsync(request.FileName, userId);

        return Ok(new VideoResponse
        {
            Id = video.Id,
            FileName = video.FileName,
            Status = video.Status.ToString(),
            UploadedAt = video.UploadedAt
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetVideos()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID in token");
        }
        var videos = await _videoService.GetUserVideosAsync(userId);

        var response = videos.Select(v => new VideoResponse
        {
            Id = v.Id,
            FileName = v.FileName,
            Status = v.Status.ToString(),
            UploadedAt = v.UploadedAt
        });

        return Ok(response);
    }

    [HttpPost("{id}/process")]
    public async Task<IActionResult> ProcessVideo(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user ID in token");
        }

        if (!await _videoService.CanProcessVideoAsync(id, userId))
        {
            return BadRequest("Video cannot be processed");
        }

       
       // await _videoProcessor.QueueVideoProcessingAsync(id);

        return Accepted(new { message = "Video processing started", videoId = id });
    }
}