using System.Net;
using System.Net.Http.Json;
using Deploy.Core;

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

        using var response = await httpClient.GetAsync(new Uri(serverAddress)).ConfigureAwait(false);

        var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        Console.WriteLine(responseText);

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

        var vladimir = new User { Name = "Vladimir", Age = 99 };

        using var createResponse = await httpClient.PostAsJsonAsync($"{serverAddress}/create", vladimir).ConfigureAwait(false);
        var vladimirWithId = await createResponse.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

        Console.WriteLine($"Id: {vladimirWithId?.Id} Name: {vladimirWithId?.Name} Age: {vladimirWithId?.Age}");
    }
}
