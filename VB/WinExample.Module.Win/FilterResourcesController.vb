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
		Protected Overrides Overloads Sub OnActivated()
			MyBase.OnActivated()
			userChoiceAction.Items.Add(New ChoiceActionItem("Current User", SecuritySystem.CurrentUserId))
			For Each user As MyUser In View.ObjectSpace.GetObjects(Of MyUser)()
				userChoiceAction.Items.Add(New ChoiceActionItem(user.UserName, user.Oid))
			Next user
		End Sub
		Protected Overrides Overloads Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			Dim editor As SchedulerListEditor = TryCast((CType(View, ListView)).Editor, SchedulerListEditor)
			If editor Is Nothing Then
				userChoiceAction.Active.SetItemValue("Scheduler", False)
			Else
				userChoiceAction.Active.SetItemValue("Scheduler", True)
				userChoiceAction.SelectedItem = userChoiceAction.Items(0)
				AddHandler editor.ResourceDataSourceCreated, AddressOf editor_ResourceDataSourceCreated
			End If
		End Sub
		Private Sub editor_ResourceDataSourceCreated(ByVal sender As Object, ByVal e As ResourceDataSourceCreatedEventArgs)
			SetResourcesFilter(e.DataSource, userChoiceAction.SelectedItem.Data)
		End Sub
		Private Sub userChoiceAction_Execute(ByVal sender As Object, ByVal e As SingleChoiceActionExecuteEventArgs) Handles userChoiceAction.Execute
			Dim editor As SchedulerListEditor = TryCast((CType(View, ListView)).Editor, SchedulerListEditor)
			SetResourcesFilter(editor.ResourcesDataSource, e.SelectedChoiceActionItem.Data)
		End Sub
		Private Sub SetResourcesFilter(ByVal dataSource As Object, ByVal resourceId As Object)
			Dim resources As XPCollection = TryCast(dataSource, XPCollection)
			If resourceId Is Nothing Then
				resources.Criteria = Nothing
			Else
				resources.Criteria = New BinaryOperator("Oid", resourceId)
			End If
		End Sub
	End Class
End Namespace
