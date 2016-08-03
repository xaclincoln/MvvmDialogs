using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmDialogs.Demo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwareApproach.TestingExtensions;
using System.Windows;

namespace MvvmDialogs.Tests
{
	[TestClass]
	public class ModelessTests
	{
		[TestMethod]
		[Description("Tests that clicking the 'New Modeless Dialog' button opens a new modeless dialog.")]		
		public void Can_Create_Modeless_Dialog()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();

			// == Act ==
			mainViewModel.NewModelessDialogCommand.Execute(null);

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(1);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;
			dlg.ShouldNotBeNull();
			dlg.IsModal.ShouldBeFalse();
			dlg.Caption.ShouldEqual("Modeless Dialog Box");
			dlg.Message.ShouldEqual("Hello World!");
		}

		[TestMethod]
		[Description("Tests that clicking the 'Ok' button on the modeless dialog requests confirmation from the user that they actually want to close the dialog.")]		
		public void Modeless_Ok_Requests_Confirmation()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModelessDialogCommand.Execute(null);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;
			
			// == Act ==
			dlg.OkCommand.Execute(null);

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(2);
			var msg_dlg = mainViewModel.Dialogs[1] as MessageBoxViewModel;
			msg_dlg.ShouldNotBeNull();
			msg_dlg.Caption.ShouldEqual("Closing");
			msg_dlg.Message.ShouldEqual("Are you sure you want to close this window?");
			msg_dlg.Buttons.ShouldEqual(MessageBoxButton.YesNo);
			msg_dlg.Image.ShouldEqual(MessageBoxImage.Question);
		}

		[TestMethod]
		[Description("Tests that clicking the 'Cancel' button on the modeless dialog requests confirmation from the user that they actually want to close the dialog.")]
		public void Modeless_Cancel_Requests_Confirmation()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModelessDialogCommand.Execute(null);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;

			// == Act ==
			dlg.CancelCommand.Execute(null);

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(2);
			var msg_dlg = mainViewModel.Dialogs[1] as MessageBoxViewModel;
			msg_dlg.ShouldNotBeNull();
			msg_dlg.Caption.ShouldEqual("Closing");
			msg_dlg.Message.ShouldEqual("Are you sure you want to close this window?");
			msg_dlg.Buttons.ShouldEqual(MessageBoxButton.YesNo);
			msg_dlg.Image.ShouldEqual(MessageBoxImage.Question);
		}

		[TestMethod]
		[Description("Tests that clicking the caption close button on the modeless dialog requests confirmation from the user that they actually want to close the dialog.")]
		public void Modeless_Caption_Bar_Close_Requests_Confirmation()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModelessDialogCommand.Execute(null);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;

			// == Act ==
			dlg.RequestClose();

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(2);
			var msg_dlg = mainViewModel.Dialogs[1] as MessageBoxViewModel;
			msg_dlg.ShouldNotBeNull();
			msg_dlg.Caption.ShouldEqual("Closing");
			msg_dlg.Message.ShouldEqual("Are you sure you want to close this window?");
			msg_dlg.Buttons.ShouldEqual(MessageBoxButton.YesNo);
			msg_dlg.Image.ShouldEqual(MessageBoxImage.Question);
		}

		[TestMethod]
		[Description("Tests that clicking the 'Yes' button on the modeless dialog's close confirmation message box causes the modeless dialog box to close.")]
		public void Modeless_Confirmation_Closes_On_Yes_Button()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModelessDialogCommand.Execute(null);

			// when we execute the Ok command a new MessageBoxViewModel (i.e. the confirmation dialog box)
			// will appear on the dialog box list, so simulate the user pressing the "Yes" button on that instance.
			mainViewModel.Dialogs.CollectionChanged += (sender, e) =>
			{
				e.NewItems.Count.ShouldEqual(1);
				var confirm_dlg = e.NewItems[0] as MessageBoxViewModel;
				confirm_dlg.ShouldNotBeNull();
				confirm_dlg.Result = MessageBoxResult.Yes;
			};
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;

			// == Act ==
			// now close the modeless dialog to trigger the confirmation dialog and the user pressing the "Yes" button.
			var closed = false;
			dlg.DialogClosing += (sender, args) => closed = true;
			dlg.OkCommand.Execute(null);

			// == Assert ==
			// the modeless dialog should have tried to close itself
			closed.ShouldBeTrue();
		}

		[TestMethod]
		[Description("Tests that clicking the 'No' button on the modeless dialog's close confirmation message box causes the modeless dialog box to remain open.")]
		public void Modeless_Confirmation_Remains_Open_On_No_Button()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModelessDialogCommand.Execute(null);

			// when we execute the Ok command a new MessageBoxViewModel (i.e. the confirmation dialog box)
			// will appear on the dialog box list, so simulate the user pressing the "No" button on that instance.
			mainViewModel.Dialogs.CollectionChanged += (sender, e) =>
			{
				e.NewItems.Count.ShouldEqual(1);
				var confirm_dlg = e.NewItems[0] as MessageBoxViewModel;
				confirm_dlg.ShouldNotBeNull();
				confirm_dlg.Result = MessageBoxResult.No;
			};
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;

			// == Act ==
			// now close the modeless dialog to trigger the confirmation dialog and the user pressing the "No" button.
			var closed = false;
			dlg.DialogClosing += (sender, args) => closed = true;
			dlg.OkCommand.Execute(null);

			// == Assert ==
			// the modeless dialog should not have tried to close itself
			closed.ShouldBeFalse();
		}
	}
}
