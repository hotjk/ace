﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Model
{
    public class Seconds : IPeriod
    {
        private Seconds() { }
        public static readonly Seconds Instance = new Seconds();

        public string Quantifier
        {
            get
            {
                return "second(s)";
            }
        }

        public string Key(string @event)
        {
            return @event + "_s";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmss");
        }

        public IEnumerable<string> KeepFields(DateTime dt)
        {
            return new string[] { dt.AddMinutes(-1).ToString("yyyyMMddHHmm"), dt.ToString("yyyyMMddHHmm") };
        }

        public long HowLong(int n)
        {
            return new TimeSpan(0, 0, n).Ticks;
        }

        public IEnumerable<string> PatrolFields(DateTime from, DateTime to)
        {
            for (DateTime dt = from; dt < to; dt = dt.AddSeconds(1))
            {
                yield return (Field(dt));
            }
        }

        public DateTime RemoveLastPeriod(DateTime dt)
        {
            return dt.AddSeconds(-1);
        }
    }
}
