using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DosingApp.Services
{
    public interface IAppHandler
    {
        Task<bool> LaunchApp(string appPackageName, string filePath);
    }
}
