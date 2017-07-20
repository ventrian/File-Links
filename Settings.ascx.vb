Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Services.Localization

Imports Ventrian.FileLinks.Common
Imports Ventrian.FileLinks.Entities

Namespace Ventrian.FileLinks

    Partial Public Class Settings
        Inherits ModuleSettingsBase

#Region " Private Members "

        Private _linkSettings As LinkSettings

#End Region

#Region " Private Methods "

        Private Sub BindCompressionType()

            For Each value As Integer In System.Enum.GetValues(GetType(CompressionType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(CompressionType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(CompressionType), value), Me.LocalResourceFile)
                drpCompressionType.Items.Add(li)
            Next

        End Sub

        Private Sub BindFolders()

            Dim folders As ArrayList = FileSystemUtils.GetFolders(PortalId)
            For Each folder As FolderInfo In folders
                Dim FolderItem As New ListItem
                If folder.FolderPath = Null.NullString Then
                    FolderItem.Text = Localization.GetString("Root", Me.LocalResourceFile)
                Else
                    FolderItem.Text = folder.FolderPath
                End If
                FolderItem.Value = folder.FolderID
                drpSelectFolder.Items.Add(FolderItem)
            Next

        End Sub

        Private Sub BindSortBy()

            For Each value As Integer In System.Enum.GetValues(GetType(SortType))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortType), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortType), value) & "Sort", Me.LocalResourceFile)
                drpSortBy.Items.Add(li)
            Next

        End Sub

        Private Sub BindSortDirection()

            For Each value As Integer In System.Enum.GetValues(GetType(SortDirection))
                Dim li As New ListItem
                li.Value = System.Enum.GetName(GetType(SortDirection), value)
                li.Text = Localization.GetString(System.Enum.GetName(GetType(SortDirection), value), Me.LocalResourceFile)
                drpSortDirection.Items.Add(li)
            Next

        End Sub

#End Region

#Region " Private Properties "

        Public ReadOnly Property LinkSettings() As LinkSettings
            Get
                If (_linkSettings Is Nothing) Then
                    _linkSettings = New LinkSettings(Settings)
                End If
                Return _linkSettings
            End Get
        End Property

#End Region

#Region " Base Method Implementations "

        Public Overrides Sub LoadSettings()

            Try

                If (Page.IsPostBack = False) Then

                    BindCompressionType()
                    BindFolders()
                    BindSortBy()
                    BindSortDirection()

                    If (drpSelectFolder.Items.FindByValue(LinkSettings.FolderID) IsNot Nothing) Then
                        drpSelectFolder.SelectedValue = LinkSettings.FolderID
                    End If
                    chkDisplayFolders.Checked = LinkSettings.DisplayFolders
                    chkDisplayFilesInFolders.Checked = LinkSettings.DisplayFilesInFolders
                    chkTrackDownloads.Checked = LinkSettings.TrackDownloads
                    txtExtensionFilter.Text = LinkSettings.ExtensionFilter
                    txtMaxFileSize.Text = LinkSettings.MaxFileSize.ToString()
                    txtSecureUrl.Text = LinkSettings.SecureUrl

                    If (drpSortBy.Items.FindByValue(LinkSettings.SortBy.ToString()) IsNot Nothing) Then
                        drpSortBy.SelectedValue = LinkSettings.SortBy.ToString()
                    End If

                    If (drpSortDirection.Items.FindByValue(LinkSettings.SortDirection.ToString()) IsNot Nothing) Then
                        drpSortDirection.SelectedValue = LinkSettings.SortDirection.ToString()
                    End If

                    chkResizeImages.Checked = LinkSettings.ResizeImages
                    chkRenameImages.Checked = LinkSettings.RenameImages
                    If Not (drpCompressionType.Items.FindByValue(LinkSettings.Compression.ToString()) Is Nothing) Then
                        drpCompressionType.SelectedValue = LinkSettings.Compression.ToString()
                    End If
                    txtImageWidth.Text = LinkSettings.ImageWidth.ToString()
                    txtImageHeight.Text = LinkSettings.ImageHeight.ToString()

                    txtTemplateHeader.Text = LinkSettings.TemplateHeader
                    txtTemplateItem.Text = LinkSettings.TemplateItem
                    txtTemplateSubItem.Text = LinkSettings.TemplateSubItem
                    txtTemplateSubEmpty.Text = LinkSettings.TemplateSubEmpty
                    txtTemplateFooter.Text = LinkSettings.TemplateFooter
                    txtTemplateEmpty.Text = LinkSettings.TemplateEmpty

                    chkEnableTerms.Checked = LinkSettings.TermsEnabled
                    txtTerms.Text = LinkSettings.TermsTemplate

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Public Overrides Sub UpdateSettings()

            Try

                If (Page.IsValid) Then

                    Dim objModuleController As New ModuleController

                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.FOLDER_ID, drpSelectFolder.SelectedValue)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.DISPLAY_FOLDERS, chkDisplayFolders.Checked)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.DISPLAY_FILES_IN_FOLDERS, chkDisplayFilesInFolders.Checked)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TRACK_DOWNLOADS, chkTrackDownloads.Checked)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.EXTENSION_FILTER, txtExtensionFilter.Text)
                    If (Convert.ToInt32(txtMaxFileSize.Text) > 0) Then
                        objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.MAX_FILE_SIZE, txtMaxFileSize.Text)
                    End If
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SORT_BY, drpSortBy.SelectedValue)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SORT_DIRECTION, drpSortDirection.SelectedValue)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.SECURE_URL, txtSecureUrl.Text)

                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.RESIZE_IMAGES, chkResizeImages.Checked.ToString())
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.RENAME_IMAGES, chkRenameImages.Checked.ToString())
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.COMPRESSION, drpCompressionType.SelectedValue)
                    If (IsNumeric(txtImageWidth.Text)) Then
                        If (Convert.ToInt32(txtImageWidth.Text) > 0) Then
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.IMAGE_WIDTH, txtImageWidth.Text)
                        End If
                    End If
                    If (IsNumeric(txtImageHeight.Text)) Then
                        If (Convert.ToInt32(txtImageHeight.Text) > 0) Then
                            objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.IMAGE_HEIGHT, txtImageHeight.Text)
                        End If
                    End If

                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TEMPLATE_HEADER, txtTemplateHeader.Text)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TEMPLATE_ITEM, txtTemplateItem.Text)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TEMPLATE_SUB_ITEM, txtTemplateSubItem.Text)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TEMPLATE_SUB_EMPTY, txtTemplateSubEmpty.Text)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TEMPLATE_FOOTER, txtTemplateFooter.Text)
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TEMPLATE_EMPTY, txtTemplateEmpty.Text)

                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TERMS_ENABLE, chkEnableTerms.Checked.ToString())
                    objModuleController.UpdateModuleSetting(Me.ModuleId, Constants.TERMS_TEMPLATE, txtTerms.Text)

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

#End Region

    End Class

End Namespace