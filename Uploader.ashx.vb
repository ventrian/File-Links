Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.FileSystem
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports Ventrian.FileLinks.Entities
Imports System.Drawing.Drawing2D

Namespace Ventrian.FileLinks

    Public Class Uploader
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _folderID As Integer = Null.NullInteger
        Private _portalID As Integer = Null.NullInteger
        Private _moduleID As Integer = Null.NullInteger
        Private _tabID As Integer = Null.NullInteger
        Private _ticket As String = Null.NullString
        Private _userID As Integer = Null.NullInteger

        Private _context As HttpContext

#End Region

#Region " Private Methods "

        Private Sub AuthenticateUserFromTicket()

            If (_ticket <> "") Then

                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(_ticket)
                Dim fi As FormsIdentity = New FormsIdentity(ticket)

                Dim roles As String() = Nothing
                HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(fi, roles)

                Dim objUser As UserInfo = UserController.GetUserByName(_portalID, HttpContext.Current.User.Identity.Name)

                If Not (objUser Is Nothing) Then
                    _userID = objUser.UserID
                    HttpContext.Current.Items("UserInfo") = objUser

                    Dim objRoleController As New RoleController
                    roles = objRoleController.GetRolesByUser(_userID, _portalID)

                    Dim strPortalRoles As String = Join(roles, New Char() {";"c})
                    _context.Items.Add("UserRoles", ";" + strPortalRoles + ";")
                End If

            End If

        End Sub

        Private Function ExtractFileExtension(ByVal fileName As String) As String

            Dim extension As String = ""

            If (fileName.Length > 0) Then
                If (fileName.IndexOf("."c) <> -1) Then
                    If (fileName.LastIndexOf("."c) < fileName.Length) Then
                        extension = fileName.Substring(fileName.LastIndexOf("."c) + 1, fileName.Length - (fileName.LastIndexOf("."c) + 1))
                    End If
                End If
            End If

            Return extension

        End Function

        Private Function ExtractFileName(ByVal path As String) As String

            Dim extractPos As Integer = path.LastIndexOf("\") + 1
            Return path.Substring(extractPos, path.Length - extractPos)

        End Function

        Private Sub ReadQueryString()

            If (_context.Request("FolderID") <> "") Then
                _folderID = Convert.ToInt32(_context.Request("FolderID"))
            End If

            If (_context.Request("ModuleID") <> "") Then
                _moduleID = Convert.ToInt32(_context.Request("ModuleID"))
            End If

            If (_context.Request("TabID") <> "") Then
                _tabID = Convert.ToInt32(_context.Request("TabID"))
            End If

            If (_context.Request("PortalID") <> "") Then
                _portalID = Convert.ToInt32(_context.Request("PortalID"))
            End If

            If (_context.Request("Ticket") <> "") Then
                _ticket = _context.Request("Ticket")
            End If

        End Sub

        Private Function RemoveExtension(ByVal fileName As String) As String

            Dim name As String = ""

            If (fileName.Length > 0) Then
                If (fileName.IndexOf("."c) <> -1) Then
                    name = fileName.Substring(0, fileName.LastIndexOf("."c))
                End If
            End If

            Return name

        End Function

#End Region

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            context.Response.ContentType = "text/plain"

            _context = context

            ReadQueryString()
            AuthenticateUserFromTicket()

            If (_ticket <> "") Then
                If (_context.Request.IsAuthenticated = False) Then
                    _context.Response.Write("-2")
                    _context.Response.End()
                End If
            End If

            Dim objModuleController As New ModuleController()
            Dim objModule As ModuleInfo = objModuleController.GetModule(_moduleID, _tabID, False)

            If (objModule IsNot Nothing) Then
                ' TODO: this section doesn't work yet.
                'Dim objModuleSettings As Hashtable = objModuleController.GetModuleSettings(_moduleID)
                'Dim objModuleSecurity As New ModuleSecurity(objModule, objModuleSettings)
                'If (objModuleSecurity.UploadPermission = False) Then
                '    Response.Write("-2")
                '    Response.End()
                'End If
            Else
                If (_context.Request.IsAuthenticated = False) Then
                    _context.Response.Write("-2")
                    _context.Response.End()
                End If
            End If


            Dim objFile As HttpPostedFile = _context.Request.Files("Filedata")
            If (objFile IsNot Nothing) Then
                Dim objPortalController As New PortalController()
                If (objPortalController.HasSpaceAvailable(_portalID, objFile.ContentLength) = False) Then
                    _context.Response.Write("-1")
                    _context.Response.End()
                End If

                Dim result As String = ""

                Dim filePath As String = PortalController.GetCurrentPortalSettings().HomeDirectoryMapPath
                If (_folderID <> Null.NullInteger) Then
                    Dim objFolders As ArrayList = FileSystemUtils.GetFolders(_portalID)
                    For Each objFolder As FolderInfo In objFolders
                        If (objFolder.FolderID = _folderID) Then
                            filePath = objFolder.PhysicalPath
                            Exit For
                        End If
                    Next
                End If

                Dim fileName As String = ExtractFileName(objFile.FileName)
                Dim fileExtension As String = ExtractFileExtension(fileName)

                If (fileExtension = "jpg" Or fileExtension = "jpeg" Or fileExtension = "gif" Or fileExtension = "png") Then

                    Dim objModuleSettings As Hashtable = objModuleController.GetModuleSettings(_moduleID)
                    Dim objLinkSettings As New Entities.LinkSettings(objModuleSettings)
                    If (objLinkSettings.ResizeImages) Then

                        Dim photo As Drawing.Image = Drawing.Image.FromStream(objFile.InputStream)

                        Dim width As Integer = photo.Width
                        Dim height As Integer = photo.Height

                        If (width > objLinkSettings.ImageWidth) Then
                            width = objLinkSettings.ImageWidth
                            height = Convert.ToInt32(height / (photo.Width / objLinkSettings.ImageWidth))
                        End If

                        If (height > objLinkSettings.ImageHeight) Then
                            height = objLinkSettings.ImageHeight
                            width = Convert.ToInt32(photo.Width / (photo.Height / objLinkSettings.ImageHeight))
                        End If

                        Dim fileNameWithoutExtension As String = RemoveExtension(fileName).Replace("/", "_").Replace(".", "_").Replace("%", "_")

                        If (objLinkSettings.RenameImages) Then
                            fileNameWithoutExtension = fileNameWithoutExtension & width.ToString() & "x" & height.ToString()
                        End If

                        fileName = fileNameWithoutExtension & "." & fileExtension

                        If (File.Exists(filePath & fileName)) Then
                            For i As Integer = 1 To 1000
                                If Not (File.Exists(filePath & fileNameWithoutExtension & "_" & i.ToString() & "." & fileExtension)) Then
                                    fileName = fileNameWithoutExtension & "_" & i.ToString() & "." & fileExtension
                                    fileNameWithoutExtension = fileNameWithoutExtension & "_" & i.ToString()
                                    Exit For
                                End If
                            Next
                        End If

                        Dim bmp As New Bitmap(width, height)
                        Dim g As Graphics = Graphics.FromImage(DirectCast(bmp, Drawing.Image))

                        If (objLinkSettings.Compression = CompressionType.Quality) Then
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic
                            g.SmoothingMode = SmoothingMode.HighQuality
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality
                            g.CompositingQuality = CompositingQuality.HighQuality
                        End If

                        g.DrawImage(photo, 0, 0, width, height)

                        photo.Dispose()


                        If (objLinkSettings.Compression = CompressionType.Quality) Then
                            Dim info As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders()
                            Dim params As New EncoderParameters
                            params.Param(0) = New EncoderParameter(Encoder.Quality, 90L)
                            bmp.Save(filePath & fileName, info(1), params)
                        Else
                            bmp.Save(filePath & fileName, ImageFormat.Jpeg)
                        End If

                        bmp.Dispose()

                        ' Update DNN File Meta Info
                        Dim strFileName As String = Path.GetFileName(filePath & fileName)
                        Dim strFolderpath As String = DotNetNuke.Common.Globals.GetSubFolderPath(filePath & fileName)
                        Dim finfo As New System.IO.FileInfo(filePath & fileName)

                        Dim strContentType As String = ""
                        Dim strExtension As String = Path.GetExtension(fileName).Replace(".", "")

                        Select Case strExtension
                            Case "jpg", "jpeg" : strContentType = "image/jpeg"
                            Case "gif" : strContentType = "image/gif"
                            Case "png" : strContentType = "image/png"
                            Case Else : strContentType = "application/octet-stream"
                        End Select

                        Dim folderID As Integer = Null.NullInteger
                        Dim objFolderController As New FolderController
                        Dim folder As FolderInfo = objFolderController.GetFolder(_portalID, strFolderpath, False)
                        If (folder Is Nothing) Then
                            folderID = objFolderController.AddFolder(_portalID, strFolderpath)
                        Else
                            folderID = folder.FolderID
                        End If

                        If (strFileName.IndexOf("'") = -1) Then
                            Dim objFiles As New FileController
                            objFiles.AddFile(_portalID, strFileName, strExtension, finfo.Length, width, height, strContentType, strFolderpath, folderID, True)
                        End If

                    Else
                        FileSystemUtils.UploadFile(filePath, objFile)
                    End If

                Else
                    FileSystemUtils.UploadFile(filePath, objFile)
                End If

                If (result <> "") Then
                    _context.Response.Write("-3")
                    _context.Response.End()
                End If

                Dim objFileController As New FileController
                Dim objSavedFile As DotNetNuke.Services.FileSystem.FileInfo = objFileController.GetFile(fileName, _portalID, _folderID)
                If (objSavedFile IsNot Nothing) Then
                    Select Case objSavedFile.Extension.ToLower()
                        Case "css"
                            objSavedFile.ContentType = "text/css"
                        Case "gif"
                            objSavedFile.ContentType = "image/gif"
                        Case "htm"
                            objSavedFile.ContentType = "text/html"
                        Case "html"
                            objSavedFile.ContentType = "text/html"
                        Case "jpg"
                            objSavedFile.ContentType = "image/jpeg"
                        Case "png"
                            objSavedFile.ContentType = "image/png"
                        Case "xml"
                            objSavedFile.ContentType = "text/xml"
                    End Select

                    objFileController.UpdateFile(objSavedFile.FileId, objSavedFile.FileName, objSavedFile.Extension, objSavedFile.Size, objSavedFile.Width, objSavedFile.Height, objSavedFile.ContentType, objSavedFile.Folder, objSavedFile.FolderId)
                End If

                _context.Response.Write(objFile.FileName)
            End If

        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class

End Namespace