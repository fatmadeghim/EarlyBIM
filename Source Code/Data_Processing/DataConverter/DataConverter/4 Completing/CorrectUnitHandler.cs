using System;
using System.Collections.Generic;
using System.Text;

namespace DataConverter
{
    class CorrectUnitHandler
    {
        /***
        This function checks if the given SingleModEntry has a correct unit (depending on its category)
        :param entry: one SingleModEntry
        :param unitPosition: position of unit in GeneralInformation
        :param valuePosition: position of value in GeneralInformation
        :param categoryPosition: position of category in GeneralInformation
        :return: bool with "true" if correct unit and false if not
        ***/
        public static bool CheckCorrectUnit(SingleModEntry entry, int valuePosition, int unitPosition, int categoryPosition)
        {
            //unit value needs to be 1
            if (entry.GeneralInformation[valuePosition] != "1")
            {
                return false;
            }

            //get desired unit
            var desiredUnit = GetDesiredUnit(entry.GeneralInformation[categoryPosition]);
            
            if(desiredUnit == DesiredUnit.qm && entry.GeneralInformation[unitPosition] == "qm")
            {
                return true;
            }
            else if(desiredUnit == DesiredUnit.m3 && entry.GeneralInformation[unitPosition] == "m3")
            {
                return true;
            }
            else if(desiredUnit == DesiredUnit.m3_or_qm && (entry.GeneralInformation[unitPosition] == "m3"|| entry.GeneralInformation[unitPosition] == "qm"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /***
        This function returns the desired unit of an entry
        :param category: string with category
        :return: enum DesiredUnit with "qm", "m3" or "qm or m3"
        ***/
        public static DesiredUnit GetDesiredUnit(string category)
        {
            //categories for flooring or cladding (in qm)
            var listCategoriesFlooringCladding = new List<string>()
            {
                "Holz / Holzböden / Kork",
                "Holz / Holzböden / Parkett",
                "Holz / Holzwerkstoffe / Funierschichtholz",
                "Holz / Vollholz / Brettschichtholzplatte",
                "Kunststoffe / Bodenbeläge / Gummi-/Kautschuk-Bodenbeläge",
                "Kunststoffe / Bodenbeläge / Linoleum-Bodenbeläge",
                "Kunststoffe / Bodenbeläge / PVC-Bodenbeläge",
                "Kunststoffe / Bodenbeläge / Textile Bodenbeläge",
                "Metalle / Aluminium / Aluminiumbleche",
                "Metalle / Blei / Bleibleche",
                "Metalle / Edelstahl / Edelstahlbleche",
                "Metalle / Stahl und Eisen / Stahlbleche",
                "Metalle / Zink / Zinkbleche",
                "Mineralische Baustoffe / Steine und Elemente / Dachziegel",
                "Mineralische Baustoffe / Steine und Elemente / Faserzement",
                "Mineralische Baustoffe / Steine und Elemente / Kunststein",
                "Mineralische Baustoffe / Steine und Elemente / Schiefer",
                "Mineralische Baustoffe / Steine und Elemente / Fliesen und Platten"
            };

            
            //frame in qm (only in qm, not in m3)
            if (category.ToLower().Contains("rahmen") || category.ToLower().Contains("aluminiumprofil"))
            {
                return DesiredUnit.qm;
            }
            //window "filling" in qm (only in qm, not in m3)
            else if (category.ToLower().Contains("füllungen"))
            {
                return DesiredUnit.qm;
            }
            //composite slabs in qm (only)
            else if (category.ToLower().Contains("systembauteile / decken"))
            {
                return DesiredUnit.qm;
            }
            //flooring or cladding in qm
            else if (listCategoriesFlooringCladding.Contains(category))
            {
                return DesiredUnit.m3_or_qm;
            }
            //Doors and gates in qm
            else if (category.ToLower().Contains("türen und tore"))
            {
                return DesiredUnit.m3_or_qm;
            }
            //coating in qm
            else if (category.ToLower().Contains("beschichtung"))
            {
                return DesiredUnit.m3_or_qm;
            }
            //roofing membrane in qm
            else if (category.ToLower().Contains("dachbahnen"))
            {
                return DesiredUnit.m3_or_qm;
            }
            //wärmedämmverbundsystem in qm
            else if (category.ToLower().Contains("wärmedämmverbundsystem"))
            {
                return DesiredUnit.m3_or_qm;
            }
            //folien und vliese in qm
            else if (category.ToLower().Contains("folien und vliese"))
            {
                return DesiredUnit.m3_or_qm;
            }
            //all others in m3
            else
            {
                return DesiredUnit.m3;
            }
        }
    }
}
