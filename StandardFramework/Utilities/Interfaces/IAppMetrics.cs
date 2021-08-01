using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardFramework.Utilities.Interfaces
{
    public interface IAppMetrics
    {
        Task UpdateScreenCount(Type screenType);
    }
}
