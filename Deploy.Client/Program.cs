using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Deploy.Core;
using SystemTextJsonPatch;
using SystemTextJsonPatch.Operations;

#pragma warning disable CA1303

namespace Deploy.Client;

public static class Program
{
    public const string IPv6ServerAddress = "https://[::1]:5230";

    public const string IPv4ServerAddress = "https://localhost:5230";

    static async Task Main()
    {
        var serverAddress = IPv4ServerAddress;
        var userId = 1;

        using var httpClient = new HttpClient
        {
            DefaultRequestVersion = HttpVersion.Version30,
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
        };

        #region GET

        Console.WriteLine("GET");

        var uri = new Uri($"{serverAddress}/{userId}");

        Stopwatch stopwatch = Stopwatch.StartNew();

        await GetAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion

        #region HEAD

        Console.WriteLine("HEAD");

        stopwatch.Restart();

        await HeadAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion

        #region POST

        Console.WriteLine("POST");

        uri = new Uri($"{serverAddress}/create");

        stopwatch.Restart();

        var postId = await PostAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion

        #region PATCH

        Console.WriteLine("PATCH");

        uri = new Uri($"{serverAddress}/patch/{postId}");

        stopwatch.Restart();

        await PatchAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion

        #region PUT

        Console.WriteLine("PUT");

        uri = new Uri($"{serverAddress}/put/{postId}");

        stopwatch.Restart();

        await PutAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion

        #region OPTIONS

        Console.WriteLine("OPTIONS");

        uri = new Uri($"{serverAddress}");

        stopwatch.Restart();

        await OptionsAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion

        #region PATCH (experimental)

        Console.WriteLine("PATCH (experimental)");

        uri = new Uri($"{serverAddress}/patch-exp/{postId}");

        stopwatch.Restart();

        await PatchExpAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion

        #region DELETE

        Console.WriteLine("DELETE");

        uri = new Uri($"{serverAddress}/delete/{postId}");

        stopwatch.Restart();

        await DeleteAsync(httpClient, uri).ConfigureAwait(false);

        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds}");

        #endregion
    }

    public static async Task GetAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        using var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Console.WriteLine(responseText);
        }
        else
        {
            var user = await response.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            WriteUserInfo(user!);
        }
    }

    public static async Task HeadAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        using var request = new HttpRequestMessage(HttpMethod.Head, uri);
        request.Version = httpClient.DefaultRequestVersion;
        request.VersionPolicy = httpClient.DefaultVersionPolicy;

        using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Status code: {response.StatusCode}");
            Console.WriteLine($"Content type: {response.Content.Headers.ContentType}");
            Console.WriteLine($"Content length: {response.Content.Headers.ContentLength}");
            Console.WriteLine($"Date: {response.Headers.Date}");
            Console.WriteLine($"Server: {response.Headers.Server}");
        }
        else
        {
            Console.WriteLine($"Error: {response.ReasonPhrase}");
        }
    }

    public static async Task<int> PostAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        var user = new User { Name = "Vladimir", Age = 99 };

        using var response = await httpClient.PostAsJsonAsync(uri, user).ConfigureAwait(false);

        var vladimirWithId = await response.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        WriteUserInfo(vladimirWithId!);

        return vladimirWithId!.Id;
    }

    public static async Task PatchAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        var patch = new JsonPatchDocument<User>();
        patch.Replace((v) => v.Name, "Vovik");
        patch.Replace((v) => v.Age, 36);

        var patchJson = JsonSerializer.Serialize(patch, options: new());
        using var content = new StringContent(patchJson, Encoding.UTF8, "application/json-patch+json");

        using var response = await httpClient.PatchAsync(uri, content).ConfigureAwait(false);

        var patchedUser = await response.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        WriteUserInfo(patchedUser!);
    }

    public static async Task PutAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        var user = new User
        {
            Name = "Warlock",
            Age = 456
        };

        using var response = await httpClient.PutAsJsonAsync(uri, user).ConfigureAwait(false);

        var updatedUser = await response.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        WriteUserInfo(updatedUser!);
    }

    public static async Task OptionsAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        using var request = new HttpRequestMessage(HttpMethod.Options, uri);
        request.Version = httpClient.DefaultRequestVersion;
        request.VersionPolicy = httpClient.DefaultVersionPolicy;

        using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Status code: {response.StatusCode}");
            Console.WriteLine($"Content length: {response.Content.Headers.ContentLength}");
            Console.WriteLine($"Date: {response.Headers.Date}");
            Console.WriteLine($"Server: {response.Headers.Server}");

            foreach (var allow in response.Content.Headers.Allow)
            {
                Console.WriteLine($"allow: {allow}");
            }
        }
        else
        {
            Console.WriteLine($"Error: {response.ReasonPhrase}");
        }
    }

    public static async Task DeleteAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        var deletedUser = await httpClient.DeleteFromJsonAsync<User>(uri).ConfigureAwait(false);

        WriteUserInfo(deletedUser!);
    }

    public static async Task PatchExpAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        // TODO: see: https://github.com/Havunen/SystemTextJsonPatch/issues/20
        var operations = new List<Operation<User>>
        {
            new Operation<User>(OperationType.Replace.ToString(), "/name", null, "Barbariska"),
            new Operation<User>("replace", "/age", null, 678)
        };

        var patchJson = JsonSerializer.Serialize(operations, options: new());
        using var content = new StringContent(patchJson, Encoding.UTF8, "application/json-patch+json");

        using var response = await httpClient.PatchAsync(uri, content).ConfigureAwait(false);

        var patchedUser = await response.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        WriteUserInfo(patchedUser!);
    }

    private static void WriteUserInfo(User user) =>
        Console.WriteLine($"Id: {user.Id} Name: {user.Name} Age: {user.Age}");
}

#pragma warning restore CA1303
