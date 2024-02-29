using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AvitoAdsParser.Models;

public abstract class SearchesDocument
{
    public ObjectId Id { get; set; }
    
    [BsonElement("user_id")]
    public ObjectId UserId { get; set; }
    
    public string Query { get; set; } = default!;
    
    public int Interval { get; set; }

    [BsonElement("last_search")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime LastSearch { get; set; }
}