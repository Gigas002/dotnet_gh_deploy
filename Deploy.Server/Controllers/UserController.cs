using System.Net.Mime;
using Deploy.Core;
using Microsoft.AspNetCore.Mvc;
using SystemTextJsonPatch;

namespace Deploy.Server.Controllers;

#pragma warning disable CA1303

/// <summary>
/// User controller
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
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
    /// <response code="400">User is null</response>
    [HttpPost("create")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PostUserAsync(User user)
    {
        Console.WriteLine("Enter into /create");

        await Program.AddUserAsync(user).ConfigureAwait(false);

        if (user == null) return BadRequest();

        return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, user);
    }

    /// <summary>
    /// Ignore this
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public void IgnoredMethod() { }

    // PATCH: patch/1
    /// <summary>
    /// Patch user
    /// </summary>
    /// <param name="id">Id of user to patch</param>
    /// <param name="patch">Patch to apply</param>
    /// <returns>A newly created User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /patch/1
    ///     [
    ///         {
    ///             "op": "replace",
    ///             "path": "/name",
    ///             "value": "Greck"
    ///         },
    ///         {
    ///             "op": "replace",
    ///             "path": "/age",
    ///             "value": 51
    ///         }
    ///     ]
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">Patch is null</response>
    [HttpPatch("patch/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json-patch+json")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PatchUserAsync(int id, JsonPatchDocument<User> patch)
    {
        Console.WriteLine($"Enter into /patch/{id}");

        var user = await Program.GetUserAsync(id).ConfigureAwait(false);

        if (patch is null) return BadRequest();

        patch.ApplyTo(user!);

        await Program.UpdateUserAsync(id, user!).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetUserAsync), new { id = user!.Id }, user);
    }
}

#pragma warning restore CA1303
