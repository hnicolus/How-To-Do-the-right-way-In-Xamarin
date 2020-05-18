using System;
using System.Windows.Input;
using Xamarin.Forms;
using ToDo.Models;

namespace ToDo.ViewModels
{
    public class TodoItemViewModel:BaseViewModel
    {

        public TodoItemViewModel(TodoItem item) => Item = item;

        public event EventHandler ItemStatusChanged;
        public TodoItem Item { get; private set; }

        /// <summary>
        ///  command to toggle the status of the item and a piece of text that describes the status
        /// </summary>
        public ICommand ToggleCompleted => new Command((arg) =>
        {
            Item.Completed = !Item.Completed;
            ItemStatusChanged?.Invoke(this, new EventArgs());
        });
        public string StatusText => Item.Completed ? "Reactivate" : "Completed";



    }
}
