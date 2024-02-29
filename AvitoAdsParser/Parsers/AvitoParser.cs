using AvitoAdsParser.Models;
using HtmlAgilityPack;

namespace AvitoAdsParser.Parsers;

public class AvitoParser
{
    public string GetResultsPage(string url)
    {
        return string.Empty;
    }
    
    public string GetProductPage(string url)
    {
        return string.Empty;
    }
    
    public List<AvitoProduct> ParseResultsPage(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var products = new List<AvitoProduct>();

        var items = document.DocumentNode.QuerySelectorAll("[data-marker='item']");
        
        foreach (var item in items)
        {
            var idTag = item.Attributes["data-item-id"]?.Value;
            long.TryParse(idTag, out var id);

            var priceTag = item.QuerySelector("[itemprop='price']")?.Attributes["content"]?.Value ?? "";
            var price = int.TryParse(priceTag, out var pVal) ? pVal : 0;
                
            products.Add(new AvitoProduct()
            {
                Id = id,
                Price = price,
                Name = item.QuerySelector("[itemprop='name']")?.InnerText ?? "",
                Description = item.QuerySelector("[class*='item-description']")?.InnerText ?? "",
                Url = "https://avito.ru" + item.QuerySelector("[data-marker='item-title']").Attributes["href"]?.Value
            });
        }

        return products;
    }

    public DetailedAvitoProducts ParseProductPage(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var idTag = document.DocumentNode.QuerySelector("div[class*='style-item-map-wrapper']")
            .Attributes["data-item-id"].Value;
        long.TryParse(idTag, out var id);

        var title = document.DocumentNode.QuerySelector("[data-marker='item-view/title-info']").InnerText;
        var priceTag = document.DocumentNode.QuerySelector("[itemprop='price']")?.Attributes["content"]?.Value ?? "";
        var price = int.TryParse(priceTag, out var pVal) ? pVal : 0;
        
        var discountedPriceTag = document.DocumentNode.QuerySelector("del[class='']")?.InnerText ?? "";
        var discountedPrice = int.TryParse(discountedPriceTag, out var dpVal) ? dpVal : 0;

        var discountPercentage = document.DocumentNode.QuerySelector("[data-marker='sale-discount']").InnerText;

        var imageUrl = document.DocumentNode.QuerySelector("[data-marker='image-frame/image-wrapper']")
            .Attributes["data-url"].Value;

        var address = document.DocumentNode.QuerySelector("[itemprop='address']").InnerText;

        var description = document.DocumentNode.QuerySelector("[data-marker='item-view/item-description']")?.InnerText; 
        
        var paramList = document.DocumentNode.QuerySelectorAll("li[class*='params-paramsList__item']");
        
        var characteristics = new Dictionary<string, string>();
            
        foreach (var characteristic in paramList)
        {
            if (characteristic.InnerText.Contains(':'))
            {
                var parts = characteristic.InnerText.Split(':');
                characteristics[parts[0]] = parts[1];
            }
        }

        return new()
        {
            Id = id,
            Price = price,
            Name = title,
            DiscountedPrice = discountedPrice,
            DiscountPercentage = discountPercentage,
            ImageUrl = imageUrl,
            Address = address,
            Description = description,
            Characteristics = characteristics
        };
    }
}