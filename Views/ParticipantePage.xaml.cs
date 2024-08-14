
using QF.ViewModels;
namespace QF.Views;

public partial class ParticipantePage : ContentPage
{
	public ParticipantePage(ParticipanteViewModel viewModel)
	{
		InitializeComponent();
        //Hacemos binding con ParticipanteViewModel
		//Ir hasta MauiProgram para agregar y que funcione
        BindingContext = viewModel;
	}
}