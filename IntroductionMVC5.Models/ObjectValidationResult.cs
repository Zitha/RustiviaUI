using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IntroductionMVC5.Models
{
    public class ObjectValidationResult
    {
        public bool HasErrors
        {
            get { return !IsValid; }
        }

        public bool IsValid { get; set; }

        public IList<ValidationResult> Results { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (ValidationResult result in Results)
            {
                sb.AppendLine(result.ErrorMessage);
            }

            return sb.ToString();
        }
    }
}