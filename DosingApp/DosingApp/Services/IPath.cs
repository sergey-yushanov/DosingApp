using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Services
{
    public interface IPath
    {
        string GetDatabasePath(string filename);
    }
}
