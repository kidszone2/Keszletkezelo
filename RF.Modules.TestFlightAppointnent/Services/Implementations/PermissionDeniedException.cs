﻿using System;
using System.Runtime.Serialization;

namespace RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Services.Implementations
{
    [Serializable]
    internal class TestFlightException : Exception
    {
        public TestFlightException()
        {
        }

        public TestFlightException(string message) : base(message)
        {
        }

        public TestFlightException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TestFlightException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}