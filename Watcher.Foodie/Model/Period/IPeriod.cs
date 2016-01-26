﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public interface IPeriod
    {
        string Key(string @event);
        string Field(DateTime dt);
        IEnumerable<string> KeepFields(DateTime dt);
        long HowLong(int n);
        IEnumerable<string> PatralFields(DateTime from, DateTime to);
    }
}