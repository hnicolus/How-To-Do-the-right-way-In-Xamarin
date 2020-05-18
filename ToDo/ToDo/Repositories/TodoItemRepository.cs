using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Repositories
{
    public class TodoItemRepository
    {

        private SQLiteAsyncConnection _connection;

        public event EventHandler<TodoItem> OnItemAdded;
        public event EventHandler<TodoItem> OnItemUpdated;

        private async Task CreateConnection()
        {
            if (_connection != null)
                return;
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var databasePath = Path.Combine(documentPath, "TodoItems.sqlite");

            _connection = new SQLiteAsyncConnection(databasePath);
            await _connection.CreateTableAsync<TodoItem>();

            if (await _connection.Table<TodoItem>().CountAsync() == 0)
            {
                await _connection.InsertAsync(new TodoItem()
                {
                    Title = "Welcome to DoToo"
                });
            }
        }
        public async Task AddItem(TodoItem item)
        {
            await CreateConnection();
            await _connection.InsertAsync(item);
            OnItemAdded?.Invoke(this, item);
        }

        public async Task AddOrUpdate(TodoItem item)
        {
            if (item.Id == 0)
            {
                await AddItem(item);
            }
            else
            {
                await UpdateItem(item);
            }
        }

        public async Task<List<TodoItem>> GetItems()
        {
            await CreateConnection();
            return await _connection.Table<TodoItem>().ToListAsync();
        }

        public async Task UpdateItem(TodoItem item)
        {
            await CreateConnection();
            await _connection.UpdateAsync(item);
            OnItemUpdated?.Invoke(this, item);
        }

    }
}
