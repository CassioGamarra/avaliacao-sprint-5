using FluentValidation;
using Sprint5API.DTOs;

namespace Sprint5API.Validators
{
    /*O validator do ViaCEP trabalha com o cliente DTO para validar o CEP enviado através da request*/
    public class ViaCepValidator : AbstractValidator<ClienteDTO>
    {
        public ViaCepValidator()
        {
            RuleFor(viaCep => viaCep.Cep).NotEmpty().WithMessage("Por favor, preencha o CEP."); 
            RuleFor(viaCep => viaCep.Cep).Matches(@"^[0-9]{5}-?[0-9]{3}$").WithMessage("O CEP informado é inválido.");
        }
        
    }
}
