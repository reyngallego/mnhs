<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebApplication3.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="styles/index.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="left-pane">
            <div class="overlay"></div>
            <div class="left-pane-content">
                <img src="images/logomnhs.png" alt="Logo" class="logo">
                <label class="title">Student Attendance Monitoring System</label>
                <div class="search-container">
                    <input type="text" class="search-bar" placeholder="Enter student's surname">
                    <button type="button" class="search-button">
                        <img src="images/icons/search-white.png" class="search-icon"/>
                    </button>
                </div>
            </div>
            <div class="footer-landing">
                <div class="footer-images">
                    <img src="images/moonwalkLogo.png" alt="Moonwalk Logo" class="footer-logo">
                    <img src="images/malasaquitLogo.png" alt="Malasaquit Logo" class="footer-logo">
                    <img src="images/logopup.png" alt="PUP Logo" class="footer-logo">
                </div>
                <h1 class="footer-caption">In partnership with Polytechnic University of the Philippines - Parañaque City Campus</h1>
            </div>
        </div>
        <div class="right-pane"></div>
    </form>
</body>
</html>
