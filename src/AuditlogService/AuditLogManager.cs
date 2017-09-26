using Pitstop.Infrastructure.Messaging;
using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AuditlogService
{
    public class AuditLogManager : IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        private string _logPath;

        public AuditLogManager(IMessageHandler messageHandler, string logpath)
        {
            _messageHandler = messageHandler;
            _logPath = logpath;

            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }

        public void Start()
        {
            _messageHandler.Start(this);
        }

        public void Stop()
        {
            _messageHandler.Stop();
        }

        public async Task<bool> HandleMessageAsync(MessageTypes messageType, string message)
        {
            string logMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff")} - {message}{Environment.NewLine}";
            string logFile = Path.Combine(_logPath, $"{DateTime.Now.ToString("yyyy-MM-dd")}-auditlog.txt");
            await File.AppendAllTextAsync(logFile, logMessage);
            Console.WriteLine(logMessage);
            return true;
        }
    }
}
