Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Security
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization

Imports Ventrian.FileLinks.Entities
Imports Ventrian.FileLinks.Base
Imports System.IO

Namespace Ventrian.FileLinks

    Partial Public Class AddFolder
        Inherits FileLinksBase

#Region " Private Members "

        Private _folderID As Integer = Null.NullInteger
        Private _editFolderID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub BindCrumbs()

            Dim objCrumbs As New List(Of CrumbInfo)

            Dim objCrumbHome As New CrumbInfo
            objCrumbHome.Url = NavigateURL()
            objCrumbHome.Caption = Localization.GetString("Home", Me.LocalResourceFile)
            objCrumbs.Add(objCrumbHome)

            Dim objCrumb As New CrumbInfo
            objCrumb.Url = Request.RawUrl
            objCrumb.Caption = Localization.GetString("AddFolder", Me.LocalResourceFile)
            objCrumbs.Add(objCrumb)

            rptBreadCrumbs.DataSource = objCrumbs
            rptBreadCrumbs.DataBind()

        End Sub

        Private Sub BindFolders()

            Dim folderPath As String = Null.NullString()
            If (LinkSettings.FolderID <> Null.NullInteger) Then
                Dim folderIDs As ArrayList = FileSystemUtils.GetFolders(Me.PortalId)
                For Each folder As FolderInfo In folderIDs
                    If (folder.FolderID = LinkSettings.FolderID) Then
                        folderPath = folder.FolderPath
                    End If
                Next
            End If

            Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalId)
            For Each folder As FolderInfo In folders
                If (folderPath = Null.NullString Or folder.FolderPath.StartsWith(folderPath)) Then
                    Dim FolderItem As New ListItem
                    If folder.FolderPath = Null.NullString Then
                        FolderItem.Text = Localization.GetString("Root", Me.LocalResourceFile)
                    Else
                        FolderItem.Text = folder.FolderPath
                    End If
                    FolderItem.Value = folder.FolderID
                    If (_folderID <> Null.NullInteger) Then
                        If (_folderID = folder.FolderID) Then
                            FolderItem.Selected = True
                        End If
                    Else
                        If (LinkSettings.FolderID <> Null.NullInteger) Then
                            If (LinkSettings.FolderID = folder.FolderID) Then
                                FolderItem.Selected = True
                            End If

                        End If
                    End If
                    drpFolders.Items.Add(FolderItem)
                End If
            Next

        End Sub

        Private Sub CheckSecurity()

            Dim objModuleSecurity As New ModuleSecurity(ModuleConfiguration, Settings)
            If (IsEditable = False And objModuleSecurity.AddFolderPermission = False) Then
                Response.Redirect(NavigateURL(), True)
            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request("FolderID") <> "" AndAlso IsNumeric(Request("FolderID"))) Then
                _folderID = Convert.ToInt32(Request("FolderID"))
            End If

            If (Request("EditFolderID") <> "" AndAlso IsNumeric(Request("EditFolderID"))) Then
                _folderID = Convert.ToInt32(Request("EditFolderID"))
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                CheckSecurity()
                ReadQueryString()
                BindCrumbs()

                If (Page.IsPostBack = False) Then
                    BindFolders()
                    txtFolder.Focus()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub drpFolders_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpFolders.SelectedIndexChanged

            Try

                Response.Redirect(EditUrl("FolderID", drpFolders.SelectedValue, "AddFolder"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click

            Try

                If (Page.IsValid) Then

                    Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalSettings.PortalId)

                    For Each folder As FolderInfo In folders
                        If (folder.FolderID = Convert.ToInt32(drpFolders.SelectedValue)) Then
                            FileSystemUtils.AddFolder(PortalSettings, folder.PhysicalPath, txtFolder.Text)
                            If (Convert.ToInt32(drpFolders.SelectedValue) <> LinkSettings.FolderID) Then
                                Response.Redirect(NavigateURL(Me.TabId, "", "FolderID=" & _folderID.ToString()), True)
                            Else
                                Response.Redirect(NavigateURL(), True)
                            End If
                            Exit For
                        End If
                    Next

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click

            Try

                If (_folderID <> Null.NullInteger) Then
                    Response.Redirect(NavigateURL(Me.TabId, "", "FolderID=" & _folderID.ToString()), True)
                Else
                    Response.Redirect(NavigateURL(), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        'Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click

        '    Try

        '        Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalSettings.PortalId)

        '        For Each folder As FolderInfo In folders
        '            If (folder.FolderID = Convert.ToInt32(drpFolders.SelectedValue)) Then
        '                Dim objDirectory As New DirectoryInfo(folder.PhysicalPath)
        '                If (objDirectory.Exists) Then
        '                    objDirectory.Delete(True)
        '                End If
        '                If (Convert.ToInt32(drpFolders.SelectedValue) <> LinkSettings.FolderID) Then
        '                    Response.Redirect(NavigateURL(Me.TabId, "", "FolderID=" & _folderID.ToString()), True)
        '                Else
        '                    Response.Redirect(NavigateURL(), True)
        '                End If
        '                Exit For
        '            End If
        '        Next

        '        FileSystemUtils.DeleteFolder(Me.PortalId,

        '    Catch exc As Exception    'Module failed to load
        '        ProcessModuleLoadException(Me, exc)
        '    End Try

        'End Sub

#End Region

    End Class

End Namespace