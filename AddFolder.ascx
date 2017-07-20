<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddFolder.ascx.vb" Inherits="Ventrian.FileLinks.AddFolder" %>
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
<br />
<table cellspacing="2" cellpadding="2" summary="Template Design Table" border="0" width="560">
<tr>
    <td class="SubHead" width="160"><dnn:label id="plParentFolder" runat="server" resourcekey="plParentFolder" suffix=":" controlname="drpFolders"></dnn:label></td>
    <td width="365"><asp:DropDownList ID="drpFolders" runat="server" CssClass="Normal" width="250px" /></td>
</tr>
<tr>
    <td class="SubHead" width="160"><dnn:label id="plFolder" runat="server" resourcekey="plFolder" suffix=":" controlname="txtFolder"></dnn:label></td>
    <td width="365"><asp:TextBox ID="txtFolder" runat="server" CssClass="NormalTextBox" width="250px" /><asp:requiredfieldvalidator id="valFolder" cssclass="NormalRed" runat="server" resourcekey="valFolder.ErrorMessage" display="Dynamic"
								errormessage="<br>You Must Enter a Valid Folder Name" controltovalidate="txtFolder" /></td>
</tr>
</table>
<p>
	<asp:LinkButton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" CssClass="CommandButton" text="Update"
		BorderStyle="none" />
	&nbsp;
	<asp:LinkButton id="cmdCancel" resourcekey="cmdCancel" runat="server" CssClass="CommandButton" text="Cancel"
		CausesValidation="False" BorderStyle="none" />
</p>