using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Services
{
    public interface IPrint
    {
        bool Print(Stream file);
    }
}
