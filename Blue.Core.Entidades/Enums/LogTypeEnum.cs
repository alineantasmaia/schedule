using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Blue.Core.Entidades.Enums
{
    public enum LogTypeEnum
    {
        [Description("log-interface-tos")]
        LOG_INTERFACE_TOS = 0,
        [Description("log-control-tower")]
        LOG_CONTROL_TOWER = 1,
        [Description("log-uptime-worker")]
        LOG_UPTIME_WORKER = 2

    }
}
