Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Scheduler.Win
Imports DevExpress.Xpo
Imports DevExpress.XtraScheduler
Imports DevExpress.ExpressApp.Scheduler
Imports DevExpress.Data.Filtering

Namespace WinExample.Module.Win
	Partial Public Class FilterResourcesController
		Inherits ViewController
		Public Sub New()
			InitializeComponent()
			RegisterActions(components)
			TargetViewId = "MyEvent_ListView"
		End Sub
		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			userChoiceAction.Items.Add(New ChoiceActionItem("Current User", SecuritySystem.CurrentUser))
			For Each user As MyUser In New XPCollection(Of MyUser)(View.ObjectSpace.Session)
				userChoiceAction.Items.Add(New ChoiceActionItem(user.UserName, user))
			Next user
			AddHandler View.ControlsCreated, AddressOf View_ControlsCreated
		End Sub

		Private Sub View_ControlsCreated(ByVal sender As Object, ByVal e As EventArgs)
			Dim editor As SchedulerListEditor = TryCast((CType(View, ListView)).Editor, SchedulerListEditor)
			If editor Is Nothing Then
				userChoiceAction.Active.SetItemValue("Scheduler", False)
			Else
				userChoiceAction.Active.SetItemValue("Scheduler", True)
				userChoiceAction.SelectedItem = userChoiceAction.Items(0)
			End If
		End Sub

		Private Sub userChoiceAction_Execute(ByVal sender As Object, ByVal e As SingleChoiceActionExecuteEventArgs) Handles userChoiceAction.Execute
			Dim editor As SchedulerListEditor = TryCast((CType(View, ListView)).Editor, SchedulerListEditor)
			Dim resources As XPCollection = TryCast(editor.ResourcesDataSource, XPCollection)
			If resources IsNot Nothing Then
				If e.SelectedChoiceActionItem.Data Is Nothing Then
					resources.Criteria = Nothing
				Else
					resources.Criteria = CriteriaOperator.Parse("Oid = ?", (TryCast(e.SelectedChoiceActionItem.Data, MyUser)).Oid)
				End If
			End If
		End Sub
	End Class
End Namespace
