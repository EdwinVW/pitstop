using System;
using System.Collections.Generic;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using WorkshopManagementAPI.Domain.Core;

namespace Pitstop.WorkshopManagementAPI.Domain.ValueObjects
{
    public class Timeslot : ValueObject
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public static Timeslot Create(DateTime startTime, DateTime endTime)
        {
            ValidateInput(startTime, endTime);

            return new Timeslot
            {
                StartTime = startTime,
                EndTime = endTime
            };
        }

        public Timeslot SetStartTime(DateTime startTime)
        {
            ValidateInput(startTime, EndTime);
            return Timeslot.Create(startTime, EndTime);
        }

        public Timeslot SetEndTime(DateTime endTime)
        {
            ValidateInput(StartTime, endTime);
            return Timeslot.Create(StartTime, endTime);
        }

        private static void ValidateInput(DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
            {
                throw new InvalidValueException($"The specified start-time may not be after the specified end-time.");
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartTime;
            yield return EndTime;
        }
    }
}