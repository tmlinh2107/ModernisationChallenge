<%@ Page Title="Modernisation Challenge" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ModernisationChallenge._Default" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <h1>
        Tasks
    </h1>

    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="table">
                <table>
                    <thead>
                        <tr>
                            <th style="width: 1px;">Completed</th>
                            <th>Details</th>
                            <th style="width: 1px;"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="TasksRepeater" OnItemCommand="TasksRepeater_ItemCommand" runat="server">
                            <ItemTemplate>
                                <asp:HiddenField ID="TaskIdHiddenField" Value='<%# Eval("Id") %>' runat="server" />

                                <tr>
                                    <td style="text-align: center; width: 1px;"><asp:CheckBox ID="CompletedCheckBox" AutoPostBack="true" Checked='<%# Eval("Completed") %>' CssClass="checkbox" OnCheckedChanged="CompletedCheckBox_CheckedChanged" runat="server" /></td>
                                    <td><%#: Eval("Details") %></td>
                                    <td style="width: 1px;">
                                        <span class="popup_menu" onmousedown="return PopupHelper.init(this, this.querySelector('span'), { mode: 'click' });">
                                            <span onclick="this.parentNode.popupHelper.hide();">
                                                <asp:LinkButton ID="EditLinkButton" CommandArgument='<%# Eval("Id") %>' CommandName="Edit" OnClientClick="fadeToBlack();" Text="Edit" runat="server" />
                                                <asp:LinkButton ID="DeleteLinkButton" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" OnClientClick="return confirm('Are you sure that you want to delete this task?');" Text="Delete" runat="server" />
                                            </span>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="info">
                            <td colspan="99">
                                <asp:LinkButton ID="CreateLinkButton" Text="+ Create a new task" OnClick="CreateLinkButton_Click" OnClientClick="fadeToBlack();" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SaveLinkButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="TasksRepeater" EventName="ItemCommand" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="DialoguePlaceHolder" Visible="false" runat="server">
                <div class="dialogue">
                    <div style="width: 750px;">
                        <div class="header">
                            <asp:LinkButton ID="CloseLinkButton" CssClass="close" OnClick="CloseLinkButton_Click" runat="server" />

                            <h2>
                                <asp:Literal ID="TitleLiteral" runat="server" />
                            </h2>
                        </div>

                        <div class="body">
                            <fieldset class="required">
                                <label>Details</label>
                                <asp:TextBox ID="DetailsTextBox" CssClass="text" Height="100" TextMode="MultiLine" runat="server" />
                            </fieldset>
                        </div>

                        <div class="footer">
                            <p class="commands">
                                <span class="grow"></span>

                                <asp:LinkButton ID="CancelLinkButton" Cssclass="button hollow" Text="Cancel" OnClick="CancelLinkButton_Click" runat="server" />
                                <asp:LinkButton ID="SaveLinkButton" CssClass="button" Text="Save" OnClick="SaveLinkButton_Click" runat="server" />
			                </p>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="CancelLinkButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="CloseLinkButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="CreateLinkButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="SaveLinkButton" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="TasksRepeater" EventName="ItemCommand" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
