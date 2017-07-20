Imports DotNetNuke.Framework

Imports Microsoft.ApplicationBlocks.Data

Imports Ventrian.FileLinks.Data

Namespace Ventrian.FileLinks

    Public Class SqlDataProvider

        Inherits DataProvider

#Region " Private Members "

        Private Const ProviderType As String = "data"

        Private _providerConfiguration As Providers.ProviderConfiguration = Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region " Constructors "

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Providers.Provider)

            _connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString()

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region " Properties "

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region " Public Methods "

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

        Public Overrides Function DescriptionList(ByVal moduleID As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_FileLinks_DescriptionList", moduleID), IDataReader)
        End Function

        Public Overrides Sub DescriptionUpdate(ByVal moduleID As Integer, ByVal path As String, ByVal description As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_FileLinks_DescriptionUpdate", moduleID, path, description)
        End Sub

        Public Overrides Function TrackCount(ByVal moduleID As Integer, ByVal path As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_FileLinks_TrackCount", moduleID, path), Integer)
        End Function

        Public Overrides Sub TrackDownload(ByVal moduleID As Integer, ByVal userID As Integer, ByVal path As String, ByVal ipAddress As String, ByVal trackDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Ventrian_FileLinks_TrackDownload", moduleID, GetNull(userID), path, ipAddress, trackDate)
        End Sub

#End Region

    End Class

End Namespace
