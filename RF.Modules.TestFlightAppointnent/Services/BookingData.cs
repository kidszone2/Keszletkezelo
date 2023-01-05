using RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Models;
using System;

namespace RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Services
{
    public class BookingData
    {
        public TestFlightBooking Booking { get; }

        public TestFlightParticipant[] Participants { get; }

        internal BookingData(
            TestFlightBooking booking,
            TestFlightParticipant[] participants
            )
        {
            Booking = booking 
                ?? throw new ArgumentNullException(nameof(booking));
            Participants = participants 
                ?? throw new ArgumentNullException(nameof(participants));
        }
    }
}