<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication3.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="styles/login.css" rel="stylesheet" /> <!-- Link to custom CSS file -->
</head>
<body>
    <div class="container">
        <div class="left">
            <div class="login-form">
                <h1>Admin Login</h1>
                <h4>Please enter your details</h4>
                <form action="/login" method="POST">
                    <label for="username">Username:</label>
                    <input type="text" id="txtUsername" name="username" required=""/>
                    <label for="password">Password:</label>
                    <input type="password" id="txtPassword" name="password" required=""/>
                    <button type="submit" id="btnLogin">Login</button>
                </form>
            </div>
        </div>
        <div class="right">
            <img src="../images/logomnhs.png" alt="Logo" class="logo"/>
            <div class="picture-form">
                <div class="overlay"></div>
            </div>
        </div>
    </div>
</body>
</html>
