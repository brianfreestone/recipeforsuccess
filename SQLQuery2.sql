USE [recipesuccess]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[spGetFriends]
		@PageNumber = 1,
		@PageSize = 10,
		@UserID = 35

SELECT	@return_value as 'Return Value'

GO
