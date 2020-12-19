using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace MTPub
{
    public class ValueEnteredEventConsumer :
        IConsumer<IValueEntered>
    {
        ILogger<ValueEnteredEventConsumer> _logger;

        public ValueEnteredEventConsumer(ILogger<ValueEnteredEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IValueEntered> context)
        {
            Debug.WriteLine($"Value: {context.Message.Value}");
        }
    }

    public class NameEnteredEventConsumer :
        IConsumer<INameEntered>
    {
        ILogger<NameEnteredEventConsumer> _logger;

        public NameEnteredEventConsumer(ILogger<NameEnteredEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<INameEntered> context)
        {
            Debug.WriteLine($"Age: {context.Message.Age}");
        }
    }
}
