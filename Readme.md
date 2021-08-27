<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128594459/10.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1099)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [FilterResourcesController.cs](./CS/WinExample.Module.Win/FilterResourcesController.cs) (VB: [FilterResourcesController.vb](./VB/WinExample.Module.Win/FilterResourcesController.vb))
* [MyEvent.cs](./CS/WinExample.Module/MyEvent.cs) (VB: [MyEvent.vb](./VB/WinExample.Module/MyEvent.vb))
* [MyUser.cs](./CS/WinExample.Module/MyUser.cs) (VB: [MyUser.vb](./VB/WinExample.Module/MyUser.vb))
<!-- default file list end -->
# How to use SingleChoiceAction to show a certain resource in Scheduler


<p><strong>Scenario:</strong><br /><br />It is necessary to use Security System users as Scheduler resources and filter Scheduler events by these users using a SingleChoiceAction.<br /><br /><strong>Steps to Implement:</strong><br /><br />1. Create a custom use class (MyUser) inherited from the SecuritySystemUser class to establish association between users and events (see <a href="https://documentation.devexpress.com/#Xaf/CustomDocument3384">How to: Implement Custom Security Objects (Users, Roles, Operation Permissions)</a>).<br />2. Create a custom event class (MyEvent) implementing the IRecurrentEvent interface. You cannot use an Event's descendant here, because the built-in Event class is already associated with resources.<br />3. Establish a many-to-many association between the MyEvent and MyUser classes.<br />4. Implement the IEvent.ResourceId property in the MyEvent class to build IDs based on the associated users collection. Pay special attention to implementing the UpdateResources method.<br />5. Create a controller (FilterResourcesController) with a SingleChoiceAction whose items are populated based on the existing MyUser objects.<br />6. Filter data source from the SchedulerListEditor.ResourcesDataSource property when the SingleChoiceAction is executed based on the selected action item.</p>
<p><br />See also: <a href="https://www.devexpress.com/Support/Center/p/E1255">How to create fully custom Role, User, Event, Resource classes for use with the Security and Scheduler modules</a></p>

<br/>


