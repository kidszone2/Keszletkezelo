using RF.Modules.TestFlightAppointment.Models;
using System;

namespace RF.Modules.TestFlightAppointment.Services
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

        TestFlightPlan FindPlanByID(int planID);

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