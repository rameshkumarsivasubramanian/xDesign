using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Takehome.API.Models;

namespace Takehome.API.Services
{
    public class MunroLibraryRepository : IMunroLibraryRepository
    {
        const string DataFolder = "Data";
        const string DataFile = "munrotab_v6.2.csv";

        IEnumerable<Munro> IMunroLibraryRepository.GetMunros()
        {
            List<Munro> retVal = new List<Munro>();

            //10. Build file location
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DataFolder, DataFile);
            //20. Read file data
            using (TextFieldParser csvParser = new TextFieldParser(filePath))
            {
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                //21. Read Header
                string[] colHeaders = csvParser.ReadFields();

                while (!csvParser.EndOfData)
                {
                    //22. Read data
                    string[] fields = csvParser.ReadFields();

                    //23. Ignore the summary rows at the end
                    if (fields[0] != "")
                    {
                        //24. Read CSV data into an object
                        Munro row = new Munro();
                        for (int i = 0; i < colHeaders.Length; i++)
                        {
                            //25. The csv file column's equivalent in the object
                            string colName = "p" + colHeaders[i]
                                .Replace(" ", "_")
                                .Replace("-", "_")
                                .Replace("(", "_")
                                .Replace(")", "_")
                                .Replace(":", "_");
                            //26. Set value
                            SetValue(row, colName, fields[i]);
                        }
                        //27. Add to the collection
                        retVal.Add(row);
                    }
                }
            }

            return retVal;
        }

        private void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            //find out the type
            Type type = inputObject.GetType();

            //get the property information based on the type
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //find the property type
            Type propertyType = propertyInfo.PropertyType;

            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            propertyVal = Convert.ChangeType(propertyVal, propertyType);
            
            //Set the value of the property
            propertyInfo.SetValue(inputObject, propertyVal, null);
        }
    }
}