using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Scheduler.Win;
using DevExpress.Xpo;
using DevExpress.XtraScheduler;
using DevExpress.ExpressApp.Scheduler;
using DevExpress.Data.Filtering;

namespace WinExample.Module.Win {
    public partial class FilterResourcesController : ViewController {
        public FilterResourcesController() {
            InitializeComponent();
            RegisterActions(components);
            TargetViewId = "MyEvent_ListView";
        }
        protected override void OnActivated() {
            base.OnActivated();
            userChoiceAction.Items.Add(new ChoiceActionItem("Current User", SecuritySystem.CurrentUser));
            foreach(MyUser user in new XPCollection<MyUser>(View.ObjectSpace.Session)){
                userChoiceAction.Items.Add(new ChoiceActionItem(user.UserName, user));
            }
            View.ControlsCreated += new EventHandler(View_ControlsCreated);
        }

        void View_ControlsCreated(object sender, EventArgs e) {
            SchedulerListEditor editor = ((ListView)View).Editor as SchedulerListEditor;
            if (editor == null) {
                userChoiceAction.Active.SetItemValue("Scheduler", false);
            } else {
                userChoiceAction.Active.SetItemValue("Scheduler", true);
                userChoiceAction.SelectedItem = userChoiceAction.Items[0];
            }
        }

        private void userChoiceAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            SchedulerListEditor editor = ((ListView)View).Editor as SchedulerListEditor;
            XPCollection resources = editor.ResourcesDataSource as XPCollection;
            if (resources != null) {
                if (e.SelectedChoiceActionItem.Data == null) {
                    resources.Criteria = null;
                } else {
                    resources.Criteria = CriteriaOperator.Parse("Oid = ?", (e.SelectedChoiceActionItem.Data as MyUser).Oid);
                }
            }
        }
    }
}
