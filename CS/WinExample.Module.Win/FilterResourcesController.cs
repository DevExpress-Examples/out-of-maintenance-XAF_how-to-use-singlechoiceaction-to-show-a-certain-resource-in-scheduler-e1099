using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Scheduler;
using DevExpress.ExpressApp.Scheduler.Win;

namespace WinExample.Module.Win {
    public partial class FilterResourcesController : ViewController {
        public FilterResourcesController() {
            InitializeComponent();
            RegisterActions(components);
            TargetViewId = "MyEvent_ListView";
        }
        protected override void OnActivated() {
            base.OnActivated();
            userChoiceAction.Items.Add(new ChoiceActionItem("Current User", SecuritySystem.CurrentUserId));
            foreach (MyUser user in View.ObjectSpace.GetObjects<MyUser>()) {
                userChoiceAction.Items.Add(new ChoiceActionItem(user.UserName, user.Oid));
            }
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            SchedulerListEditor editor = ((ListView)View).Editor as SchedulerListEditor;
            if (editor == null) {
                userChoiceAction.Active.SetItemValue("Scheduler", false);
            } else {
                userChoiceAction.Active.SetItemValue("Scheduler", true);
                userChoiceAction.SelectedItem = userChoiceAction.Items[0];
                editor.ResourceDataSourceCreated += new EventHandler<ResourceDataSourceCreatedEventArgs>(editor_ResourceDataSourceCreated);
            }
        }
        void editor_ResourceDataSourceCreated(object sender, ResourceDataSourceCreatedEventArgs e) {
            SetResourcesFilter(e.DataSource, userChoiceAction.SelectedItem.Data);
        }
        private void userChoiceAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            SchedulerListEditor editor = ((ListView)View).Editor as SchedulerListEditor;
            SetResourcesFilter(editor.ResourcesDataSource, e.SelectedChoiceActionItem.Data);
        }
        private void SetResourcesFilter(object dataSource, object resourceId) {
            XPCollection resources = dataSource as XPCollection;
            if (resourceId == null) {
                resources.Criteria = null;
            } else {
                resources.Criteria = new BinaryOperator("Oid", resourceId);
            }
        }
    }
}
