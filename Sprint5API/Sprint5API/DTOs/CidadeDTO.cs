using System;
using System.Collections.Generic;

namespace Sprint5API.DTOs
{
    public class CidadeDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public IList<ClienteDTO> Clientes { get; set; }
    }
}
