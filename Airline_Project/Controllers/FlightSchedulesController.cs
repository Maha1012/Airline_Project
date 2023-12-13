using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Airline_Project.Models;

namespace Airline_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightSchedulesController : ControllerBase
    {
        private readonly AirlineDatabaseContext _context;

        public FlightSchedulesController(AirlineDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/FlightSchedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightSchedule>>> GetFlightSchedules()
        {
            return await _context.FlightSchedules.ToListAsync();
        }

        // GET: api/FlightSchedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightSchedule>> GetFlightSchedule(int id)
        {
            var flightSchedule = await _context.FlightSchedules.FindAsync(id);

            if (flightSchedule == null)
            {
                return NotFound();
            }

            return flightSchedule;
        }

        // PUT: api/FlightSchedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlightSchedule(int id, FlightSchedule flightSchedule)
        {
            if (id != flightSchedule.ScheduleId)
            {
                return BadRequest();
            }

            _context.Entry(flightSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightScheduleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FlightSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateSchedule")]
        public IActionResult CreateSchedule([FromBody] FlightSchedule scheduleInput)
        {
            if (scheduleInput == null)
            {
                return BadRequest("Schedule data is null");
            }

            try
            {
                // Validate date and time
                DateTime currentDateTime = DateTime.Now;
                if (scheduleInput.DateTime < currentDateTime)
                {
                    return BadRequest("Scheduled date and time must be in the future");
                }

                // Parse FlightDuration from string to TimeSpan

                FlightSchedule schedule = new FlightSchedule
                {
                    FlightName = scheduleInput.FlightName,
                    SourceAirportId = scheduleInput.SourceAirportId,
                    DestinationAirportId = scheduleInput.DestinationAirportId,
                    FlightDuration = scheduleInput.FlightDuration,
                    DateTime = scheduleInput.DateTime,
                    // Other properties...
                };

                _context.FlightSchedules.Add(schedule);
                _context.SaveChanges();

                // Retrieve the newly created schedule ID
                int scheduleId = schedule.ScheduleId;

                // Retrieve FlightDetails based on FlightName
                FlightDetail flightDetails = _context.FlightDetails
                    .FirstOrDefault(fd => fd.FlightName == scheduleInput.FlightName);

                if (flightDetails == null)
                {
                    return BadRequest("Flight details not found for the specified FlightName");
                }

                // Use FlightCapacity from FlightDetails
                int totalCapacity = flightDetails.FlightCapacity;
                int seatsPerRow = 6; // Set the desired number of seats per row

                // Populate the seat table with default status "Unbooked"
                for (char row = 'A'; row <= 'Z' && totalCapacity > 0; row++)
                {
                    for (int seatNumber = 1; seatNumber <= seatsPerRow && totalCapacity > 0; seatNumber++)
                    {
                        string seat = $"{row}{seatNumber}";

                        // Create a new seat with default status "Unbooked"
                        Seat newSeat = new Seat
                        {
                            SeatNumber = seat,
                            ScheduleId = scheduleId,
                            Status = "Unbooked",
                        };

                        // Add the seat to the context
                        _context.Seats.Add(newSeat);

                        totalCapacity--;
                    }
                }

                // Save changes to the database
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }

            return Ok("Schedule and seats created successfully");
        }


    

// DELETE: api/FlightSchedules/5
[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlightSchedule(int id)
        {
            var flightSchedule = await _context.FlightSchedules.FindAsync(id);
            if (flightSchedule == null)
            {
                return NotFound();
            }

            _context.FlightSchedules.Remove(flightSchedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightScheduleExists(int id)
        {
            return _context.FlightSchedules.Any(e => e.ScheduleId == id);
        }
    }
}
