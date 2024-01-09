using FluentValidation;

namespace Zoo.Data.Dtos;

public record CreateEnclosureDto(string Name, string Size, string Location, string[] Objects, string[]? AllowedSpecies);
public record UpdateEnclosureDto(string Name, string Size, string Location, string[] Objects, string[]? AllowedSpecies);


public class CreateEnclosureValidator : AbstractValidator<CreateEnclosureDto>
{
    public CreateEnclosureValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().NotNull();
        RuleFor(dto => dto.Size).NotEmpty().NotNull();
        RuleFor(dto => dto.Location).NotEmpty().NotNull();
        RuleFor(dto => dto.Objects).NotEmpty().NotNull();
        RuleFor(dto => dto.AllowedSpecies).Null();
    }
}

public class UpdateEnclosureValidator : AbstractValidator<UpdateEnclosureDto>
{
    public UpdateEnclosureValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().NotNull();
        RuleFor(dto => dto.Size).NotEmpty().NotNull();
        RuleFor(dto => dto.Location).NotEmpty().NotNull();
        RuleFor(dto => dto.Objects).NotEmpty().NotNull();
        RuleFor(dto => dto.AllowedSpecies).Null();
    }
}