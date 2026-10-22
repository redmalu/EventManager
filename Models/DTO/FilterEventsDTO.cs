using System.ComponentModel.DataAnnotations;

namespace EventManager.Models.DTO
{
    public class FilterEventsDTO : IValidatableObject
    {
        public string? Title { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        [MinLength(1, ErrorMessage = "Номер страницы не меньше 1")]
        public int? Page { get; set; } = 1;
        
        [Range(5, 100, ErrorMessage ="Размер страницы не меньше 5 и не больше 100 элементов")]
        public int? PageSize { get; set; } = 10;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (To < From)
                yield return new ValidationResult("Дата окончания не должна быть раньше даты начала", new[] { nameof(To) });
        }
    }
}
