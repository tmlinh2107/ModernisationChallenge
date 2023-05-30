using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernisationChallenge.DataAccess;
using ModernisationChallenge.Website.Models;
using ModernisationChallenge.Website.Models.Vm;
using Swashbuckle.AspNetCore.Annotations;
using TaskEntity = ModernisationChallenge.DataAccess.Task;

namespace ModernisationChallenge.Website.Controllers.Tasks;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/v1/tasks")]
public class TasksController : ControllerBase
{
    private readonly DataContext _context;

    public TasksController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get task by id", Description = "Get task by id")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound("The task not found");
        }

        var taskVm = new TaskVm(task);
        return Ok(taskVm);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(Summary = "Get all tasks", Description = "Get all tasks")]
    public async Task<IActionResult> ListTasksAsync()
    {
        var tasks = await _context.Tasks.ToListAsync();
        if (!tasks.Any())
        {
            return Ok(new TaskVm());
        }

        var tasksVm = tasks.Where(x => !x.DateDeleted.HasValue).OrderBy(x => x.DateCreated).Select(x => new TaskVm(x)).ToList();
        return Ok(tasksVm);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create a new task", Description = "Create a new task")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] CreateOrUpdateTaskRequest request)
    {
        var task = new TaskEntity()
        {
            Completed = false,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            Details = request.Detail,
        };

        _context.Tasks.Add(task);
        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            return CreatedAtAction(nameof(GetById), new { });
        }

        return BadRequest();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Update task", Description = "Update task")]
    public async Task<IActionResult> UpdateTaskAsync(int id, [FromBody] CreateOrUpdateTaskRequest request)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound("The task not found");
        }

        task.Details = request.Detail;
        task.DateModified = DateTime.UtcNow;

        _context.Tasks.Update(task);

        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            return NoContent();
        }

        return BadRequest();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Update task", Description = "Update task")]
    public async Task<IActionResult> DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound("The task not found");
        }

        task.DateDeleted = DateTime.UtcNow;

        _context.Tasks.Update(task);

        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            var taskVm = new TaskVm(task);
            return Ok(taskVm);
        }
        return BadRequest();
    }

    [HttpPut("{id}/completed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Update task is complated", Description = "Update task is complated")]
    public async Task<IActionResult> UpdateTaskCompletedAsync(int id, [FromBody] bool isCompleted)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound("The task not found");
        }

        task.Completed = isCompleted;
        task.DateModified = DateTime.UtcNow;

        _context.Tasks.Update(task);

        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            return Ok();
        }
        return BadRequest();
    }
}