using System.ComponentModel;
using Xamarin.Forms;
using hiroku.ViewModels;

namespace hiroku.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
