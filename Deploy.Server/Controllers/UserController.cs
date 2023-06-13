using System.Net.Mime;
using Deploy.Core;
using Microsoft.AspNetCore.Mvc;
using SystemTextJsonPatch;

namespace Deploy.Server.Controllers;

#pragma warning disable CA1303
#pragma warning disable CA1721

/// <summary>
/// User controller
/// </summary>
// [AutoValidateAntiforgeryToken]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("/")]
public class UserController : ControllerBase
{
    #region Properties

    private readonly Context _context;

    #endregion

    #region Constructors

    /// <summary>
    /// Database context
    /// </summary>
    public UserController(Context context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    #region GET/HEAD

    // HEAD: 5
    // GET: 5
    /// <summary>
    /// Get user from database by id
    /// </summary>
    /// <param name="id">User's id</param>
    /// <returns>User or response state</returns>
    /// <response code="200">Returns the newly created user</response>
    /// <response code="400">User is null</response>
    [HttpGet("{id}")]
    [HttpHead("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> GetUserAsync(int id)
    {
        Console.WriteLine($"Enter into GET/HEAD: /{id}");

        var user = await Program.GetUserAsync(_context, id).ConfigureAwait(false);

        if (user is null)
            return BadRequest(new ProblemDetails { Detail = $"Object with id={id} doesn't exist" });
        else
            return Ok(user);
    }

    #endregion

    #region POST

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
        Console.WriteLine("Enter into POST: /create");

        await Program.AddUserAsync(_context, user).ConfigureAwait(false);

        if (user == null) return BadRequest();

        return CreatedAtAction(nameof(GetUserAsync), new { id = user.Id }, user);
    }

    #endregion

    #region PATCH

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
        Console.WriteLine($"Enter into PATCH: /patch/{id}");

        var userToUpdate = await Program.GetUserAsync(_context, id).ConfigureAwait(false);
        var update = Program.CloneUser(userToUpdate!);

        if (patch is null) return BadRequest();

        patch.ApplyTo(update!);

        await Program.UpdateUserAsync(_context, userToUpdate!, update!).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetUserAsync), new { id = userToUpdate!.Id }, userToUpdate);
    }

    #endregion

    #region PUT

    // PUT: put/1
    /// <summary>
    /// Put user
    /// </summary>
    /// <param name="id">Id of user to update</param>
    /// <param name="newUser">Update for user</param>
    /// <returns>A newly created User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /put/1
    ///     {
    ///         "name": "Petka",
    ///         "age": 88
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns the newly created user</response>
    /// <response code="400">New user is null</response>
    [HttpPut("put/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PutUserAsync(int id, User newUser)
    {
        Console.WriteLine($"Enter into PUT: /put/{id}");

        var user = await Program.GetUserAsync(_context, id).ConfigureAwait(false);

        if (newUser is null) return BadRequest();

        await Program.UpdateUserAsync(_context, user!, newUser).ConfigureAwait(false);

        return Ok(user);
    }

    #endregion

    #region OPTIONS

    // OPTIONS:
    /// <summary>
    /// Options
    /// </summary>
    /// <returns>Response code</returns>
    [HttpOptions]
    public ActionResult Options()
    {
        Console.WriteLine("Enter into OPTIONS: /");

        Response.Headers.Add("Allow", "GET, HEAD, POST, PATCH, PUT, OPTIONS, DELETE");

        return Ok();
    }
    
    #endregion

    #region DELETE

    // DELETE: delete/1
    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="id">Id of user to delete</param>
    /// <returns>Response code</returns>
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        Console.WriteLine($"Enter into DELETE: /delete/{id}");

        var user = await Program.GetUserAsync(_context, id).ConfigureAwait(false);

        await Program.DeleteUserAsync(_context, user!).ConfigureAwait(false);

        return Ok();
    }

    #endregion

    /// <summary>
    /// Ignore this
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public void IgnoredMethod() { }

    #endregion
}

#pragma warning restore CA1303
