using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using ToDo.Repositories;
using ToDo.Views;
using System.Collections.ObjectModel;
using ToDo.Models;

namespace ToDo.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly TodoItemRepository _repository;

        public ICommand AddItem => new Command(async () =>
        {
            var itemView = Resolver.Resolve<ItemView>();
            await Navigation.PushAsync(itemView).ConfigureAwait(false);
        });

        public string FilterText => ShowAll ? "All" : "Active";
        public ICommand ToggleFilter => new Command(async () =>
        {
            ShowAll = !ShowAll;
            await LoadDataAsync().ConfigureAwait(false);
        });

        public TodoItemViewModel SelectedItem
        {
            get
            {
                return null;
            }
            set
            {
                Device.BeginInvokeOnMainThread(async () => await NavigateToItemAsync(value).ConfigureAwait(false));
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }
        public bool ShowAll { get; set; }
        public ObservableCollection<TodoItemViewModel> Items { get; set; }

        private async Task NavigateToItemAsync(TodoItemViewModel item)
        {
            if (item == null)
                return;

            var itemView = Resolver.Resolve<ItemView>();
            var vm = itemView.BindingContext as ItemViewModel;
            vm.Item = item.Item;

            await Navigation.PushAsync(itemView).ConfigureAwait(false);
        }

        public MainViewModel(TodoItemRepository repository)
        {

            //Added the new item to the view list
            repository.OnItemAdded += (sender, item) =>
                Items.Add(CreateTodoItemViewModel(item));

            repository.OnItemUpdated += (sender, item) =>
                Task.Run(async () => await LoadDataAsync().ConfigureAwait(false));

            repository.OnItemDelete += (sender, item) => 
                Task.Run(async () => await LoadDataAsync().ConfigureAwait(false));


            _repository = repository;
            Task.Run(async () => await LoadDataAsync().ConfigureAwait(false));

        }

        private async Task LoadDataAsync()
        {
            var items = await _repository.GetItemsAsync().ConfigureAwait(false);

            if (!ShowAll)
                items = items.Where(x => !x.Completed).ToList();

            var itemViewModels = items.Select(i => CreateTodoItemViewModel(i));

            Items = new ObservableCollection<TodoItemViewModel>(itemViewModels);
        }

        private TodoItemViewModel CreateTodoItemViewModel(TodoItem item)
        {
            var itemViewModel = new TodoItemViewModel(item);
            itemViewModel.ItemStatusChanged += ItemStatusChanged;
            return itemViewModel;
        }

        private void ItemStatusChanged(object sender, EventArgs e)
        {
            if(sender is TodoItemViewModel item)
            {
                if(!ShowAll && item.Item.Completed)
                {
                    Items.Remove(item);
                }
                Task.Run(() => _repository.UpdateItemAsync(item.Item));
            }
        }
    }
}
