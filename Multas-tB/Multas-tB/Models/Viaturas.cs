using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas_tB.Models {
   public class Viaturas {

      public Viaturas() {
         ListaDeMultas = new HashSet<Multas>();
      }

      [Key]
      public int ID { get; set; } // primary key

      // dados de uma Viatura
      public string Matricula { get; set; }

      public string Marca { get; set; }

      public string Modelo { get; set; }

      public string Cor { get; set; }

      // dados do dono de uma Viatura
      public string NomeDono { get; set; }

      public string MoradaDono { get; set; }

      public string CodPostalDono { get; set; }

      // referência às multas q um condutor 'recebe' numa Viatura
      public virtual ICollection<Multas> ListaDeMultas { get; set; }

   }
}