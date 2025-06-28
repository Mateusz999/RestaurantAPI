using FluentValidation;
using RestaurationAPI.Entities;

namespace RestaurationAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDBContext dBContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.confirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                   var emailInUse = dBContext.Users.Any(u => u.Email == value);
                    if(emailInUse)
                    {
                        context.AddFailure("Email","Email is taken");
                    }
                });
        }
    }
}
