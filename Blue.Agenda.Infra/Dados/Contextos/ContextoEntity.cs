using Blue.Core.Entidades.Entidades.BLUE;
using Blue.Agenda.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Blue.Agenda.Infra.Dados.Contextos
{
    public class ContextoEntity : DbContext
    {
        public ContextoEntity(DbContextOptions<ContextoEntity> opcoes) : base(opcoes)
        {            
        }        
        //Exemplo de como adicionar entidade no Entity        
        public DbSet<TipoErro> TipoErro { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
