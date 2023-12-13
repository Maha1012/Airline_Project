using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Airline_Project.Models; // Make sure to import the correct namespace for your models
using System;
using System.Linq;

namespace Airline_Project.Controllers
{
    public class AdminFlightDetailController : Controller
    {
        private readonly AirlineDatabaseContext _context; // Replace YourDbContext with the actual DbContext used in your application

        public AdminFlightDetailController(AirlineDatabaseContext context)
        {
            _context = context;
        }

        // GET: AdminFlightDetailController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("GetFlightDetails")]
        public IActionResult GetFlightDetails()
        {
            var flightDetails = _context.FlightDetails.ToList(); // You can modify this query based on your requirements

            return Ok(flightDetails);
        }

        [HttpPost("PostFlightDetails")]
        public IActionResult PostFlightDetails(FlightCapAndStatus details)
        {
            var latestFlight = _context.FlightDetails.OrderByDescending(f => f.FlightName).FirstOrDefault();

            string flightName = "MahaAirline1"; // Default value if no flights exist yet

            if (latestFlight != null)
            {
                string latestFlightName = latestFlight.FlightName;

                if (int.TryParse(latestFlightName.Replace("MahaAirline", ""), out int flightNumber))
                {
                    flightNumber++;
                    flightName = "MahaAirline" + flightNumber;
                }
                else
                {
                    throw new InvalidOperationException("Invalid flight name format");
                }
            }

            FlightDetail newFlight = new FlightDetail
            {
                FlightName = flightName,
                FlightCapacity = details.FlightCapacity,
                IsActive = details.IsActive
            };

            _context.FlightDetails.Add(newFlight);
            _context.SaveChanges();

            return Ok();
        }

        public class FlightCapAndStatus
        {
            public int FlightCapacity { get; set; }
            public bool IsActive { get; set; }
        }

        // GET: AdminFlightDetailController/Create
        public ActionResult Create()
        {
            return View();
        }
    }
}
