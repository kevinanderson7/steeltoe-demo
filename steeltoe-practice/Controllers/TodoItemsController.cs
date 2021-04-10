using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using steeltoe_practice.Models;

namespace steeltoe_practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly Models.TodoContext _context;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(Models.TodoContext context, ILogger<TodoItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            if (id == 0)
            {
                var newItem = new TodoItem()
                {
                    IsComplete = false,
                    Name = "A new auto-generated todo item"
                };
                _context.TodoItems.Add(newItem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Super secret id==0 was provided, so a new item was auto-added.");
                return null;
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
    }
}
