using System;
using System.Globalization;

namespace Pitstop.WorkshopManagementAPI.Domain.ValueObjects
{
    public class WorkshopPlanningId
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";
        private string _id;

        public string Value => _id;

        public WorkshopPlanningId(DateTime date)
        {
            _id = date.ToString(DATE_FORMAT);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return string.Equals(this.Value, ((WorkshopPlanningId)obj).Value);
        }
        
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(WorkshopPlanningId id) => id.Value;
        public static implicit operator DateTime(WorkshopPlanningId id) => 
            DateTime.ParseExact(id.Value, DATE_FORMAT, CultureInfo.InvariantCulture);
    }
}