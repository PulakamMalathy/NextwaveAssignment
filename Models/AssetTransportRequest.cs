using Newtonsoft.Json;

public class AssetTransportRequest
{
    [JsonProperty("userid")]
    public int UserId { get; set; }
    [JsonProperty("fromaddress")]
    public string FromAddress { get; set; }
    [JsonProperty("toaddress")]
    public string ToAddress { get; set; }
    [JsonProperty("numberofassets")]
    public int NumberOfAssets { get; set; }
    [JsonProperty("assettype")]
    public AssetType AssetType { get; set; }
    [JsonProperty("sensitivity")]
    public Sensitivity sensitivity { get; set; }
    [JsonProperty("pickupdatetime")]
    public string PickUpDateTime { get; set; }
    [JsonProperty("enddatetime")]
    public string EndDateTime { get; set; }
}