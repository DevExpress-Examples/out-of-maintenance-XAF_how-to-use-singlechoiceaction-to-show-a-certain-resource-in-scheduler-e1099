Imports Microsoft.VisualBasic
Imports System
Namespace WinExample.Module.Win
	Partial Public Class FilterResourcesController
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim choiceActionItem1 As New DevExpress.ExpressApp.Actions.ChoiceActionItem()
			Me.userChoiceAction = New DevExpress.ExpressApp.Actions.SingleChoiceAction(Me.components)
			' 
			' userChoiceAction
			' 
			Me.userChoiceAction.Caption = "Users"
			Me.userChoiceAction.Id = "UserChoiceAction"
			choiceActionItem1.Caption = "All Users"
			Me.userChoiceAction.Items.Add(choiceActionItem1)
'			Me.userChoiceAction.Execute += New DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(Me.userChoiceAction_Execute);

		End Sub

		#End Region

		Private WithEvents userChoiceAction As DevExpress.ExpressApp.Actions.SingleChoiceAction
	End Class
End Namespace
