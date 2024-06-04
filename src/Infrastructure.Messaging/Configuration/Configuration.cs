namespace Pitstop.Infrastructure.Messaging.Configuration;

public static class Configuration
{
    private const int DEFAULT_PORT = 5672;
    private static string DEFAULT_VIRTUAL_HOST = "/";

    private static string _host;
    private static string _virtualHost;
    private static string _userName;
    private static string _password;
    private static string _exchange;
    private static string _queue;
    private static string _routingKey;
    private static int _port;
    private static List<string> _errors;
    private static bool _isValid;

    public static void UseRabbitMQMessageHandler(this IServiceCollection services, IConfiguration config)
    {
        GetRabbitMQSettings(config, "RabbitMQHandler");
        services.AddTransient<IMessageHandler>(_ => new RabbitMQMessageHandler(
            _host, _virtualHost, _userName, _password, _exchange, _queue, _routingKey, _port));
    }

    public static void UseRabbitMQMessagePublisher(this IServiceCollection services, IConfiguration config)
    {
        GetRabbitMQSettings(config, "RabbitMQPublisher");
        services.AddTransient<IMessagePublisher>(_ => new RabbitMQMessagePublisher(
            _host, _virtualHost, _userName, _password, _exchange, _port));
    }

    private static void GetRabbitMQSettings(IConfiguration config, string sectionName)
    {
        _isValid = true;
        _errors = new List<string>();

        var configSection = config.GetSection(sectionName);
        if (!configSection.Exists())
        {
            throw new InvalidConfigurationException($"Required config-section '{sectionName}' not found.");
        }

        // get configuration settings
        DetermineHost(configSection);
        DetermineVirtualHost(configSection);
        DeterminePort(configSection);
        DetermineUsername(configSection);
        DeterminePassword(configSection);
        DetermineExchange(configSection);
        if (sectionName == "RabbitMQHandler")
        {
            DetermineQueue(configSection);
            DetermineRoutingKey(configSection);
        }

        // handle possible errors
        if (!_isValid)
        {
            var errorMessage = new StringBuilder("Invalid RabbitMQ configuration:");
            _errors.ForEach(e => errorMessage.AppendLine(e));
            throw new InvalidConfigurationException(errorMessage.ToString());
        }
    }

    private static void DetermineHost(IConfigurationSection configSection)
    {
        _host = configSection["Host"];
        if (string.IsNullOrEmpty(_host))
        {
            _errors.Add("Required config-setting 'Host' not found.");
            _isValid = false;
        }
    }

    private static void DetermineVirtualHost(IConfigurationSection configSection)
    {
        string vhostSetting = configSection["VirtualHost"];
        if (string.IsNullOrEmpty(vhostSetting))
        {
            _virtualHost = DEFAULT_VIRTUAL_HOST;
        }
        else
        {
            _virtualHost = configSection["VirtualHost"];
        }
    }

    private static void DeterminePort(IConfigurationSection configSection)
    {
        string portSetting = configSection["Port"];
        if (string.IsNullOrEmpty(portSetting))
        {
            _port = DEFAULT_PORT;
        }
        else
        {
            if (int.TryParse(portSetting, out int result))
            {
                _port = result;
            }
            else
            {
                _isValid = false;
                _errors.Add("Unable to parse config-setting 'Port' into an integer.");
            }
        }
    }
    private static void DetermineUsername(IConfigurationSection configSection)
    {
        _userName = configSection["UserName"];
        if (string.IsNullOrEmpty(_userName))
        {
            _isValid = false;
            _errors.Add("Required config-setting 'UserName' not found.");
        }
    }

    private static void DeterminePassword(IConfigurationSection configSection)
    {
        _password = configSection["Password"];
        if (string.IsNullOrEmpty(_password))
        {
            _isValid = false;
            _errors.Add("Required config-setting 'Password' not found.");
        }
    }

    private static void DetermineExchange(IConfigurationSection configSection)
    {
        _exchange = configSection["Exchange"];
        if (string.IsNullOrEmpty(_exchange))
        {
            _isValid = false;
            _errors.Add("Required config-setting 'Exchange' not found.");
        }
    }

    private static void DetermineQueue(IConfigurationSection configSection)
    {
        _queue = configSection["Queue"];
        if (string.IsNullOrEmpty(_queue))
        {
            _isValid = false;
            _errors.Add("Required config-setting 'Queue' not found.");
        }
    }

    private static void DetermineRoutingKey(IConfigurationSection configSection)
    {
        _routingKey = configSection["RoutingKey"] ?? "";
    }
}