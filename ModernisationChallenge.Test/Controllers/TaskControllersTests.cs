using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernisationChallenge.Website.Controllers.Tasks;
using ModernisationChallenge.Website.Models;
using ModernisationChallenge.Website.Models.Vm;
using Xunit;
using DataContext = ModernisationChallenge.DataAccess.DataContext;
using TaskEntity = ModernisationChallenge.DataAccess.Task;

namespace ModernisationChallenge.Website.Test.Controllers
{
    public class TaskControllersTests
    {
        private readonly TasksController _tasksController;
        private DataContext _context;

        public TaskControllersTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "ModernisationChallenge").Options;

            _context = new DataContext(options);

            _tasksController = new TasksController(_context);
        }

        private async Task InsertTasks()
        {
            _context.Tasks.AddRange(new List<TaskEntity>()
            {
                new TaskEntity
                {
                    Completed = false,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    Details = "Task 1"
                },
                new TaskEntity
                {
                    Completed = false,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    Details = "Task 2"
                }
            });

            _ = await _context.SaveChangesAsync();
        }

        [Fact]
        public async void ShouldCreateInstance_NotNull_Success()
        {
            await InsertTasks();
            var controller = new TasksController(_context);
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task Should_Return_ListTasks_Success()
        {
            await InsertTasks();
            var result = await _tasksController.ListTasksAsync();
            var okResult = result as OkObjectResult;
            var taskVm = okResult.Value as List<TaskVm>;
            Assert.NotNull(taskVm);
        }

        [Fact]
        public async Task Should_Return_GetTaskById_Success()
        {
            await InsertTasks();
            var result = await _tasksController.GetById(1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var taskVm = okResult.Value as TaskVm;
            Assert.Equal(1, taskVm.Id);
        }

        [Fact]
        public async Task Should_Return_GetTaskById_Failed()
        {
            var result = await _tasksController.GetById(123456);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Should_CreateTask_ValidInput_Success()
        {
            var result = await _tasksController.CreateTaskAsync(new CreateOrUpdateTaskRequest("Task 1"));
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task UpdateTask_ValidInput_Success()
        {
            var result = await _tasksController.UpdateTaskAsync(1, new CreateOrUpdateTaskRequest("Task 1852"));
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTask_ValidInput_Failed()
        {
            var result = await _tasksController.UpdateTaskAsync(3, new CreateOrUpdateTaskRequest("Task 1852"));
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}