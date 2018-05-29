using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multas_tB.Models {
   // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

   /// <summary>
   /// classe para efetuar a geração de utilizadores
   /// </summary>
   public class ApplicationUser : IdentityUser {

      /// <summary>
      /// os atributos q aqui vão ser adicionados
      /// serão adicionados à tabela dos utilizadores
      /// </summary>
      public string NomeProprio { get; set; }
      public string Apelido { get; set; }
      public DateTime? DataNascimento { get; set; }
      public string NIF { get; set; }

      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
         // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
         var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
         // Add custom user claims here
         return userIdentity;
      }
   }



   /// <summary>
   /// classe responsável pela criação da base de dados
   
   
   
   ///   - da autenticação
   ///   - do 'negócio'
   /// </summary>
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
      public ApplicationDbContext()
          : base("MultasDBConnectionString", throwIfV1Schema: false) {
      }

      static ApplicationDbContext() {
         // Set the database intializer which is run once during application start
         // This seeds the database with admin user credentials and admin role
         Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
      }

      public static ApplicationDbContext Create() {
         return new ApplicationDbContext();
      }

      // vamos colocar, aqui, as instruções relativas às tabelas do 'negócio'
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
