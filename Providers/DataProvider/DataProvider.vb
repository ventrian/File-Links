Imports DotNetNuke.Framework

Namespace Ventrian.FileLinks.Data

    Public MustInherit Class DataProvider

#Region " Shared/Static Methods "

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Reflection.CreateObject("data", "Ventrian.FileLinks", "Ventrian.FileLinks"), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region " Abstract Methods "

        Public MustOverride Sub DescriptionUpdate(ByVal moduleID As Integer, ByVal path As String, ByVal description As String)
        Public MustOverride Function DescriptionList(ByVal moduleID As Integer) As IDataReader

        Public MustOverride Function TrackCount(ByVal moduleID As Integer, ByVal path As String) As Integer
        Public MustOverride Sub TrackDownload(ByVal moduleID As Integer, ByVal userID As Integer, ByVal path As String, ByVal ipAddress As String, ByVal trackDate As DateTime)
        
#End Region

    End Class

End Namespace
