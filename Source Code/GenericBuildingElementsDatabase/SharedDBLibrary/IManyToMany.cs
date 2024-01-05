using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDBLibrary
{
    public interface IManyToMany<T1, T2> where T1 : IHasID
                                         where T2 : IHasID
    {
        public int Id1 { get; set; }
        public T1 Type1 { get; set; }
        public int Id2 { get; set; }
        public T2 Type2 { get; set; }
    }

    public interface IManyToManyT1HasName<T1, T2> : IManyToMany<T1, T2> where T1 : IHasName, IHasID
                                                                       where T2 : IHasID
    {
        public string getT1Name();
    }

    public interface IManyToManyT2HasName<T1, T2> : IManyToMany<T1, T2> where T2 : IHasName, IHasID
                                                                        where T1 : IHasID
    {
        public string getT2Name();
    }

    public static class ManyToManyExtensionMethods
    //ExtensionMethods for ManyToManyRelations (or collections of them)
    {

    }
}
