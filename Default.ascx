<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Default.ascx.vb" Inherits="Ventrian.FileLinks._Default" %>
<div id="FileLinks">
<table cellpadding="0" cellspacing="0" width="100%">
<tr>
	<td align="left">
		<asp:Repeater ID="rptBreadCrumbs" Runat="server">
		<ItemTemplate><a href='<%# DataBinder.Eval(Container.DataItem, "Url") %>' class="NormalBold"><%# DataBinder.Eval(Container.DataItem, "Caption") %></a></ItemTemplate>
		<SeparatorTemplate>&nbsp;&#187;&nbsp;</SeparatorTemplate>
		</asp:Repeater>
	</td>
	<td align="right">
	    <asp:TextBox ID="txtSearch" runat="server" CssClass="NormalTextBox" />
	    <asp:LinkButton ID="cmdSearch" runat="server" ResourceKey="Search" CssClass="CommandButton" />
	</td>
</tr>
</table>

<asp:Panel HorizontalAlign="Center" ID="pnlCommandBar" Runat="server" Visible="false">
	<asp:HyperLink ID="lnkAddNewFiles" Runat="server" CssClass="CommandButton" EnableViewState="False" ResourceKey="AddFiles" Visible="false" />&nbsp;&nbsp;<asp:HyperLink ID="lnkAddFolder" Runat="server" CssClass="CommandButton" EnableViewState="False" ResourceKey="AddFolder" Visible="false" /><br /><br />
</asp:Panel>

<asp:PlaceHolder ID="phSearchCriteria" runat="server" EnableViewState="false" Visible="false">
    <asp:Label ID="lblSearch" runat="server" EnableViewState="false" CssClass="Normal" />&nbsp;<asp:HyperLink ID="lnkSearchClear" runat="server" ResourceKey="ClearSearch" CssClass="CommandButton" />
</asp:PlaceHolder>

<asp:PlaceHolder ID="phFiles" runat="server" />
<asp:Label ID="lblConfigure" runat="server" ResourceKey="Configure" EnableViewState="false" CssClass="Normal" Visible="false" />
</div>
    
<asp:PlaceHolder ID="phTerms" runat="server" Visible="false">
<!-- modal content -->
<div id='confirm'>
    <div class="header"><asp:label ID="lblAcceptTerms" runat="server" ResourceKey="lblAcceptTerms" /></div>
    <div class="message"><asp:Literal ID="litTerms" runat="server" /></div>
    <div class='buttons'>
	    <div class='no simplemodal-close'>No</div><div class='yes'>Yes</div>
    </div>
</div>

<!-- preload the images -->
<div style='display:none'>
    <img src='<%= Me.ResolveUrl("Images/Confirm/header.gif") %>' alt='terms-header' />
    <img src='<%= Me.ResolveUrl("Images/Confirm/button.gif") %>' alt='terms-button' />
</div>
</asp:PlaceHolder>

<script type="text/javascript">

jQuery(function ($) {
	$('#FileLinks a.EditFile').click(function (e) {
		e.preventDefault();
		
		jQuery('#EditFileForm div.name').html("<b>File Path:</b> " + jQuery(this).attr('FileName'));
		jQuery('#EditFileForm #txtFileDescription').val(jQuery(this).attr('Description'));
		jQuery('#EditFileForm #filepath').html(jQuery(this).attr('href'));
		
		jQuery('#EditFileForm').modal({
		    closeHTML: "<a href='#' title='Close' class='modal-close'>x</a>",
		    position: ["20%",],
		    overlayId: 'edit-overlay',
		    containerId: 'edit-container'
	    });
	    
	});
});

jQuery(function ($) {
	$('#EditFileForm input.save-description').click(function (e) {
		e.preventDefault();
		
		var description = jQuery('#EditFileForm #txtFileDescription').val();
		var path = jQuery('#EditFileForm #filepath').html();
		
        jQuery.post("<%= Page.ResolveUrl("~/DesktopModules/FileLinks/SaveDesc.ashx") %>?path=" + path, { Description: description, M: <%= ModuleID %> },
           function(data) {
             alert("Description Saved.");
		     location.reload();
           });
           
		$.modal.close().delay(1000);
	});
});

</script>

<div id="EditFileForm" style="display:none">
    <div class="header"><asp:label ID="lblEditFile" runat="server" ResourceKey="lblEditFile" /></div>
    <div class="message">
        <div class="name"></div>
        <b><asp:label ID="lblDescription" runat="server" ResourceKey="lblDescription" /></b><br />
        <textarea style="width: 475px;" class="NormalTextBox" id="txtFileDescription" cols="20" rows="5" name="txtFileDescription"></textarea>
        <div id="filepath" style="display: none;"></div>
    </div>
    <div class="buttons">
        <input type="button" id="btnSaveDescription" class="save-description" value="Save" />
    </div>
</div>