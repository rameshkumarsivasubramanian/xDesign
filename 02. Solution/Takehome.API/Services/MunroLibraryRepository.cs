using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
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

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DataFolder, DataFile);
            using (TextFieldParser csvParser = new TextFieldParser(filePath))
            {
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                //Read Header
                string[] colHeaders = csvParser.ReadFields();

                while (!csvParser.EndOfData)
                {
                    //Read current line fields
                    string[] fields = csvParser.ReadFields();

                    //Ignore the summary rows at the end
                    if (fields[0] != "")
                    {
                        Munro row = new Munro();
                        for (int i = 0; i < colHeaders.Length; i++)
                        {
                            string colName = "p" + colHeaders[i]
                                .Replace(" ", "_")
                                .Replace("-", "_")
                                .Replace("(", "_")
                                .Replace(")", "_")
                                .Replace(":", "_");

                            SetValue(row, colName, fields[i]);
                        }
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
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);

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