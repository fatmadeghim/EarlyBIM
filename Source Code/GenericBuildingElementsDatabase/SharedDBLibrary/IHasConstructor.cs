using System;
using System.Collections.Generic;
using System.Text;

namespace SharedDBLibrary
{
    //These interfaces ensure (not entirely!) allow constraints for specific constructors alongside the new() constraint. 
    //Sort of acts like a new(T) constraint.
    //Look into IIsName.cs for reference on how to implement these correctly.
    public interface IHasConstructor<T>
    {
        public void Constructor(T par1);
    }

    public interface IHasConstructor<T1, T2>
    {
        public void Constructor(T1 par1, T2 par2);
    }

    public interface IHasConstructor<T1, T2, T3>
    {
        public void Constructor(T1 par1, T2 par2, T3 par3);
    }

    public interface IHasConstructor<T1, T2, T3, T4>
    {
        public void Constructor(T1 par1, T2 par2, T3 par3, T4 par4);
    }

    public interface IHasConstructor<T1, T2, T3, T4, T5>
    {
        public void Constructor(T1 par, T2 par2, T3 par3, T4 par4, T5 par5);
    }

    public interface IHasConstructor<T1, T2, T3, T4, T5, T6>
    {
        public void Constructor(T1 par, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6);
    }

    public interface IHasConstructor<T1, T2, T3, T4, T5, T6, T7>
    {
        public void Constructor(T1 par, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6, T7 par7);
    }

    public interface IHasConstructor<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public void Constructor(T1 par, T2 par2, T3 par3, T4 par4, T5 par5, T6 par6, T7 par7, T8 par8);
    }
}
