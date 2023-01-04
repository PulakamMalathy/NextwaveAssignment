using Newtonsoft.Json;

public class RiderInfo
{
    [JsonProperty("userid")]
    public int UserId { get; set; }
    [JsonProperty("numberofassets")]
    public int NumberOfAssets { get; set; }
    [JsonProperty("fromaddress")]
    public string FromAddress { get; set; }
    [JsonProperty("toaddress")]
    public string ToAddress { get; set; }
    [JsonProperty("ridestartdatetime")]
    public string RideStartDateTime { get; set; }
    [JsonProperty("travelmedium")]
    public TravelMedium TravelMedium { get; set; }

}