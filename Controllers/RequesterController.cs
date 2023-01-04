using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;

public class RequestController : Controller
{
    readonly string baseCons = "server=localhost;userid=root;password=plsdb;database=travelrides";


    [Route("api/addrequest/"), HttpPost]
    public async Task<IActionResult> AddRequest([FromBody] AssetTransportRequest request)
    {
        if (request == null || request.UserId < 0)
        {
            return BadRequest("could not save request . invalid request");
        }
        try
        {
            var q = string.Format(@"Insert into assetrequests (userid,fromaddress,toAddress,assettype,sensitivity,pickupdate,endtime,numberofassets,createdon) 
            values ({0},'{1}','{2}',{3},{4},'{5}','{6}',{7},NOW());",
             request.UserId,
             request.FromAddress,
             request.ToAddress,
             (int)request.AssetType,
             (int)request.sensitivity,
             Convert.ToDateTime(request.PickUpDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
             Convert.ToDateTime(request.EndDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
             request.NumberOfAssets);

            using var con = new MySqlConnection(baseCons);
            await con.ExecuteAsync(q, commandType: CommandType.Text);
            return Ok("request added successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("error occured in saving request");
            return StatusCode(StatusCodes.Status500InternalServerError, "error occured in saving request");
        }

    }

    [Route("api/allrequests/"), HttpGet]
    public async Task<IActionResult> GetAllRequests(int userid, RequestStatus status, AssetType assetType, bool getlatest)
    {
        if (userid <= 0)
        {
            return BadRequest("Invalid userid");
        }
        using var con = new MySqlConnection(baseCons);
        var assetQuery = assetType > 0 ? string.Format("and AssetType = {0}", (int)assetType) : string.Empty;
        var statusQuery = status > 0 ? (status == RequestStatus.expired ? "and endtime <= NOW()" : "and endtime > NOW()") : string.Empty;
        var sortQuery = getlatest ? "order by createdon desc" : "order by createdon";
        var q = string.Format(@"select * ,CASE WHEN endtime <= now() THEN 2
                    WHEN endtime > now() THEN 1
                    END AS status from assetrequests  where userId= {0} {1} {2} {3};", userid, assetQuery, statusQuery, sortQuery);
        var reqList = (await con.QueryAsync<AssetRequest>(q, commandType: CommandType.Text)).ToList();
        return Ok(reqList);
    }

    [Route("api/matchedrides/"), HttpGet]
    public async Task<IActionResult> GetMatchedRides(int userid)
    {
        if (userid <= 0)
        {
            return BadRequest("Invalid userid");
        }
        using var con = new MySqlConnection(baseCons);
        var q = string.Format(@"select r.id as rideid, r.UserId as rideruserid, r.fromaddress, r.toAddress, r.startdate , r.numberofassests
        from assetrequests as ar join 
ridersinfo as r on STRCMP(ar.fromaddress , r.fromaddress) =0 
and STRCMP(ar.toAddress , r.toAddress) =0 
and r.startdate <= ar.endtime and ar.userid = {0};", userid);
        var mathcedRides = (await con.QueryAsync<MathcedRide>(q, commandType: CommandType.Text)).ToList();
        var rideAppliedQuery = string.Format(@"select rideid from appliedrides where requesterid = {0}", userid);
        var ridesapplied = (await con.QueryAsync<int>(rideAppliedQuery, commandType: CommandType.Text)).ToList();
        foreach (var ride in mathcedRides)
        {
            ride.applystatus = ridesapplied.Contains(ride.rideId) ? (int)AppliedStatus.Applied : (int)AppliedStatus.NotApplied;
        }
        return Ok(mathcedRides);
    }

    [Route("api/applyride/"), HttpPost]
    public async Task<IActionResult> ApplyRide(int userid, int riderid, string fromAddress, string toAddress, int numberOfAssests, string startDate)
    {
        if (userid <= 0 || riderid <= 0)
        {
            return BadRequest("Invalid request");
        }
        using var con = new MySqlConnection(baseCons);
        var q = string.Format(@"insert into appliedrides (requesterid,rideid)  
    select {0},r.id from  ridersinfo as r where r.userid = {1} and r.toAddress='{2}'
    and r.fromaddress='{3}' and r.numberofassests={4} and r.startdate='{5}';", userid, riderid, toAddress, fromAddress, numberOfAssests, startDate);
        await con.ExecuteAsync(q, commandType: CommandType.Text);
        return Ok("Ride applied successfully");
    }
}