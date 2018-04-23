Imports Microsoft.VisualBasic
Imports DevExpress.ExpressApp
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security

Namespace WinExample.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim user1 As MyUser = ObjectSpace.FindObject(Of MyUser)(New BinaryOperator("UserName", "Admin"))
			If user1 Is Nothing Then
				user1 = ObjectSpace.CreateObject(Of MyUser)()
				user1.UserName = "Admin"
				user1.FirstName = "Admin"
				user1.SetPassword("")
			End If
			Dim user2 As MyUser = ObjectSpace.FindObject(Of MyUser)(New BinaryOperator("UserName", "User"))
			If user2 Is Nothing Then
				user2 = ObjectSpace.CreateObject(Of MyUser)()
				user2.UserName = "User"
				user2.FirstName = "User"
				user2.SetPassword("")
			End If
			Dim adminRole As Role = ObjectSpace.FindObject(Of Role)(New BinaryOperator("Name", "Administrators"))
			If adminRole Is Nothing Then
				adminRole = ObjectSpace.CreateObject(Of Role)()
				adminRole.Name = "Administrators"
			End If
			Dim userRole As Role = ObjectSpace.FindObject(Of Role)(New BinaryOperator("Name", "Users"))
			If userRole Is Nothing Then
				userRole = ObjectSpace.CreateObject(Of Role)()
				userRole.Name = "Users"
			End If
			Do While adminRole.PersistentPermissions.Count > 0
				ObjectSpace.Delete(adminRole.PersistentPermissions(0))
			Loop
			Do While userRole.PersistentPermissions.Count > 0
				ObjectSpace.Delete(userRole.PersistentPermissions(0))
			Loop
			adminRole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
			adminRole.AddPermission(New EditModelPermission(ModelAccessModifier.Allow))
			adminRole.Save()
			userRole.AddPermission(New ObjectAccessPermission(GetType(Object), ObjectAccess.AllAccess))
			userRole.AddPermission(New ObjectAccessPermission(GetType(User), ObjectAccess.ChangeAccess, ObjectAccessModifier.Deny))
			userRole.AddPermission(New ObjectAccessPermission(GetType(Role), ObjectAccess.AllAccess, ObjectAccessModifier.Deny))
			userRole.AddPermission(New EditModelPermission(ModelAccessModifier.Deny))
			userRole.Save()
			user1.Roles.Add(adminRole)
			user2.Roles.Add(userRole)
			user1.Save()
			user2.Save()
		End Sub
	End Class
End Namespace
