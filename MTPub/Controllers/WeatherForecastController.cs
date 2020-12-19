using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MTPub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        readonly IPublishEndpoint _publishEndpoint;
        readonly IBus _bus;
        readonly IBusControl _busControl;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBusControl busControl, IBus bus, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _busControl = busControl;
            _bus = bus;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await _bus.Send<IValueEntered>(new ValueEntered()
            {
                Value = DateTime.Now.ToString()
            });
            await _bus.Send<INameEntered>(new
            {
                Age = DateTime.Now.Second
            });

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
