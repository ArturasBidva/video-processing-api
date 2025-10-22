using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using video_processing_api.Services;
using video_processing_api.Dto.Requests;
using video_processing_api.Dto.Responses;
using video_processing_api.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VideoController : ControllerBase
{
    private readonly VideoService _videoService;
    private readonly IBackgroundVideoProcessor _videoProcessor;

    public VideoController(VideoService videoService)
    {
        _videoService = videoService;
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

        // Check if video exists and user owns it
        var video = await _videoService.GetVideoAsync(id, userId);
        if (video == null)
        {
            return NotFound("Video not found");
        }

        // Check if video can be processed (Uploaded or Failed status)
        if (video.Status != VideoStatus.Uploaded && video.Status != VideoStatus.Failed)
        {
            return BadRequest("Video can only be processed when in Uploaded or Failed status");
        }

        // Update status to QueuedForProcessing
        await _videoService.UpdateVideoStatusAsync(id, VideoStatus.QueuedForProcessing);

        return Accepted(new
        {
            message = "Video processing started",
            videoId = id,
            status = "QueuedForProcessing"
        });
    }
}