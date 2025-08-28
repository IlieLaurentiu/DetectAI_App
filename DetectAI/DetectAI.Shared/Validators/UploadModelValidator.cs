using DetectAI.Shared.Models;
using FluentValidation;
using System.Collections.Generic;


namespace DetectAI.Shared.Validators
{
    public class UploadModelValidator : AbstractValidator<UploadModel>
    {
        public UploadModelValidator()
        {
            RuleFor(x => x.Files)
            .Must(files => files is { Count: > 0 })
            .WithMessage("There must be at least 1 file.");
        }


        public System.Func<object, string, System.Threading.Tasks.Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UploadModel>.CreateWithOptions((UploadModel)model, x => x.IncludeProperties(propertyName)));
            return result.IsValid ? System.Array.Empty<string>() : result.Errors.ConvertAll(e => e.ErrorMessage);
        };
    }
}