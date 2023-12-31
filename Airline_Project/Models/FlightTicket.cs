﻿using System;
using System.Collections.Generic;

namespace Airline_Project.Models;

public partial class FlightTicket
{
    public int TicketNo { get; set; }

    public Guid BookingId { get; set; }

    public int ScheduleId { get; set; }

    public string SeatNo { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public string Gender { get; set; } = null!;

    //public string? BookingType { get; set; }

    //public virtual Booking Booking { get; set; } = null!;

    //public virtual FlightSchedule Schedule { get; set; } = null!;
}
