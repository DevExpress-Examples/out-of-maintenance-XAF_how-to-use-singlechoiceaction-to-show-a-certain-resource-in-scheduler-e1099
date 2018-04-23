# How to use SingleChoiceAction to show a certain resource in Scheduler


<p><strong>Scenario:</strong><br /><br />It is necessary to use Security System users as Scheduler resources and filter Scheduler events by these users using a SingleChoiceAction.<br /><br /><strong>Steps to Implement:</strong><br /><br />1. Create a custom use class (MyUser) inherited from the SecuritySystemUser class to establish association between users and events (see <a href="https://documentation.devexpress.com/#Xaf/CustomDocument3384">How to: Implement Custom Security Objects (Users, Roles, Operation Permissions)</a>).<br />2. Create a custom event class (MyEvent) implementing the IRecurrentEvent interface. You cannot use an Event's descendant here, because the built-in Event class is already associated with resources.<br />3. Establish a many-to-many association between the MyEvent and MyUser classes.<br />4. Implement the IEvent.ResourceId property in the MyEvent class to build IDs based on the associated users collection. Pay special attention to implementing the UpdateResources method.<br />5. Create a controller (FilterResourcesController) with a SingleChoiceAction whose items are populated based on the existing MyUser objects.<br />6. Filter data source from the SchedulerListEditor.ResourcesDataSource property when the SingleChoiceAction is executed based on the selected action item.</p>
<p><br />See also: <a href="https://www.devexpress.com/Support/Center/p/E1255">How to create fully custom Role, User, Event, Resource classes for use with the Security and Scheduler modules</a></p>

<br/>


