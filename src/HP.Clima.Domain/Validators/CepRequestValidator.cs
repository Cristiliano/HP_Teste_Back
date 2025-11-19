using FluentValidation;
using HP.Clima.Domain.DTOs;
using System.Text.RegularExpressions;

namespace HP.Clima.Domain.Validators;

public partial class CepRequestValidator : AbstractValidator<CepRequestDto>
{
    public CepRequestValidator()
    {
        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("CEP é obrigatório")
            .Must(BeAValidCep)
            .WithMessage("CEP deve conter apenas números e pode ter hífen no formato XXXXX-XXX ou XXXXXXXX");
    }
    
    private static bool BeAValidCep(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return false;
            
        if (!HasOnlyNumbers().IsMatch(cep) && !HasNumbersWithHyphen().IsMatch(cep))
            return false;
            
        var normalizedCep = cep.Replace("-", "");
        
        return VerifyExistBetweenOneAndEight().IsMatch(normalizedCep);
    }

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex HasOnlyNumbers();

    [GeneratedRegex(@"^\d+-\d+$")]
    private static partial Regex HasNumbersWithHyphen();

    [GeneratedRegex(@"^\d{1,8}$")]
    private static partial Regex VerifyExistBetweenOneAndEight();
}
