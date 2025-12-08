using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;

namespace PrimeiraApi
{
    // P: O que faz a classe ser uma controller? 
    // R: Ela herda da classe ControllerBase
    [Route("/api/todo")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public TodoController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        // P: O que faz um método ser uma action?
        // R: Anotação de Rota / Método(Verbo): []
        /// <summary>
        /// Retorna uma lista com todas as tarefas
        /// </summary>
        /// <response code="200">Requisição bem Sucedida, retorna a lista de tarefas</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TodoItem>))]
        public ActionResult GetAll()
        {
            return Ok(appDbContext.Todos);
        }

        /// <summary>
        /// Retorna os dados detalhados de uma Todo Item em particular.
        /// </summary>
        /// <param name="id">É o ID que identifica a Todo Item cujos dados devem ser retornados.</param>
        /// <response code="404">A tarefa com o ID especificado não foi encontrada.</response>
        /// <response code="200">A tarefa foi encontrada e seus dados foram retornados com sucesso.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoItem))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
        public ActionResult GetById(int id)
        {
            var todo = appDbContext.Todos.Find(id);

            if (todo == null)
                return NotFound();
            
            return Ok(todo);
        }

        /// <summary>
        /// Cria uma nova tarefa com os dados especificados.
        /// </summary>
        /// <param name="item">Dados da Tarefa a serem criados</param>
        /// <response code="409">Tarefa com o ID especificado já existe.</response>
        /// <response code="201">Tarefa foi criada com sucesso.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoItem))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(void))]
        public ActionResult Create([FromBody] TodoItem item)
        {
            var existing = appDbContext.Todos.Find(item.Id);
            if (existing != null)
                return Conflict();

            appDbContext.Todos.Add(item);
            appDbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        /// <summary>
        /// Remove o Todo Item com o Id especificado
        /// </summary>
        /// <param name="id">Id da tarefa a ser removida.</param>
        /// <response code="204">Tarefa removida com sucesso.</response>
        /// <response code="404">Tarefa não encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(void))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
        public ActionResult DeleteById(int id)
        {
            var todo = appDbContext.Todos.Find(id);
            if (todo == null)
                return NotFound();

            appDbContext.Todos.Remove(todo);
            appDbContext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Atualiza os dados de um Todo Item
        /// </summary>
        /// <param name="id">Id do Todo Item a ser atualizado</param>
        /// <param name="item">Novos dados do Todo Item</param>
        /// <response code="400">Os ids não correspondem</response>
        /// <response code="404">A tarefa não foi encontrada</response>
        /// <response code="204">Atualização feita com sucesso.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(void))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(void))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
        public ActionResult Update(int id, [FromBody] TodoItem item)
        {
            if (id != item.Id)
                return BadRequest();

            var existing = appDbContext.Todos.Find(id);
            if (existing == null)
                return NotFound();

            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.IsFinished = item.IsFinished;

            appDbContext.Todos.Update(existing);
            appDbContext.SaveChanges();

            return NoContent();
        }
    }
}
