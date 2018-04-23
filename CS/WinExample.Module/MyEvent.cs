using System;

using DevExpress.Xpo;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo.Metadata;
using System.ComponentModel;
using System.Xml;
using System.Text;

namespace WinExample.Module {
    [DefaultClassOptions]
    public class MyEvent : DevExpress.Persistent.BaseImpl.BaseObject, IEvent, ISupportRecurrences
    {
#if MediumTrust
		[Persistent("ResourceIds"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
		public String resourceIds;
#else
        [Persistent("ResourceIds"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        private String resourceIds;
#endif
        private EventImpl appointmentImpl = new EventImpl();
        [Persistent("RecurrencePattern")]
        private DevExpress.Persistent.BaseImpl.Event recurrencePattern;
        private string recurrenceInfoXml;
        private void UpdateResources() {
            Resources.SuspendChangedEvents();
            try {
                while (Resources.Count > 0) {
                    Resources.Remove(Resources[0]);
                }
                if (!String.IsNullOrEmpty(resourceIds)) {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(resourceIds);
                    foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes) {
                        MyUser resource = Session.GetObjectByKey<MyUser>(new Guid(xmlNode.Attributes["Value"].Value));
                        if (resource != null) {
                            Resources.Add(resource);
                        }
                    }
                }
            } finally {
                Resources.ResumeChangedEvents();
            }
        }
        private void Resources_CollectionChanged(object sender, XPCollectionChangedEventArgs e) {
            if ((e.CollectionChangedType == XPCollectionChangedType.AfterAdd) || (e.CollectionChangedType == XPCollectionChangedType.AfterRemove)) {
                UpdateResourceIds();
                OnChanged("ResourceId");
            }
        }
        private void session_ObjectSaving(object sender, ObjectManipulationEventArgs e) {
        }
        protected override XPCollection CreateCollection(XPMemberInfo property) {
            XPCollection result = base.CreateCollection(property);
            if (property.Name == "Resources") {
                result.CollectionChanged += new XPCollectionChangedEventHandler(Resources_CollectionChanged);
            }
            return result;
        }
        public MyEvent(Session session)
            : base(session) {
            session.ObjectSaving += new ObjectManipulationEventHandler(session_ObjectSaving);
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            appointmentImpl.AfterConstruction();
        }
        public void UpdateResourceIds() {
            resourceIds = String.Empty;
            Resources.SuspendChangedEvents();
            StringBuilder sb = new StringBuilder();
            try {
                sb.AppendLine("<ResourceIds>");
                foreach (MyUser resource in Resources)
                {
                    sb.AppendFormat(@"<ResourceId Type=""{0}"" Value=""{1}"" />", resource.Id.GetType().FullName, resource.Id);
                }
                sb.AppendLine("</ResourceIds>");
            } finally {
                Resources.ResumeChangedEvents();
            }
            resourceIds = sb.ToString();
        }
        [NonPersistent, Browsable(false)]
        public string AppointmentId {
            get { return Oid.ToString(); }
        }
        [Size(250)]
        public string Subject {
            get { return appointmentImpl.Subject; }
            set {
                appointmentImpl.Subject = value;
                OnChanged("Subject", null, value);
            }
        }
        [Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        public string Description {
            get { return appointmentImpl.Description; }
            set {
                appointmentImpl.Description = value;
                OnChanged("Description");
            }
        }
        [Indexed]
        public DateTime StartOn {
            get { return appointmentImpl.StartOn; }
            set {
                appointmentImpl.StartOn = value;
                OnChanged("StartOn");
            }
        }
        [Indexed]
        public DateTime EndOn {
            get { return appointmentImpl.EndOn; }
            set {
                appointmentImpl.EndOn = value;
                OnChanged("EndOn");
            }
        }
        public bool AllDay {
            get { return appointmentImpl.AllDay; }
            set {
                appointmentImpl.AllDay = value;
                OnChanged("AllDay");
            }
        }
        public string Location {
            get { return appointmentImpl.Location; }
            set {
                appointmentImpl.Location = value;
                OnChanged("Location");
            }
        }
        public int Label {
            get { return appointmentImpl.Label; }
            set {
                appointmentImpl.Label = value;
                OnChanged("Label");
            }
        }
        public int Status {
            get { return appointmentImpl.Status; }
            set {
                appointmentImpl.Status = value;
                OnChanged("Status");
            }
        }
        public int Type {
            get { return appointmentImpl.Type; }
            set {
                appointmentImpl.Type = value;
                OnChanged("Type");
            }
        }
        [Association("MyEvent-MyUser", typeof(MyUser))]
        public XPCollection Resources {
            get { return GetCollection("Resources"); }
        }
        [NonPersistent(), Browsable(false)]
        public String ResourceId {
            get {
                if (resourceIds == null) {
                    UpdateResourceIds();
                }
                return resourceIds;
            }
            set {
                if (resourceIds != value) {
                    resourceIds = value;
                    UpdateResources();
                }
            }
        }
        [DevExpress.Xpo.DisplayName("Recurrence"), Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        public string RecurrenceInfoXml {
            get {
                return recurrenceInfoXml;
            }
            set {
                recurrenceInfoXml = value;
                OnChanged("RecurrenceInfoXml");
            }
        }
        public IEvent RecurrencePattern {
            get {
                return recurrencePattern;
            }
            set {
                recurrencePattern = (DevExpress.Persistent.BaseImpl.Event)value;
                OnChanged("RecurrencePattern");
            }
        }
        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("MyEventIntervalValid", DefaultContexts.Save, "The start date must be less than the end date", UsedProperties = "StartOn, EndOn")]
        public bool IsIntervalValid { get { return StartOn < EndOn; } }
    }

}
