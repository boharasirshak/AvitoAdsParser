using System.Net;
using AvitoAdsParser;
using AvitoAdsParser.Models;
using AvitoAdsParser.Parsers;
using MongoDB.Bson;
using MongoDB.Driver;

EnvParser.Load(".env");
var mongoDbUrl = Environment.GetEnvironmentVariable("MONGODB_URI");
var mongoDbName = Environment.GetEnvironmentVariable("MONGODB_NAME");

if (mongoDbUrl == null)
{
    Console.WriteLine("MONGODB_URI environment variable is not set. Exiting.");
    Environment.Exit(1);
}

try
{
    var client = new MongoClient(mongoDbUrl);
    var database = client.GetDatabase(mongoDbName);
    var searchesCollection = database.GetCollection<SearchesDocument>("searches");
    var results = await searchesCollection.FindAsync(Builders<SearchesDocument>.Filter.Empty);
    var searches = await results.ToListAsync();
    
    foreach (var search in searches)
    {
        if (DateTime.Now.Subtract(search.LastSearch) < TimeSpan.FromSeconds(search.Interval))
        {
            continue;
        }
        var query = search.Query;
        var parser = new AvitoParser();
        var productsHtml = parser.GetResultsPage(query);
        var products = parser.ParseResultsPage(productsHtml);
        var productsCollection = database.GetCollection<AvitoProductDocument>("products");
        
        foreach (var product in products)
        {
            var productDocument = await productsCollection
                .FindAsync(avitoProduct => avitoProduct.ProductId == product.Id);

            if (productDocument.Any())
            {   
                var detailedProductHtml = parser.GetProductPage(product.Url);
                var detailedProduct = parser.ParseProductPage(detailedProductHtml);
                await productsCollection.InsertOneAsync(new AvitoProductDocument()
                {
                    ProductId = detailedProduct.Id
                });
                
                // send data to telegram, via bot webhook
                Console.WriteLine(detailedProduct.ToString());
            }

        }
    }

}
catch (Exception e) 
{   
    Console.WriteLine(e.Message);
    Console.WriteLine("Failed to connect to MongoDB. Exiting.");
    Environment.Exit(1);
}

// const string url =
//     "https://www.avito.ru/all/telefony/mobile/novyy-ASgBAgICAkSwwQ2I_Dfo6w74_dsC?" +
//     "f=ASgBAQICBESwwQ2I_Dfk4A2Iwlzo6w74_dsCoNkS0tyOAwFA5uANJPTBXPbBXA";
//
// var cookiesContainer = new CookieContainer();
// var handler = new HttpClientHandler()
// {
//     CookieContainer = cookiesContainer,
//     UseCookies = true,
//     
// };
//
// using var client = new HttpClient(handler);
// client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:123.0) Gecko/20100101 Firefox/123.0");
// client.DefaultRequestHeaders.Add("Referer", "https://www.avito.ru/all/telefony/mobile/novyy-ASgBAgICAkSwwQ2I_Dfo6w74_dsC?f=ASgBAQICBESwwQ2I_Dfk4A2Iwlzo6w74_dsCoNkS0tyOAwFA5uANJPTBXPbBXA");
// client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
// client.DefaultRequestHeaders.Add("Host", "www.avito.ru");
//
// var response = await client.GetAsync("https://www.avito.ru/barnaul/telefony/iphone_13_128_gb_3003393118");
// Console.WriteLine(response.StatusCode);
// var content = await response.Content.ReadAsStringAsync();
// File.WriteAllText("test.html", content);
// var response = await client.GetAsync("https://www.avito.ru");
// Console.WriteLine(response.StatusCode);
// response = await client.GetAsync(url);
// Console.WriteLine(response.StatusCode);
// var content = await response.Content.ReadAsStringAsync();
// File.WriteAllText("page.html", content);
//
// var parser = new AvitoParser();
// var products = parser.ParseResultsPage(content);
//
// var stream = File.AppendText("products.txt");
// foreach (var product in products)
// {
//     stream.WriteLine(product.Url);
//     Console.WriteLine(product.Name);
// }

// var data = File.ReadAllText("test.html");