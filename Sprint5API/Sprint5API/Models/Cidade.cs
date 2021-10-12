using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sprint5API.Models
{
    public class Cidade
    {

        public Guid Id { get; set; } 
        public string Nome { get; set; } 
        public string Estado { get; set; }
        public List<Cliente> Clientes { get; set; }
    }
}
