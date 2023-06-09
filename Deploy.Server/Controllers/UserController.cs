using System.Net.Mime;
using Deploy.Core;
using Microsoft.AspNetCore.Mvc;

namespace Deploy.Server.Controllers;

#pragma warning disable CA1303

/// <summary>
/// User controller
/// </summary>
[ApiController]
[Route("/")]
public class UserController : ControllerBase
{
    // GET: 5
    /// <summary>
    /// Get user from database by id
    /// </summary>
    /// <param name="id">User's id</param>
    /// <returns>User or response state</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]  
    public async Task<ActionResult<User>> GetUserAsync(int id)
    {
        Console.WriteLine($"Enter into /{id}");

        var user = await Program.GetUserAsync(id).ConfigureAwait(false);

        if (user is null)
            return NotFound(new { Message = $"Object with id={id} doesn't exist" });
        else
            return Ok(user);
    }

    // POST: create
    /// <summary>
    /// Add user to database
    /// </summary>
    /// <param name="user">User to add</param>
    /// <returns>A newly created User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /create
    ///     {
    ///        "Name": "Mikhail",
    ///        "Age": 69
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="404">User is null</response>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]  
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PostUserAsync(User user)
    {
        Console.WriteLine("Enter into /create");

        await Program.AddUserAsync(user).ConfigureAwait(false);

        if (user == null) return NotFound();

        return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, user);
    }

    /// <summary>
    /// Ignore this
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public void IgnoredMethod() { }
}

#pragma warning restore CA1303
