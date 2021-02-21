<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="RecipeForSuccess_mvc.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=INFO4430-rs-dev\sqlexpress;Initial Catalog=recipesuccess;Persist Security Info=True;User ID=sa;Password=IPp2muWQ1f5s" ProviderName="System.Data.SqlClient" SelectCommand="SELECT users.user_id, users.username, users.first_name, users.last_name, users.is_admin FROM users INNER JOIN passwords_users ON users.user_id = passwords_users.user_id INNER JOIN passwords ON passwords_users.password_id = passwords.password_id WHERE (@password = (SELECT TOP (1) p.password FROM users AS u INNER JOIN passwords_users AS pu ON u.user_id = pu.user_id INNER JOIN passwords AS p ON pu.password_id = p.password_id WHERE (u.email = @email) ORDER BY p.password_changed DESC))">
            <SelectParameters>
                <asp:Parameter Name="password" />
                <asp:Parameter Name="email" />
            </SelectParameters>
        </asp:SqlDataSource>
</body>
</html>
<asp:SqlDataSource runat="server" ConnectionString="Data Source=INFO4430-rs-dev\sqlexpress;Initial Catalog=recipesuccess;Persist Security Info=True;User ID=sa;Password=IPp2muWQ1f5s" ProviderName="System.Data.SqlClient" SelectCommand="SELECT users.username, users.email, passwords.password FROM users INNER JOIN passwords_users ON users.user_id = passwords_users.user_id INNER JOIN passwords ON passwords_users.password_id = passwords.password_id"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
    </form>
