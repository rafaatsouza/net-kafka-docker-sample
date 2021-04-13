using System;
using FluentValidation;
using KafkaDockerSample.Ui.Api.Dtos;

namespace KafkaDockerSample.Ui.Api.Validators
{
    public class RegisterOccurrenceValidator: AbstractValidator<RegisterOccurrence> 
    {
        public RegisterOccurrenceValidator() 
        {
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Date)
                .NotEqual(DateTime.MinValue);
        }
    }
}