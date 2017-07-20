<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Settings.ascx.vb" Inherits="Ventrian.FileLinks.Settings" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table id="tblGeneral" cellspacing="2" cellpadding="2" summary="General Settings Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSelectFolder" runat="server" resourcekey="plSelectFolder" suffix=":" controlname="drpSelectFolder"></dnn:label></td>
	<td><asp:DropDownList id="drpSelectFolder" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plDisplayFolders" runat="server" resourcekey="plDisplayFolders" suffix=":" controlname="chkDisplayFolders"></dnn:label></td>
	<td><asp:CheckBox id="chkDisplayFolders" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plDisplayFilesInFolders" runat="server" resourcekey="plDisplayFilesInFolders" suffix=":" controlname="chkDisplayFilesInFolders"></dnn:label></td>
	<td><asp:CheckBox id="chkDisplayFilesInFolders" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plTrackDownloads" runat="server" resourcekey="plTrackDownloads" suffix=":" controlname="chkTrackDownloads"></dnn:label></td>
	<td><asp:CheckBox id="chkTrackDownloads" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plExtensionFilter" runat="server" resourcekey="plExtensionFilter" suffix=":" controlname="txtExtensionFilter"></dnn:label></td>
	<td><asp:TextBox id="txtExtensionFilter" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSortBy" runat="server" suffix=":" controlname="drpSortBy"></dnn:label></td>
	<td><asp:DropDownList id="drpSortBy" Runat="server" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSortDirection" runat="server" suffix=":" controlname="drpSortDirection"></dnn:label></td>
	<td><asp:DropDownList id="drpSortDirection" Runat="server" width="250px" CssClass="NormalTextBox"></asp:DropDownList></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plMaxFileSize" runat="server" suffix=":" controlname="txtMaxFileSize"></dnn:label></td>
	<td>
		<asp:textbox id="txtMaxFileSize" cssclass="NormalTextBox" runat="server" maxlength="10" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valMaxFileSize" cssclass="NormalRed" runat="server" resourcekey="valMaxFileSize.ErrorMessage"
			display="Dynamic" errormessage="<br>Max File Size is Required" controltovalidate="txtMaxFileSize"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valMaxFileSizeIsNumber" Runat="server" errormessage="<br>Max File Size must be a Number"
			controltovalidate="txtMaxFileSize" CssClass="NormalRed" resourceKey="valMaxFileSizeIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSecureUrl" runat="server" resourcekey="plSecureUrl" suffix=":" controlname="txtSecureUrl"></dnn:label></td>
	<td><asp:TextBox id="txtSecureUrl" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshImageSettings" cssclass="Head" runat="server" text="Image Settings" section="tblImageSettings" resourcekey="ImageSettings" IsExpanded="True" />
<table id="tblImageSettings" cellspacing="2" cellpadding="2" summary="Image Settings Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plResizeImages" runat="server" resourcekey="plResizeImages" suffix=":" controlname="chkResizeImages"></dnn:label></td>
	<td><asp:CheckBox id="chkResizeImages" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plRenameImages" runat="server" resourcekey="plRenameImages" suffix=":" controlname="chkRenameImages"></dnn:label></td>
	<td><asp:CheckBox id="chkRenameImages" Runat="server" width="250px" CssClass="NormalTextBox" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plCompressionType" runat="server" resourcekey="plCompressionType" suffix=":" controlname="drpCompressionType"></dnn:label></td>
	<td><asp:DropDownList id="drpCompressionType" Runat="server" CssClass="NormalTextBox" width="250px"></asp:DropDownList></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plImageWidth" runat="server" suffix=":" controlname="txtImageWidth"></dnn:label></td>
	<td>
		<asp:textbox id="txtImageWidth" cssclass="NormalTextBox" runat="server" maxlength="10" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valImageWidth" cssclass="NormalRed" runat="server" resourcekey="valImageWidth.ErrorMessage"
			display="Dynamic" errormessage="<br>Image Width is Required" controltovalidate="txtImageWidth"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valImageWidthIsNumber" Runat="server" errormessage="<br>Image Width must be a Number"
			controltovalidate="txtImageWidth" CssClass="NormalRed" resourceKey="valImageWidthIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer" />
	</td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plImageHeight" runat="server" suffix=":" controlname="txtImageHeight"></dnn:label></td>
	<td>
		<asp:textbox id="txtImageHeight" cssclass="NormalTextBox" runat="server" maxlength="10" width="250px"></asp:textbox>
		<asp:requiredfieldvalidator id="valImageHeight" cssclass="NormalRed" runat="server" resourcekey="valImageHeight.ErrorMessage"
			display="Dynamic" errormessage="<br>Image Height is Required" controltovalidate="txtImageHeight"></asp:requiredfieldvalidator>
		<asp:CompareValidator id="valImageHeightIsNumber" Runat="server" errormessage="<br>Image Height must be a Number"
			controltovalidate="txtImageHeight" CssClass="NormalRed" resourceKey="valImageHeightIsNumber.ErrorMessage"
			Display="Dynamic" Operator="DataTypeCheck" Type="Integer" />
	</td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshTemplate" cssclass="Head" runat="server" text="Template Settings" section="tblTemplate" resourcekey="TemplateSettings" IsExpanded="false" />
<table id="tblTemplate" cellspacing="2" cellpadding="2" summary="Template Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200"><dnn:label id="plHeader" runat="server" resourcekey="plHeader" suffix=":" controlname="txtTemplateHeader"></dnn:label></td>
	<td><asp:TextBox ID="txtTemplateHeader" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plItem" runat="server" resourcekey="plItem" suffix=":" controlname="txtTemplateItem"></dnn:label></td>
	<td><asp:TextBox ID="txtTemplateItem" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSubItem" runat="server" resourcekey="plSubItem" suffix=":" controlname="txtTemplateSubItem"></dnn:label></td>
	<td><asp:TextBox ID="txtTemplateSubItem" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plSubEmpty" runat="server" resourcekey="plSubEmpty" suffix=":" controlname="txtTemplateSubEmpty"></dnn:label></td>
	<td><asp:TextBox ID="txtTemplateSubEmpty" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plFooter" runat="server" resourcekey="plFooter" suffix=":" controlname="txtTemplateFooter"></dnn:label></td>
	<td><asp:TextBox ID="txtTemplateFooter" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plEmpty" runat="server" resourcekey="plEmpty" suffix=":" controlname="txtTemplateEmpty"></dnn:label></td>
	<td><asp:TextBox ID="txtTemplateEmpty" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshTemplateHelp" cssclass="Head" runat="server" text="Template Help" section="tblTemplateHelp" resourcekey="TemplateHelp" IsExpanded="false" />
<table id="tblTemplateHelp" cellspacing="2" cellpadding="2" summary="Template Help Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200">[CONTENTTYPE]</td>
	<td><asp:label id="lblContentType" resourcekey="ContentType" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[DATECREATED]</td>
	<td><asp:label id="lblDateCreated" resourcekey="DateCreated" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[DATECREATED:XXX]</td>
	<td><asp:label id="Label1" resourcekey="DateCreatedXXX" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[DATEMODIFIED]</td>
	<td><asp:label id="lblDateModified" resourcekey="DateModified" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[DATEMODIFIED:XXX]</td>
	<td><asp:label id="Label2" resourcekey="DateModifiedXXX" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[DELETE]</td>
	<td><asp:label id="lblDelete" resourcekey="DeleteToken" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[DESCRIPTION]</td>
	<td><asp:label id="lblDescription" resourcekey="DescriptionToken" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[DOWNLOADCOUNT]</td>
	<td><asp:label id="lblDownloadCount" resourcekey="DownloadCountToken" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[EXTENSION]</td>
	<td><asp:label id="lblExtension" resourcekey="Extension" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[LINK]</td>
	<td><asp:label id="lblLink" resourcekey="Link" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[ICON]</td>
	<td><asp:label id="lblIcon" resourcekey="Icon" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[ICONURL]</td>
	<td><asp:label id="lblIconUrl" resourcekey="IconUrl" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[ISALTERNATE][/ISALTERNATE]</td>
	<td><asp:label id="lblIsAlternate" resourcekey="IsAlternate" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[ISFILE][/ISFILE]</td>
	<td><asp:label id="lblIsFile" resourcekey="IsFile" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[ISFOLDER][/ISFOLDER]</td>
	<td><asp:label id="lblIsFolder" resourcekey="IsFolder" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[NAME]</td>
	<td><asp:label id="lblName" resourcekey="Name" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[NAMENOEXTENSION]</td>
	<td><asp:label id="lblNameNoExtension" resourcekey="NameNoExtension" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[SECURE]</td>
	<td><asp:label id="lblSecure" resourcekey="SecureToken" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
<tr>
	<td class="SubHead" width="200">[SIZE]</td>
	<td><asp:label id="lblSize" resourcekey="Size" cssclass="Normal" runat="server" enableviewstate="False"></asp:label></td>
</tr>
</table>
<br />
<dnn:SectionHead id="dshTerms" cssclass="Head" runat="server" text="Terms & Conditions" section="tblTermsConditions" resourcekey="TermsConditions" IsExpanded="false" />
<table id="tblTermsConditions" cellspacing="2" cellpadding="2" summary="Terms and Conditions Design Table" border="0" runat="server">
<tr>
	<td class="SubHead" width="200">
	    <dnn:label id="plEnableTerms" runat="server" resourcekey="plEnableTerms" suffix=":" controlname="chkEnableTerms"></dnn:label>
        
    </td>
	<td><asp:CheckBox id="chkEnableTerms" Runat="server" width="250px" CssClass="NormalTextBox" /><br /><asp:Label ID="lblEnableTerms" runat="server" resourceKey="EnableTermsNote" CssClass="NormalBold" /></td>
</tr>
<tr>
	<td class="SubHead" width="200"><dnn:label id="plTermsTemplate" runat="server" resourcekey="plTermsTemplate" suffix=":" controlname="txtTerms"></dnn:label></td>
	<td><asp:TextBox ID="txtTerms" Runat="server" TextMode="MultiLine" CssClass="NormalTextBox" Width="250px" Rows="10" /></td>
</tr>
</table>