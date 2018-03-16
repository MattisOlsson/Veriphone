<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurePayment.ascx.cs" Inherits="Geta.Commerce.Payments.Verifone.HostedPages.Manager.Apps.Order.Payments.Plugins.VerifoneHostedPages.ConfigurePayment" %>

<table class="DataForm">
    <tbody>
        <tr>
            <td class="FormLabelCell">Merchant Agreement Code:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtMerchantAgreementCode" />
                <asp:RequiredFieldValidator ID="val1" runat="server" ControlToValidate="txtMerchantAgreementCode" ErrorMessage="The merchant agreement code is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Success URL:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtSuccessUrl" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSuccessUrl" ErrorMessage="The success URL is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Cancel URL:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtCancelUrl" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCancelUrl" ErrorMessage="The cancel URL is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Error URL:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtErrorUrl" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtErrorUrl" ErrorMessage="The error URL is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Expired URL:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtExpiredUrl" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtExpiredUrl" ErrorMessage="The expired URL is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Rejected URL:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtRejectedUrl" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtRejectedUrl" ErrorMessage="The rejected URL is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Web shop name:</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtWebShopName" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtWebShopName" ErrorMessage="The web shop name is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Is production:</td>
            <td class="FormFieldCell">
                <asp:CheckBox runat="server" id="cbIsProduction" />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Production URL (node 1):</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtProductionUrl" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProductionUrl" ErrorMessage="The production URL (node 1) is required." />
            </td>
        </tr>
        <tr>
            <td class="FormLabelCell">Production URL (node 2):</td>
            <td class="FormFieldCell">
                <asp:TextBox runat="server" ID="txtProductionUrl2" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtProductionUrl2" ErrorMessage="The production URL (node 2) is required." />
            </td>
        </tr>
    </tbody>
</table>