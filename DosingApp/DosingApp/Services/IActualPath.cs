﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DosingApp.Services
{
    public interface IActualPath
    {
        string GetActualPathFromUri(string uriString);
    }
}
