using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoListData;
using TodoListModels;

namespace FTC2022_MakingAListAndCheckingItTwice.Controllers
{
    public class TodoListController : Controller
    {
        private readonly TodoListDataContext _context;

        public TodoListController(TodoListDataContext context)
        {
            _context = context;
        }

        // TODO: Utilize this method to get the current user id:
        // protected async Task<string> GetCurrentUserId()
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     return userId;
        // }

        // GET: TodoList
        public async Task<IActionResult> Index()
        {
            return _context.ToDoItems != null ?
                        View(await _context.ToDoItems.ToListAsync()) :
                        Problem("Entity set 'TodoListDataContext.ToDoItems'  is null.");
        }

        public async Task<IActionResult> AdminIndex()
        {
              return _context.ToDoItems != null ? 
                          View(await _context.ToDoItems.ToListAsync()) :
                          Problem("Entity set 'TodoListDataContext.ToDoItems'  is null.");
        }

        // GET: TodoList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var todoListItem = await _context.ToDoItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoListItem == null)
            {
                return NotFound();
            }

            return View(todoListItem);
        }

        // GET: TodoList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DetailText,IsCompleted,Status")] TodoListItem todoListItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoListItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoListItem);
        }

        // GET: TodoList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var todoListItem = await _context.ToDoItems.FindAsync(id);
            if (todoListItem == null)
            {
                return NotFound();
            }
            return View(todoListItem);
        }

        // POST: TodoList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DetailText,IsCompleted,Status")] TodoListItem todoListItem)
        {
            if (id != todoListItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoListItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoListItemExists(todoListItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoListItem);
        }

        // GET: TodoList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDoItems == null)
            {
                return NotFound();
            }

            var todoListItem = await _context.ToDoItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoListItem == null)
            {
                return NotFound();
            }

            return View(todoListItem);
        }

        // POST: TodoList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoItems == null)
            {
                return Problem("Entity set 'TodoListDataContext.ToDoItems'  is null.");
            }
            var todoListItem = await _context.ToDoItems.FindAsync(id);
            if (todoListItem != null)
            {
                _context.ToDoItems.Remove(todoListItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoListItemExists(int id)
        {
          return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /**************** Ajax Endpoints ************************/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTodoListItemStatus(string detailId, string newStatus)
        {
            var success = int.TryParse(detailId, out int todoId);
            if (!success)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("Todo Item Not Found!", MediaTypeNames.Text.Plain);
            }

            var item = await _context.ToDoItems.SingleOrDefaultAsync(x => x.Id == todoId);
            if (item is null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("Todo Item Not Found!", MediaTypeNames.Text.Plain);
            }

            success = int.TryParse(newStatus, out int updStatus);
            if (!success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Invalid Status", MediaTypeNames.Text.Plain);
            }

            var newItemStatus = (ItemStatus)updStatus;
            item.Status = newItemStatus;
            switch (newItemStatus)
            {
                case ItemStatus.NotStarted:
                case ItemStatus.InProgress:
                case ItemStatus.OnHold:
                    item.IsCompleted = false;
                    break;
                case ItemStatus.Abandoned:
                case ItemStatus.Completed:
                    item.IsCompleted = true;
                    break;
                default:
                    break;
            }
            await _context.SaveChangesAsync();
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("Todo Item Updated!", MediaTypeNames.Text.Plain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTodoListItem(string detailId, string detailText)
        {
            var success = int.TryParse(detailId, out int todoId);
            if (!success)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("Todo Item Not Found!", MediaTypeNames.Text.Plain);
            }

            var item = await _context.ToDoItems.SingleOrDefaultAsync(x => x.Id == todoId);
            if (item is null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("Record Not Found!", MediaTypeNames.Text.Plain);
            }

            item.DetailText = detailText;
            await _context.SaveChangesAsync();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("Record Updated!", MediaTypeNames.Text.Plain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTodoListItem(string text, string status)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Invalid Details Text", MediaTypeNames.Text.Plain);
            }
            bool success = int.TryParse(status, out int updStatus);
            if (!success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Invalid Status", MediaTypeNames.Text.Plain);
            }

            var item = new TodoListItem();
            item.DetailText = text;

            var newItemStatus = (ItemStatus)updStatus;
            item.Status = newItemStatus;

            switch (newItemStatus)
            {
                case ItemStatus.NotStarted:
                case ItemStatus.InProgress:
                case ItemStatus.OnHold:
                    item.IsCompleted = false;
                    break;
                case ItemStatus.Abandoned:
                case ItemStatus.Completed:
                    item.IsCompleted = true;
                    break;
                default:
                    break;
            }

            //TODO: Prevent Duplicates?

            await _context.ToDoItems.AddAsync(item);
            await _context.SaveChangesAsync();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("Todo Item Added!", MediaTypeNames.Text.Plain);
        }
    }
}
