Imports DotNetNuke.Common.Utilities

Namespace Ventrian.FileLinks

    Public Class TrackInfo

#Region " Private Members "

        Dim _moduleID As Integer
        Dim _userID As Integer
        Dim _path As String
        Dim _ipAddress As String
        Dim _trackDate As String

#End Region

#Region " Public Properties "

        Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal Value As Integer)
                _userID = Value
            End Set
        End Property

        Public Property Path() As String
            Get
                Return _path
            End Get
            Set(ByVal Value As String)
                _path = Value
            End Set
        End Property

        Public Property IPAddress() As String
            Get
                Return _ipAddress
            End Get
            Set(ByVal Value As String)
                _ipAddress = Value
            End Set
        End Property

        Public Property TrackDate() As DateTime
            Get
                Return _trackDate
            End Get
            Set(ByVal value As DateTime)
                _trackDate = value
            End Set
        End Property

#End Region

    End Class

End Namespace

