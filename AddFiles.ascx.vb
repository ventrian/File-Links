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

Namespace Ventrian.FileLinks

    Partial Public Class AddFiles
        Inherits FileLinksBase

#Region " Private Members "

        Private _folderID As Integer = Null.NullInteger

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
            objCrumb.Caption = Localization.GetString("AddFiles", Me.LocalResourceFile)
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
            If (IsEditable = False And objModuleSecurity.UploadPermission = False) Then
                Response.Redirect(NavigateURL(), True)
            End If

        End Sub

        Private Sub ReadQueryString()

            If (Request("FolderID") <> "" AndAlso IsNumeric(Request("FolderID"))) Then
                _folderID = Convert.ToInt32(Request("FolderID"))
            End If

        End Sub

#End Region

#Region " Protected Methods "

        Protected Function GetMaxFileSize() As String

            Return LinkSettings.MaxFileSize.ToString()

        End Function

        Protected Function GetUploadUrl() As String

            Return Page.ResolveUrl("~/DesktopModules/FileLinks/Uploader.ashx?PortalID=" & Me.PortalId.ToString())

        End Function

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            CheckSecurity()
            ReadQueryString()
            BindCrumbs()

            If (Request.IsAuthenticated) Then
                litTicketID.Text = Request.Cookies(System.Web.Security.FormsAuthentication.FormsCookieName()).Value
            Else
                litTicketID.Text = ""
            End If

            divFolders.Visible = LinkSettings.DisplayFolders

            If (Page.IsPostBack = False) Then
                BindFolders()
            End If

            litFolderID.Text = drpFolders.SelectedValue
            litModuleID.Text = ModuleId.ToString()
            litTabID.Text = TabId.ToString()

            litSelectFileDescription.Text = Localization.GetString("SelectFileDescription", Me.LocalResourceFile)
            If (LinkSettings.ExtensionFilter.Trim() <> "") Then
                Dim filter As String = ""
                For Each val As String In LinkSettings.ExtensionFilter.Split(","c)
                    If (filter = "") Then
                        filter = "*." & val
                    Else
                        filter = filter & ";*." & val
                    End If
                Next
                litFileTypes.Text = filter
            Else
                litFileTypes.Text = "*.*"
            End If

        End Sub

        Protected Sub cmdReturn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReturn.Click

            If (_folderID <> Null.NullInteger) Then
                Response.Redirect(NavigateURL(Me.TabId, "", "FolderID=" & _folderID.ToString()), True)
            Else
                Response.Redirect(NavigateURL(), True)
            End If

        End Sub

        Protected Sub drpFolders_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpFolders.SelectedIndexChanged

            Try

                Response.Redirect(EditUrl("FolderID", drpFolders.SelectedValue, "AddFiles"), True)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace