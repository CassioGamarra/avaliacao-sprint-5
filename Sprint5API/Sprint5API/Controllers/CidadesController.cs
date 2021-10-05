using Microsoft.AspNetCore.Mvc;
using Sprint5API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprint5API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadesController : ControllerBase
    {
        [HttpPost]
        public void Adicionar([FromBody] Cidade cidade)
        {
            Console.WriteLine(cidade.Nome);
            Console.WriteLine(cidade.Estado);
        }

        [HttpGet]
        public void BuscarTodos()
        {

        }

        [HttpGet]
        public void BuscarPorId(Guid guid)
        {

        }

        [HttpPut]
        public void Editar(Guid guid)
        {

        }

        [HttpDelete]
        public void Excluir(Guid guid)
        {

        }
    }
}
