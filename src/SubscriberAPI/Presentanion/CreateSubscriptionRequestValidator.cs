using FluentValidation;
using Microsoft.OpenApi.Extensions;
using SubscriberAPI.Contracts;
using SubscriberAPI.Domain;

namespace SubscriberAPI.Presentanion
{
    public class CreateSubscriptionRequestValidator: AbstractValidator<SubscriptionRequest>
    {
        public CreateSubscriptionRequestValidator() 
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("The UserId is not be Null");
            RuleFor(x => x.ChatId)
                .NotNull().WithMessage("The ChatId is not be Null")
                .Length(1, 15);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The Name is not be Null");
            RuleFor(x => x.Brand)
                .NotNull().WithMessage("The Brand is not be Null");
            RuleFor(x => x.Price)
                .NotNull().WithMessage("The Price is not be Null")
                .GreaterThan(0).WithMessage("The Price will be more than 0")
                .InclusiveBetween(0.01m, 10000m);
            //RuleFor(x => x.Url)
            //    .NotNull().WithMessage("The Url is not be Null");
        }

    }
}

