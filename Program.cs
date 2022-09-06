using System.Text;
using Newtonsoft.Json;

string subscriptionKey = "{Your subscription key here}";
string endpoint = "https://api.bing.microsoft.com/v7.0/search";

const string query = "hummingbirds";

// Create a dictionary to store relevant headers
Dictionary<String, String> relevantHeaders = new Dictionary<String, String>();

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Searching the Web for: " + query);

// Construct the URI of the search request
var uriQuery = endpoint + "?q=" + Uri.EscapeDataString(query);

// Perform the Web request and get the response
using (HttpClient httpClient = new HttpClient())
using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uriQuery))
{
    requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
    HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
    string json = await response.Content.ReadAsStringAsync();

    // Extract Bing HTTP headers
    foreach (var header in response.Headers.ToList())
    {
        if (header.Key.StartsWith("BingAPIs-") || header.Key.StartsWith("X-MSEdge-"))
            relevantHeaders[header.Key] = string.Join(",", header.Value);
    }

    // Show headers
    Console.WriteLine("Relevant HTTP Headers:");
    foreach (var header in relevantHeaders)
        Console.WriteLine(header.Key + ": " + header.Value);

    Console.WriteLine("JSON Response:");
    dynamic parsedJson = JsonConvert.DeserializeObject(json);
    Console.WriteLine(JsonConvert.SerializeObject(parsedJson, Formatting.Indented));
}