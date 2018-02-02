using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntroductionMVC5.Models
{
    public class ObjectValidator
    {
        public static ObjectValidationResult Validate(object objectToValidate)
        {
            var result = new ObjectValidationResult();

            var context = new ValidationContext(objectToValidate, serviceProvider: null, items: null);
            IList<ValidationResult> results = new List<ValidationResult>();

            result.IsValid = Validator.TryValidateObject(objectToValidate, context, results);

            if (!result.IsValid)
            {
                result.Results = results;
            }

            return result;
        }
    }
}