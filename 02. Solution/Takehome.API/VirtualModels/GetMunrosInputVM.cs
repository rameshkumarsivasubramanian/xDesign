using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Takehome.API.Models;

namespace Takehome.API.VirtualModels
{
    public class GetMunrosInputVM : IValidatableObject
    {
        public Hill_Categories filterBy { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public int minHeight { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public int maxHeight { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public int rowsCount { get; set; }
        public string sortBy { get; set; }

        public GetMunrosInputVM()
        {
            filterBy = Hill_Categories.Both;
            minHeight = 0;
            maxHeight = 0;
            rowsCount = 0;
            sortBy = "Height DESC, Name";
        }

        public IEnumerable<SortColumnVM> SortColumns
        {
            get
            {
                List<SortColumnVM> revVal = new List<SortColumnVM>();
                foreach (string tkn in sortBy.Split(","))
                    revVal.Add(new SortColumnVM(tkn));

                return revVal;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Custom validations
            List<ValidationResult> retVal = new List<ValidationResult>();

            //10. Check if maxHeight is greater than minHeight
            if (minHeight != 0 && maxHeight != 0 && minHeight > maxHeight)
            {
                retVal.Add(new ValidationResult(
                    string.Format("maxHeight({0}) must be greater than minHeight({1})!", maxHeight, minHeight),
                    new[] { "maxHeight" }));
            }

            //20. Check if the sort columns provided by the user is all valid
            int invalidColumnsCount = SortColumns.Count(s => !s.IsValidColumn);
            if (invalidColumnsCount > 0)
            {
                retVal.Add(new ValidationResult(
                    BuildSortByErrMsg(),
                    new[] { "sortBy" }));
            }

            return retVal;
        }

        private string BuildSortByErrMsg()
        {
            //Miscellaneous: Error message build dynamically based on the columns in the output viewmodel
            string errorMsg = "You can sort by columns ";
            PropertyInfo[] properties = typeof(MunroVM).GetProperties();
            foreach (PropertyInfo pi in properties)
                errorMsg += string.Format("{0}, ", pi.Name);
            errorMsg = errorMsg.Substring(0, errorMsg.Length - 2) + " suffixed by sort direction ASC(default) or DESC";

            return errorMsg;
        }
    }
}
