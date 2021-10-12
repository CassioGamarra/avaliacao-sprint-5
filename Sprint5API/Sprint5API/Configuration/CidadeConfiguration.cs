using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sprint5API.Models;
using System;

namespace Sprint5API.Configuration
{
    public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ToTable("Cidades");
             
            builder.Property<Guid>("Id").IsRequired();
            builder.Property(cidade => cidade.Nome).IsRequired();
            builder.Property(cidade => cidade.Estado).IsRequired(); 
        } 
    }
}
