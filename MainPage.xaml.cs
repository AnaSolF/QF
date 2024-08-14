using QF.ViewModels;
namespace QF
{
    public partial class MainPage : ContentPage
    {

        public MainPage(CreatePartViewModel viewModel)
        {
            InitializeComponent();
            BindingContext=viewModel;
        }
        //Hacemos binding con CreatePartViewModel
        //Tambièn lo podemos trasladar a otra vista cuando agreguemos màs modelos, DTOs y views,
        //ya que esta linkeado con la pàgina de inicio, principal MainPage
        //Vamos a MauiProgram(Igual que hicimos anteriormente en el otro archivo xaml.cs)
    }

}
