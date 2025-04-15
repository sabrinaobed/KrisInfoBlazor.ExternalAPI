KrisInformation Blazor WebAssembly App

1- This project is a Blazor WebAssembly application that consumes data from the Swedish Krisinformation API. The application fetches and displays news alerts, and allows users to view detailed information about each news item.

Features
- Fetches real-time news from [Krisinformation.se API](https://api.krisinformation.se/v3/news)
- Displays a list of news headlines with publication dates
- "Läs mer" (Read more) button navigates to a details page
- Renders full HTML content from the API
- Handles common issues like null values and placeholder text (`{0}`)

2-Project Structure

KrisInfoBlazor

-- Pages
-- Index.razor            --> Home page showing news list
-- Details.razor          --> Details view for individual articles
-- Services
-- KrisInfoService.cs     --> API logic using HttpClient
-- Models
-- NewsItem.cs            --> C# model representing the API response

3-Setup Instructions

1. Create the Project
- Create a Blazor WebAssembly App
- Use `.NET 8.0`
- Select "No Authentication"

2. Add Folders
Create:
Models
Services

3. Add Newtonsoft.Json
Install via NuGet:
Install-Package Newtonsoft.Json


4-Creating the Model

Create `NewsItem.cs` under the `Models` folder:

public class NewsItem
{
    public string Identifier { get; set; }
    public string Headline { get; set; }
    public string Preamble { get; set; }
    public string BodyText { get; set; }
    public DateTime Published { get; set; }
}


5-Creating the API Service

KrisInfoService.cs

public class KrisInfoService
{
    private readonly HttpClient _http;

    public KrisInfoService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<NewsItem>> GetNewsAsync()
    {
        var url = "https://api.krisinformation.se/v3/news?numberOfNewsArticles=10";
        var response = await _http.GetStringAsync(url);
        return JsonConvert.DeserializeObject<List<NewsItem>>(response);
    }
}


6-Register in `Program.cs`

builder.Services.AddScoped<KrisInfoService>();




7-Index Page
`Index.razor`

@inject KrisInfoService NewsService
@using KrisInfoBlazor.Models

<h3>KrisInformation - Nyheter</h3>
@if (items == null)
{
    <p>Laddar nyheter...</p>
}
else
{
    <table>
        <thead>
            <tr>
                <th>Rubrik</th>
                <th>Publicerad</th>
                <th>Mer Info</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in items)
            {
                <tr>
                    <td>@item.Headline</td>
                    <td>@item.Published.ToString("dd/MM/yyyy")</td>
                    <td><NavLink href=@($"/details/{item.Identifier}")>Läs mer</NavLink></td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    List<NewsItem> items;

    protected override async Task OnInitializedAsync()
    {
        items = await NewsService.GetNewsAsync();
    }
}


8-Details Page
`Details.razor`

@page "/details/{id}"
@inject KrisInfoService NewsService
@using KrisInfoBlazor.Models
@using Microsoft.AspNetCore.Components

@if (item == null)
{
    <p>Laddar detaljer...</p>
}
else
{
    <div class="card" style="padding: 20px; max-width: 800px;">
        <h4>@item.Headline</h4>
        <p><strong>Publicerad:</strong> @item.Published.ToString("dd/MM/yyyy HH:mm:ss")</p>
        @((MarkupString)(item.BodyText?.Replace("{0}", "")))
        <br />
        <NavLink class="btn btn-secondary mt-3" href="/">Tillbaka</NavLink>
    </div>
}

@code {
    [Parameter]
    public string? id { get; set; }

    NewsItem? item;

    protected override async Task OnInitializedAsync()
    {
        var allItems = await NewsService.GetNewsAsync();
        item = allItems.FirstOrDefault(i => i.Identifier == id);
    }
}


9-Final Tips

- Use `<link>` to Bootstrap in `wwwroot/index.html` for styles
- Handle null values using `?.` safely
- Avoid route conflicts (only one component per route)
- Replace `{0}` placeholders in BodyText using `.Replace("{0}", "")`

