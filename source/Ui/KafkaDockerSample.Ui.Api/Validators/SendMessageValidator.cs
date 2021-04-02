using FluentValidation;
using KafkaDockerSample.Ui.Api.Dtos;

namespace KafkaDockerSample.Ui.Api.Validators
{
    public class SendMessageValidator: AbstractValidator<SendMessage> 
    {
        public SendMessageValidator() 
        {
            RuleFor(x => x.Message)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .NotNull();
        }
    }
}