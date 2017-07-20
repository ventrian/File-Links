Imports System.Web
Imports System.Web.Services

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions

Imports Ventrian.FileLinks.Base
Imports Ventrian.FileLinks.Data
Imports Ventrian.FileLinks.Entities

Namespace Ventrian.FileLinks

    Public Class SaveDesc
        Implements System.Web.IHttpHandler

#Region " Private Members "

        Private _filePath As String = Null.NullString
        Private _fileDescription As String = Null.NullString
        Private _moduleID As Integer = Null.NullInteger

#End Region

#Region " Private Methods "

        Private Sub ReadQueryString(ByVal context As HttpContext)

            If (context.Request("Path") <> "") Then
                _filePath = context.Server.UrlDecode(context.Request("Path"))
            End If

            If (context.Request("Description") <> "") Then
                _fileDescription = context.Server.UrlDecode(context.Request("Description"))
            End If

            If (context.Request("M") <> "") Then
                If (IsNumeric(context.Request("M"))) Then
                    _moduleID = Convert.ToInt32(context.Request("M"))
                End If
            End If

        End Sub

#End Region

#Region " Event Handlers "

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            context.Response.ContentType = "text/plain"

            ReadQueryString(context)

            If (_filePath <> Null.NullString And _fileDescription <> Null.NullString And _moduleID <> Null.NullInteger) Then

                Dim objDescription As New DescriptionInfo
                objDescription.ModuleID = _moduleID
                objDescription.Description = _fileDescription
                objDescription.Path = _filePath

                Dim objDescriptionController As New DescriptionController
                objDescriptionController.Update(objDescription)

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