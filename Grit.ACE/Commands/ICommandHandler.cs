﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE
{
    public interface ICommandHandler<T> where T : Command
    {
        void Execute(T command);
    }
}
