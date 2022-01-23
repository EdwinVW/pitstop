namespace Pitstop.TestUtils;

public static class TestDataGenerators
{
    private static Random _rnd = new Random();
    private static string _validLicenseNumberChars = "DFGHJKLNPRSTXYZ";

    /// <summary>
    /// Generate random licensenumber.
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