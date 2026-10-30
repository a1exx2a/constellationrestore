using System.ComponentModel.DataAnnotations;

namespace ConstellationRestore.Dtos;

public class RestoreRequest : IValidatableObject
{
    [Required(ErrorMessage = "Поле 'reference' обязательно.")]
    public string? Reference { get; set; }

    [Required(AllowEmptyStrings = true, ErrorMessage = "Поле 'damaged' обязательно (может быть пустым).")]
    public string? Damaged { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Reference is { } reference)
        {
            for (int i = 0; i < reference.Length; i++)
            {
                char c = reference[i];
                if (c < 'A' || c > 'Z')
                {
                    yield return new ValidationResult(
                        $"Reference содержит недопустимый символ '{c}'. Разрешены только A-Z.",
                        new[] { nameof(Reference) });
                    yield break;
                }

                for (int j = 0; j < i; j++)
                {
                    if (reference[j] == c)
                    {
                        yield return new ValidationResult(
                            $"Reference содержит повторяющийся символ '{c}'.",
                            new[] { nameof(Reference) });
                        yield break;
                    }
                }
            }
        }

        if (Reference is { } refCode && Damaged is { } damaged)
        {
            for (int i = 0; i < damaged.Length; i++)
            {
                char d = damaged[i];
                bool found = false;

                for (int j = 0; j < refCode.Length; j++)
                {
                    if (refCode[j] == d)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    yield return new ValidationResult(
                        $"Damaged содержит символ '{d}', которого нет в reference.",
                        new[] { nameof(Damaged) });
                    yield break;
                }
            }
        }
    }
}