<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usercontrolsplit.ascx.cs" Inherits="WebAppDotNetWebFormsTest.Layout.usercontrolsplit" %>

<asp:DropDownList ID="DropDownSplit" OnClick="DropDownList_Click()" onchange="DropDownList_Change()" ClientIDMode="Static"   runat="server"></asp:DropDownList>
<script runat="server">


</script>

<script type="text/javascript">
    var initial = true;
    function DropDownList_Click() {
        console.log("JS hello");
        console.log("DropDownSplit");
        if (initial) {
            initial =false;
            $("#DropDownSplit").empty();
            var select = document.getElementById("DropDownSplit");
            var options = ["10", "20", "30", "40", "50"];

            for (var i = 0; i < options.length; i++) {
                var opt = options[i];
                var el = document.createElement("option");
                el.textContent = opt;
                console.log(el.textContent);
                el.value = opt;
                select.appendChild(el);
            }
            jqxhr = $.ajax({
                type: "GET",
                url: "default.aspx",
                data: { customerNumber: "0039999080", postcode2: "second" },
                context: document.body,
                dataType: "json",
                success: function (json) {
                    document.getElementById("MainContent_MainUserControl2_myDropdown1_postcode1").value = json.address1;
                    document.getElementById("MainContent_MainUserControl2_myDropdown1_postcode2").value = json.returnCode;
                    // MainContent_MainUserControl2_myDropdown1_postcode1
                    console.log(json.address1);
                    console.log(json.returnCode);
                }
            });
        }
       
    }
    function DropDownList_Change() {

        event.preventDefault();

    }

</script>