using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sprint5API.Models;
using System;

namespace Sprint5API.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");
             
            builder.Property<Guid>("Id").IsRequired();
            builder.Property(cliente => cliente.Nome).IsRequired();
            builder.Property(cliente => cliente.DataNascimento).IsRequired();
            builder.Property(cliente => cliente.CidadeId).IsRequired().HasColumnName("CidadeId");
            builder.Property(cliente => cliente.Cep).IsRequired();
            builder.Property(cliente => cliente.Logradouro).IsRequired();
            builder.Property(cliente => cliente.Bairro).IsRequired(); 

            builder.HasOne(cidade => cidade.Cidade)
                .WithMany(cliente => cliente.Clientes)
                .HasForeignKey(cliente => cliente.CidadeId)
                .HasPrincipalKey(cidade => cidade.Id);
        }
    }
}
