using RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Models;
using System;

namespace RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Services
{
    public interface ITestFlightBookingManager
    {
        BookingData FindBookingByID(int bookingID);

        TestFlightBooking[] FindBookingsByUser(
            int userID,
            DateTime? from,
            DateTime? to
            );

        TestFlightBooking[] FindBookingsByDate(
            DateTime? from,
            DateTime? to,
            bool findAll
            );

        TestFlightBooking CreateBooking(
            TestFlightBooking booking
            );

        bool IsSlotAvailable(
            DateTime from,
            int duration
            );

        void CancelBooking(
            int bookingID
            );

        TestFlightParticipant AddParticipantTo(
            int bookingID,
            TestFlightParticipant participant
            );

        TestFlightPlan[] FindFlightPlans(bool findAll);
    }
}