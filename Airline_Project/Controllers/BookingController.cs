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
    public class BookingController : ControllerBase
    {
        private readonly AirlineDatabaseContext _context;

        public BookingController(AirlineDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(Guid id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(Guid id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // DTO for receiving data
        // DTO for receiving data
        public class BookingFlightTicketDto
        {
            // Fields from Bookings table
            public string Status { get; set; }
            public string BookingType { get; set; }
            public Guid UserId { get; set; }

            // Fields from FlightTickets table
            public int ScheduleId { get; set; }
            public string SeatNo { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }

            // New property for SeatDetails
            public List<SeatDetailsDto> SeatDetails { get; set; }
        }

        // DTO for SeatDetails
        public class SeatDetailsDto
        {
            public int ScheduleId { get; set; }
            public string SeatNo { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
        }


        [Route("api/[controller]")]
        [ApiController]
        public class BookingFlightTicketController : ControllerBase
        {
            private readonly AirlineDatabaseContext _context;

            public BookingFlightTicketController(AirlineDatabaseContext context)
            {
                _context = context;
            }

            

                // POST: api/BookingFlightTicket/PostBookingFlightTicket
                [HttpPost("PostBookingFlightTicket")]
                public async Task<ActionResult> PostBookingFlightTicket(BookingFlightTicketDto dto)
                {
                    try
                    {
                        // Save details into the Bookings table
                        var booking = new Booking
                        {
                            Status = dto.Status,
                            BookingType = dto.BookingType,
                            UserId = dto.UserId,
                            // Other properties...
                        };

                        _context.Bookings.Add(booking);
                        await _context.SaveChangesAsync();

                        // Save details into the FlightTickets table
                        var flightTicket = new FlightTicket
                        {
                            BookingId = booking.BookingId,
                            ScheduleId = dto.ScheduleId,
                            SeatNo = dto.SeatNo,
                            Name = dto.Name,
                            Age = dto.Age,
                            Gender = dto.Gender,
                            // Other properties...
                        };

                        _context.FlightTickets.Add(flightTicket);
                        await _context.SaveChangesAsync();

                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Error: {ex.Message}");
                    }
                }
            }


            // DELETE: api/Booking/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteBooking(Guid id)
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return NotFound();
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool BookingExists(Guid id)
            {
                return _context.Bookings.Any(e => e.BookingId == id);
            }
        }
    }

