using Newtonsoft.Json;

public class MathcedRide
{
    public int rideId { get; set; }
    public int riderUserId { get; set; }
    public string fromAddress { get; set; }
    public string toAddress { get; set; }
    public DateTime startDate { get; set; }
    public int numberOfAssests { get; set; }
    public int applystatus { get; set; }
}