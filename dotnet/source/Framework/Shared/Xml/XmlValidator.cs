using System;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Text;

namespace Imi.Framework.Shared.Xml
{
    /// <summary>
    /// Summary description for XMLValidator.
    /// </summary>
    public class XmlValidator
    {
        private StringBuilder errorReport;
        private int errorCount;

        /// <summary>
        /// Aggregates error messages recieved when validating XML files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ValidationHandler(object sender, ValidationEventArgs args)
        {
            errorReport.Append(string.Format("{0}: {1}\n", args.Severity, args.Message));
            errorCount++;
        }

        /// <summary>
        /// Uses the supplied schema file to validate the supplied XML file.
        /// Validation stops when the number of errors equals numberOfErrors.
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="schemaFile"></param>
        /// <param name="numberOfErrors"></param>
        /// <returns></returns>
        public string ValidateFile(string xmlFile, string schemaFile, int numberOfErrors)
        {
            errorReport = new StringBuilder();
            errorCount = 0;

            // Create the XmlSchemaSet class.
            XmlSchemaSet sc = new XmlSchemaSet();

            // Add the schema to the collection.
            sc.Add("urn:validation-schema", schemaFile);

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            FileStream stream = new FileStream(xmlFile, FileMode.Open);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(stream, settings);

            while ((reader.Read()) && (errorCount < numberOfErrors)) ;
            return (errorReport.ToString());
        }



        /// <summary>
        /// Uses the supplied schema file to validate the supplied XML file.
        /// Validation stops when the number of errors equals numberOfErrors.
        /// </summary>
        /// <param name="xmlStream"></param>
        /// <param name="schemaStream"></param>
        /// <param name="numberOfErrors"></param>
        /// <returns></returns>
        public string ValidateStream(Stream xmlStream, Stream schemaStream, int numberOfErrors)
        {
            errorReport = new StringBuilder();
            errorCount = 0;

            // Create the XmlSchemaSet class.
            XmlSchemaSet sc = new XmlSchemaSet();

            // Add the schema to the collection.
            sc.Add("urn:validation-schema", XmlReader.Create(schemaStream));

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(xmlStream, settings);

            while ((reader.Read()) && (errorCount < numberOfErrors)) ;
            return (errorReport.ToString());
        }

        /// <summary>
        /// Uses the supplied schema file to validate the supplied XML file.
        /// Validation stops when the number of errors equals numberOfErrors.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="schemaStream"></param>
        /// <param name="numberOfErrors"></param>
        /// <returns></returns>
        public String ValidateString(String xmlString, Stream schemaStream, int numberOfErrors)
        {
            errorReport = new StringBuilder();
            errorCount = 0;

            // Create the XmlSchemaSet class.
            XmlSchemaSet sc = new XmlSchemaSet();

            // Add the schema to the collection.
            sc.Add("urn:validation-schema", XmlReader.Create(schemaStream));

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(new StringReader(xmlString), settings);

            while ((reader.Read()) && (errorCount < numberOfErrors)) ;
            return (errorReport.ToString());
        
        }


    }
}
