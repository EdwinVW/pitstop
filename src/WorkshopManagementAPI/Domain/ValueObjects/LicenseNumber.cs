using System.Collections.Generic;
using System.Text.RegularExpressions;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;
using WorkshopManagementAPI.Domain.Core;

namespace Pitstop.WorkshopManagementAPI.Domain.ValueObjects
{
    public class LicenseNumber : ValueObject
    {
        private const string NUMBER_PATTERN = @"^((\d{1,3}|[a-z]{1,3})-){2}(\d{1,3}|[a-z]{1,3})$";

        public string Value { get; private set; }

        public static LicenseNumber Create(string value)
        {
            if (!Regex.IsMatch(value, NUMBER_PATTERN, RegexOptions.IgnoreCase))
            {
                throw new InvalidValueException($"The specified license-number '{value}' was not in the correct format.");
            }
            return new LicenseNumber { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(LicenseNumber licenseNumber) => licenseNumber.Value;
    }
}