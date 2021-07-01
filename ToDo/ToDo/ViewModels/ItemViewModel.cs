using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Models;
using ToDo.Repositories;
using Xamarin.Forms;

namespace ToDo.ViewModels
{
    public class ItemViewModel :BaseViewModel
    {
        private readonly TodoItemRepository _repository;

        public TodoItem Item { get; set; }
        
        public ItemViewModel(TodoItemRepository repository)
        {
            _repository = repository;
            Item = new TodoItem { Due = DateTime.Now.AddDays(1) };
            
        }

        public ICommand Save => new Command(async () =>
        {
            await _repository.AddOrUpdateAsync(Item).ConfigureAwait(false);
            await Navigation.PopAsync().ConfigureAwait(false);
        });
    }
}
