Namespace Ventrian.FileLinks.Entities

    Public Class DescriptionInfo

#Region " Private Members "

        Dim _moduleID As Integer
        Dim _path As String
        Dim _description As String

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

        Public Property Path() As String
            Get
                Return _path
            End Get
            Set(ByVal Value As String)
                _path = Value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal Value As String)
                _description = Value
            End Set
        End Property

#End Region

    End Class

End Namespace
