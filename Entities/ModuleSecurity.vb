Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Permissions

Namespace Ventrian.FileLinks.Entities

    Public Class ModuleSecurity

#Region " Private Members "

        Private _uploadPermission As Boolean = False
        Private _deletePermission As Boolean = False
        Private _addFolderPermission As Boolean = False
        Private _deleteFolderPermission As Boolean = False
        Private _secureFolderPermission As Boolean = False
        Private _viewSecurePermission As Boolean = False

#End Region

#Region " Constructors "

        Public Sub New(ByRef objModule As ModuleInfo, ByRef objSettings As Hashtable)

            If (objSettings.Contains("Permissions-Initialized") = False) Then

                Dim objPermissionController As New PermissionController()

                Dim objPermissions As ArrayList = objPermissionController.GetPermissionByCodeAndKey("VENTRIAN_FILE_LINKS", "UPLOAD")
                If (objPermissions.Count = 0) Then
                    Dim objPermission As New PermissionInfo()
                    objPermission.ModuleDefID = objModule.ModuleDefID
                    objPermission.PermissionCode = "VENTRIAN_FILE_LINKS"
                    objPermission.PermissionKey = "UPLOAD"
                    objPermission.PermissionName = "Upload"
                    objPermissionController.AddPermission(objPermission)
                End If

                objPermissions = objPermissionController.GetPermissionByCodeAndKey("VENTRIAN_FILE_LINKS", "DELETEFILES")
                If (objPermissions.Count = 0) Then
                    Dim objPermission As New PermissionInfo()
                    objPermission.ModuleDefID = objModule.ModuleDefID
                    objPermission.PermissionCode = "VENTRIAN_FILE_LINKS"
                    objPermission.PermissionKey = "DELETEFILES"
                    objPermission.PermissionName = "Delete Files"
                    objPermissionController.AddPermission(objPermission)
                End If

                Dim objModuleController As New ModuleController
                objModuleController.UpdateModuleSetting(objModule.ModuleID, "Permissions-Initialized", "True")

            End If

            _uploadPermission = ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "UPLOAD")
            _deletePermission = (ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "DELETEFILES") Or ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "DELETE"))

            If (objSettings.Contains("Permissions-Initialized-FL2") = False) Then

                Dim objPermissionController As New PermissionController()

                Dim objPermissions As ArrayList = objPermissionController.GetPermissionByCodeAndKey("VENTRIAN_FILE_LINKS", "ADDFOLDER")
                If (objPermissions.Count = 0) Then
                    Dim objPermission As New PermissionInfo()
                    objPermission.ModuleDefID = objModule.ModuleDefID
                    objPermission.PermissionCode = "VENTRIAN_FILE_LINKS"
                    objPermission.PermissionKey = "ADDFOLDER"
                    objPermission.PermissionName = "Add Folder"
                    objPermissionController.AddPermission(objPermission)
                End If

                objPermissions = objPermissionController.GetPermissionByCodeAndKey("VENTRIAN_FILE_LINKS", "DELETEFOLDER")
                If (objPermissions.Count = 0) Then
                    Dim objPermission As New PermissionInfo()
                    objPermission.ModuleDefID = objModule.ModuleDefID
                    objPermission.PermissionCode = "VENTRIAN_FILE_LINKS"
                    objPermission.PermissionKey = "DELETEFOLDER"
                    objPermission.PermissionName = "Delete Folder"
                    objPermissionController.AddPermission(objPermission)
                End If

                Dim objModuleController As New ModuleController
                objModuleController.UpdateModuleSetting(objModule.ModuleID, "Permissions-Initialized-FL2", "True")

            End If

            _addFolderPermission = ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "ADDFOLDER")
            _deleteFolderPermission = ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "DELETEFOLDER")

            If (objSettings.Contains("Permissions-Initialized-FL3") = False) Then
                Dim objPermissionController As New PermissionController()

                Dim objPermissions As ArrayList = objPermissionController.GetPermissionByCodeAndKey("VENTRIAN_FILE_LINKS", "SECUREFOLDER")
                If (objPermissions.Count = 0) Then
                    Dim objPermission As New PermissionInfo()
                    objPermission.ModuleDefID = objModule.ModuleDefID
                    objPermission.PermissionCode = "VENTRIAN_FILE_LINKS"
                    objPermission.PermissionKey = "SECUREFOLDER"
                    objPermission.PermissionName = "Secure Folder"
                    objPermissionController.AddPermission(objPermission)
                End If

                Dim objModuleController As New ModuleController
                objModuleController.UpdateModuleSetting(objModule.ModuleID, "Permissions-Initialized-FL3", "True")
            End If

            _secureFolderPermission = ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "SECUREFOLDER")

            If (objSettings.Contains("Permissions-Initialized-FL4") = False) Then
                Dim objPermissionController As New PermissionController()

                Dim objPermissions As ArrayList = objPermissionController.GetPermissionByCodeAndKey("VENTRIAN_FILE_LINKS", "VIEWSECURE")
                If (objPermissions.Count = 0) Then
                    Dim objPermission As New PermissionInfo()
                    objPermission.ModuleDefID = objModule.ModuleDefID
                    objPermission.PermissionCode = "VENTRIAN_FILE_LINKS"
                    objPermission.PermissionKey = "VIEWSECURE"
                    objPermission.PermissionName = "View Secure"
                    objPermissionController.AddPermission(objPermission)
                End If

                Dim objModuleController As New ModuleController
                objModuleController.UpdateModuleSetting(objModule.ModuleID, "Permissions-Initialized-FL4", "True")
            End If

            _viewSecurePermission = ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "VIEWSECURE")

        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property DeletePermission() As Boolean
            Get
                Return _deletePermission
            End Get
        End Property

        Public ReadOnly Property UploadPermission() As Boolean
            Get
                Return _uploadPermission
            End Get
        End Property

        Public ReadOnly Property AddFolderPermission() As Boolean
            Get
                Return _addFolderPermission
            End Get
        End Property

        Public ReadOnly Property DeleteFolderPermission() As Boolean
            Get
                Return _deleteFolderPermission
            End Get
        End Property

        Public ReadOnly Property SecureFolderPermission() As Boolean
            Get
                Return _secureFolderPermission
            End Get
        End Property

        Public ReadOnly Property ViewSecurePermission() As Boolean
            Get
                Return _viewSecurePermission
            End Get
        End Property

#End Region

    End Class

End Namespace
