using Microsoft.AspNetCore.Mvc;

namespace PrimeiraApi
{
    // P: O que faz a classe ser uma controller? 
    // R: Ela herda da classe ControllerBase
    [Route("/api/todo")] 
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public TodoController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        // P: O que faz um método ser uma action?
        // R: Anotação de Rota / Método(Verbo): []
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(appDbContext.Todos);
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var todo = appDbContext.Todos.Find(id);

            if (todo == null)
                return NotFound();
            
            return Ok(todo);
        }

        [HttpPost]
        public ActionResult Create([FromBody] TodoItem item)
        {
            appDbContext.Todos.Add(item);
            appDbContext.SaveChanges();

            return Create(item);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteById(int id)
        {
            var todo = appDbContext.Todos.Find(id);
            if (todo == null)
                return NotFound();

            appDbContext.Todos.Remove(todo);
            appDbContext.SaveChanges();

            return NoContent();
        }


        [HttpPut("{id}")]
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
