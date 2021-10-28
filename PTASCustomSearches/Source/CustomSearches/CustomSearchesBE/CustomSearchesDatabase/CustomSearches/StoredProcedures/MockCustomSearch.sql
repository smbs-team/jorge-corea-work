CREATE PROCEDURE [cus].[MockCustomSearch]
	@numberOfBuildings INT
AS

	SELECT * FROM [dynamics].ptas_parceldetail2_CustomSearches
		WHERE ptas_numberofbuildings = @numberOfBuildings

RETURN
