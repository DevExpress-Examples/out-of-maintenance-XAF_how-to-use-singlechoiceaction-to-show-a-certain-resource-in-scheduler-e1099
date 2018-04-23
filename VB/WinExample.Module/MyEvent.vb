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
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
#If MediumTrust Then
		<Persistent("ResourceIds"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
		Public resourceIds As String
#Else
		<Persistent("ResourceIds"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
		Private resourceIds As String
#End If
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
                        Dim loader As New DevExpress.XtraScheduler.Xml.AppointmentResourceIdXmlLoader(xmlNode)
                        Dim keyMemberValue As Object = loader.ObjectFromXml()
                        Dim resource As MyUser = Session.GetObjectByKey(Of MyUser)(New Guid(keyMemberValue.ToString()))
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
		Protected Overrides Function CreateCollection(ByVal [property] As XPMemberInfo) As XPCollection
			Dim result As XPCollection = MyBase.CreateCollection([property])
			If [property].Name = "Resources" Then
				AddHandler result.CollectionChanged, AddressOf Resources_CollectionChanged
			End If
			Return result
		End Function
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
        Private _Subject As String
		<Size(250)> _
		Public Property Subject() As String Implements DevExpress.Persistent.Base.General.IEvent.Subject
			Get
                Return _Subject
			End Get
            Set(ByVal value As String)
                SetPropertyValue("Subject", _Subject, value)
            End Set
        End Property
        Private _Description As String
		<Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
		Public Property Description() As String Implements DevExpress.Persistent.Base.General.IEvent.Description
			Get
                Return _Description
			End Get
			Set(ByVal value As String)
                SetPropertyValue("Description", _Description, value)
			End Set
        End Property
        Private _StartOn As DateTime
		<Indexed> _
		Public Property StartOn() As DateTime Implements DevExpress.Persistent.Base.General.IEvent.StartOn
			Get
                Return _StartOn
			End Get
			Set(ByVal value As DateTime)
                SetPropertyValue("StartOn", _StartOn, value)
			End Set
        End Property
        Private _EndOn As DateTime
		<Indexed> _
		Public Property EndOn() As DateTime Implements DevExpress.Persistent.Base.General.IEvent.EndOn
			Get
                Return _EndOn
			End Get
			Set(ByVal value As DateTime)
                SetPropertyValue("EndOn", _EndOn, value)
			End Set
        End Property
        Private _AllDay As Boolean
		Public Property AllDay() As Boolean Implements DevExpress.Persistent.Base.General.IEvent.AllDay
			Get
                Return _AllDay
			End Get
			Set(ByVal value As Boolean)
                SetPropertyValue("AllDay", _AllDay, value)
			End Set
        End Property
        Private _Location As String
		Public Property Location() As String Implements DevExpress.Persistent.Base.General.IEvent.Location
			Get
                Return _Location
			End Get
			Set(ByVal value As String)
                SetPropertyValue("Location", _Location, value)
			End Set
        End Property
        Private _Label As Integer
		Public Property Label() As Integer Implements DevExpress.Persistent.Base.General.IEvent.Label
			Get
                Return _Label
			End Get
			Set(ByVal value As Integer)
                SetPropertyValue("Label", _Label, value)
			End Set
        End Property
        Private _Status As Integer
		Public Property Status() As Integer Implements DevExpress.Persistent.Base.General.IEvent.Status
			Get
                Return _Status
			End Get
			Set(ByVal value As Integer)
                SetPropertyValue("Status", _Status, value)
			End Set
        End Property
        Private _Type As Integer
		Public Property Type() As Integer Implements DevExpress.Persistent.Base.General.IEvent.Type
			Get
                Return _Type
			End Get
			Set(ByVal value As Integer)
                SetPropertyValue("Type", _Type, value)
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
        Private _recurrenceInfoXml As String
        <DevExpress.Xpo.DisplayName("Recurrence"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(GetType(ObjectValidatorLargeNonDelayedMember))> _
        Public Property RecurrenceInfoXml() As String Implements DevExpress.Persistent.Base.General.IRecurrentEvent.RecurrenceInfoXml
            Get
                Return _recurrenceInfoXml
            End Get
            Set(ByVal value As String)
                SetPropertyValue("RecurrenceInfoXml", _recurrenceInfoXml, value)
            End Set
        End Property
        <Persistent("RecurrencePattern")> _
        Private _recurrencePattern As MyEvent
        Public Property RecurrencePattern() As IRecurrentEvent Implements DevExpress.Persistent.Base.General.IRecurrentEvent.RecurrencePattern
            Get
                Return _recurrencePattern
            End Get
            Set(ByVal value As IRecurrentEvent)
                SetPropertyValue("RecurrencePattern", CType(_recurrencePattern, MyEvent), value)
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
