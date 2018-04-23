Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.General
Imports System.Drawing
Imports System.ComponentModel

Namespace WinExample.Module
	<DefaultClassOptions> _
	Public Class MyUser
		Inherits DevExpress.Persistent.BaseImpl.User
		Implements IResource
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		#If MediumTrust Then
		<Persistent("Color"), Browsable(False)> _
		Public color_Renamed As Int32
#Else
		<Persistent("Color")> _
		Private color_Renamed As Int32
#End If
		Public Overrides Sub AfterConstruction()
			MyBase.AfterConstruction()
			color_Renamed = Color.White.ToArgb()
		End Sub
		<NonPersistent, Browsable(False)> _
		Public ReadOnly Property Id() As Object Implements IResource.Id
			Get
				Return Oid
			End Get
		End Property
		Public Property Caption() As String Implements IResource.Caption
			Get
				Return UserName
			End Get
			Set(ByVal value As String)
				UserName = value
				OnChanged("Caption")
			End Set
		End Property
		<NonPersistent, Browsable(False)> _
		Public ReadOnly Property OleColor() As Int32 Implements IResource.OleColor
			Get
				Return ColorTranslator.ToOle(Color.FromArgb(color_Renamed))
			End Get
		End Property
		<Association("MyEvent-MyUser", GetType(MyEvent))> _
		Public ReadOnly Property Events() As XPCollection
			Get
				Return GetCollection("Events")
			End Get
		End Property
		<NonPersistent> _
		Public Property Color() As Color
			Get
				Return Color.FromArgb(color_Renamed)
			End Get
			Set(ByVal value As Color)
				color_Renamed = value.ToArgb()
				OnChanged("Color")
			End Set
		End Property
	End Class

End Namespace
