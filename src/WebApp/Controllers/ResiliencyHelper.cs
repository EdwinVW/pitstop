using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PitStop.Controllers
{
    public class ResiliencyHelper
    {
        private ILogger _logger;

        public ResiliencyHelper(ILogger logger)
        {
            _logger = logger;
        }

        public  async Task<IActionResult> ExecuteResilient(Func<Task<IActionResult>> action, IActionResult fallbackResult)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .RetryAsync(5);

            var fallbackPolicy = Policy<IActionResult>
                .Handle<Exception>()
                .FallbackAsync(
                    fallbackResult,
                    (e, c) => Task.Run(() => _logger.LogError(e.Exception.ToString())));

            return await fallbackPolicy
                .WrapAsync(retryPolicy)
                .ExecuteAsync(action)
                .ConfigureAwait(false);
        }
    }
}
