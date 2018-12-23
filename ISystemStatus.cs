using System;
using System.Collections.Generic;
using System.Text;

namespace NancyStandalone
{
    public interface ISystemStatus
    {
       

        ServiceStatus ServiceStatus { get; }
    }
}