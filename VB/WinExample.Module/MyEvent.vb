Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.General
Imports DevExpress.Xpo.Metadata
Imports System.ComponentModel
Imports System.Xml
Imports System.Text

Namespace WinExample.Module
	<DefaultClassOptions> _
	Public Class MyEvent
		Inherits DevExpress.Persistent.BaseImpl.BaseObject
		Implements IRecurrentEvent
#If MediumTrust Then
		<Persistent("ResourceIds"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
		Public resourceIds As String
#Else
		<Persistent("ResourceIds"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
		Private resourceIds As String
#End If
		Private appointmentImpl As New EventImpl()
		<Persistent("RecurrencePattern")> _
		Private recurrencePattern_Renamed As DevExpress.Persistent.BaseImpl.Event
		Private recurrenceInfoXml_Renamed As String
		Private Sub UpdateResources()
			Resources.SuspendChangedEvents()
			Try
				Do While Resources.Count > 0
					Resources.Remove(Resources(0))
				Loop
				If (Not String.IsNullOrEmpty(resourceIds)) Then
					Dim xmlDocument As New XmlDocument()
					xmlDocument.LoadXml(resourceIds)
					For Each xmlNode As XmlNode In xmlDocument.DocumentElement.ChildNodes
						Dim resource As MyUser = Session.GetObjectByKey(Of MyUser)(New Guid(xmlNode.Attributes("Value").Value))
						If resource IsNot Nothing Then
							Resources.Add(resource)
						End If
					Next xmlNode
				End If
			Finally
				Resources.ResumeChangedEvents()
			End Try
		End Sub
		Private Sub Resources_CollectionChanged(ByVal sender As Object, ByVal e As XPCollectionChangedEventArgs)
			If (e.CollectionChangedType = XPCollectionChangedType.AfterAdd) OrElse (e.CollectionChangedType = XPCollectionChangedType.AfterRemove) Then
				UpdateResourceIds()
				OnChanged("ResourceId")
			End If
		End Sub
		Private Sub session_ObjectSaving(ByVal sender As Object, ByVal e As ObjectManipulationEventArgs)
		End Sub
		Protected Overrides Function CreateCollection(ByVal [property] As XPMemberInfo) As XPCollection
			Dim result As XPCollection = MyBase.CreateCollection([property])
			If [property].Name = "Resources" Then
				AddHandler result.CollectionChanged, AddressOf Resources_CollectionChanged
			End If
			Return result
		End Function
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
			AddHandler session.ObjectSaving, AddressOf session_ObjectSaving
		End Sub
		Public Overrides Sub AfterConstruction()
			MyBase.AfterConstruction()
			appointmentImpl.AfterConstruction()
		End Sub
		Public Sub UpdateResourceIds()
			resourceIds = String.Empty
			Resources.SuspendChangedEvents()
			Dim sb As New StringBuilder()
			Try
				sb.AppendLine("<ResourceIds>")
				For Each resource As MyUser In Resources
					sb.AppendFormat("<ResourceId Type=""{0}"" Value=""{1}"" />", resource.Id.GetType().FullName, resource.Id)
				Next resource
				sb.AppendLine("</ResourceIds>")
			Finally
				Resources.ResumeChangedEvents()
			End Try
			resourceIds = sb.ToString()
		End Sub
		<NonPersistent, Browsable(False)> _
		Public ReadOnly Property AppointmentId() As Object Implements DevExpress.Persistent.Base.General.IEvent.AppointmentId
			Get
				Return Oid
			End Get
		End Property
		<Size(250)> _
		Public Property Subject() As String Implements DevExpress.Persistent.Base.General.IEvent.Subject
			Get
				Return appointmentImpl.Subject
			End Get
			Set(ByVal value As String)
				appointmentImpl.Subject = value
				OnChanged("Subject", Nothing, value)
			End Set
		End Property
		<Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
		Public Property Description() As String Implements DevExpress.Persistent.Base.General.IEvent.Description
			Get
				Return appointmentImpl.Description
			End Get
			Set(ByVal value As String)
				appointmentImpl.Description = value
				OnChanged("Description")
			End Set
		End Property
		<Indexed> _
		Public Property StartOn() As DateTime Implements DevExpress.Persistent.Base.General.IEvent.StartOn
			Get
				Return appointmentImpl.StartOn
			End Get
			Set(ByVal value As DateTime)
				appointmentImpl.StartOn = value
				OnChanged("StartOn")
			End Set
		End Property
		<Indexed> _
		Public Property EndOn() As DateTime Implements DevExpress.Persistent.Base.General.IEvent.EndOn
			Get
				Return appointmentImpl.EndOn
			End Get
			Set(ByVal value As DateTime)
				appointmentImpl.EndOn = value
				OnChanged("EndOn")
			End Set
		End Property
		Public Property AllDay() As Boolean Implements DevExpress.Persistent.Base.General.IEvent.AllDay
			Get
				Return appointmentImpl.AllDay
			End Get
			Set(ByVal value As Boolean)
				appointmentImpl.AllDay = value
				OnChanged("AllDay")
			End Set
		End Property
		Public Property Location() As String Implements DevExpress.Persistent.Base.General.IEvent.Location
			Get
				Return appointmentImpl.Location
			End Get
			Set(ByVal value As String)
				appointmentImpl.Location = value
				OnChanged("Location")
			End Set
		End Property
		Public Property Label() As Integer Implements DevExpress.Persistent.Base.General.IEvent.Label
			Get
				Return appointmentImpl.Label
			End Get
			Set(ByVal value As Integer)
				appointmentImpl.Label = value
				OnChanged("Label")
			End Set
		End Property
		Public Property Status() As Integer Implements DevExpress.Persistent.Base.General.IEvent.Status
			Get
				Return appointmentImpl.Status
			End Get
			Set(ByVal value As Integer)
				appointmentImpl.Status = value
				OnChanged("Status")
			End Set
		End Property
		Public Property Type() As Integer Implements DevExpress.Persistent.Base.General.IEvent.Type
			Get
				Return appointmentImpl.Type
			End Get
			Set(ByVal value As Integer)
				appointmentImpl.Type = value
				OnChanged("Type")
			End Set
		End Property
		<Association("MyEvent-MyUser", GetType(MyUser))> _
		Public ReadOnly Property Resources() As XPCollection
			Get
				Return GetCollection("Resources")
			End Get
		End Property
		<NonPersistent(), Browsable(False)> _
		Public Property ResourceId() As String Implements DevExpress.Persistent.Base.General.IEvent.ResourceId
			Get
				If resourceIds Is Nothing Then
					UpdateResourceIds()
				End If
				Return resourceIds
			End Get
			Set(ByVal value As String)
				If resourceIds <> value Then
					resourceIds = value
					UpdateResources()
				End If
			End Set
		End Property
		<DevExpress.Xpo.DisplayName("Recurrence"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
		Public Property RecurrenceInfoXml() As String Implements DevExpress.Persistent.Base.General.ISupportRecurrences.RecurrenceInfoXml
			Get
				Return recurrenceInfoXml_Renamed
			End Get
			Set(ByVal value As String)
				recurrenceInfoXml_Renamed = value
				OnChanged("RecurrenceInfoXml")
			End Set
		End Property
		Public Property RecurrencePattern() As IRecurrentEvent Implements DevExpress.Persistent.Base.General.ISupportRecurrences.RecurrencePattern
			Get
				Return recurrencePattern_Renamed
			End Get
			Set(ByVal value As IRecurrentEvent)
				recurrencePattern_Renamed = CType(value, DevExpress.Persistent.BaseImpl.Event)
				OnChanged("RecurrencePattern")
			End Set
		End Property
		<NonPersistent, Browsable(False), RuleFromBoolProperty("MyEventIntervalValid", DefaultContexts.Save, "The start date must be less than the end date", UsedProperties := "StartOn, EndOn")> _
		Public ReadOnly Property IsIntervalValid() As Boolean
			Get
				Return StartOn < EndOn
			End Get
		End Property
	End Class

End Namespace
