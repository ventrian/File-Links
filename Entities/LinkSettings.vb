Imports Ventrian.FileLinks.Common

Namespace Ventrian.FileLinks.Entities

    Public Class LinkSettings

#Region " Private Members "

        Private _settings As Hashtable

#End Region

#Region " Constructors "

        Sub New(ByVal settings As Hashtable)

            _settings = settings

        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property Compression() As CompressionType
            Get
                If (_settings.Contains(Constants.COMPRESSION)) Then
                    Return CType(System.Enum.Parse(GetType(CompressionType), _settings(Constants.COMPRESSION).ToString()), CompressionType)
                Else
                    Return Constants.COMPRESSION_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property DisplayFolders() As Boolean
            Get
                If (_settings.Contains(Constants.DISPLAY_FOLDERS)) Then
                    Return Convert.ToBoolean(_settings(Constants.DISPLAY_FOLDERS).ToString())
                Else
                    Return Constants.DISPLAY_FOLDERS_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property DisplayFilesInFolders() As Boolean
            Get
                If (_settings.Contains(Constants.DISPLAY_FILES_IN_FOLDERS)) Then
                    Return Convert.ToBoolean(_settings(Constants.DISPLAY_FILES_IN_FOLDERS).ToString())
                Else
                    Return Constants.DISPLAY_FILES_IN_FOLDERS_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TrackDownloads() As Boolean
            Get
                If (_settings.Contains(Constants.TRACK_DOWNLOADS)) Then
                    Return Convert.ToBoolean(_settings(Constants.TRACK_DOWNLOADS).ToString())
                Else
                    Return Constants.TRACK_DOWNLOADS_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ExtensionFilter() As String
            Get
                If (_settings.Contains(Constants.EXTENSION_FILTER)) Then
                    Return _settings(Constants.EXTENSION_FILTER).ToString()
                Else
                    Return Constants.EXTENSION_FILTER_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property FolderID() As Integer
            Get
                If (_settings.Contains(Constants.FOLDER_ID)) Then
                    Return Convert.ToInt32(_settings(Constants.FOLDER_ID).ToString())
                Else
                    Return Constants.FOLDER_ID_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ImageHeight() As Integer
            Get
                If (_settings.Contains(Constants.IMAGE_HEIGHT)) Then
                    Return Convert.ToInt32(_settings(Constants.IMAGE_HEIGHT).ToString())
                Else
                    Return Constants.IMAGE_HEIGHT_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ImageWidth() As Integer
            Get
                If (_settings.Contains(Constants.IMAGE_WIDTH)) Then
                    Return Convert.ToInt32(_settings(Constants.IMAGE_WIDTH).ToString())
                Else
                    Return Constants.IMAGE_WIDTH_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property MaxFileSize() As Integer
            Get
                If (_settings.Contains(Constants.MAX_FILE_SIZE)) Then
                    Return Convert.ToInt32(_settings(Constants.MAX_FILE_SIZE).ToString())
                Else
                    Return Constants.MAX_FILE_SIZE_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property RenameImages() As Boolean
            Get
                If (_settings.Contains(Constants.RENAME_IMAGES)) Then
                    Return Convert.ToBoolean(_settings(Constants.RENAME_IMAGES).ToString())
                Else
                    Return Constants.RENAME_IMAGES_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property ResizeImages() As Boolean
            Get
                If (_settings.Contains(Constants.RESIZE_IMAGES)) Then
                    Return Convert.ToBoolean(_settings(Constants.RESIZE_IMAGES).ToString())
                Else
                    Return Constants.RESIZE_IMAGES_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SecureUrl() As String
            Get
                If (_settings.Contains(Constants.SECURE_URL)) Then
                    Return _settings(Constants.SECURE_URL).ToString()
                Else
                    Return Constants.SECURE_URL_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SortBy() As SortType
            Get
                If (_settings.Contains(Constants.SORT_BY)) Then
                    Return CType(System.Enum.Parse(GetType(SortType), _settings(Constants.SORT_BY).ToString()), SortType)
                Else
                    Return Constants.SORT_BY_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property SortDirection() As SortDirection
            Get
                If (_settings.Contains(Constants.SORT_DIRECTION)) Then
                    Return CType(System.Enum.Parse(GetType(SortDirection), _settings(Constants.SORT_DIRECTION).ToString()), SortDirection)
                Else
                    Return Constants.SORT_DIRECTION_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateHeader() As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_HEADER)) Then
                    Return _settings(Constants.TEMPLATE_HEADER).ToString()
                Else
                    Return Constants.TEMPLATE_HEADER_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateItem() As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_ITEM)) Then
                    Return _settings(Constants.TEMPLATE_ITEM).ToString()
                Else
                    Return Constants.TEMPLATE_ITEM_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateSubItem() As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_SUB_ITEM)) Then
                    Return _settings(Constants.TEMPLATE_SUB_ITEM).ToString()
                Else
                    Return Constants.TEMPLATE_SUB_ITEM_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateSubEmpty() As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_SUB_EMPTY)) Then
                    Return _settings(Constants.TEMPLATE_SUB_EMPTY).ToString()
                Else
                    Return Constants.TEMPLATE_SUB_EMPTY_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateFooter() As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_FOOTER)) Then
                    Return _settings(Constants.TEMPLATE_FOOTER).ToString()
                Else
                    Return Constants.TEMPLATE_FOOTER_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TemplateEmpty() As String
            Get
                If (_settings.Contains(Constants.TEMPLATE_EMPTY)) Then
                    Return _settings(Constants.TEMPLATE_EMPTY).ToString()
                Else
                    Return Constants.TEMPLATE_EMPTY_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TermsEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.TERMS_ENABLE)) Then
                    Return Convert.ToBoolean(_settings(Constants.TERMS_ENABLE).ToString())
                Else
                    Return Constants.TERMS_ENABLE_DEFAULT
                End If
            End Get
        End Property

        Public ReadOnly Property TermsTemplate() As String
            Get
                If (_settings.Contains(Constants.TERMS_TEMPLATE)) Then
                    Return _settings(Constants.TERMS_TEMPLATE).ToString()
                Else
                    Return Constants.TERMS_TEMPLATE_DEFAULT
                End If
            End Get
        End Property

#End Region

    End Class

End Namespace
