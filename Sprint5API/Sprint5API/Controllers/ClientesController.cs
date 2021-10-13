using Microsoft.AspNetCore.Mvc;
using Sprint5API.Data;
using Sprint5API.DTOs;
using Sprint5API.Models;
using Sprint5API.Validators;
using System;
using System.Collections.Generic;
using FluentValidation.Results;
using System.Linq;
using AutoMapper;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks; 

namespace Sprint5API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private DatabaseContext _context;
        private IMapper _mapper;

        public ClientesController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] ClienteDTO clienteDTO)
        {
            var viaCepValidator = new ViaCepValidator();
            ValidationResult resultado = viaCepValidator.Validate(clienteDTO);
            IList<ValidationFailure> erros = resultado.Errors;

            if (!resultado.IsValid)
            {
                return BadRequest(erros);
            }

            ViaCepDTO viaCepDTO = await BuscarCidadePorCep(clienteDTO.Cep);

            if(viaCepDTO != null)
            {
                CidadesController cidadesController = new CidadesController(_context, _mapper);
                CidadeDTO cidadeDTO = cidadesController.BuscarPorCidadeEstado(viaCepDTO.localidade, viaCepDTO.uf);

                clienteDTO.Cep = clienteDTO.Cep.Replace("-", "").Trim();

                if (!string.IsNullOrEmpty(viaCepDTO.logradouro))
                {
                    clienteDTO.Logradouro = viaCepDTO.logradouro;
                }

                if (!string.IsNullOrEmpty(viaCepDTO.bairro))
                {
                    clienteDTO.Bairro = viaCepDTO.bairro;
                }

                clienteDTO.DataNascimento = DateTime.Parse(clienteDTO.DataNascimento.ToString());

                var clientValidator = new ClienteValidator();
                resultado = clientValidator.Validate(clienteDTO);
                erros = resultado.Errors;
                if (!resultado.IsValid)
                {
                    return BadRequest(erros);
                }
                 
                Cidade cidade = _mapper.Map<Cidade>(cidadeDTO);

                if (cidadeDTO == null)
                { 
                    cidadeDTO = new CidadeDTO();  
                    cidadeDTO.Nome = viaCepDTO.localidade;
                    cidadeDTO.Estado = viaCepDTO.uf;

                    cidade = _mapper.Map<Cidade>(cidadeDTO);

                    _context.Cidades.Add(cidade);
                    _context.SaveChanges();
                }
                clienteDTO.CidadeId = cidade.Id; 

                Cliente cliente = _mapper.Map<Cliente>(clienteDTO);

                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                 
                return CreatedAtAction(nameof(BuscarPorId), new { Id = cliente.Id }, cliente);
            }
            return NotFound("Ocorreu um erro ao pesquisar o CEP");
        }

        [HttpGet]
        public IActionResult BuscarTodos()
        { 
            List<ClienteDTO> clienteDTO = new List<ClienteDTO>();
            foreach (Cliente cliente in _context.Clientes)
            {
                clienteDTO.Add(_mapper.Map<ClienteDTO>(cliente));
            }

            return Ok(clienteDTO);
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            Cliente cliente = _context.Clientes.FirstOrDefault(cliente => cliente.Id == id);

            if(cliente != null)
            {
                ClienteDTO clienteDTO = _mapper.Map<ClienteDTO>(cliente);
                return Ok(cliente);
            }

            return NotFound(new { Mensagem = "Cliente não encontrado(a)!", Erro = true });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ClienteDTO clienteDTO)
        {  
            Cliente cliente = _context.Clientes.FirstOrDefault(cliente => cliente.Id == id);
              
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado!");
            }

            if (cliente.Nome == clienteDTO.Nome && cliente.DataNascimento == clienteDTO.DataNascimento && cliente.Cep == clienteDTO.Cep && cliente.Logradouro == clienteDTO.Logradouro && cliente.Bairro == clienteDTO.Bairro)
            {
                return Ok("Nenhuma alteração realizada!");
            } 

            ViaCepDTO viaCepDTO = await BuscarCidadePorCep(clienteDTO.Cep); 

            if (viaCepDTO != null)
            {
                CidadesController cidadesController = new CidadesController(_context, _mapper);
                CidadeDTO cidadeDTO = cidadesController.BuscarPorCidadeEstado(viaCepDTO.localidade, viaCepDTO.uf);

                if (!string.IsNullOrEmpty(viaCepDTO.logradouro))
                {
                    clienteDTO.Logradouro = viaCepDTO.logradouro;
                }

                if (!string.IsNullOrEmpty(viaCepDTO.bairro))
                {
                    clienteDTO.Bairro = viaCepDTO.bairro;
                }

                var clientValidator = new ClienteValidator();
                ValidationResult resultado = clientValidator.Validate(clienteDTO);
                IList<ValidationFailure> erros = resultado.Errors;
                if (!resultado.IsValid)
                {
                    return BadRequest(erros);
                }


                Cidade cidade = _mapper.Map<Cidade>(cidadeDTO);

                if (cidadeDTO == null)
                { 
                    cidadeDTO = new CidadeDTO();  
                    cidadeDTO.Nome = viaCepDTO.localidade;
                    cidadeDTO.Estado = viaCepDTO.uf;

                    cidade = _mapper.Map<Cidade>(cidadeDTO);

                    _context.Cidades.Add(cidade);
                    _context.SaveChanges();
                } 

                clienteDTO.Id = cliente.Id;
                clienteDTO.Cidade = cliente.Cidade;
                clienteDTO.CidadeId = cidade.Id;

                _mapper.Map(clienteDTO, cliente);
                _context.Update(cliente); 
                _context.SaveChanges();

                return Ok("Cliente atualizado(a) com sucesso!"); ;
            }
            return NotFound("Ocorreu um erro ao pesquisar o CEP");
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(Guid id)
        {
            Cliente cliente = _context.Clientes.FirstOrDefault(cliente => cliente.Id == id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado!");
            } 
            _context.Remove(cliente);
            _context.SaveChanges();
            return Ok("Cliente removido(a) com sucesso!");
        }
         
        public async Task<ViaCepDTO> BuscarCidadePorCep(string cep)
        {
            //TODO
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://viacep.com.br/ws/");

            HttpResponseMessage resposta = await httpClient.GetAsync($"{cep}/json");
            
            if(resposta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViaCepDTO viaCepDTO = JsonConvert.DeserializeObject<ViaCepDTO>(await resposta.Content.ReadAsStringAsync());
                
                if(viaCepDTO.erro == true)
                {
                    return null;
                }
                
                return viaCepDTO;
            }
            return null;
        }

    }
}
