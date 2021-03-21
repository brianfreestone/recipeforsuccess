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
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="Data Source=INFO4430-rs-dev\sqlexpress;Initial Catalog=recipesuccess;Persist Security Info=True;User ID=sa;Password=IPp2muWQ1f5s" ProviderName="System.Data.SqlClient" SelectCommand="SELECT recipe_instruction.measure_value, ingredient.name, recipe_instruction.instruction, recipe_instruction.ingredient_id FROM recipe_instruction INNER JOIN ingredient ON recipe_instruction.ingredient_id = ingredient.ingredient_id WHERE (recipe_instruction.recipe_id = @recipe_id)">
        <SelectParameters>
            <asp:Parameter Name="recipe_id" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="Data Source=INFO4430-rs-dev\sqlexpress;Initial Catalog=recipesuccess;Persist Security Info=True;User ID=sa;Password=IPp2muWQ1f5s" ProviderName="System.Data.SqlClient" SelectCommand="SELECT pu.user_id AS primaryUserID, pu.username AS primaryUsername, pu.first_name + ' ' + pu.last_name AS primaryUser, su.user_id AS secondaryUserID, su.username AS secondaryUsername, su.first_name + ' ' + su.last_name AS secondaryUser FROM friends INNER JOIN users AS pu ON friends.primary_user_id = pu.user_id INNER JOIN users AS su ON friends.secondary_user_id = su.user_id WHERE (friends.primary_user_id = @userID) OR (friends.secondary_user_id = @userID)">
        <SelectParameters>
            <asp:Parameter Name="userID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="Data Source=INFO4430-rs-dev\sqlexpress;Initial Catalog=recipesuccess;Persist Security Info=True;User ID=sa;Password=IPp2muWQ1f5s" ProviderName="System.Data.SqlClient" SelectCommand="SELECT recipe.name, recipe.created, users.username, users.first_name, users.last_name, users.user_id FROM recipe INNER JOIN users ON recipe.user_id = users.user_id WHERE (recipe.user_id IN (SELECT primary_user_id FROM (SELECT friends.secondary_user_id, friends_1.primary_user_id FROM friends CROSS JOIN friends AS friends_1 WHERE (friends.primary_user_id = @userID) AND (friends_1.secondary_user_id = @userID)) AS derivedtbl_1 UNION SELECT secondary_user_id FROM (SELECT friends_2.secondary_user_id, friends_1.primary_user_id FROM friends AS friends_2 CROSS JOIN friends AS friends_1 WHERE (friends_2.primary_user_id = @userID) AND (friends_1.secondary_user_id = @userID)) AS derivedtbl_1_1))">
        <SelectParameters>
            <asp:Parameter Name="userID" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
