﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public interface IClickable
{
    Image Icon
    {
        get;
        set;
    }

    int Count
    {
        get;
    }

    Text StackSize
    {
        get;
    }
}
