using System.Text.RegularExpressions;
using Pitstop.WorkshopManagementAPI.Domain.Exceptions;

namespace WorkshopManagementAPI.Domain.ValueObjects
{
    public class LicenseNumber
    {
        private const string NUMBER_PATTERN = @"^((\d{1,3}|[a-z]{1,3})-){2}(\d{1,3}|[a-z]{1,3})$";
        private string _value;

        public string Value => _value;

        private LicenseNumber(string licenseNumber) => _value = Value;

        public static LicenseNumber Create(string value)
        {
            if (!Regex.IsMatch(value, NUMBER_PATTERN, RegexOptions.IgnoreCase))
            {
                throw new InvalidValueException("The specified license-number was not in the correct format.");
            }
            return new LicenseNumber(value);
        }

        public static LicenseNumber CreateWithoutValidation(string value) => 
            new LicenseNumber(value);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return string.Equals(this.Value, ((LicenseNumber)obj).Value);
        }
        
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(LicenseNumber licenseNumber) => licenseNumber.Value;
    }
}