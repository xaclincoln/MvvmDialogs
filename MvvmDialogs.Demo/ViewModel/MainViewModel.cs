using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace MvvmDialogs.Demo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
		private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
		public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; }set { _Dialogs = value; } }

		public MainViewModel()
        {
			// any dialog boxes added in the constructor won't appear until DialogBehavior.DialogViewModels gets bound to the Dialogs collection.
        }

		public ICommand CloseAllCommand { get { return new RelayCommand(OnCloseAll); } }
		public void OnCloseAll()
		{
			this.Dialogs.Clear();
		}

		public ICommand NewModalDialogCommand { get { return new RelayCommand(OnNewModalDialog); } }
		public void OnNewModalDialog()
		{
			this.Dialogs.Add(new CustomDialogViewModel
			{
				Message = "Hello World!",
				Caption = "Modal Dialog Box",

				OnOk = (sender) =>
				{
					//sender.Close();
					new MessageBoxViewModel("You pressed ok!", "Bye bye!").Show(this.Dialogs);
				},

				OnCancel = (sender) =>
				{
					sender.Close();
					new MessageBoxViewModel("You pressed cancel!", "Bye bye!").Show(this.Dialogs);
				},

				OnCloseRequest = (sender) =>
				{
					//sender.Close();
					new MessageBoxViewModel("You clicked the caption bar close button!", "Bye bye!").Show(this.Dialogs);
				}
			});
		}

		public ICommand NewMinimalDialogCommand { get { return new RelayCommand(OnNewMinimalDialog); } }
		public void OnNewMinimalDialog()
		{
			this.Dialogs.Add(new MinimalDialogViewModel());
		}

		public ICommand NewModelessDialogCommand { get { return new RelayCommand(OnNewModelessDialog); } }
		public void OnNewModelessDialog()
		{
			var confirmClose = new Action<CustomDialogViewModel>((sender) =>
			{
				if (new MessageBoxViewModel {
						Caption = "Closing",
						Message = "Are you sure you want to close this window?",
						Buttons = MessageBoxButton.YesNo,
						Image = MessageBoxImage.Question}
					.Show(this.Dialogs) == MessageBoxResult.Yes)
						sender.Close();
			});

			new CustomDialogViewModel(false)
			{
				Message = "Hello World!",
				Caption = "Modeless Dialog Box",
				OnOk = confirmClose,
				OnCancel = confirmClose,
				OnCloseRequest = confirmClose
			}.Show(this.Dialogs);
		}

		public ICommand NewMessageBoxCommand { get { return new RelayCommand(OnNewMessageBox); } }
		public void OnNewMessageBox()
		{
			new MessageBoxViewModel
			{
				Caption = "Message Box",
				Message = "This is a message box!",
				Image = MessageBoxImage.Information
			}.Show(this.Dialogs);
		}

		public ICommand NewOpenFileDialogCommand { get { return new RelayCommand(OnNewOpenFileDialog); } }
		public void OnNewOpenFileDialog()
		{
			var dlg = new OpenFileDialogViewModel {
				Title = "Select a file (I won't actually do anything with it)",
				Filter="All files (*.*)|*.*",
				Multiselect=false
			};
			
			if (dlg.Show(this.Dialogs))
				new MessageBoxViewModel { Message = "You selected the following file: " + dlg.FileName + "."}.Show(this.Dialogs);
			else
				new MessageBoxViewModel { Message = "You didn't select a file." }.Show(this.Dialogs);
		}

    }
}