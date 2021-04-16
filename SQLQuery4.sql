USE [recipesuccess]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[spGetFriendsRecipeIDs]
		@userID = 35

SELECT	@return_value as 'Return Value'

GO
