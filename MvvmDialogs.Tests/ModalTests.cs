using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvvmDialogs.Demo.ViewModel;
using SoftwareApproach.TestingExtensions;

namespace MvvmDialogs.Tests
{
	[TestClass]
	public class ModalTests
	{
		[TestMethod]
		[Description("Tests that clicking the 'New Modal Dialog' button opens a new modal dialog.")]
		public void Can_Create_Modal_Dialog()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();

			// == Act ==
			mainViewModel.NewModalDialogCommand.Execute(null);

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(1);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;
			dlg.ShouldNotBeNull();
			dlg.IsModal.ShouldBeTrue();
			dlg.Caption.ShouldEqual("Modal Dialog Box");
			dlg.Message.ShouldEqual("Hello World!");
		}

		[TestMethod]
		[Description("Tests that clicking the 'Ok' button on the modal dialog box lets the user know what button they pressed.")]
		public void Modal_Ok_Notifies_User()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModalDialogCommand.Execute(null);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;

			// == Act ==
			dlg.OkCommand.Execute(null);

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(2);
			var msg_dlg = mainViewModel.Dialogs[1] as MessageBoxViewModel;
			msg_dlg.ShouldNotBeNull();
			msg_dlg.Caption.ShouldEqual("Bye bye!");
			msg_dlg.Message.ShouldEqual("You pressed ok!");
		}

		[TestMethod]
		[Description("Tests that clicking the 'Cancel' button on the modal dialog box lets the user know what button they pressed.")]
		public void Modal_Cancel_Notifies_User()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModalDialogCommand.Execute(null);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;

			// == Act ==
			dlg.OnCancel(dlg);

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(2);
			var msg_dlg = mainViewModel.Dialogs[1] as MessageBoxViewModel;
			msg_dlg.ShouldNotBeNull();
			msg_dlg.Caption.ShouldEqual("Bye bye!");
			msg_dlg.Message.ShouldEqual("You pressed cancel!");
		}

		[TestMethod]
		[Description("Tests that clicking the caption close button on the model dialog box lets the user know what button they pressed.")]		
		public void Modal_Caption_Bar_Close_Notifies_User()
		{
			// == Arrange ==
			var mainViewModel = new MainViewModel();
			mainViewModel.NewModalDialogCommand.Execute(null);
			var dlg = mainViewModel.Dialogs[0] as CustomDialogViewModel;

			// == Act ==
			dlg.RequestClose();

			// == Assert ==
			mainViewModel.Dialogs.Count.ShouldEqual(2);
			var msg_dlg = mainViewModel.Dialogs[1] as MessageBoxViewModel;
			msg_dlg.ShouldNotBeNull();
			msg_dlg.Caption.ShouldEqual("Bye bye!");
			msg_dlg.Message.ShouldEqual("You clicked the caption bar close button!");
		}

	}
}
