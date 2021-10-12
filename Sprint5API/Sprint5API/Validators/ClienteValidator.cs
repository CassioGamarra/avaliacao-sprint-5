using FluentValidation;
using Sprint5API.DTOs;

namespace Sprint5API.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteDTO>
    {
        public ClienteValidator()
        {
            RuleFor(cliente => cliente.Nome).NotEmpty().WithMessage("Por favor, preencha o nome do cliente.");
            RuleFor(cliente => cliente.DataNascimento).NotEmpty().WithMessage("Por favor, preencha a data de nascimento.");
            RuleFor(cliente => cliente.Cep).NotEmpty().WithMessage("Por favor, preencha o CEP.");
            RuleFor(cliente => cliente.Cep).Length(8,9).WithMessage("O CEP informado é inválido.");
            RuleFor(cliente => cliente.Cep).Matches(@"^[0-9]{5}-?[0-9]{3}$").WithMessage("O CEP informado é inválido.");
            RuleFor(cliente => cliente.Logradouro).NotEmpty().WithMessage("Não foi possível encontrar o logradouro através do CEP, por favor, informe esse campo manualmente.");
            RuleFor(cliente => cliente.Bairro).NotEmpty().WithMessage("Não foi possível encontrar o bairro através do CEP, por favor, informe esse campo manualmente.");
        }
    }
}
