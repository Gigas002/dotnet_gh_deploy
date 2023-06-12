using System.Net;
using System.Net.Http.Json;
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

        using var httpClient = new HttpClient()
        {
            DefaultRequestVersion = HttpVersion.Version30,
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
        };

        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

        // GET
        Console.WriteLine("GET");

        var responseText = string.Empty;

        using var getUserResponse = await httpClient.GetAsync(new Uri($"{serverAddress}/{userId}"))
                                                    .ConfigureAwait(false);

        if (getUserResponse.StatusCode == HttpStatusCode.NotFound)
        {
            responseText = await getUserResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            Console.WriteLine(responseText);
        }
        else
        {
            var user = await getUserResponse.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            Console.WriteLine($"Id: {user?.Id} Name: {user?.Name} Age: {user?.Age}");
        }

        // POST
        Console.WriteLine("POST");

        var vladimir = new User { Name = "Vladimir", Age = 99 };

        using var createResponse = await httpClient.PostAsJsonAsync($"{serverAddress}/create", vladimir).ConfigureAwait(false);
        var vladimirWithId = await createResponse.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        Console.WriteLine($"Id: {vladimirWithId?.Id} Name: {vladimirWithId?.Name} Age: {vladimirWithId?.Age}");

        // PATCH
        Console.WriteLine("PATCH");

        var patch = new JsonPatchDocument<User>();
        patch.Replace((v) => v.Name, "Vovik");
        patch.Replace((v) => v.Age, 36);

        var patchJson = JsonSerializer.Serialize(patch, options: new());

        using var patchResponse = await httpClient.PatchAsJsonAsync(new Uri($"{serverAddress}/patch/{vladimirWithId!.Id}"),
                                                                    patch).ConfigureAwait(false);

        var patchedUser = await patchResponse.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        Console.WriteLine($"Id: {patchedUser?.Id} Name: {patchedUser?.Name} Age: {patchedUser?.Age}");
    }
}

#pragma warning restore CA1303
