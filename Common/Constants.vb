Imports Ventrian.FileLinks.Entities

Namespace Ventrian.FileLinks.Common

    Public Class Constants

        Public Const JAVASCRIPT_VERSION As String = "010000"

        Public Const FOLDER_ID As String = "FolderID"
        Public Const FOLDER_ID_DEFAULT As Integer = -1

        Public Const COMPRESSION As String = "CompressionType"
        Public Const COMPRESSION_DEFAULT As CompressionType = CompressionType.Quality

        Public Const DISPLAY_FOLDERS As String = "DisplayFolders"
        Public Const DISPLAY_FOLDERS_DEFAULT As Boolean = True

        Public Const DISPLAY_FILES_IN_FOLDERS As String = "DisplayFilesInFolders"
        Public Const DISPLAY_FILES_IN_FOLDERS_DEFAULT As Boolean = False

        Public Const TRACK_DOWNLOADS As String = "TrackDownloads"
        Public Const TRACK_DOWNLOADS_DEFAULT As Boolean = False

        Public Const EXTENSION_FILTER As String = "ExtensionFilter"
        Public Const EXTENSION_FILTER_DEFAULT As String = ""

        Public Const SECURE_URL As String = "SecureRedirectUrl"
        Public Const SECURE_URL_DEFAULT As String = ""

        Public Const IMAGE_WIDTH As String = "ImageWidth"
        Public Const IMAGE_WIDTH_DEFAULT As Integer = 600

        Public Const IMAGE_HEIGHT As String = "ImageHeight"
        Public Const IMAGE_HEIGHT_DEFAULT As Integer = 480

        Public Const MAX_FILE_SIZE As String = "FileLinksMaxFileSize"
        Public Const MAX_FILE_SIZE_DEFAULT As Integer = 8096

        Public Const RENAME_IMAGES As String = "RenameImages"
        Public Const RENAME_IMAGES_DEFAULT As Boolean = False

        Public Const RESIZE_IMAGES As String = "ResizeImages"
        Public Const RESIZE_IMAGES_DEFAULT As Boolean = False

        Public Const SORT_BY As String = "FileLinksSortBy"
        Public Const SORT_BY_DEFAULT As SortType = SortType.Name

        Public Const SORT_DIRECTION As String = "FileLinksSortDirection"
        Public Const SORT_DIRECTION_DEFAULT As SortDirection = SortDirection.Ascending

        Public Const TEMPLATE_HEADER As String = "TemplateHeader"
        Public Const TEMPLATE_ITEM As String = "TemplateItem"
        Public Const TEMPLATE_SUB_ITEM As String = "TemplateSubItem"
        Public Const TEMPLATE_SUB_EMPTY As String = "TemplateSubEmpty"
        Public Const TEMPLATE_FOOTER As String = "TemplateFooter"
        Public Const TEMPLATE_EMPTY As String = "TemplateEmpty"

        Public Const TEMPLATE_HEADER_DEFAULT As String = "" _
            & "<table width=""100%"">" _
            & "<tr>" _
            & "<td class=""NormalBold"">Name</td>" _
            & "<td class=""NormalBold"" width=""150px"">Date Modified</td>" _
            & "<td class=""NormalBold"" width=""150px"">Content Type</td>" _
            & "<td class=""NormalBold"" width=""100px"">Size</td>" _
            & "</tr>"

        Public Const TEMPLATE_ITEM_DEFAULT As String = "" _
            & "<tr>" _
            & "<td class=""Normal"">[DELETE][SECURE]<img src=""[ICONURL]"" align=""absmiddle"" /> <a href=""[LINK]""[ISFILE] target=""_blank"" class=""Terms""[/ISFILE]>[NAME]</a></td>" _
            & "<td class=""Normal"">[DATEMODIFIED]</td>" _
            & "<td class=""Normal"">[CONTENTTYPE]</td>" _
            & "<td class=""Normal"">[ISFILE][SIZE] KB[/ISFILE]</td>" _
            & "</tr>"

        Public Const TEMPLATE_SUB_ITEM_DEFAULT As String = "" _
            & "<tr>" _
            & "<td class=""Normal"">...&nbsp;[DELETE][SECURE]<img src=""[ICONURL]"" align=""absmiddle"" /> <a href=""[LINK]""[ISFILE] target=""_blank"" class=""Terms""[/ISFILE]>[NAME]</a></td>" _
            & "<td class=""Normal"">[DATEMODIFIED]</td>" _
            & "<td class=""Normal"">[CONTENTTYPE]</td>" _
            & "<td class=""Normal"">[ISFILE][SIZE] KB[/ISFILE]</td>" _
            & "</tr>"

        Public Const TEMPLATE_SUB_EMPTY_DEFAULT As String = "" _
            & "<tr>" _
            & "<td class=""Normal"" colspan=""4"">...&nbsp;[RESX:NoFiles]</td>" _
            & "</tr>"

        Public Const TEMPLATE_FOOTER_DEFAULT As String = "" _
            & "</table>"

        Public Const TEMPLATE_EMPTY_DEFAULT As String = "" _
            & "<div class=""Normal"">[RESX:NoFiles]</div>"

        Public Const TERMS_ENABLE As String = "TermsEnable"
        Public Const TERMS_ENABLE_DEFAULT As Boolean = False

        Public Const TERMS_TEMPLATE As String = "TermsTemplate"
        Public Const TERMS_TEMPLATE_DEFAULT As String = "This is an example of terms and conditions that can be customised in module settings... "

    End Class

End Namespace

