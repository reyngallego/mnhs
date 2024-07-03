<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="stud-search-list.aspx.cs" Inherits="WebApplication3.stud_search_list" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Search List</title>
    <link href="styles/stud-search-list.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="banner">
            <div class="overlay">
                <img src="images/logomnhs.png" alt="Logo" class="banner-logo">
            </div>
        </div>
        <div class="container">
            <h1>Student Search Results</h1>
            <table id="stud-search-res">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Grade and Section</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Harris, Leanne  F.</td>
                        <td>11 - Gold</td>
                    </tr>
                    <tr>
                        <td>Harris, Mohammad Jaoshannee O.</td>
                        <td>11 - Aluminum</td>
                    </tr>
                </tbody>
            </table>
            <button type="button" class="close-button">Close</button>
        </div>
    </form>
</body>
</html>
