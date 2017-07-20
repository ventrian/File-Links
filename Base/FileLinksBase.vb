Imports DotNetNuke.Entities.Modules
Imports Ventrian.FileLinks.Entities

Namespace Ventrian.FileLinks.Base

    Public Class FileLinksBase
        Inherits PortalModuleBase

#Region " Private Members "

        Private _linkSettings As LinkSettings

#End Region

#Region " Protected Methods "

        Protected ReadOnly Property LinkSettings() As LinkSettings
            Get
                If (_linkSettings Is Nothing) Then
                    _linkSettings = New LinkSettings(Settings)
                End If
                Return _linkSettings
            End Get
        End Property

#End Region

    End Class

End Namespace
