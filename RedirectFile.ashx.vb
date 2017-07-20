Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions

Namespace Ventrian.FileLinks

    Public Class RedirectFile
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _filePath As String = Null.NullString
        Private _moduleID As Integer = Null.NullInteger
        Private _userID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub DoRedirect(ByVal context As HttpContext)

            context.Response.Redirect(_filePath, True)

        End Sub

        Private Sub ReadQueryString(ByVal context As HttpContext)

            If (context.Request("Path") <> "") Then
                _filePath = context.Server.UrlDecode(context.Request("Path"))
            End If
            If (context.Request("M") <> "") Then
                If (IsNumeric(context.Request("M"))) Then
                    _moduleID = Convert.ToInt32(context.Request("M"))
                End If
            End If
            If (context.Request("U") <> "") Then
                If (IsNumeric(context.Request("U"))) Then
                    _userID = Convert.ToInt32(context.Request("U"))
                End If
            End If

        End Sub

        Private Sub TrackDownload(ByVal context As HttpContext)

            Dim objTrackInfo As New TrackInfo

            objTrackInfo.TrackDate = DateTime.Now
            objTrackInfo.ModuleID = _moduleID
            objTrackInfo.Path = _filePath
            objTrackInfo.UserID = _userID
            objTrackInfo.IPAddress = context.Request.UserHostAddress

            Dim objTrackController As New TrackController
            objTrackController.Add(objTrackInfo)

        End Sub

#End Region

#Region " Event Handlers "

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            context.Response.ContentType = "text/plain"

            ReadQueryString(context)

            If (_filePath <> Null.NullString And _moduleID <> Null.NullInteger) Then
                TrackDownload(context)
                DoRedirect(context)
            Else
                context.Response.Write("Unable to redirect file")
            End If


        End Sub

#End Region

#Region " Properties "

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

#End Region

    End Class

End Namespace