using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace DataConverter
{
    public class XmlHandler
    {

        /***
            Returns the applicability string from a Xml file specified by input string path
            :param path: string with path to a Xml file
            :return: string
        ***/

        public static string ReadApplicability(string path)
        {
            bool foundDirectory = File.Exists(path);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            XElement root = XElement.Load(path);
            XNamespace odm = root.Name.Namespace;
            IEnumerable<XElement> show = root.Descendants();
            //searches only for German application texts (currently two datasets in oekobaudat, that exist only in English)
            var x =
                from el in root
                .Elements(odm + "processInformation")
                .Elements(odm + "technology")
                .Elements(odm + "technologicalApplicability")
                where el.Attribute(XNamespace.Xml + "lang").Value == "de"
                select el;
           
            if (x.Count() > 0)
            {
                string applicability = x.First().Value;

                //Remove special characters from TechnologicalApplicability (Regex would work as well, but a lot slower)
                applicability = applicability.Replace("\n", String.Empty);
                applicability = applicability.Replace("\r", String.Empty);
                applicability = applicability.Replace("\t", String.Empty);
                applicability = applicability.Replace(";", ",");

                return applicability;
            }

            else
            {

                //throw new NullReferenceException();
                return "";
            }
        }//ReadFromXml()

        /***
           This function adds additional information to referenceFluxName from xml to help find the units of the entries
           :param entry: desired SingleModEntry to be completed
           :param uuidPosition: position of UUID in generalInformation
           :return: void (SingleModEntry automatically passed as ref)
       ***/
        public static void AddTechnologyDescription(SingleModEntry entry, int uuidPosition, int referenceFluxNamePosition, string pathXmlFolder)
        {
            string filePath = @pathXmlFolder + entry.GeneralInformation[uuidPosition] + ".xml";

            bool foundDirectory = File.Exists(filePath);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            XElement root = XElement.Load(filePath);
            XNamespace odm = root.Name.Namespace;
            IEnumerable<XElement> show = root.Descendants();
            var x =
                from el in root
                .Elements(odm + "processInformation")
                .Elements(odm + "technology")
                .Elements(odm + "technologyDescriptionAndIncludedProcesses")
                where el.Attribute(XNamespace.Xml + "lang").Value == "de"
                select el;

            string technologicalDescription = "";
            if (x.Count() > 0)
            {
                technologicalDescription = x.First().Value;

                //Remove special characters from TechnologicalApplicability (Regex would work as well, but a lot slower)
                technologicalDescription = technologicalDescription.Replace("\n", String.Empty);
                technologicalDescription = technologicalDescription.Replace("\r", String.Empty);
                technologicalDescription = technologicalDescription.Replace("\t", String.Empty);
                technologicalDescription = technologicalDescription.Replace(";", ",");
                entry.GeneralInformation[referenceFluxNamePosition] += " Additional Info from Xml: " + technologicalDescription;
            }
        }//AddTechnologyDescription

    }//XmlHandler
}//DataConverter
