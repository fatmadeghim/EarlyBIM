using System;
using System.Collections.Generic;
using System.Text;

using SharedDBLibrary;

namespace KnowledgeDB
{
    public class Unit : IHasID
    {
        public int Id { get; set; }
        public int ReferenceValue { get; set; }
        public string ReferenceUnit { get; set; }
        public Unit() { }
        public Unit( int referenceValue, string referenceUnit)
        {
            ReferenceValue = referenceValue;
            ReferenceUnit = referenceUnit;
        }
    }
}
