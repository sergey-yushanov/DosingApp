using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DosingApp.Services
{
    public interface IPathFinder
    {
        Task<string> GetDatabasePathAsync(string filename);
    }
}
