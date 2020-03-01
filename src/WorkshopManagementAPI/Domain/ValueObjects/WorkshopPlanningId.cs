using System;
using System.Collections.Generic;
using System.Globalization;
using WorkshopManagementAPI.Domain.Core;

namespace Pitstop.WorkshopManagementAPI.Domain.ValueObjects
{
    public class WorkshopPlanningId : ValueObject
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";
        private string _id;

        public string Value => _id;

        public WorkshopPlanningId(DateTime date)
        {
            _id = date.ToString(DATE_FORMAT);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(WorkshopPlanningId id) => id.Value;
        public static implicit operator DateTime(WorkshopPlanningId id) => 
            DateTime.ParseExact(id.Value, DATE_FORMAT, CultureInfo.InvariantCulture);
    }
}