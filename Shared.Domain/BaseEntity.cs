using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Domain
{
    public class BaseEntity : BaseError
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime LastModified { get; set; }

    }

    
    public class BaseError
    {
        public IEnumerable<String> GetErrors()
        {
            return Errors;

        }


        [NotMapped]
        private List<string> Errors { get; set; } = new List<string>();

        //to remove
        public bool HaveErrors => Errors != null && Errors.Count > 0;
        public bool IsValid => Errors == null || Errors.Count == 0;

        public void BeginValidation()
        {
            Errors = new List<string>();
        }
        public void ValidateEmptyOrNull(string propName, string propVal)
        {
            if (string.IsNullOrEmpty(propVal))
                AddEmptyError(propName);
        }
        public void ValidateEmptyOrNull(string propName, Enum propVal)
        {
            if (propVal is null)
                AddEmptyError(propName);
        }
        public void ValidateNull(string propName, int? propVal)
        {
            if (!propVal.HasValue)
                AddEmptyError(propName);
        }
        public void ValidateNull(string propName, bool? propVal)
        {
            if (!propVal.HasValue)
                AddEmptyError(propName);
        }
        public void ValidateEmptyList<T>(string propName, List<T> list)
        {
            if (!list.Any())
                AddEmptyError(propName);
        }
        public void ValidateNull(string propName, decimal? propVal)
        {
            if (!propVal.HasValue)
                AddEmptyError(propName);
        }
        public void ValidateNull(string propName, DateTime? propVal)
        {
            if (!propVal.HasValue)
                AddEmptyError(propName);
        }
        public void AddError(string desc)
        {
            Errors.Add(desc);
        }
        public void AddErrors(List<string> errors)
        {
            Errors.AddRange(errors);
        }
        public void AddEmptyError(string propName)
        {
            Errors.Add($"{propName} is mandatory");
        }
    }
}