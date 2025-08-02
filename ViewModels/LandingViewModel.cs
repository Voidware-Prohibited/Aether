using System;
using Newtonsoft.Json;
using RestSharp.Serializers;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TargetVectorLauncher.ViewModels;

using RestSharp;
using RestSharp.Authenticators;

public partial class LandingViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _jsonString = "Empty";

    public async void GetJson()
    {
        var options = new RestClientOptions("https://v0-new-project-bn49xn3lqqj.vercel.app/api") {
            Authenticator = new HttpBasicAuthenticator("username", "password")
        };
        var client = new RestClient(options);
        var request = new RestRequest("getnews");
        // The cancellation token comes from the caller. You can still make a call without it.
        var response = await client.GetAsync(request);
        if (response.IsSuccessStatusCode)
        {
            // JsonString = "Test String";
            // JsonString = response?.Content;
            Console.WriteLine(JsonString);
        }
        // JsonString = response.Content.ToString();
        // var response = await client.GetAsync(request, cancellationToken);
    }
}