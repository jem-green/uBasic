//  Copyright (c) 2017, Jeremy Green All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace uBasicLibrary
{
    public interface IInterpreter
    {
        void Abort(string text);
        void Init();
        void Load(byte[] data);
        void Run();
        void Reset();
        bool IsFinished();
        void Stop();
    }
}
