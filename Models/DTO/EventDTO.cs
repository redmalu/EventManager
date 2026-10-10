using System.ComponentModel.DataAnnotations;

namespace EventManager.Models.DTO
{
    public class EventDTO : IValidatableObject
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Заголовок обязательный для заполнения")]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Дата начала обязательна для заполнения")]
        public required DateTime StartAt { get; set; }
        [Required(ErrorMessage = "Дата окончания обязательна для заполнения")]
        public required DateTime EndAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndAt <= StartAt)
                yield return new ValidationResult("Дата окончания не должна быть раньше или такой же как дата начала", new[] { nameof(EndAt) }); 
        }
    }
}
