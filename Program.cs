using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = "https://wofdb.com/api/beta/puzzle?versionId=1|3&categorySlug=samename&page=1&rows=144";
            HttpResponseMessage response = await client.GetAsync(url);

            if(response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                
                JObject parsedData = JObject.Parse(jsonData);

                if (parsedData["data"] is JObject dataObject)
                {
                    foreach (var puzzleItem in dataObject.Properties())
                    {
                       JObject puzzleDetails = (JObject)puzzleItem.Value;

                       string puzzleText = puzzleDetails["puzzle"]?.ToString() ?? "Unknown puzzle";
                       string category = puzzleDetails["category"]?["name"]?.ToString() ?? "Unknown category";

                       Console.WriteLine($"Category: {category}");
                       Console.WriteLine($"Puzzle: {puzzleText}");
                    }
                }
                else
                {
                    Console.WriteLine("No puzzles available in the 'data' object.");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }
}