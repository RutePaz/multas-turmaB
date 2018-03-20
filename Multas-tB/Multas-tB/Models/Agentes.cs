using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas_tB.Models {
   public class Agentes {

      public Agentes() {
         ListaDeMultas = new HashSet<Multas>();
      }

      [Key]
      public int ID { get; set; }

      public string Nome { get; set; }

      public string Esquadra { get; set; }

      public string Fotografia { get; set; }

      // referência às multas q um Agente 'emite'
      public virtual ICollection<Multas> ListaDeMultas { get; set; }
   }
}