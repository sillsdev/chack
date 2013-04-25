﻿using System;
using System.Linq;

namespace Chorus.VcsDrivers.Mercurial
{
    public interface IHgTransport
    {
        void Push();
        bool Pull();
        void Clone();
    }
}
