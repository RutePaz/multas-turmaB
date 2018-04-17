using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Multas_tB.Models {
   public class MultasDb : DbContext {

      // construtor q indica qual a base de dados a utilizar
      public MultasDb() : base("name=MultasDBConnectionString") { }
      
      // descrever os nomes das tabelas na Base de Dados
      public virtual DbSet<Multas> Multas { get; set; } // tabela Multas
      public virtual DbSet<Condutores> Condutores { get; set; } // tabela Condutores
      public virtual DbSet<Agentes> Agentes { get; set; } // tabela Agentes
      public virtual DbSet<Viaturas> Viaturas { get; set; } // tabela Viaturas

      protected override void OnModelCreating(DbModelBuilder modelBuilder) {

         modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
         modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

         base.OnModelCreating(modelBuilder);
      }

   }
}