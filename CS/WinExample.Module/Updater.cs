using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;

namespace WinExample.Module
{
    public class Updater : ModuleUpdater
    {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            MyUser user1 = Session.FindObject<MyUser>(new BinaryOperator("UserName", "Admin"));
            if (user1 == null)
            {
                user1 = new MyUser(Session);
                user1.UserName = "Admin";
                user1.FirstName = "Admin";
                user1.SetPassword("");
            }
            MyUser user2 = Session.FindObject<MyUser>(new BinaryOperator("UserName", "User"));
            if (user2 == null)
            {
                user2 = new MyUser(Session);
                user2.UserName = "User";
                user2.FirstName = "User";
                user2.SetPassword("");
            }
            Role adminRole = Session.FindObject<Role>(new BinaryOperator("Name", "Administrators"));
            if (adminRole == null)
            {
                adminRole = new Role(Session);
                adminRole.Name = "Administrators";
            }
            Role userRole = Session.FindObject<Role>(new BinaryOperator("Name", "Users"));
            if (userRole == null)
            {
                userRole = new Role(Session);
                userRole.Name = "Users";
            }
            while (adminRole.PersistentPermissions.Count > 0)
            {
                Session.Delete(adminRole.PersistentPermissions[0]);
            }
            while (userRole.PersistentPermissions.Count > 0)
            {
                Session.Delete(userRole.PersistentPermissions[0]);
            }
            adminRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            adminRole.AddPermission(new EditModelPermission(ModelAccessModifier.Allow));
            adminRole.Save();
            userRole.AddPermission(new ObjectAccessPermission(typeof(object), ObjectAccess.AllAccess));
            userRole.AddPermission(new ObjectAccessPermission(typeof(User),
               ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny));
            userRole.AddPermission(new ObjectAccessPermission(typeof(Role),
               ObjectAccess.AllAccess, ObjectAccessModifier.Deny));
            userRole.AddPermission(new EditModelPermission(ModelAccessModifier.Deny));
            userRole.Save();
            user1.Roles.Add(adminRole);
            user2.Roles.Add(userRole);
            user1.Save();
            user2.Save();
        }
    }
}
