using System; 

namespace Sprint5API.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid CidadeId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Cidade Cidade { get; set; }
    }
}
