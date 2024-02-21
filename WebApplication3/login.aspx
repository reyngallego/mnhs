<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication3.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <link href="Content/bootstrap.rtl.min.css" rel="stylesheet" />
    <script src="scripts/bootstrap.bundle.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Montserrat:wght@300;400;500;600&display=swap" />
    <title>Queue Management System</title>
    <link rel="stylesheet" type="text/css" href="styles/stylesheet.css" />
</head>
<body>
    <div class="frame-parent">
        <div class="rectangle-parent">
            <div class="frame-child">
                <div class="login-container">
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/logo2.png" AlternateText="Logo" CssClass="logo-image" />
                    <h2>PUPPQ CAMPUS QUEUE MANAGEMENT SYSTEM</h2>
                    <p>Sign in to start your session</p>
                    <div id="errorMessage" class="error-message" runat="server"></div>
                    <form id="Form1" runat="server" onsubmit="return validateForm()">

                        <div class="form-group">
<label for="username">
    <img src="images/icons/employeeid.png" alt="Employee ID Icon" width="20" height="20"/>
    EmployeeId
</label>                            <asp:TextBox ID="username" runat="server" CssClass="input-field" />
                        </div>
                        <div class="form-group">
<label for="password">
    <img src="images/icons/Password.png" alt="Password Icon" width="20" height="20"/>
    Password
</label>                            <asp:TextBox ID="password" runat="server" CssClass="input-field" TextMode="Password" />
                        </div>
                        <div class="sign-in">
                            <asp:Button ID="btnSignIn" runat="server" Text="Sign In" OnClick="btnSignIn_Click" OnClientClick="return validateForm();" />
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        // Add this script to set placeholder text
        window.onload = function () {
            var usernameTextBox = document.getElementById("<%= username.ClientID %>");
            var passwordTextBox = document.getElementById("<%= password.ClientID %>");

            usernameTextBox.placeholder = "Enter Username";
            passwordTextBox.placeholder = "Enter Password";
        }

        function validateForm() {
            // Rest of your validation code
        }
    </script>
</body>
</html>
