<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TGZCOGetEnedoShopInfo1.ascx.cs" Inherits="WebAppDotNetWebFormsTest.API.TGZCOGetEnedoShopInfo" %>
<asp:DropDownList ID="MyShopList" ClientIDMode="Static"  runat="server"></asp:DropDownList>

<script type="text/javascript">
    $('#MyShopList').on("click", function () {
        console.log("MyShopList");
        $("#MyShopList").empty();
        var select = document.getElementById("MyShopList");
        var options = ["1", "2", "3", "4", "5"];

        for (var i = 0; i < options.length; i++) {
            var opt = options[i];
            var el = document.createElement("option");
            el.textContent = opt;
            el.value = opt;
            select.appendChild(el);
        }

        organizationnumber = "000";
        jqxhr = $.ajax({
            type: "GET",
            url: "default.aspx",
            data: { loaded:'loaded', organizationnumber: organizationnumber },
            context: document.body,
            dataType: "json",
            success: function (json) {
                console.log(json.address);
                console.log(json.phonenumber);
                console.log(json.servicename);
                console.log(json.proprietaryservice);
                document.getElementById("txtAddress").value = json.address;
                document.getElementById("txtPhoneNumber").value = json.phonenumber;
                document.getElementById("txtServiceName").value = json.servicename;
                document.getElementById("txtProprietaryService").value = json.proprietaryservice;

            },
            error: function (request, error) {
                // This callback function will trigger on unsuccessful action                
                console.log(request);
                console.log(error);
                alert('Error!');
            }
        });
        console.log(jqxhr);
        event.preventDefault();
    });
</script>