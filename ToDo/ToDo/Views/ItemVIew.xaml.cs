using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemView : ContentPage
    {
        public ItemView(ItemViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;

        }
    }
}