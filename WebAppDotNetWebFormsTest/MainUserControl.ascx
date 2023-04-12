<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainUserControl.ascx.cs" Inherits="WebAppDotNetWebFormsTest.MainUserControl" %>
<%@ Register Src="~/API/TGZCOGetEnedoShopInfo.ascx" TagPrefix="uc1" TagName="TGZCOGetEnedoShopInfo" %>
<%@ Register Src="~/API/usercontrolsplit.ascx" TagPrefix="uc1" TagName="usercontrolsplit" %>
<%@ Register Src="~/API/myDropdown.ascx" TagPrefix="uc1" TagName="myDropdown" %>



My Shop
<uc1:TGZCOGetEnedoShopInfo runat="server"  />
Split
<uc1:usercontrolsplit runat="server" ID="usercontrolsplit" />
myDropdown
<uc1:myDropdown runat="server" ID="myDropdown" />

<script type="text/javascript">
    $('#MainContent_ctl00_myDropdown_MyShopDropDownList').on("click", function () {
        console.log("here");
        jqxhr = $.ajax({
            type: "GET",
            url: "default.aspx",
            data: { customerNumber: "0039999080", postcode2: "second" },
            context: document.body,
            dataType: "json",
            success: function (json) {

                // MainContent_MainUserControl2_myDropdown1_postcode1
                console.log(json.address1);
                console.log(json.returnCode);
            }
        });
        event.preventDefault();
    });
</script>
