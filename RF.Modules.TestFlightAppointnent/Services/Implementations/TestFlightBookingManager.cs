﻿using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.UI.UserControls;
using RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Services.Implementations
{
    internal class TestFlightBookingManager : ServiceLocator<ITestFlightBookingManager, TestFlightBookingManager>, ITestFlightBookingManager
    {
        public TestFlightBookingManager() { }

        internal TestFlightBookingManager(
            IUserController userController
            )
        {
            UserController = userController
                ?? throw new ArgumentNullException(nameof(userController));
        }

        private IUserController UserController { get; }

        private bool HasAccess(TestFlightBooking booking)
        {
            var currentUser = UserController.GetCurrentUserInfo();
            if (currentUser is null || currentUser.UserID == Null.NullInteger)
                return false;

            return currentUser.IsAdmin
                || currentUser.UserID == booking.CreatedByUserID;
        }

        private void AssertAccess(TestFlightBooking booking)
        {
            if (!HasAccess(booking))
                throw new TestFlightException("Permission denied.");
        }

        public TestFlightParticipant AddParticipantTo(
            int bookingID,
            TestFlightParticipant participant
            )
        {
            using (var ctx = DataContext.Instance())
            {
                var r = ctx.GetRepository<TestFlightParticipant>();
                r.Insert(participant);

                return participant;
            }
        }

        public void CancelBooking(int bookingID)
        {
            using (var ctx = DataContext.Instance())
            {
                var r = ctx.GetRepository<TestFlightBooking>();
                var booking = r.GetById(bookingID);
                if (booking != null)
                {
                    AssertAccess(booking);
                    booking.IsCancelled = true;
                    r.Update(booking);
                }
            }
        }

        public TestFlightBooking CreateBooking(TestFlightBooking booking)
        {
            var currentUser = UserController.GetCurrentUserInfo();
            if (currentUser.UserID == Null.NullInteger)
                throw new TestFlightException("Guests can't create booking.");

            booking.CreatedByUserID = currentUser.UserID;
            booking.CreatedOnDate = DateTime.Now;

            using (var ctx = DataContext.Instance())
            {

                var r = ctx.GetRepository<TestFlightBooking>();
                r.Insert(booking);
            }

            return booking;
        }

        public BookingData FindBookingByID(int bookingID)
        {
            using (var ctx = DataContext.Instance())
            {
                var r = ctx.GetRepository<TestFlightBooking>();
                var booking = r.GetById(bookingID);
                if (booking is null || !HasAccess(booking))
                    return null;

                var participants = ctx.GetRepository<TestFlightParticipant>()
                    .Find("WHERE BookingID = @0", bookingID)
                    .ToArray();

                return new BookingData(booking, participants);
            }
        }

        public TestFlightBooking[] FindBookingsByDate(DateTime? from, DateTime? to, bool findAll)
        {
            using (var ctx = DataContext.Instance())
            {
                var actualFrom = from ?? DateTime.MinValue;
                var actualTo = to ?? DateTime.MaxValue;

                return ctx.GetRepository<TestFlightBooking>()
                    .Find(
                        "WHERE @0 <= CreatedOnDate AND CreatedOnDate <= @1 AND (IsCancelled = 0 OR @2 = 1)",
                        actualFrom,
                        actualTo,
                        findAll
                        )
                    .ToArray();
            }
        }

        public TestFlightBooking[] FindBookingsByUser(int userID, DateTime? from, DateTime? to)
        {
            using (var ctx = DataContext.Instance())
            {
                var actualFrom = from ?? DateTime.MinValue;
                var actualTo = to ?? DateTime.MaxValue;

                return ctx.GetRepository<TestFlightBooking>()
                    .Find(
                        "WHERE @0 <= CreatedOnDate AND CreatedOnDate <= @1 AND CreatedByUserID = @2",
                        actualFrom,
                        actualTo,
                        userID
                        )
                    .ToArray();
            }
        }

        public TestFlightPlan[] FindFlightPlans(bool findAll)
        {
            using (var ctx = DataContext.Instance())
            {
                return ctx.GetRepository<TestFlightPlan>()
                    .Find("WHERE IsPublic = 1 OR @0 = 1", findAll)
                    .ToArray();
            }
        }

        public bool IsSlotAvailable(DateTime from, int duration)
        {
            using (var ctx = DataContext.Instance())
            {                
                var to = from.AddHours(duration);

                var results = ctx.GetRepository<TestFlightBooking>()
                    .Find(
                        "WHERE @0 <= DATEADD(HOUR, Duration, DepartureAt) AND DepartureAt <= @1 AND IsCancelled = 0",
                        from,
                        to
                        )
                    .ToArray();

                return results.Length == 0;
            }
        }

        protected override Func<ITestFlightBookingManager> GetFactory()
            => () => new TestFlightBookingManager(
                DotNetNuke.Entities.Users.UserController.Instance
                );
    }
}