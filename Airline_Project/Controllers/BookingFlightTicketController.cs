using Airline_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Airline_Project.Controllers.BookingController;
using Microsoft.EntityFrameworkCore; // Add this line

namespace Airline_Project.Controllers
{
    // DTO for receiving data
    public class BookingFlightTicketDto
    {
        // Fields from Bookings table
        public string Status { get; set; }
        public string BookingType { get; set; }
        public Guid UserId { get; set; }

        // Fields from FlightTickets table
        public int ScheduleId { get; set; }
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

    // Your controller
    // Your controller
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
                // Generate a new GUID for BookingId
                var bookingId = Guid.NewGuid();

                // Check if the BookingId does not exist in the Bookings table
                while (await _context.Bookings.AnyAsync(b => b.BookingId == bookingId))
                {
                    // If it exists, generate a new GUID
                    bookingId = Guid.NewGuid();
                }

                // Save details into the Bookings table
                var booking = new Booking
                {
                    Status = dto.Status,
                    BookingType = dto.BookingType,
                    UserId = dto.UserId,
                    BookingId = bookingId, // Use the generated BookingId
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Save details into the FlightTickets table for each seat
                var seatDetails = dto.SeatDetails;
                for (int i = 0; i < seatDetails.Count; i++)
                {
                    var seatDetail = seatDetails[i];

                    var flightTicket = new FlightTicket
                    {
                        BookingId = booking.BookingId, // Use the generated BookingId
                        ScheduleId = dto.ScheduleId,
                        SeatNo = seatDetail.SeatNo,
                        Name = seatDetail.Name,
                        Age = seatDetail.Age,
                        Gender = seatDetail.Gender,
                    };

                    _context.FlightTickets.Add(flightTicket);
                    await _context.SaveChangesAsync();

                    // Update the seat status to 'Booked'
                    var seat = await _context.Seats
                        .Where(s => s.SeatNumber == seatDetail.SeatNo && s.ScheduleId == seatDetail.ScheduleId)
                        .FirstOrDefaultAsync();

                    if (seat != null)
                    {
                        seat.Status = "Booked";
                        await _context.SaveChangesAsync();
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // Log or return the exception details
                return BadRequest($"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
            }
        }
    }
}