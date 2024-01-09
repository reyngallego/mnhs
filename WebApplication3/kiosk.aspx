<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kiosk.aspx.cs" Inherits="WebApplication3.kiosk" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PUP QUEUE MANAGEMENT SYSTEM</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap" rel="stylesheet"/>

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <link rel="stylesheet" type="text/css" href="styles/kiosk.css" />
    <script type="text/javascript">

        function insertNewStudentData(newData) {
            $.ajax({
                type: "POST",
                url: "/api/queue/submitqueuedata", // Update the URL to match your API endpoint
                data: JSON.stringify(newData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Handle the successful insertion

                    // After successful insertion, retrieve updated student data
                    getStudentData();
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }

        function showQueueTicket(queueTicket) {
            $('#queueTicketModal').modal('show');
            $('#queueTicketValue').text(queueTicket);
        }
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
            <h1>PUP PARANAQUE CAMPUS QUEUE MANAGEMENT SYSTEM</h1>
            
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
                    <td><asp:TextBox ID="txtPurpose" runat="server" required="true" CssClass="input-field"></asp:TextBox></td>
                </tr>
            </table>

            <br />

            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-submit"
OnClientClick='<%# "showQueueTicket(\"" + GeneratedQueueTicket + "\");" %>' />
        </div>
    </form>
</body>
</html>
