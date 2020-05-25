using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public GetMunrosInputVM()
        {
            filterBy = Hill_Categories.Both;
            minHeight = 0;
            maxHeight = 0;
            rowsCount = 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (minHeight != 0 && maxHeight != 0 && minHeight > maxHeight)
            {
                yield return new ValidationResult(
                    string.Format("minHeight({0}) must be less than maxHeight({1})!", minHeight, maxHeight),
                    new[] { "minHeight", "maxHeight" });
            }
        }
    }
}
