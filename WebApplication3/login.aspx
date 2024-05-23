<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication3.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
        <link href="styles/login.css" rel="stylesheet" /> <!-- Link to custom CSS file -->

    <script type="text/javascript">
        function validateForm() {
            var username = document.getElementById('<%= txtUsername.ClientID %>').value;
            var password = document.getElementById('<%= txtPassword.ClientID %>').value;

            if (username === "" || password === "") {
                alert("Username and Password are required.");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" onsubmit="return validateForm()">
        <div class="container">
            <div class="row justify-content-center align-items-center" style="height:90vh;">
                <div class="col-md-6">
                    <div class="login-container">
                        <h2 class="text-center mb-4">Login</h2>
                        <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="text-danger"></asp:Label>
                        <div class="form-group">
                            <label for="txtUsername">Username:</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPassword">Password:</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
