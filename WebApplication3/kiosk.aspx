<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kiosk.aspx.cs" Inherits="WebApplication3.kiosk" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PUP PARANAQUE QUEUE MANAGEMENT SYSTEM</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <link rel="stylesheet" type="text/css" href="styles/kiosk.css" />
  <script type="text/javascript">

    function showQueueTicket(queueTicket) {
        $('#queueTicketModal').modal('show');
        $('#queueTicketValue').text(queueTicket);
        $('#<%= btnSubmit.ClientID %>').prop('disabled', true);
    }

  $(document).ready(function () {
    $('#queueTicketModal').on('hidden.bs.modal', function () {
        window.location.href = window.location.href; // Redirect to the same page
    });

    var purposeOptions = {
        'REGISTRAR': ['Getting Diploma', 'Other Registrar Purposes'],
        'CASHIER': ['Paying Tuition Fee', 'Paying Uniform Fee', 'Other Cashier Purposes'],
        'DIRECTOR': ['Signing Paper', 'Other Director Purposes'],
        'STUDENTAFFAIRSANDSERVICES': ['Getting School ID', 'Reporting Bullying', 'Other Student Affairs Purposes']
    };

    function updatePurposeOptions() {
        var selectedDepartment = $('#<%= ddlDepartment.ClientID %>').val();
        var purposeDropdown = $('#<%= ddlPurpose.ClientID %>');

        // Clear existing options
        purposeDropdown.empty();

        // Populate options based on the selected department
        var options = purposeOptions[selectedDepartment] || [];
        for (var i = 0; i < options.length; i++) {
            purposeDropdown.append($('<option>', {
                value: options[i],
                text: options[i]
            }));
        }

        // Set the first option as the default selected value
        if (options.length > 0) {
            purposeDropdown.val(options[0]);
        }
    }

    // Attach the update function to the change event of the Department dropdown
    $('#<%= ddlDepartment.ClientID %>').change(updatePurposeOptions);

    // Call the update function on page load to set initial options
    updatePurposeOptions();
});


</script>



</head>
<body>
    <div class="modal fade" id="queueTicketModal" tabindex="-1" role="dialog" aria-labelledby="queueTicketModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="queueTicketModalLabel">Queue Ticket Information</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>Your queue ticket is: <span id="queueTicketValue"></span></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <form id="form1" runat="server">
        <div class="container">
            <div class="logo-container">
                <img src="/images/logo2.png" alt="Logo" class="logo">
            </div>
            <div class="content-container">
                <h1 class="logo-heading">PUP PARANAQUE CAMPUS QUEUE MANAGEMENT SYSTEM</h1>
                <!-- Other content goes here -->

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br /><br />

                <table>
                    <tr>
                        <td>Name:</td>
                        <td><asp:TextBox ID="txtName" runat="server" required="true" CssClass="input-field"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Department:</td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="input-field">
                                <asp:ListItem Value="REGISTRAR">REGISTRAR</asp:ListItem>
                                <asp:ListItem Value="CASHIER">CASHIER</asp:ListItem>
                                <asp:ListItem Value="DIRECTOR">DIRECTOR</asp:ListItem>
                                <asp:ListItem Value="STUDENTAFFAIRSANDSERVICES">STUDENT AFFAIRS AND SERVICES</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Purpose:</td>
                        <td>    <asp:DropDownList ID="ddlPurpose" runat="server" required="true" CssClass="input-field"></asp:DropDownList>
</td>
                    </tr>
                </table>

                <br />

                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-submit"
                    OnClientClick='<%# "showQueueTicket(\"" + GeneratedQueueTicket + "\");" %>' />
            </div>
        </div>
    </form>
</body>
</html>
