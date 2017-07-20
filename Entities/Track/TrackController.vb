Imports Ventrian.FileLinks.Data

Namespace Ventrian.FileLinks

    Public Class TrackController

        Public Sub Add(ByVal objTrack As TrackInfo)

            DataProvider.Instance().TrackDownload(objTrack.ModuleID, objTrack.UserID, objTrack.Path, objTrack.IPAddress, objTrack.TrackDate)

        End Sub

        Public Function GetCount(ByVal moduleID As Integer, ByVal path As String) As Integer

            Return DataProvider.Instance().TrackCount(moduleID, path)

        End Function

    End Class

End Namespace