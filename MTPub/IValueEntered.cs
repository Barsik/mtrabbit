using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTPub
{
    public interface IValueEntered
    {
        string Value { get; }
    }

    public interface INameEntered
    {
        int Age { get; }
    }

    public class ValueEntered : IValueEntered
    {
        public string Value { get; set; }
    }
}
