using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Repositories
{
    public interface ITodoItemRepository
    {
     
        Task<List<TodoItem>> GetItemsAsync();
        Task AddItemAsync(TodoItem item);
        Task UpdateItemAsync(TodoItem item);
        Task AddOrUpdateAsync(TodoItem item);
        Task DeleteAsync(int id);

    }
}
