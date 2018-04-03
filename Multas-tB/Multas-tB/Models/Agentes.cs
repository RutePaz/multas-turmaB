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

      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório!")]
      [RegularExpression("[A-ZÍÉÂÁ][a-záéíóúàèìòùâêîôûäëïöüãõç]+(( |'|-| dos | da | de | e | d')[A-ZÍÉÂÁ][a-záéíóúàèìòùâêîôûäëïöüãõç]+){1,3}",
           ErrorMessage = "O {0} apenas pode conter letras e espaços em branco. Cada palavra começa em Maiúscula, seguida de minúsculas...")]
      public string Nome { get; set; }

      [Required(ErrorMessage = "A {0} é de preenchimento obrigatório!")]
      [RegularExpression("[A-ZÍÉÂÁ]*[a-záéíóúàèìòùâêîôûäëïöüãõç -]*", ErrorMessage ="A {0} só pode conter letras.")]
      public string Esquadra { get; set; }

      public string Fotografia { get; set; }

      // referência às multas q um Agente 'emite'
      public virtual ICollection<Multas> ListaDeMultas { get; set; }
   }
}