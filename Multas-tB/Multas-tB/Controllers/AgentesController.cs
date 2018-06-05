using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Multas_tB.Models;

namespace Multas_tB.Controllers {


   // [Authorize] // só pessoas autenticadas é que podem executar estes recursos
   [Authorize(Roles = "Agentes,GestaoDePessoal")]

   public class AgentesController : Controller {

      // cria uma variável que representa a Base de Dados
      private ApplicationDbContext db = new ApplicationDbContext();

      // GET: Agentes
      /// <summary>
      /// lista todos os agentes
      /// </summary>
      /// <returns></returns>
      [AllowAnonymous] // apesar de haver restrições de acesso,
      // um user anónimo consegue aceder
      public ActionResult Index() {
         // db.Agentes.ToList() -> em sql: SELECT * FROM Agentes;
         // enviar para a View uma lista com todos os Agentes, da BD


         //***********************************************************
         // recuperar os dados pessoais da pessoa que se autenticou
         //***********************************************************

         //var dadosPessoais = db.Users.Find(User.Identity.GetUserId());
         //// agora, com este objeto, já posso utilizar
         //// os dados pessoais de um utilizador no meu programa
         //// por exemplo:
         //Session["nomeUtilizador"] = dadosPessoais.NomeProprio
         //                            + " "
         //                            + dadosPessoais.Apelido;

         //var dadosPessoais = db.Utilizadores
         //                     .Where(u => u.NomeRegistoDoUtilizador
         //                                  .Equals(User.Identity.Name))
         //                     .FirstOrDefault();
         // agora, com este objeto, já posso utilizar
         // os dados pessoais de um utilizador no meu programa
         // por exemplo:
         //Session["nomeUtilizador"] = dadosPessoais.NomeProprio
         //                            + " "
         //                            + dadosPessoais.Apelido;








         // obter a lista de todos os agentes
         // em SQL:  SELECT * FROM Agentes ORDER BY Nome;
         var listaDeAgentes = db.Agentes.OrderBy(a => a.Nome).ToList();

         return View(listaDeAgentes);
         //  return View();
      }

      // GET: Agentes/Details/5
      /// <summary>
      /// 
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public ActionResult Details(int? id) {
         // se se escrever 'int?' é possível
         // não fornecer o valor para o ID e não há erro

         // proteção para o caso de não ter sido fornecido um ID válido
         if(id == null) {
            // instrução original
            // devolve um erro qd não há ID
            // logo, não é possível pesquisar por um Agente
            // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // redirecionar para uma página que nós controlamos
            return RedirectToAction("Index");
         }

         // procura na BD, o Agente cujo ID foi fornecido
         Agentes agente = db.Agentes.Find(id);

         // proteção para o caso de não ter sido encontrado qq Agente
         // que tenha o ID fornecido
         if(agente == null) {
            // o agente não foi encontrado
            // logo, gera-se uma msg de erro
            // return HttpNotFound();

            // redirecionar para uma página que nós controlamos
            return RedirectToAction("Index");
         }



         /// falta, aqui, fazer a mesma proteção 
         /// que foi feita no GET do método Edit



         // entrega à View os dados do Agente encontrado
         return View(agente);
      }



      // GET: Agentes/Create
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      [Authorize(Roles = "GestaoDePessoal")]
      public ActionResult Create() {
         // apresenta a View para se inserir um novo Agente
         return View();
      }




      // POST: Agentes/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      /// <summary>
      /// 
      /// </summary>
      /// <param name="agente"></param>
      /// <param name="uploadFotografia"></param>
      /// <returns></returns>
      // anotador para HTTP Post 
      [HttpPost]
      // anotador para proteção por roubo de identidade
      [ValidateAntiForgeryToken]
      [Authorize(Roles = "GestaoDePessoal")]
      public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase uploadFotografia) {
         // escrever os dados de um novo Agente na BD 

         // especificar o ID do novo Agente
         // testar se há registos na tabela dos Agentes
         // if (db.Agentes.Count()!=0){ }

         // ou então, usar a instrução TRY/CATCH
         int idNovoAgente = 0;
         try {
            idNovoAgente = db.Agentes.Max(a => a.ID) + 1;
         }
         catch(Exception) {
            idNovoAgente = 1;
         }

         // guardar o ID do novo Agente
         agente.ID = idNovoAgente;

         // especificar (escolher) o nome do ficheiro
         string nomeImagem = "Agente_" + idNovoAgente + ".jpg";

         // var. auxiliar
         string path = "";

         // validar se a imagem foi fornecida
         if(uploadFotografia != null) {
            // o ficheiro foi fornecido
            // validar se o q foi fornecido é uma imagem ----> fazer em casa
            // formatar o tamanho da imagem

            // criar o caminho completo até ao sítio onde o ficheiro
            // será guardado
            path = Path.Combine(Server.MapPath("~/imagens/"), nomeImagem);

            // guardar o nome do ficheiro na BD
            agente.Fotografia = nomeImagem;
         }
         else {
            // não foi fornecido qq ficheiro
            // gerar uma mensagem de erro
            ModelState.AddModelError("", "Não foi fornecida uma imagem...");
            // devolver o controlo à View
            return View(agente);
         }

         // ModelState.IsValid -> confronta os dados fornecidos da View
         //                       com as exigências do Modelo
         if(ModelState.IsValid) {
            try {
               // adiciona o novo Agente à BD
               db.Agentes.Add(agente);
               // faz 'Commit' às alterações
               db.SaveChanges();
               // escrever o ficheiro com a fotografia no disco rígido, na pasta 'imagens'
               uploadFotografia.SaveAs(path);

               // se tudo correr bem, redireciona para a página de Index
               return RedirectToAction("Index");
            }
            catch(Exception ex) {
               ModelState.AddModelError("", "Houve um erro com a criação do novo Agente...");

               /// se existir uma classe chamada 'Erro.cs'
               /// iremos nela registar os dados do erro
               /// - criar um objeto desta classe
               /// - atribuir a esse objeto os dados do erro
               ///   - nome do controller
               ///   - nome do método
               ///   - data + hora do erro
               ///   - mensagem do erro (ex)
               ///   - dados que se tentavam inserir
               ///   - outros dados considerados relevante
               /// - guardar o objeto na BD
               /// 
               /// - notificar um GESTOR do sistema, por email,
               ///   ou por outro meio, da ocorrência do erro 
               ///   e dos seus dados              

            }
         }

         // se houver um erro, 
         // reapresenta os dados do Agente na View
         return View(agente);
      }





      // GET: Agentes/Edit/5
      /// <summary>
      /// apresentar na View os dados de um agente,
      /// para eventual edição
      /// </summary>
      /// <param name="id"> identifica o agente a editar</param>
      /// <returns>view</returns>
      public ActionResult Edit(int? id) {
         // se se escrever 'int?' é possível
         // não fornecer o valor para o ID e não há erro

         // proteção para o caso de não ter sido fornecido um ID válido
         if(id == null) {
            // instrução original
            // devolve um erro qd não há ID
            // logo, não é possível pesquisar por um Agente
            // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // redirecionar para uma página que nós controlamos
            return RedirectToAction("Index");
         }

         // procura na BD, o Agente cujo ID foi fornecido
         Agentes agente = db.Agentes.Find(id);

         // proteção para o caso de não ter sido encontrado qq Agente
         // que tenha o ID fornecido
         if(agente == null) {
            // o agente não foi encontrado
            // logo, gera-se uma msg de erro
            // return HttpNotFound();

            // redirecionar para uma página que nós controlamos
            return RedirectToAction("Index");
         }

         // existe Agente
         // contudo, será que posso editá-lo?
         if(User.IsInRole("GestaoDePessoal") ||
            User.Identity.Name.Equals(agente.UserName)) {
            // entrega à View os dados do Agente encontrado
            return View(agente);
         }
         else {
            // não há permissão para editar o Agente
            return RedirectToAction("Index");
         }
      }





      // POST: Agentes/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      /// <summary>
      /// concretiza a edição dos dados de um agente
      /// </summary>
      /// <param name="agente"> dados do agente a alterar </param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia,UserName")] Agentes agente) {

      /// se o utilizador pertence à role 'GestaoDePessoal', 
      ///      posso efetuar a edição, sem qq restrição
      /// se o utilizador não pertence à role acima referida
      ///      e NÃO é o dono dos dados, NADA se pode fazer
      /// se o utilizador não pertence à role 
      ///      e É O DONO dos dados, apenas pode alterar o 'nome' e a 'fotografia'
      ///      TAREFAS:
      ///        1- pesquisar os dados antigos do Agente na BD
      ///        2- substituir nos dados novos, 
      ///           o valor da 'esquadra' pelo dados antigo da 'esquadra'
      ///        3- guardar dados na BD
      ///        
      ///        nota: claro que a validação do NOME e da FOTOGRAFIA também tem de acontecer






         if(ModelState.IsValid) {
            // neste caso já existe um Agente
            // apenas quero EDITAR os seus dados
            db.Entry(agente).State = EntityState.Modified;
            // efetuar 'Commit'
            db.SaveChanges();

            return RedirectToAction("Index");
         }
         return View(agente);
      }

      // GET: Agentes/Delete/5
      /// <summary>
      /// apresenta na View os dados de um agente,
      /// com vista à sua, eventual, eliminação
      /// </summary>
      /// <param name="id">identificador do Agente a apagar</param>
      /// <returns></returns>
      [Authorize(Roles = "GestaoDePessoal")]
      public ActionResult Delete(int? id) {

         // verificar se foi fornecido um ID válido
         if(id == null) {
            return RedirectToAction("Index");
         }

         // pesquisar pelo Agente, cujo ID foi fornecido
         Agentes agente = db.Agentes.Find(id);

         // verificar se o Agente foi encontrado
         if(agente == null) {
            // o Agente não existe
            // redirecionar para a página inicial
            return RedirectToAction("Index");
         }

         // apresentar os dados na View
         return View(agente);
      }





      // POST: Agentes/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      [Authorize(Roles = "GestaoDePessoal")]
      public ActionResult DeleteConfirmed(int id) {
         Agentes agente = db.Agentes.Find(id);
         try {
            // remove o Agente da BD



            db.Agentes.Remove(agente);
            // Commit
            db.SaveChanges();
            return RedirectToAction("Index");

         }
         catch(Exception ex) {
            ModelState.AddModelError("", string.Format("Não é possível apagar o Agente nº {0} - {1}, porque há multas associadas a ele...",
                                         id, agente.Nome)
            );
         }
         // se cheguei aqui é pq houve um problema
         // devolvo os dados do Agente à View
         return View(agente);
      }

      protected override void Dispose(bool disposing) {
         if(disposing) {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
