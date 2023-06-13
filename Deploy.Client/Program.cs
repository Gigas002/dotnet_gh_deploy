using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Deploy.Core;
using SystemTextJsonPatch;

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

        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

        #region GET

        Console.WriteLine("GET");

        var uri = new Uri($"{serverAddress}/{userId}");

        await GetAsync(httpClient, uri).ConfigureAwait(false);

        #endregion

        #region POST

        Console.WriteLine("POST");

        uri = new Uri($"{serverAddress}/create");

        var postId = await PostAsync(httpClient, uri).ConfigureAwait(false);

        #endregion

        #region PATCH

        Console.WriteLine("PATCH");

        uri = new Uri($"{serverAddress}/patch/{postId}");

        await PatchAsync(httpClient, uri).ConfigureAwait(false);

        #endregion
    }

    public static async Task GetAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        using var getUserResponse = await httpClient.GetAsync(uri).ConfigureAwait(false);

        if (getUserResponse.StatusCode == HttpStatusCode.NotFound)
        {
            var responseText = await getUserResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            Console.WriteLine(responseText);
        }
        else
        {
            var user = await getUserResponse.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            WriteUserInfo(user!);
        }
    }

    public static async Task<int> PostAsync(HttpClient httpClient, Uri uri)
    {
        if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

        var user = new User { Name = "Vladimir", Age = 99 };

        using var createResponse = await httpClient.PostAsJsonAsync(uri, user).ConfigureAwait(false);
        
        var vladimirWithId = await createResponse.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

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

        using var patchResponse = await httpClient.PatchAsync(uri, content).ConfigureAwait(false);

        var patchedUser = await patchResponse.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        WriteUserInfo(patchedUser!);
    }

    private static void WriteUserInfo(User user) =>
        Console.WriteLine($"Id: {user.Id} Name: {user.Name} Age: {user.Age}");
}

#pragma warning restore CA1303
