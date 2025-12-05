using Microsoft.AspNetCore.Mvc;

namespace PrimeiraApi
{
    // P: O que faz a classe ser uma controller? 
    // R: Ela herda da classe ControllerBase
    [Route("/api/todo")]
    public class TodoController : ControllerBase
    {

        // P: O que faz um método ser uma action?
        // R: Anotação de Rota / Método(Verbo): []
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(SimulatedDatabase.Todos);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var mensagem = "Retorna os detalhes da tarefa com ID = " + id;
            return Ok(mensagem);
        }

        [HttpPost]
        public ActionResult Create(TodoItem item)
        {
            // item e adicionar o banco de dados.
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            return Ok();
        }


        [HttpPut("{id}")]
        public ActionResult Update(int id, TodoItem item)
        {
            return Ok();
        }
    }
}
