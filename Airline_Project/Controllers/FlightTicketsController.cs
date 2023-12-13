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
    public class FlightTicketsController : ControllerBase
    {
        private readonly AirlineDatabaseContext _context;

        public FlightTicketsController(AirlineDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/FlightTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightTicket>>> GetFlightTickets()
        {
            return await _context.FlightTickets.ToListAsync();
        }

        // GET: api/FlightTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightTicket>> GetFlightTicket(int id)
        {
            var flightTicket = await _context.FlightTickets.FindAsync(id);

            if (flightTicket == null)
            {
                return NotFound();
            }

            return flightTicket;
        }

        // PUT: api/FlightTickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlightTicket(int id, FlightTicket flightTicket)
        {
            if (id != flightTicket.TicketNo)
            {
                return BadRequest();
            }

            _context.Entry(flightTicket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightTicketExists(id))
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

        // POST: api/FlightTickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlightTicket>> PostFlightTicket(FlightTicket flightTicket)
        {
            _context.FlightTickets.Add(flightTicket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlightTicket", new { id = flightTicket.TicketNo }, flightTicket);
        }

        // DELETE: api/FlightTickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlightTicket(int id)
        {
            var flightTicket = await _context.FlightTickets.FindAsync(id);
            if (flightTicket == null)
            {
                return NotFound();
            }

            _context.FlightTickets.Remove(flightTicket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightTicketExists(int id)
        {
            return _context.FlightTickets.Any(e => e.TicketNo == id);
        }
    }
}
