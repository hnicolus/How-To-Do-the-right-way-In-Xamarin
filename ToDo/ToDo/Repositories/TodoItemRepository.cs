using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Repositories
{
    public class TodoItemRepository:ITodoItemRepository
    {

        private SQLiteAsyncConnection _connection;

        public event EventHandler<TodoItem> OnItemAdded;
        public event EventHandler<TodoItem> OnItemUpdated;
        public event EventHandler<TodoItem> OnItemDelete;

        private async Task CreateConnectionAsync()
        {
            if (_connection != null)
                return;
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var databasePath = Path.Combine(documentPath, "TodoItems.sqlite");

            _connection = new SQLiteAsyncConnection(databasePath);
            await _connection.CreateTableAsync<TodoItem>().ConfigureAwait(false);

            if (await _connection.Table<TodoItem>().CountAsync().ConfigureAwait(false) == 0)
            {
                await _connection.InsertAsync(new TodoItem()
                {
                    Title = "Welcome to Will Do"
                }).ConfigureAwait(false);
            }
        }
        public async Task AddItemAsync(TodoItem item)
        {
            await CreateConnectionAsync().ConfigureAwait(false);
            
            await _connection.InsertAsync(item).ConfigureAwait(false);
            
            OnItemAdded?.Invoke(this, item);
        }

        public async Task AddOrUpdateAsync(TodoItem item)
        {
            if (item.Id == 0)
                await AddItemAsync(item).ConfigureAwait(false);
            else
                await UpdateItemAsync(item).ConfigureAwait(false);
        }

        public async Task<List<TodoItem>> GetItemsAsync()
        {
            await CreateConnectionAsync().ConfigureAwait(false);
            
            return await _connection.Table<TodoItem>().ToListAsync().ConfigureAwait(false);
        }

        public async Task UpdateItemAsync(TodoItem item)
        {
            await CreateConnectionAsync().ConfigureAwait(false);
           
            await _connection.UpdateAsync(item).ConfigureAwait(false);
            
            OnItemUpdated?.Invoke(this, item);
        }

        public async Task DeleteAsync(int id)
        {
             await CreateConnectionAsync().ConfigureAwait(false);

            var item = await _connection.GetAsync<TodoItem>(id).ConfigureAwait(false);
            
            await _connection.DeleteAsync<TodoItem>(item).ConfigureAwait(false);
            
            OnItemDelete?.Invoke(this, item);
        }
    }
}
