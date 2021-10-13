using System;
using System.Collections.Generic; 

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
