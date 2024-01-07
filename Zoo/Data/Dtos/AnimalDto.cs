using FluentValidation;

namespace Zoo.Data.Dtos;

public record CreateAnimalDto(string Species, string Food, int Amount);
public record UpdateAnimalDto(string Species, string Food, int Amount);

// Modified Animal DTO Validators
public class CreateAnimalValidator : AbstractValidator<CreateAnimalDto>
{
    public CreateAnimalValidator()
    {
        RuleFor(dto => dto.Species).NotEmpty().NotNull();
        RuleFor(dto => dto.Food).NotEmpty().NotNull();
        RuleFor(dto => dto.Amount).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
    }
}

public class UpdateAnimalValidator : AbstractValidator<UpdateAnimalDto>
{
    public UpdateAnimalValidator()
    {
        RuleFor(dto => dto.Species).NotEmpty().NotNull();
        RuleFor(dto => dto.Food).NotEmpty().NotNull();
        RuleFor(dto => dto.Amount).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
    }
}