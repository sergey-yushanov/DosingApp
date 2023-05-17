using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Services
{
    public interface IAccessFolder
    {
        string GetFolderPath(string folderName);
    }
}
