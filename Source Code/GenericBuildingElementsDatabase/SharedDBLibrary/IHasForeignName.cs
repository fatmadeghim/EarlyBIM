using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDBLibrary
{
    public interface IHasForeignName<T> : IHasName
                                          where T : IsName
    {
        public int NameId { get; set; }
        public T Name { get; set; }
    }
}
