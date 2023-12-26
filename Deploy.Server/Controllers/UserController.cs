using System.Net.Mime;
using Deploy.Core;
using Deploy.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using SystemTextJsonPatch;
using SystemTextJsonPatch.Operations;

namespace Deploy.Server.Controllers;

#pragma warning disable CA1303

// TODO: attributes are replaceable with:
// [ProducesResponseType<User>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
// [ProducesResponseType(StatusCodes.Status400BadRequest)]
// but works only with NSwag, doesn't work with Swashbuckle

/// <summary>
/// User controller
/// </summary>
[AutoValidateAntiforgeryToken]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("/")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    #region Properties

    private readonly IUserRepository _userRepository = userRepository;

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
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> GetUserAsync(int id)
    {
        Console.WriteLine($"Enter into GET/HEAD: /{id}");

        var user = await _userRepository.GetUserAsync(id).ConfigureAwait(false);

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
    [ProducesResponseType<User>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PostUserAsync(User user)
    {
        Console.WriteLine("Enter into POST: /create");

        if (user is null) return BadRequest();

        (_, var result) = await _userRepository.AddUserAsync(user).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetUserAsync), new { id = result!.Id }, result);
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
    [ProducesResponseType<User>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(MediaTypeNames.Application.JsonPatch)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PatchUserAsync(int id, JsonPatchDocument<User> patch)
    {
        Console.WriteLine($"Enter into PATCH: /patch/{id}");

        var userToUpdate = await _userRepository.GetUserAsync(id).ConfigureAwait(false);
        var update = userToUpdate!.Clone();

        if (patch is null) return BadRequest();

        patch.ApplyTo(update!);

        await _userRepository.UpdateUserAsync(userToUpdate, update).ConfigureAwait(false);

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
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PutUserAsync(int id, User newUser)
    {
        Console.WriteLine($"Enter into PUT: /put/{id}");

        var user = await _userRepository.GetUserAsync(id).ConfigureAwait(false);

        if (newUser is null) return BadRequest();

        await _userRepository.UpdateUserAsync(user!, newUser).ConfigureAwait(false);

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

        Response.Headers.Allow = "GET, HEAD, POST, PATCH, PUT, OPTIONS, DELETE";

        return Ok();
    }

    #endregion

    #region DELETE

    // DELETE: delete/1
    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="id">Id of user to delete</param>
    /// <returns>Deleted user</returns>
    /// <response code="200">Returns the removed user</response>
    [HttpDelete("delete/{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> DeleteUserAsync(int id)
    {
        Console.WriteLine($"Enter into DELETE: /delete/{id}");

        var user = await _userRepository.GetUserAsync(id).ConfigureAwait(false);
        (_, var result) = await _userRepository.DeleteUserAsync(user!).ConfigureAwait(false);

        return Ok(result);
    }

    #endregion

    #region PATCH (experimental)

    // PATCH: patch-exp/1
    /// <summary>
    /// Patch user
    /// </summary>
    /// <param name="id">Id of user to patch</param>
    /// <param name="operations">Patch to apply</param>
    /// <returns>A newly created User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /patch-exp/1
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
    [HttpPatch("patch-exp/{id}")]
    [ProducesResponseType<User>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(MediaTypeNames.Application.JsonPatch)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<User>> PatchUserExpAsync(int id, IEnumerable<Operation<User>> operations)
    {
        Console.WriteLine($"Enter into PATCH: /patch-exp/{id}");

        if (operations is null) return BadRequest();

        var patch = new JsonPatchDocument<User>(operations.ToList(), new());

        var userToUpdate = await _userRepository.GetUserAsync(id).ConfigureAwait(false);
        var update = userToUpdate!.Clone();

        patch.ApplyTo(update!);

        await _userRepository.UpdateUserAsync(userToUpdate, update).ConfigureAwait(false);

        return CreatedAtAction(nameof(GetUserAsync), new { id = userToUpdate!.Id }, userToUpdate);
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
