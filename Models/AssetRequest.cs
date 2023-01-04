public class AssetRequest
{
    public int UserId { get; set; }
    public Sensitivity sensitivity { get; set; }
    public AssetType assetType { get; set; }
    public DateTime pickupdate { get; set; }
    public DateTime endtime { get; set; }
    public int numberofassets { get; set; }
    public string fromAddress { get; set; }
    public string toAddress { get; set; }
    public RequestStatus status { get; set; }
    public DateTime createdOn { get; set; }
}