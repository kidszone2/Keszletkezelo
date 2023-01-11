using RF.Modules.TestFlightAppointment.Models;
using System;

namespace RF.Modules.TestFlightAppointment.Services
{
    public class BookingData
    {
        public TestFlightBooking Booking { get; }

        public TestFlightParticipant[] Participants { get; }

        public TestFlightPlan Plan { get; }

        internal BookingData(
            TestFlightBooking booking,
            TestFlightPlan plan,
            TestFlightParticipant[] participants
            )
        {
            Booking = booking
                ?? throw new ArgumentNullException(nameof(booking));
            Plan = plan
                ?? throw new ArgumentNullException(nameof(plan));
            Participants = participants
                ?? throw new ArgumentNullException(nameof(participants));
        }
    }
}