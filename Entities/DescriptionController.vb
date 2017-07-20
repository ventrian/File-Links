Imports Ventrian.FileLinks.Data
Imports DotNetNuke.Common.Utilities

Namespace Ventrian.FileLinks.Entities

    Public Class DescriptionController

        Public Function List(ByVal moduleID As Integer) As List(Of DescriptionInfo)


            Dim key As String = moduleID.ToString() & "-FILE-LINKS-DESC"

            Dim objDescriptionList As List(Of DescriptionInfo) = CType(DataCache.GetCache(key), List(Of DescriptionInfo))

            If (objDescriptionList Is Nothing) Then
                objDescriptionList = CBO.FillCollection(Of DescriptionInfo)(DataProvider.Instance().DescriptionList(moduleID))
                DataCache.SetCache(key, objDescriptionList)
            End If


            Return objDescriptionList

        End Function

        Public Sub Update(ByVal objDescription As DescriptionInfo)

            DataProvider.Instance().DescriptionUpdate(objDescription.ModuleID, objDescription.Path, objDescription.Description)
            DataCache.RemoveCache(objDescription.ModuleID.ToString() & "-FILE-LINKS-DESC")

        End Sub

    End Class

End Namespace
