<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddFiles.ascx.vb" Inherits="Ventrian.FileLinks.AddFiles" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td align="left">
		<asp:Repeater ID="rptBreadCrumbs" Runat="server">
		<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
		<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
		</asp:Repeater>
	</td>
</tr>
</table>

<script type="text/javascript" src='<%= ResolveUrl("JS/SWFUpload/swfupload.2.2.0.js") %>'></script>
<script type="text/javascript" src='<%= ResolveUrl("JS/SWFUpload/handlers.2.2.0.js") %>'></script>
<script type="text/javascript">
	var swfu;
	window.onload = function () {
		swfu = new SWFUpload({
			// Backend Settings
			upload_url: "<%= GetUploadUrl() %>",	// Relative to the SWF file
			post_params: {
			    "Ticket" : '<asp:literal id="litTicketID" runat="server" />', 
			    "FolderID" : '<asp:literal id="litFolderID" runat="server" />', 
			    "ModuleID" : '<asp:literal id="litModuleID" runat="server" />', 
			    "TabID" : '<asp:literal id="litTabID" runat="server" />'
			},	// Relative to the SWF file
			
			file_size_limit : "<%= GetMaxFileSize() %>",	
			file_types : '<asp:Literal ID="litFileTypes" Runat="server" />',
			file_types_description : '<asp:Literal ID="litSelectFileDescription" Runat="server" />',
			file_upload_limit : "0",    // Zero means unlimited

            // Event Handler Settings - these functions as defined in Handlers.js
			// The handlers are not part of SWFUpload but are part of my website and control how
			// my website reacts to the SWFUpload events.
			file_queue_error_handler : fileQueueError,
			file_dialog_complete_handler : fileDialogComplete,
			upload_progress_handler : uploadProgress,
			upload_error_handler : uploadError,
			upload_success_handler : uploadSuccess,
			upload_complete_handler : uploadComplete,

			// Button Settings
			button_image_url : '<%= ResolveUrl("~/DesktopModules/FileLinks/Images/SWFUpload/XPButtonNoText_160x22.png") %>',	// Relative to the SWF file
			button_placeholder_id : "spanButtonPlaceholder",
			button_width: 160,
			button_height: 22,
			button_text : '<span class="button"><asp:Label ID="lblSelectFiles" Runat="server" EnableViewState="False" ResourceKey="SelectFiles" /></span>',
			button_text_style : '.button { font-family: Tahoma,Arial,Helvetica; font-size: 11px; font-weight:bold; text-align: center; }',
			button_text_top_padding: 2,
			button_text_left_padding: 5,

			// Flash Settings
			flash_url : '<%= ResolveUrl("JS/SWFUpload/swfupload.2.2.0.swf") %>',	// Relative to this file

			custom_settings : {
				upload_target : "sg_progress_container",
				image_path : '<%= ResolveUrl("~/DesktopModules/FileLinks/Images/SWFUpload/") %>'
			},
			
			// Debug Settings
			debug: false
		});
	}
</script>

<div align="center" id="divFolders" runat="server">
    <asp:DropDownList ID="drpFolders" runat="server" CssClass="Normal" AutoPostBack="true" /><br /><br />
</div>
<div id="sg_upload_container" style="margin: 0px 10px;" align="center">
    <div>
        <span id="spanButtonPlaceholder"></span>
    </div>
    <div id="sg_progress_container" class="Normal" style="margin-top: 10px;" align="center"></div>
    <div id="sg_files_container" class="Normal" style="margin-top: 10px;visibility: hidden;">
        <div class="progressWrapper" style="opacity: 1;">
            <div class="progressContainer" align="left">
                <b>Files Uploaded:</b>
                <ul id="files" class="normal" align="left"></ul>
            </div>
        </div>
    </div>       
</div>
<div>
    <asp:LinkButton ID="cmdReturn" runat="server" ResourceKey="Return" CssClass="CommandButton" />
</div>
