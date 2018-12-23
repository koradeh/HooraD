using System;
using System.Collections.Generic;
using System.Text;

namespace NancyStandalone
{
    public class SystemStatus : ISystemStatus
    {
        

       public ServiceStatus ServiceStatus { get; set; }

      
        
    }
    public enum ServiceStatus
    {
        Running,
        Stopped
    }
}