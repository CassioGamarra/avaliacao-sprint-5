﻿using System;
namespace Sprint5API.DTOs
{
    public class ClienteDTO
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
    }
}
