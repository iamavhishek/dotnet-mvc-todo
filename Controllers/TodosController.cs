using FinalTodoApp.Data;
using FinalTodoApp.Models;
using FinalTodoApp.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalTodoApp.Controllers
{
    [Authorize]
    public class TodosController : Controller
    {
        private readonly MVCDbContext mvcc;

        public TodosController(MVCDbContext mvcc)
        {
            this.mvcc = mvcc;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var todos = await mvcc.Todos.ToListAsync();
            return View(todos);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel AddTodoRequest)
        {

            var todo = new Todo()
            {
                Id = Guid.NewGuid(),
                Title = AddTodoRequest.Title,
                Description = AddTodoRequest.Description,
                Due = AddTodoRequest.Due.ToUniversalTime(),
                isCompleted = false
            };
            await mvcc.Todos.AddAsync(todo);
            await mvcc.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var todo = await mvcc.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo != null)
            {
                var viewModel = new UpdateTodoViewModel()
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    Due = todo.Due.ToUniversalTime(),
                    isCompleted = todo.isCompleted,
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateTodoViewModel model)
        {
            var todo = await mvcc.Todos.FindAsync(model.Id);
            if (todo != null)
            {
                todo.Title = model.Title;
                todo.Description = model.Description;
                todo.Due = model.Due.ToUniversalTime();
                todo.isCompleted = model.isCompleted;
                await mvcc.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateTodoViewModel model)
        {
            var todo = await mvcc.Todos.FindAsync(model.Id);
            if (todo != null)
            {
                mvcc.Todos.Remove(todo);
                await mvcc.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
