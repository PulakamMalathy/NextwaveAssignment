using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

public class RiderController : Controller
{
    readonly string baseCons = "server=localhost;userid=root;password=plsdb;database=travelrides";

    [Route("api/addride/"), HttpPost]
    public async Task<IActionResult> AddRide([FromBody] RiderInfo request)
    {
        if (request == null || request.UserId < 0)
        {
            return BadRequest("Please provide valid info");
        }
        try
        {
            var q = string.Format(@"Insert into ridersinfo (userid,fromaddress,toaddress,numberofassests,startdate,travelmedium) 
            values ({0},'{1}','{2}',{3},'{4}',{5});",
             request.UserId,
             request.FromAddress,
             request.ToAddress,
             request.NumberOfAssets,
             Convert.ToDateTime(request.RideStartDateTime).ToString("yyyy-MM-dd HH:mm:ss"),
             (int)request.TravelMedium);

            using var con = new MySqlConnection(baseCons);
            await con.ExecuteAsync(q, commandType: CommandType.Text);
            return Ok("ride added successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("error occured in saving request");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}