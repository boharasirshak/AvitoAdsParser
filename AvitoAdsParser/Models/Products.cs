using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AvitoAdsParser.Models;

public class AvitoProduct
{
    public string Name { get; init; } = default!;
    
    public long Id { get; init; }
    
    public string? Description { get; init; }
    
    public string Url { get; init; } = default!;
    
    public int Price { get; init; } 
}

public class DetailedAvitoProducts: AvitoProduct
{
    public int DiscountedPrice { get; init; }
    
    public string DiscountPercentage { get; init; } = default!;
    
    public string ImageUrl { get; init; } = default!;
    
    public string? Address { get; init; }
    
    public Dictionary<string, string> Characteristics { get; init; } = new();
    
    
    public override string ToString()
    {
        return $"Name: {Name}\n" +
               $"Id: {Id}\n" +
               $"Description: {Description}\n" +
               $"Url: {Url}\n" +
               $"Price: {Price}\n" +
               $"DiscountedPrice: {DiscountedPrice}\n" +
               $"DiscountPercentage: {DiscountPercentage}\n" +
               $"ImageUrl: {ImageUrl}\n" +
               $"Address: {Address}\n" +
               $"Characteristics: {string.Join(", ", Characteristics)}";
    }
}

public class AvitoProductDocument
{
    public ObjectId Id { get; set; }
    
    [BsonElement("product_id")]
    public long ProductId { get; set; }
}