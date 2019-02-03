using HarunBingo.Context;
using HarunBingo.Data.Helpers;
using HarunBingo.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using System.Net;

namespace HarunBingo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class BingoController : ControllerBase
    {
        private const string API_KEY = "AIzaSyA8XaJKZmqULy2iDsg0OTJ9JHlBt0Gmglo";
        private HarunContext _db;
        
        public BingoController(HarunContext db)
        {
            _db = db;
        }

        // GET: api/Bingo
        [HttpGet]
        public IActionResult Get()
        {
            var sh = new JsonResult(_db.ShopLocations.OrderBy(x=>x.DistanceInKm).ToList());
            sh.ContentType = "application/json";
            sh.StatusCode = 200;
            return new JsonResult(sh);
        }


        // GET: api/Bingo
        [HttpPost]
        public IActionResult Post([System.Web.Http.FromUri]string coordinates, [System.Web.Http.FromUri]string distance)
        {
            string template = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0}&radius={1}&type=supermarket&keyword=bingo&fields=formatted_address,name&key={2}";
            string request = string.Format(template, coordinates, distance, API_KEY);
            WebClient webpage = new WebClient();

            string[] crrdns = coordinates.Split(',');
            var response = webpage.DownloadString(request);

            RootObject model = JsonConvert.DeserializeObject<RootObject>(response);
            if (model.status != "OK")
                return BadRequest();

            _db.Database.ExecuteSqlCommand("TRUNCATE TABLE [ShopLocations]");

            foreach(var result in model.results)
            {
                ShopLocation shop = new ShopLocation
                {
                    Name = result.name,
                    Address = result.vicinity,
                    GeoCoordinates = result.geometry.location.lat + "," + result.geometry.location.lng,
                    DistanceInKm = MathHelp.GetDistanceFromLatLonInKm(double.Parse(crrdns[0], CultureInfo.InvariantCulture), double.Parse(crrdns[1], CultureInfo.InvariantCulture), result.geometry.location.lat, result.geometry.location.lng).ToString() + "km"
                };
                _db.ShopLocations.Add(shop);
            }
            _db.SaveChanges();

            var sh = new JsonResult(_db.ShopLocations.ToList());
            sh.ContentType = "application/json";
            sh.StatusCode = 200;
            return new JsonResult(sh);
        }

    }
}
