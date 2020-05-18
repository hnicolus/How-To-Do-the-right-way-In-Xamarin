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
            await Navigation.PushAsync(itemView);
        });

        public string FilterText => ShowAll ? "All" : "Active";
        public ICommand ToggleFilter => new Command(async () =>
        {
            ShowAll = !ShowAll;
            await LoadData();
        });
        public TodoItemViewModel SelectedItem
        {
            get
            {
                return null;
            }
            set
            {
                Device.BeginInvokeOnMainThread(async () => await NavigateToItem(value));
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        private async Task NavigateToItem(TodoItemViewModel item)
        {
            if (item == null)
                return;

            var itemView = Resolver.Resolve<ItemView>();
            var vm = itemView.BindingContext as ItemViewModel;
            vm.Item = item.Item;

            await Navigation.PushAsync(itemView);
        }
        public bool ShowAll { get; set; }
        public ObservableCollection<TodoItemViewModel> Items { get; set; }
        public MainViewModel(TodoItemRepository repository)
        {

            repository.OnItemAdded += (sender, item) =>
                Items.Add(CreateTodoItemViewModel(item));

            repository.OnItemUpdated += (sender, item) =>
                Task.Run(async () => await LoadData());


            _repository = repository;
    
            Task.Run(async () => await LoadData());

        }

        private async Task LoadData()
        {
            var items = await _repository.GetItems();

            if (!ShowAll)
            {
                items = items.Where(x => x.Completed == false).ToList();
            }
            var itemViewModels = items.Select(i =>
            CreateTodoItemViewModel(i));

            Items = new ObservableCollection<TodoItemViewModel>
            (itemViewModels);
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
                Task.Run(() => 
                _repository.UpdateItem(item.Item));
            }
        }
    }
}
