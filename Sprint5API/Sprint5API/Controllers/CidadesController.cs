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
using Microsoft.EntityFrameworkCore;

namespace Sprint5API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadesController : ControllerBase
    {

        private DatabaseContext _context;
        private IMapper _mapper;

        public CidadesController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] CidadeDTO cidadeDTO)
        {
            var cidadeValidator = new CidadeValidator();
            ValidationResult result = cidadeValidator.Validate(cidadeDTO);
            IList<ValidationFailure> erros = result.Errors;
            if (!result.IsValid) 
            {
                return BadRequest(erros);
            }


            CidadeDTO cidadeCadastrada = BuscarPorCidadeEstado(cidadeDTO.Nome, cidadeDTO.Estado);
            if (cidadeCadastrada == null) 
            {
                Cidade cidade = _mapper.Map<Cidade>(cidadeDTO); 
                _context.Cidades.Add(cidade);
                _context.SaveChanges();
                return CreatedAtAction(nameof(BuscarPorId), new { Id = cidade.Id }, cidade);
            }
            return BadRequest( new { Mensagem = "Cidade já cadastrada!", Erro = true });
            
        }

        [HttpGet]
        public IActionResult BuscarTodos()
        {
            List<CidadeDTO> cidadeDTO = new List<CidadeDTO>();
            var cidades = _context.Cidades.Include("Clientes").ToList();
            foreach (Cidade cidade in cidades)
            { 
                cidadeDTO.Add(_mapper.Map<CidadeDTO>(cidade));
            }

            return Ok(cidadeDTO);
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(Guid id)
        {
            Cidade cidade = _context.Cidades.Include("Clientes").FirstOrDefault(cidade => cidade.Id == id);

            if (cidade != null) 
            {
                CidadeDTO cidadeDTO = _mapper.Map<CidadeDTO>(cidade);
                return Ok(cidadeDTO);
            }

            return NotFound(new { Mensagem = "Cidade não encontrada!", Erro = true });
        }

        /*
         Verifica se já existe a cidade informada por CEP
         */
        public CidadeDTO BuscarPorCidadeEstado(string nome, string estado)
        {
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Nome == nome && cidade.Estado == estado);
            if (cidade != null)
            {
                CidadeDTO cidadeDTO = _mapper.Map<CidadeDTO>(cidade);
                return cidadeDTO;
            }
            return null;
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(Guid id, [FromBody] CidadeDTO cidadeDTO)
        {
            var cidadeValidator = new CidadeValidator();
            ValidationResult resultado = cidadeValidator.Validate(cidadeDTO);
            IList<ValidationFailure> erros = resultado.Errors;
            if(!resultado.IsValid)
            {
                return BadRequest(erros);
            }


            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Id == id);
            if (cidade == null) 
            {
                return NotFound();   
            }

            if(cidadeDTO.Nome == cidade.Nome && cidadeDTO.Estado == cidade.Estado)
            {
                return Ok("Nenhuma alteração realizada!");
            }

            CidadeDTO cidadeCadastrada = BuscarPorCidadeEstado(cidadeDTO.Nome, cidadeDTO.Estado);

            if (cidadeCadastrada != null)
            {
                return BadRequest("Cidade já cadastrada!");
            }
            cidadeDTO.Id = cidade.Id;        
            _mapper.Map(cidadeDTO, cidade);
            _context.Update(cidade);
            _context.SaveChanges();
            return Ok("Atualizado com sucesso!");

        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(Guid id)
        {
            Cidade cidade = _context.Cidades.FirstOrDefault(cidade => cidade.Id == id);
            if(cidade == null)
            {
                return NotFound("Cidade não encontrada!");
            }

            try
            {
                _context.Remove(cidade);
                _context.SaveChanges();

                return Ok("Cidade removida com sucesso!");
            } catch(Exception e)
            { 
                return BadRequest("Ocorreu um erro ao excluir a cidade");
            } 
        }
    }
}
