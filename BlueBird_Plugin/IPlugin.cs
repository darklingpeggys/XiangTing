﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird_Plugin
{
    public interface IPlugin
    {
        string name { get; }
        string info { get; }
        string key { get; }
    }
}
