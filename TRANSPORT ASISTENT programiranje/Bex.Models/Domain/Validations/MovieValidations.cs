using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demos.Club.Models
{
    public partial class Movie : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (String.Equals(Title, Director))
            //{
            object additionalInfo = "YES";
            //    validationContext.Items.TryGetValue(
            //        "MovieTitle_IsEqualTo_MovieDirector", out additionalInfo);

            yield return new ValidationResult(
                $"Title can not be same as Director. {additionalInfo?.ToString()}");
            //}
        }
    }
}
