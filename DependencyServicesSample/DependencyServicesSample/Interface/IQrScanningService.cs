using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DependencyServicesSample.Interface
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
