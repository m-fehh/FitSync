﻿using System.ComponentModel.DataAnnotations;

namespace CoreSystem
{
    public abstract class ModelBase
    {
        [Key]
        public long Id { get; protected set; }

        [Required]
        public DateTime CreatedAt { get; protected set; }

        [Required]
        public DateTime UpdatedAt { get; protected set; }

        public bool IsDeleted { get; set; }


        protected ModelBase()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
        }

        public virtual bool Validate(out List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(this);
            validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
                return false;

            var requiredProperties = GetType().GetProperties()
                .Where(prop => prop.GetCustomAttributes(typeof(RequiredAttribute), true).Any())
                .ToList();

            foreach (var property in requiredProperties)
            {
                var value = property.GetValue(this);

                if (value == null || (value is string stringValue && string.IsNullOrWhiteSpace(stringValue)))
                {
                    validationResults.Add(new ValidationResult($"{property.Name} é obrigatório."));
                    return false;
                }
            }

            return true;
        }

        public void UpdateUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
