using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Airline_Project.Models;
using static Airline_Project.Controllers.AdminFlightDetailController;

namespace Airline_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightDetailController : ControllerBase
    {
        private readonly AirlineDatabaseContext _context;

        public FlightDetailController(AirlineDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/FlightDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightDetail>>> GetFlightDetails()
        {
            return await _context.FlightDetails.ToListAsync();
        }

        // GET: api/FlightDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightDetail>> GetFlightDetail(string id)
        {
            var flightDetail = await _context.FlightDetails.FindAsync(id);

            if (flightDetail == null)
            {
                return NotFound();
            }

            return flightDetail;
        }
        [HttpPut("{flightName}")]
        public async Task<IActionResult> PutFlightDetail(string flightName, FlightDetail updatedFlight)
        {
            if (flightName != updatedFlight.FlightName)
            {
                return BadRequest();
            }

            // Ensure that FlightID is not modified
            var existingFlight = await _context.FlightDetails.FirstOrDefaultAsync(f => f.FlightName == flightName);
            if (existingFlight == null)
            {
                return NotFound();
            }

            // Update the values of existingFlight with those from updatedFlight
            existingFlight.FlightCapacity = updatedFlight.FlightCapacity;
            existingFlight.IsActive = updatedFlight.IsActive;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightDetailExists(flightName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Move this line outside the if block
            return NoContent();
        }



        // POST: api/FlightDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostFlightDetails")]
            public IActionResult PostFlightDetails(FlightCapAndStatus details)
            {
                try
                {
                    var latestFlight = _context.FlightDetails.OrderByDescending(f => f.FlightName).FirstOrDefault();

                    string flightName = "MahaAirline1"; // Default value if no flights exist yet

                    if (latestFlight != null)
                    {
                        string latestFlightName = latestFlight.FlightName;
                        int flightNumber;
                        if (int.TryParse(latestFlightName.Replace("MahaAirline", ""), out flightNumber))
                        {
                            flightNumber++;
                            flightName = "MahaAirline" + flightNumber;
                        }
                        else
                        {
                            // Handle the case where the flight name format cannot be parsed
                            // For example, log the error or use a default value
                            flightName = "DefaultAirline"; // You can set a default value
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
                catch (Exception ex)
                {
                    return BadRequest($"Error creating flight: {ex.Message}");
                }
            }



            // DELETE: api/FlightDetail/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteFlightDetail(string id)
            {
                var flightDetail = await _context.FlightDetails.FindAsync(id);
                if (flightDetail == null)
                {
                    return NotFound();
                }

                _context.FlightDetails.Remove(flightDetail);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool FlightDetailExists(string id)
            {
                return _context.FlightDetails.Any(e => e.FlightName == id);
            }
        }
    }

