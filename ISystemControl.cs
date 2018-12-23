using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NancyStandalone
{
    public interface ISystemControl
    {
        Task Reconfigure(ISystemConfiguration configuration);
        Task Start(ISystemConfiguration configuration);
        Task Stop();

        ISystemStatus Status { get; }

    }
}