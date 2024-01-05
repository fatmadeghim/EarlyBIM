using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDBLibrary
{
    public interface IHasName : IHasID
    {
        public string getName();
    }
}

