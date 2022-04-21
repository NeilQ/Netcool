﻿using System;
using System.Collections.Generic;

namespace Netcool.Core;

public class IgnoreCaseStringComparer: IEqualityComparer<string>
{
    public bool Equals(string x, string y)
    {
        return string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase);
    }
    public int GetHashCode(string code)
    {
        return code.GetHashCode();
    }
}
