
Imports System.IO

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization

Imports Ventrian.FileLinks.Base
Imports Ventrian.FileLinks.Entities
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Permissions

Namespace Ventrian.FileLinks

    Partial Public Class _Default
        Inherits FileLinksBase
        Implements IActionable

#Region " Private Members "

        Private _folderID As Integer = Null.NullInteger
        Private _search As String = Null.NullString
        Private _moduleSecurity As ModuleSecurity

#End Region

#Region " Private Properties "

        Private ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return CType(Me.Page, DotNetNuke.Framework.CDefault)
            End Get
        End Property

        Private ReadOnly Property ModuleSecurity() As ModuleSecurity
            Get
                If (_moduleSecurity Is Nothing) Then
                    _moduleSecurity = New ModuleSecurity(Me.ModuleConfiguration, Me.Settings)
                End If
                Return _moduleSecurity
            End Get
        End Property


#End Region

#Region " Private Methods "

        Private Sub BindBreadCrumbs()

            Dim objCrumbs As New List(Of CrumbInfo)
            Dim objFolders As ArrayList = FileSystemUtils.GetFolders(PortalId)

            Dim objCrumbHome As New CrumbInfo
            objCrumbHome.Url = NavigateURL()
            objCrumbHome.Caption = Localization.GetString("Home", Me.LocalResourceFile)
            objCrumbs.Add(objCrumbHome)

            Dim parent As String = ""
            For Each objFolder As FolderInfo In objFolders
                If (objFolder.FolderID = _folderID) Then
                    Dim objCrumb As New CrumbInfo
                    objCrumb.Url = NavigateURL(TabId, "", "FolderID=" & objFolder.FolderID.ToString())
                    objCrumb.Caption = objFolder.FolderName
                    objCrumbs.Add(objCrumb)

                    If (objFolder.FolderPath <> "") Then
                        Dim path As String = objFolder.FolderPath

                        While path.Trim("/"c).LastIndexOf("/"c) <> -1
                            path = path.Substring(0, path.Trim("/"c).LastIndexOf("/"c))
                            Dim objFolderCrumb As FolderInfo = GetFolder(path & "/"c, objFolders)
                            If (objFolderCrumb IsNot Nothing) Then
                                If (LinkSettings.FolderID = objFolderCrumb.FolderID) Then
                                    Exit While
                                End If
                                Dim objCrumbParent As New CrumbInfo
                                objCrumbParent.Url = NavigateURL(TabId, "", "FolderID=" & objFolderCrumb.FolderID.ToString())
                                objCrumbParent.Caption = objFolderCrumb.FolderName
                                objCrumbs.Insert(1, objCrumbParent)
                            End If
                        End While
                    End If
                End If
            Next

            rptBreadCrumbs.DataSource = objCrumbs
            rptBreadCrumbs.DataBind()

        End Sub

        Private Function GetFolder(ByVal path As String, ByVal folders As ArrayList) As FolderInfo

            For Each objFolder As FolderInfo In folders
                If (objFolder.FolderPath = path) Then
                    Return objFolder
                End If
            Next

            Return Nothing

        End Function

        Private Sub BindFileItems()

            If (_folderID = Null.NullInteger) Then
                _folderID = LinkSettings.FolderID
            End If

            Dim delimStr As String = "[]"
            Dim delimiter As Char() = delimStr.ToCharArray()

            Dim itemIndex As Integer = 1

            Dim objFileItems As List(Of FileItem) = GetFileItems(_folderID, LinkSettings.DisplayFolders)

            Dim criteria As String = _search.ToLower().Trim()
            If (criteria <> Null.NullString()) Then
                objFileItems = objFileItems.FindAll(Function(f As FileItem) f.Name.ToLower().Contains(criteria))
            End If

            If (objFileItems.Count > 0) Then

                Dim objLiteralHeader As New Literal
                objLiteralHeader.ID = Globals.CreateValidID("FileLinks-Header-" & TabModuleId.ToString())
                objLiteralHeader.Text = LinkSettings.TemplateHeader
                phFiles.Controls.Add(objLiteralHeader)

                For Each objFileItem As FileItem In objFileItems

                    If (objFileItem.FileItemType = FileItemType.File) Then
                        If (LinkSettings.ExtensionFilter.Trim() <> "") Then
                            For Each extension As String In LinkSettings.ExtensionFilter.Split(","c)
                                If (extension.ToLower() = objFileItem.Extension.ToLower()) Then
                                    ProcessItem(objFileItem, LinkSettings.TemplateItem.Split(delimiter), itemIndex)
                                    Exit For
                                End If
                            Next
                        Else
                            ProcessItem(objFileItem, LinkSettings.TemplateItem.Split(delimiter), itemIndex)
                        End If
                    Else
                        ' Folder
                        ProcessItem(objFileItem, LinkSettings.TemplateItem.Split(delimiter), itemIndex)

                        If (LinkSettings.DisplayFilesInFolders) Then
                            If Not (Settings.ContainsKey("SecureFolder-" & objFileItem.InternalID.ToString())) Then
                                Dim objSubFileItems As List(Of FileItem) = GetFileItems(objFileItem.InternalID, False)

                                If (criteria <> Null.NullString()) Then
                                    objSubFileItems = objSubFileItems.FindAll(Function(f As FileItem) f.Name.ToLower().Contains(criteria))
                                End If

                                If (objSubFileItems.Count > 0) Then
                                    For Each objSubFileItem As FileItem In objSubFileItems
                                        If (objSubFileItem.FileItemType = FileItemType.File) Then
                                            If (LinkSettings.ExtensionFilter.Trim() <> "") Then
                                                For Each extension As String In LinkSettings.ExtensionFilter.Split(","c)
                                                    If (extension.ToLower() = objSubFileItem.Extension.ToLower()) Then
                                                        ProcessItem(objSubFileItem, LinkSettings.TemplateSubItem.Split(delimiter), itemIndex)
                                                        Exit For
                                                    End If
                                                Next
                                            Else
                                                ProcessItem(objSubFileItem, LinkSettings.TemplateSubItem.Split(delimiter), itemIndex)
                                            End If
                                        End If
                                        itemIndex = itemIndex + 1
                                    Next
                                Else
                                    ProcessEmpty(LinkSettings.TemplateSubEmpty.Split(delimiter), itemIndex)
                                End If
                            End If
                        End If

                    End If

                    itemIndex = itemIndex + 1

                Next

                Dim objLiteralFooter As New Literal
                objLiteralFooter.Text = LinkSettings.TemplateFooter
                phFiles.Controls.Add(objLiteralFooter)

            Else

                ProcessEmpty(LinkSettings.TemplateEmpty.Split(delimiter), itemIndex)

                'Dim objLabel As New Label
                'objLabel.ID = Globals.CreateValidID("FileLinks-NoFiles-" & TabModuleId.ToString())
                'objLabel.Text = Localization.GetString("NoFiles", Me.LocalResourceFile)
                'objLabel.CssClass = "Normal"
                'phFiles.Controls.Add(objLabel)

            End If

        End Sub

        Private Function RemoveFileExtension(ByVal fileName As String) As String

            Dim extension As String = ""

            If (fileName.Length > 0) Then
                If (fileName.IndexOf("."c) <> -1) Then
                    If (fileName.LastIndexOf("."c) < fileName.Length) Then
                        extension = fileName.Substring(fileName.LastIndexOf("."c) + 1, fileName.Length - (fileName.LastIndexOf("."c) + 1))
                    End If
                End If
            End If

            Return fileName.Replace("." & extension, "")

        End Function

        Private Sub GetChildDirectories(ByVal objDirectory As DirectoryInfo, ByRef directoryPaths As List(Of DirectoryInfo))

            Dim objChildDirectories() As DirectoryInfo = objDirectory.GetDirectories()

            For Each objChildDirectory As DirectoryInfo In objChildDirectories
                directoryPaths.Insert(0, objChildDirectory)
                GetChildDirectories(objChildDirectory, directoryPaths)
            Next

        End Sub

        Private Sub GetChildFiles(ByVal objDirectory As DirectoryInfo, ByRef directoryFiles As List(Of System.IO.FileInfo))

            Dim objFiles() As System.IO.FileInfo = objDirectory.GetFiles()
            For Each objFile As System.IO.FileInfo In objFiles
                directoryFiles.Add(objFile)
            Next

            Dim objChildDirectories() As DirectoryInfo = objDirectory.GetDirectories()

            For Each objChildDirectory As DirectoryInfo In objChildDirectories
                GetChildFiles(objChildDirectory, directoryFiles)
            Next

        End Sub

        Private Function GetFileItems(ByVal folderID As Integer, ByVal getFolders As Boolean) As List(Of FileItem)

            Dim objFileItems As New List(Of FileItem)

            Dim objFiles As ArrayList = FileSystemUtils.GetFilesByFolder(PortalId, folderID)
            For Each objFile As DotNetNuke.Services.FileSystem.FileInfo In objFiles
                Dim objFileItem As New FileItem
                If (IO.File.Exists(objFile.PhysicalPath)) Then
                    objFileItem.DateModified = IO.File.GetLastWriteTime(objFile.PhysicalPath)
                    objFileItem.DateCreated = IO.File.GetCreationTime(objFile.PhysicalPath)
                Else
                    objFileItem.DateModified = DateTime.MinValue
                    objFileItem.DateCreated = DateTime.MinValue
                End If
                objFileItem.ContentType = objFile.ContentType
                objFileItem.Extension = objFile.Extension
                objFileItem.FileItemType = FileItemType.File
                objFileItem.Icon = GetIcon(objFileItem)
                objFileItem.InternalID = objFile.FileId
                objFileItem.Name = objFile.FileName
                objFileItem.Size = objFile.Size
                objFileItem.Folder = objFile.Folder
                If (objFile.StorageLocation = 1) Then
                    ' Secure
                    objFileItem.Link = DotNetNuke.Common.Globals.ApplicationPath & "/LinkClick.aspx?fileticket=" & UrlUtils.EncryptParameter(UrlUtils.GetParameterValue("fileid=" & objFile.FileId)) & "&tabid=" & TabId.ToString
                Else
                    objFileItem.Link = PortalSettings.HomeDirectory & objFile.Folder & objFile.FileName
                End If
                objFileItems.Add(objFileItem)
            Next
            objFileItems.Sort()

            If (getFolders) Then
                Dim objFolderItems As New List(Of FileItem)
                Dim objFolders As ArrayList = FileSystemUtils.GetFolders(PortalId)

                For Each objFolder As FolderInfo In objFolders
                    If (objFolder.FolderID = _folderID) Then
                        Dim objSubFolders As ArrayList = FileSystemUtils.GetFoldersByParentFolder(PortalId, objFolder.FolderPath)
                        For Each objSubFolder As FolderInfo In objSubFolders
                            Dim objFileItem As New FileItem
                            objFileItem.ContentType = ""
                            objFileItem.Extension = ""
                            objFileItem.FileItemType = FileItemType.Folder
                            objFileItem.Icon = GetIcon(objFileItem)
                            objFileItem.InternalID = objSubFolder.FolderID
                            objFileItem.Name = objSubFolder.FolderName
                            objFileItem.Size = Null.NullInteger
                            objFileItem.Folder = objSubFolder.FolderPath
                            objFileItem.Link = NavigateURL(TabId, "", "FolderID=" & objSubFolder.FolderID)
                            If (IO.Directory.Exists(objSubFolder.PhysicalPath)) Then
                                objFileItem.DateModified = IO.Directory.GetLastWriteTime(objSubFolder.PhysicalPath)
                                objFileItem.DateCreated = IO.Directory.GetCreationTime(objSubFolder.PhysicalPath)
                            Else
                                objFileItem.DateModified = DateTime.MinValue
                                objFileItem.DateCreated = DateTime.MinValue
                            End If
                            objFolderItems.Add(objFileItem)
                        Next
                        Exit For
                    End If
                Next
                objFolderItems.Sort()

                Dim index As Integer = 0
                For Each objFileItem As FileItem In objFolderItems
                    objFileItems.Insert(index, objFileItem)
                    index = index + 1
                Next
            End If

            Return objFileItems

        End Function

        Private Function GetIcon(ByVal objFileItem As FileItem) As String

            Dim url As String = ""
            Dim imageDirectory As String = "~/images/FileManager/Icons/"

            Try
                If objFileItem.FileItemType = FileItemType.Folder Then
                    url = imageDirectory & "ClosedFolder.gif"
                Else
                    If objFileItem.Extension <> "" AndAlso System.IO.File.Exists(Server.MapPath(imageDirectory & objFileItem.Extension & ".gif")) Then
                        url = imageDirectory + objFileItem.Extension + ".gif"
                    Else
                        url = imageDirectory & "File.gif"
                    End If
                End If
            Catch exc As Exception   'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
            Return url

        End Function

        Private Sub ProcessEmpty(ByVal templateTokens As String(), ByVal itemIndex As Integer)

            For iPtr As Integer = 0 To templateTokens.Length - 1 Step 2

                phFiles.Controls.Add(New LiteralControl(templateTokens(iPtr).ToString()))

                If iPtr < templateTokens.Length - 1 Then

                    Select Case templateTokens(iPtr + 1)

                        Case "CONTENTTYPE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = ""
                            phFiles.Controls.Add(objLiteral)

                        Case Else

                            If (templateTokens(iPtr + 1).ToUpper().StartsWith("RESX:")) Then
                                Dim variable As String = templateTokens(iPtr + 1).Substring(5, templateTokens(iPtr + 1).Length - 5)

                                If (variable <> "") Then
                                    Dim objLiteral As New Literal
                                    objLiteral.Text = Localization.GetString(variable, Me.LocalResourceFile)
                                    objLiteral.EnableViewState = False
                                    phFiles.Controls.Add(objLiteral)
                                    Exit Select
                                End If
                            End If

                            Dim objLiteralOther As New Literal
                            objLiteralOther.Text = "[" & templateTokens(iPtr + 1) & "]"
                            objLiteralOther.EnableViewState = False
                            phFiles.Controls.Add(objLiteralOther)

                    End Select

                End If

            Next

        End Sub

        Private Sub ProcessItem(ByVal objFileItem As FileItem, ByVal templateTokens As String(), ByVal itemIndex As Integer)

            For iPtr As Integer = 0 To templateTokens.Length - 1 Step 2

                phFiles.Controls.Add(New LiteralControl(templateTokens(iPtr).ToString()))

                If iPtr < templateTokens.Length - 1 Then

                    Select Case templateTokens(iPtr + 1)

                        Case "CONTENTTYPE"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFileItem.ContentType
                            phFiles.Controls.Add(objLiteral)

                        Case "DATECREATED"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFileItem.DateCreated.ToString()
                            phFiles.Controls.Add(objLiteral)

                        Case "DATEMODIFIED"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFileItem.DateModified.ToString()
                            phFiles.Controls.Add(objLiteral)

                        Case "DELETE"
                            If (objFileItem.FileItemType = FileItemType.File) Then
                                If (ModuleSecurity.DeletePermission) Then
                                    Dim cmdDelete As New ImageButton
                                    cmdDelete.ImageUrl = "~\Images\Delete.gif"
                                    cmdDelete.AlternateText = Localization.GetString("Delete", Me.LocalResourceFile)
                                    cmdDelete.ImageAlign = ImageAlign.AbsMiddle
                                    cmdDelete.CommandArgument = objFileItem.Name.ToString()
                                    cmdDelete.CommandName = objFileItem.FileItemType.ToString()
                                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")
                                    phFiles.Controls.Add(cmdDelete)

                                    AddHandler CType(phFiles.Controls(phFiles.Controls.Count - 1), ImageButton).Command, AddressOf cmdDelete_Command
                                End If
                            End If

                            If (objFileItem.FileItemType = FileItemType.Folder) Then
                                If (ModuleSecurity.DeleteFolderPermission) Then
                                    Dim cmdDelete As New ImageButton
                                    cmdDelete.ImageUrl = "~\Images\Delete.gif"
                                    cmdDelete.AlternateText = Localization.GetString("Delete", Me.LocalResourceFile)
                                    cmdDelete.ImageAlign = ImageAlign.AbsMiddle
                                    cmdDelete.CommandArgument = objFileItem.InternalID.ToString()
                                    cmdDelete.CommandName = objFileItem.FileItemType.ToString()
                                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("Confirmation", LocalResourceFile) & "');")
                                    phFiles.Controls.Add(cmdDelete)

                                    AddHandler CType(phFiles.Controls(phFiles.Controls.Count - 1), ImageButton).Command, AddressOf cmdDeleteFolder_Command
                                End If
                            End If

                        Case "DESCRIPTION"
                            If (objFileItem.FileItemType = FileItemType.File) Then
                                Dim objDescriptionController As New DescriptionController()
                                Dim objDescriptionList As List(Of DescriptionInfo) = objDescriptionController.List(ModuleId)

                                For Each objDescription As DescriptionInfo In objDescriptionList
                                    If (objDescription.Path.ToLower() = objFileItem.Link.ToLower()) Then
                                        Dim objLiteral As New Literal
                                        objLiteral.Text = objDescription.Description
                                        phFiles.Controls.Add(objLiteral)
                                        Exit For
                                    End If
                                Next
                            End If

                        Case "DOWNLOADCOUNT"
                            If (objFileItem.FileItemType = FileItemType.File) Then
                                Dim objTrackController As New TrackController
                                Dim count As Integer = objTrackController.GetCount(ModuleId, objFileItem.Link)
                                Dim objLiteral As New Literal
                                objLiteral.Text = count.ToString()
                                phFiles.Controls.Add(objLiteral)
                            End If

                        Case "EDIT"
                            If (objFileItem.FileItemType = FileItemType.File) Then
                                If (ModuleSecurity.DeletePermission) Then
                                    Dim description = ""

                                    Dim objDescriptionController As New DescriptionController()
                                    Dim objDescriptionList As List(Of DescriptionInfo) = objDescriptionController.List(ModuleId)

                                    For Each objDescription As DescriptionInfo In objDescriptionList
                                        If (objDescription.Path.ToLower() = objFileItem.Link.ToLower()) Then
                                            description = objDescription.Description
                                        End If
                                    Next

                                    Dim objLiteral As New Literal
                                    objLiteral.Text = "<a href=""" & objFileItem.Link & """ Description=""" & description & """ FileName=""" & objFileItem.Name & """ class=""EditFile""><img border=""0"" alt=""Edit"" src=""" & Page.ResolveUrl("~/images/edit.gif") & """></a>"
                                    phFiles.Controls.Add(objLiteral)
                                End If
                            End If

                        Case "EXTENSION"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFileItem.Extension
                            phFiles.Controls.Add(objLiteral)

                        Case "LINK"
                            Dim objLiteral As New Literal
                            If (objFileItem.FileItemType = FileItemType.Folder) Then
                                If (Settings.ContainsKey("SecureFolder-" & objFileItem.InternalID.ToString())) Then
                                    If (ModuleSecurity.ViewSecurePermission Or IsEditable) Then
                                        objLiteral.Text = objFileItem.Link
                                    Else
                                        If (LinkSettings.SecureUrl <> "") Then
                                            objLiteral.Text = LinkSettings.SecureUrl
                                        Else
                                            objLiteral.Text = "#"
                                        End If
                                    End If
                                Else
                                    objLiteral.Text = objFileItem.Link
                                End If
                            Else
                                If (LinkSettings.TrackDownloads) Then
                                    'objLiteral.Text = EditUrl("Path", Server.UrlEncode(objFileItem.Link), "ServeFile")
                                    objLiteral.Text = Page.ResolveUrl("~/DesktopModules/FileLinks/RedirectFile.ashx?Path=" & Server.UrlEncode(objFileItem.Link) & "&M=" & ModuleId.ToString() & "&U=" & UserId.ToString())
                                Else
                                    objLiteral.Text = objFileItem.Link
                                End If
                            End If
                            phFiles.Controls.Add(objLiteral)

                        Case "ICON"
                            Dim objImage As New Image
                            objImage.ImageUrl = objFileItem.Icon
                            phFiles.Controls.Add(objImage)

                        Case "ICONURL"
                            Dim objLiteral As New Literal
                            If (objFileItem.FileItemType = FileItemType.Folder) Then
                                If (Settings.ContainsKey("SecureFolder-" & objFileItem.InternalID.ToString())) Then
                                    objLiteral.Text = Page.ResolveUrl("~\DesktopModules\FileLinks\Images\folder_lock.gif")
                                Else
                                    objLiteral.Text = Page.ResolveUrl(objFileItem.Icon)
                                End If
                            Else
                                objLiteral.Text = Page.ResolveUrl(objFileItem.Icon)
                            End If
                            phFiles.Controls.Add(objLiteral)

                        Case "ISALTERNATE"
                            If (itemIndex Mod 2) Then
                                While (iPtr < templateTokens.Length - 1)
                                    If (templateTokens(iPtr + 1) = "/ISALTERNATE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISALTERNATE"
                            ' Do Nothing

                        Case "ISFILE"
                            If (objFileItem.FileItemType <> FileItemType.File) Then
                                While (iPtr < templateTokens.Length - 1)
                                    If (templateTokens(iPtr + 1) = "/ISFILE") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISFILE"
                            ' Do Nothing

                        Case "ISFOLDER"
                            If (objFileItem.FileItemType <> FileItemType.Folder) Then
                                While (iPtr < templateTokens.Length - 1)
                                    If (templateTokens(iPtr + 1) = "/ISFOLDER") Then
                                        Exit While
                                    End If
                                    iPtr = iPtr + 1
                                End While
                            End If

                        Case "/ISFOLDER"
                            ' Do Nothing

                        Case "NAME"
                            Dim objLiteral As New Literal
                            objLiteral.Text = objFileItem.Name
                            phFiles.Controls.Add(objLiteral)

                        Case "NAMENOEXTENSION"
                            Dim objLiteral As New Literal
                            Try
                                objLiteral.Text = RemoveFileExtension(objFileItem.Name)
                            Catch
                                objLiteral.Text = objFileItem.Name
                            End Try
                            phFiles.Controls.Add(objLiteral)

                        Case "SECURE"
                            If (objFileItem.FileItemType = FileItemType.Folder) Then
                                If (ModuleSecurity.SecureFolderPermission Or IsEditable) Then
                                    Dim cmdSecure As New ImageButton
                                    If (Settings.ContainsKey("SecureFolder-" & objFileItem.InternalID.ToString())) Then
                                        cmdSecure.ImageUrl = "~\DesktopModules\FileLinks\Images\lock.gif"
                                        cmdSecure.CommandName = "Unlock" & objFileItem.FileItemType.ToString()
                                    Else
                                        cmdSecure.ImageUrl = "~\DesktopModules\FileLinks\Images\lock_open.gif"
                                        cmdSecure.CommandName = "Lock" & objFileItem.FileItemType.ToString()
                                    End If
                                    cmdSecure.AlternateText = Localization.GetString("SecureFolder", Me.LocalResourceFile)
                                    cmdSecure.ImageAlign = ImageAlign.AbsMiddle
                                    cmdSecure.CommandArgument = objFileItem.InternalID.ToString()
                                    phFiles.Controls.Add(cmdSecure)

                                    AddHandler CType(phFiles.Controls(phFiles.Controls.Count - 1), ImageButton).Command, AddressOf cmdSecureFolder_Command
                                End If
                            End If

                        Case "SIZE"
                            If (objFileItem.FileItemType = FileItemType.File) Then
                                Dim objLiteral As New Literal
                                objLiteral.Text = (objFileItem.Size \ 1000).ToString("##,##0")
                                phFiles.Controls.Add(objLiteral)
                            End If

                        Case Else

                            If (templateTokens(iPtr + 1).ToUpper().StartsWith("DATECREATED:")) Then
                                Dim field As String = templateTokens(iPtr + 1).Substring(12, templateTokens(iPtr + 1).Length - 12).Trim()
                                Dim objLiteral As New Literal
                                objLiteral.Text = objFileItem.DateCreated.ToString(field)
                                phFiles.Controls.Add(objLiteral)
                            End If

                            If (templateTokens(iPtr + 1).ToUpper().StartsWith("DATEMODIFIED:")) Then
                                Dim field As String = templateTokens(iPtr + 1).Substring(13, templateTokens(iPtr + 1).Length - 13).Trim()
                                Dim objLiteral As New Literal
                                objLiteral.Text = objFileItem.DateCreated.ToString(field)
                                phFiles.Controls.Add(objLiteral)
                            End If

                    End Select

                End If

            Next

        End Sub

        Private Sub ReadQueryString()

            If (Request("FolderID") <> "" AndAlso IsNumeric(Request("FolderID"))) Then
                _folderID = Convert.ToInt32(Request("FolderID"))
                If (Settings.ContainsKey("SecureFolder-" & _folderID.ToString())) Then
                    If (IsEditable = False And ModuleSecurity.ViewSecurePermission = False) Then
                        If (LinkSettings.SecureUrl <> "") Then
                            Response.Redirect(LinkSettings.SecureUrl, True)
                        Else
                            Response.Redirect(NavigateURL(), True)
                        End If
                    End If
                End If

                Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalId)
                For Each folder As FolderInfo In folders
                    If (folder.FolderID = _folderID) Then
                        Return
                    End If
                Next
                _folderID = Null.NullInteger
            End If

            If (Request("FileSearch") <> "") Then
                _search = Server.UrlDecode(Request("FileSearch"))
            End If

        End Sub

        Private Sub RegisterScripts()

            If (HttpContext.Current.Items("FileLinks-ScriptsRegistered") Is Nothing) Then

                If (HttpContext.Current.Items("jquery_registered") Is Nothing And HttpContext.Current.Items("jQueryRequested") Is Nothing) Then

                    Dim objCSS As System.Web.UI.Control = BasePage.FindControl("CSS")

                    If Not (objCSS Is Nothing) Then

                        Dim litLink As New Literal
                        If (LinkSettings.TermsEnabled) Then
                            litLink.Text = "" & vbCrLf _
                                                        & "<script type=""text/javascript"" src='" & Me.ResolveUrl("JS/Confirm/jquery.js") & "'></script>" & vbCrLf
                        End If
                        objCSS.Controls.Add(litLink)

                        If (LinkSettings.TermsEnabled) Then
                            litLink.Text = "" & vbCrLf _
                            & "<script type=""text/javascript"" src='" & Me.ResolveUrl("JS/Confirm/jquery.simplemodal.js") & "'></script>" & vbCrLf _
                            & "<script type=""text/javascript"" src='" & Me.ResolveUrl("JS/Confirm/terms.js") & "'></script>" & vbCrLf
                        Else
                            litLink.Text = "" & vbCrLf _
                                                    & "<script type=""text/javascript"" src='" & Me.ResolveUrl("JS/Confirm/jquery.simplemodal.js") & "'></script>" & vbCrLf

                        End If
                        phFiles.Controls.Add(litLink)
                    End If

                Else

                    Dim litLink As New Literal
                    If (LinkSettings.TermsEnabled) Then
                        litLink.Text = "" & vbCrLf _
                        & "<script type=""text/javascript"" src='" & Me.ResolveUrl("JS/Confirm/jquery.simplemodal.js") & "'></script>" & vbCrLf _
                        & "<script type=""text/javascript"" src='" & Me.ResolveUrl("JS/Confirm/terms.js") & "'></script>" & vbCrLf
                    Else
                        litLink.Text = "" & vbCrLf _
                                                & "<script type=""text/javascript"" src='" & Me.ResolveUrl("JS/Confirm/jquery.simplemodal.js") & "'></script>" & vbCrLf

                    End If
                    phFiles.Controls.Add(litLink)

                End If

                HttpContext.Current.Items.Add("FileLinks-ScriptsRegistered", "true")

            End If

            If (LinkSettings.TermsEnabled) Then
                phTerms.Visible = True
                litTerms.Text = LinkSettings.TermsTemplate
            End If

        End Sub

#End Region

#Region " Private Strutures "

        Private Class FileItem
            Implements IComparable

            Public InternalID As Integer
            Public FileItemType As FileItemType
            Public Icon As String
            Public Link As String
            Public Name As String
            Public DateModified As DateTime
            Public DateCreated As DateTime
            Public Extension As String
            Public Size As Integer
            Public ContentType As String
            Public Folder As String

            Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo

                If TypeOf obj Is FileItem Then

                    Dim objFileItem As FileItem = CType(obj, FileItem)

                    If Not objFileItem Is Nothing Then

                        Select Case (_Default.SortBy)
                            Case SortType.ModifiedDate
                                If (_Default.SortDirection = WebControls.SortDirection.Ascending) Then
                                    Return Me.DateModified.CompareTo(objFileItem.DateModified)
                                Else
                                    Return Me.DateModified.CompareTo(objFileItem.DateModified) * -1
                                End If
                            Case SortType.Name
                                If (_Default.SortDirection = WebControls.SortDirection.Ascending) Then
                                    Return Me.Name.CompareTo(objFileItem.Name)
                                Else
                                    Return Me.Name.CompareTo(objFileItem.Name) * -1
                                End If
                            Case SortType.Size
                                If (_Default.SortDirection = WebControls.SortDirection.Ascending) Then
                                    Return Me.Size.CompareTo(objFileItem.Size)
                                Else
                                    Return Me.Size.CompareTo(objFileItem.Size) * -1
                                End If
                        End Select

                        Return Me.Name.CompareTo(objFileItem.Name)

                    End If

                End If

                Return -1

            End Function

        End Class

        Private Enum FileItemType

            File
            Folder

        End Enum

        Public Shared SortBy As SortType = FileLinks.Common.Constants.SORT_BY_DEFAULT
        Public Shared SortDirection As SortDirection = FileLinks.Common.Constants.SORT_DIRECTION_DEFAULT

#End Region

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try
                Dim objModuleSecurity As New ModuleSecurity(Me.ModuleConfiguration, Me.Settings)

                If (LinkSettings.FolderID <> Null.NullInteger) Then
                    ReadQueryString()
                    If (_folderID <> Null.NullInteger AndAlso LinkSettings.FolderID <> _folderID) Then
                        BindBreadCrumbs()
                    End If
                    _Default.SortBy = LinkSettings.SortBy
                    _Default.SortDirection = LinkSettings.SortDirection
                    BindFileItems()

                    If (IsEditable OrElse objModuleSecurity.UploadPermission) Then
                        pnlCommandBar.Visible = True
                        lnkAddNewFiles.Visible = True
                        If (Request("FolderID") <> "") Then
                            lnkAddNewFiles.NavigateUrl = EditUrl("FolderID", Request("FolderID"), "AddFiles")
                        Else
                            lnkAddNewFiles.NavigateUrl = EditUrl("AddFiles")
                        End If
                    End If

                    If (IsEditable OrElse objModuleSecurity.AddFolderPermission) Then
                        pnlCommandBar.Visible = True
                        lnkAddFolder.Visible = True
                        If (Request("FolderID") <> "") Then
                            lnkAddFolder.NavigateUrl = EditUrl("FolderID", Request("FolderID"), "AddFolder")
                        Else
                            lnkAddFolder.NavigateUrl = EditUrl("AddFolder")
                        End If
                    End If

                    If (_search <> "") Then
                        phSearchCriteria.Visible = True
                        Dim criteria As String = Localization.GetString("FilterSearch", Me.LocalResourceFile)
                        If (criteria.Contains("{0}")) Then
                            lblSearch.Text = String.Format(criteria, _search)
                        Else
                            lblSearch.Text = criteria
                        End If
                        lnkSearchClear.NavigateUrl = NavigateURL(TabId)
                    End If
                    
                Else
                    lblConfigure.Visible = True
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            Try

                If (LinkSettings.FolderID <> Null.NullInteger) Then
                    RegisterScripts()
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub cmdDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)

            If (e.CommandName = "File") Then

                Dim folderPath As String = ""
                If (_folderID <> Null.NullInteger) Then
                    Dim objFolders As ArrayList = FileSystemUtils.GetFolders(PortalId)
                    For Each objFolder As FolderInfo In objFolders
                        If (objFolder.FolderID = _folderID) Then
                            folderPath = objFolder.FolderPath
                            Exit For
                        End If
                    Next
                Else
                    If (LinkSettings.FolderID <> Null.NullInteger) Then
                        Dim objFolders As ArrayList = FileSystemUtils.GetFolders(PortalId)
                        For Each objFolder As FolderInfo In objFolders
                            If (objFolder.FolderID = LinkSettings.FolderID) Then
                                folderPath = objFolder.FolderPath
                                Exit For
                            End If
                        Next
                    End If
                End If

                If (System.IO.File.Exists(PortalSettings.HomeDirectoryMapPath & folderPath.Replace("/"c, "\"c) & e.CommandArgument)) Then
                    FileSystemUtils.DeleteFile(PortalSettings.HomeDirectoryMapPath & folderPath.Replace("/"c, "\"c) & e.CommandArgument, PortalSettings, True)
                End If

                Response.Redirect(Request.RawUrl, True)

            End If

        End Sub

        Private Sub cmdDeleteFolder_Command(ByVal sender As Object, ByVal e As CommandEventArgs)

            If (e.CommandName = "Folder") Then

                Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalSettings.PortalId)

                For Each folder As FolderInfo In folders
                    If (folder.FolderID = e.CommandArgument) Then
                        Dim objDirectory As New DirectoryInfo(folder.PhysicalPath)
                        If (objDirectory.Exists) Then

                            Dim directoryFiles As New List(Of System.IO.FileInfo)
                            GetChildFiles(objDirectory, directoryFiles)
                            For Each objFile In directoryFiles
                                FileSystemUtils.DeleteFile(objFile.FullName, PortalSettings)
                            Next

                            Dim directoryPaths As New List(Of DirectoryInfo)
                            GetChildDirectories(objDirectory, directoryPaths)
                            directoryPaths.Add(objDirectory)

                            For Each objDirectory In directoryPaths
                                FileSystemUtils.DeleteFolder(PortalId, objDirectory, objDirectory.Name)
                                FileSystemUtils.SynchronizeFolder(Me.PortalId, objDirectory.FullName, objDirectory.FullName.Replace(PortalSettings.HomeDirectoryMapPath, "").Replace("\", "/"), True)
                            Next

                        End If
                        Exit For
                    End If
                Next

                Response.Redirect(Request.RawUrl, True)

            End If

        End Sub

        Private Sub cmdSecureFolder_Command(ByVal sender As Object, ByVal e As CommandEventArgs)

            If (e.CommandName = "LockFolder") Then

                Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalSettings.PortalId)

                For Each folder As FolderInfo In folders
                    If (folder.FolderID = e.CommandArgument) Then

                        Dim objModuleController As New ModuleController

                        Dim objDirectory As New DirectoryInfo(folder.PhysicalPath)
                        If (objDirectory.Exists) Then

                            Dim directoryPaths As New List(Of DirectoryInfo)
                            GetChildDirectories(objDirectory, directoryPaths)
                            directoryPaths.Add(objDirectory)

                            For Each objDirectory In directoryPaths
                                For Each f As FolderInfo In folders
                                    If (objDirectory.FullName.ToLower().TrimEnd("\") = (PortalSettings.HomeDirectoryMapPath & f.FolderPath.Replace("/", "\")).ToLower().TrimEnd("\")) Then
                                        objModuleController.UpdateModuleSetting(Me.ModuleId, "SecureFolder-" & f.FolderID.ToString(), "True")
                                        Exit For
                                    End If
                                Next
                            Next

                        End If

                        Exit For
                    End If
                Next

            End If

            If (e.CommandName = "UnlockFolder") Then


                Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalSettings.PortalId)

                For Each folder As FolderInfo In folders
                    If (folder.FolderID = e.CommandArgument) Then

                        Dim objModuleController As New ModuleController

                        Dim objDirectory As New DirectoryInfo(folder.PhysicalPath)
                        If (objDirectory.Exists) Then

                            Dim directoryPaths As New List(Of DirectoryInfo)
                            GetChildDirectories(objDirectory, directoryPaths)
                            directoryPaths.Add(objDirectory)

                            For Each objDirectory In directoryPaths
                                For Each f As FolderInfo In folders
                                    If (objDirectory.FullName.ToLower().TrimEnd("\") = (PortalSettings.HomeDirectoryMapPath & f.FolderPath.Replace("/", "\")).ToLower().TrimEnd("\")) Then
                                        objModuleController.DeleteModuleSetting(Me.ModuleId, "SecureFolder-" & f.FolderID.ToString())
                                        Exit For
                                    End If
                                Next
                            Next

                        End If

                        Exit For
                    End If
                Next

            End If

            Response.Redirect(Request.RawUrl, True)

        End Sub

        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click

            Try

                If (txtSearch.Text.Trim() <> "") Then

                    Response.Redirect(NavigateURL(TabId, "", "FileSearch=" & Server.UrlEncode(txtSearch.Text)), True)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

#Region " Optional Interfaces "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements DotNetNuke.Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New DotNetNuke.Entities.Modules.Actions.ModuleActionCollection

                If (LinkSettings.FolderID <> Null.NullInteger) Then
                    Dim objModuleSecurity As New ModuleSecurity(Me.ModuleConfiguration, Me.Settings)
                    If (IsEditable OrElse objModuleSecurity.UploadPermission) Then
                        If (Request("FolderID") <> "") Then
                            Actions.Add(GetNextActionID, Localization.GetString("AddFiles.Text", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("FolderID", Request("FolderID"), "AddFiles"), False, SecurityAccessLevel.View, True, False)
                        Else
                            Actions.Add(GetNextActionID, Localization.GetString("AddFiles.Text", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("AddFiles"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If

                    If (IsEditable OrElse objModuleSecurity.AddFolderPermission) Then
                        If (Request("FolderID") <> "") Then
                            Actions.Add(GetNextActionID, Localization.GetString("AddFolder.Text", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("FolderID", Request("FolderID"), "AddFolder"), False, SecurityAccessLevel.View, True, False)
                        Else
                            Actions.Add(GetNextActionID, Localization.GetString("AddFolder.Text", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("AddFolder"), False, SecurityAccessLevel.View, True, False)
                        End If
                    End If
                End If

                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace