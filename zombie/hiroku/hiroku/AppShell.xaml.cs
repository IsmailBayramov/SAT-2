using System;
using System.Collections.Generic;
using hiroku.ViewModels;
using hiroku.Views;
using Xamarin.Forms;

namespace hiroku
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}

