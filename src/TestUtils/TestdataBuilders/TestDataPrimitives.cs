namespace Pitstop.TestUtils;

/// <summary>
/// Centralized random test-data generators for primitive values.
/// Keeps domain-specific builders (in each test project) free from hardcoded defaults.
/// </summary>
public static class TestDataPrimitives
{
    private static readonly Random _rnd = new Random();
    private static readonly string _validLicenseNumberChars = "DFGHJKLNPRSTXYZ";

    private static readonly string[] _firstNames =
        ["Jan", "Piet", "Klaas", "Maria", "Sophie", "Erik", "Anna", "Henk", "Lisa", "Tom"];
    private static readonly string[] _lastNames =
        ["de Vries", "Jansen", "Bakker", "Visser", "Smit", "Mulder", "Bos", "De Groot", "Peters", "Hendriks"];
    private static readonly string[] _streets =
        ["Kerkstraat", "Dorpsstraat", "Hoofdstraat", "Molenweg", "Schoolstraat", "Stationsweg", "Nieuwstraat", "Parallelweg"];
    private static readonly string[] _cities =
        ["Amsterdam", "Rotterdam", "Utrecht", "Den Haag", "Eindhoven", "Groningen", "Tilburg", "Almere"];
    // Brand and model are paired so generated vehicles are always realistic combinations.
    private static readonly CarType[] _carTypes =
    [
        new("Volkswagen", "Tiguan"),
        new("Toyota", "Corolla"),
        new("BMW", "3 Serie"),
        new("Mercedes", "A-Klasse"),
        new("Peugeot", "308"),
        new("Renault", "Clio"),
        new("Ford", "Focus"),
        new("Opel", "Astra"),
    ];

    public static string RandomName() =>
        $"{Pick(_firstNames)} {Pick(_lastNames)}";

    public static string RandomAddress() =>
        $"{Pick(_streets)} {_rnd.Next(1, 200)}";

    public static string RandomPostalCode() =>
        $"{_rnd.Next(1000, 9999)} {(char)('A' + _rnd.Next(26))}{(char)('A' + _rnd.Next(26))}";

    public static string RandomCity() => Pick(_cities);

    public static string RandomPhoneNumber() =>
        $"+316{_rnd.Next(10000000, 99999999)}";

    public static string RandomEmailAddress(string name = null)
    {
        // Derive from name when provided, otherwise generate a random one
        var localPart = name?.ToLowerInvariant().Replace(" ", ".") ?? $"user{_rnd.Next(1000, 9999)}";
        return $"{localPart}@example.com";
    }

    public static string RandomGuid() => Guid.NewGuid().ToString();

    public static CarType RandomCar() => _carTypes[_rnd.Next(_carTypes.Length)];

    public static string RandomDescription() =>
        $"Maintenance job {_rnd.Next(1000, 9999)}";

    /// <summary>
    /// Returns a (startTime, endTime) tuple representing a business-hours timeslot on today's date.
    /// startTime is between 08:00 and 14:00, endTime is 2-4 hours later.
    /// </summary>
    public static (DateTime Start, DateTime End) RandomTimeslot()
    {
        int startHour = _rnd.Next(8, 15);
        int duration = _rnd.Next(2, 5);
        return (DateTime.Today.AddHours(startHour), DateTime.Today.AddHours(startHour + duration));
    }

    /// <summary>
    /// Generate random Dutch license-plate number.
    /// </summary>
    public static string GenerateRandomLicenseNumber()
    {
        int type = _rnd.Next(1, 9);
        string licenseNumber = null;
        switch (type)
        {
            case 1: // 99-AA-99
                licenseNumber = string.Format("{0:00}-{1}-{2:00}", _rnd.Next(1, 99), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                break;
            case 2: // AA-99-AA
                licenseNumber = string.Format("{0}-{1:00}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 99), GenerateRandomCharacters(2));
                break;
            case 3: // AA-AA-99
                licenseNumber = string.Format("{0}-{1}-{2:00}", GenerateRandomCharacters(2), GenerateRandomCharacters(2), _rnd.Next(1, 99));
                break;
            case 4: // 99-AA-AA
                licenseNumber = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(2), GenerateRandomCharacters(2));
                break;
            case 5: // 99-AAA-9
                licenseNumber = string.Format("{0:00}-{1}-{2}", _rnd.Next(1, 99), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                break;
            case 6: // 9-AAA-99
                licenseNumber = string.Format("{0}-{1}-{2:00}", _rnd.Next(1, 9), GenerateRandomCharacters(3), _rnd.Next(1, 10));
                break;
            case 7: // AA-999-A
                licenseNumber = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(2), _rnd.Next(1, 999), GenerateRandomCharacters(1));
                break;
            case 8: // A-999-AA
                licenseNumber = string.Format("{0}-{1:000}-{2}", GenerateRandomCharacters(1), _rnd.Next(1, 999), GenerateRandomCharacters(2));
                break;
        }

        return licenseNumber;
    }

    private static string Pick(string[] items) => items[_rnd.Next(items.Length)];

    private static string GenerateRandomCharacters(int amount)
    {
        char[] chars = new char[amount];
        for (int i = 0; i < amount; i++)
        {
            chars[i] = _validLicenseNumberChars[_rnd.Next(_validLicenseNumberChars.Length - 1)];
        }
        return new string(chars);
    }
}

/// <summary>
/// Pairs a vehicle brand with a model so generated test data is always realistic.
/// </summary>
public record CarType(string Brand, string Model);