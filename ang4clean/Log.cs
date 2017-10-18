using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VSAng
{

    public static class Log
    {
        public static ILoggerFactory LoggerFactory;

        public static ILogger CreateLogger<T>()
        {
            return LoggerFactory?.CreateLogger(typeof(T).FullName);
        }
    }
}
