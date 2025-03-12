namespace DeveloperCodingTest.Api.Controllers;

using Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[Route("api/stories")]
[ApiController]
public class StoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public StoriesController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet("best")]
    [EnableRateLimiting("GetBestStories")]
    [ProducesResponseType<List<StoryDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBestStories([FromQuery] int n)
    {
        if (n <= 0)
            return BadRequest(new ErrorResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "The number of stories must be greater than zero."
            });

        var stories = await _mediator.Send(new GetBestStoriesQuery(n));
        return Ok(stories);
    }
}
