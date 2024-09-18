using MauiAppTask2CRUD.ViewModels;

namespace MauiAppTask2CRUD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }

}
