using Microsoft.AspNetCore.Mvc;
using TodoListWebapi.Context;
using TodoListWebapi.Model;

namespace TodoListWebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context; //Add Database context
        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        //Create Todo Task
        [HttpPost("CreateTodoTask")]
        public async Task<IActionResult> CreateTask([FromBody] Todo todotask)
        {
            try
            {
                //Checks if request is null/empty
                if (todotask == null)
                {
                    return BadRequest("Task data is empty");
                }

                //Create new Task
                var task = new Todo
                {
                    TaskName = todotask.TaskName,
                    StartDate = todotask.StartDate,
                    EndDate = todotask.EndDate,
                    IsCompleted = false,
                    UserId = todotask.UserId
                };
                //Add and save data to the Database
                await _context.Todos.AddAsync(task);
                await _context.SaveChangesAsync();

                return Ok(task);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        //Get all Todo Task
        [HttpGet("GetAllTodoTasks")]
        public IActionResult RetrieveTodoLists()
        {
            //This initializes an empty `Todo` list to store the fetched tasks from the database.
            List<Todo> todos = new List<Todo>();
            try
            {
                todos = _context.Todos.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
            return Ok(todos);
        }

        //Get Task by User Id
        [HttpGet("GetUserTodoTasks/{userId}")]
        public IActionResult GetUserTodoTasks(string userId)
        {
            try
            {
                var todos = _context.Todos.Where(todo => todo.UserId == userId).ToList();
                return Ok(todos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        //Update Todo Task by Id
        [HttpPut("UpdateTodoTask/{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] Todo updateTodo)
        {
            try
            {
                //Find the existing task in the database
                var existingTask = await _context.Todos.FindAsync(taskId);

                //Checks if Task exist
                if (existingTask == null)
                {
                    return NotFound("Task not found");
                }

                // Update task properties
                existingTask.TaskName = updateTodo.TaskName;
                existingTask.StartDate = updateTodo.StartDate;
                existingTask.EndDate = updateTodo.EndDate;
                existingTask.IsCompleted = updateTodo.IsCompleted;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(existingTask);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        //Delete Todod Task by Id
        [HttpDelete("DeleteTodoTask/{taskId}")]
        public async Task<IActionResult> RemoveTask(int taskId)
        {
            try
            {
                //Find the task in the database
                var task = await _context.Todos.FindAsync(taskId);

                //Checks if task exist in the database
                if (task == null)
                {
                    return NotFound("Task not found");
                }

                // Remove the task from the database
                _context.Todos.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Poof! Task gone. Like it was never there." });


            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // Patch Todo Task IsCompleted Status
        [HttpPatch("UpdateIsCompleted/{taskId}")]
        public async Task<IActionResult> UpdateIsCompleted(int taskId, [FromBody] bool isCompleted)
        {
            try
            {
                // Find the existing task in the database
                var existingTask = await _context.Todos.FindAsync(taskId);

                // Check if task exists
                if (existingTask == null)
                {
                    return NotFound("Task not found");
                }

                // Update only the IsCompleted property
                existingTask.IsCompleted = isCompleted;

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(new { message = "Task completion status updated successfully", task = existingTask });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

    }
}
