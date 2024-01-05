using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DataConverter
{
    /***
            This public class contains constants that are used throughout the code
    ***/
    public class Constants
    {
        public const string strSeparator = ";";
    }

    /***
            This enum contains possibilities for the desired unit
    ***/
    public enum DesiredUnit
    {
        m3,
        qm,
        m3_or_qm
    }
}
