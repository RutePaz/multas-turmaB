using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas_tB.Models {
   public class Utilizadores {


      /// <summary>
      /// os atributos q aqui vão ser adicionados
      /// serão adicionados à tabela dos utilizadores
      /// </summary>

      [Key]
      public int ID { get; set; }

      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      [Display(Name = "Nome próprio")]
      public string NomeProprio { get; set; }
      public string Apelido { get; set; }
      public DateTime DataNascimento { get; set; }
      public string NIF { get; set; }

      //*********************************************************
      // o atributo seguinte vai criar uma chave forasteira
      // para a tabela da 'autenticação'
      //*********************************************************
      public string NomeRegistoDoUtilizador { get; set; }

   }
}
