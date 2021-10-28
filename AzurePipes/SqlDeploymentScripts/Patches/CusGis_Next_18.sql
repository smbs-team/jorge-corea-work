DROP VIEW IF EXISTS dynamics.vw_LandModelAdjustments
DROP FUNCTION IF EXISTS [cus].[FN_GetNonModelAdjs] 
DROP FUNCTION IF EXISTS [cus].[FN_ShouldApplyModelAdj]
GO -- 1. Drops Land Adjustments items

CREATE VIEW dynamics.vw_LandModelAdjustments

AS
/*
Author: Jairo Barquero
Date Created:  06/17/2021
Description:    List of characterictics for Model Adjustments

IMPORTANT NOTE: Model Adjustments are new, this view and the functions related to model required proper testing to be integrated.
				I'm not sure what characteristics can have model adjustments, that is why I'm adding the 5 different types.
				

Modifications:
07/26/2021 - Hairo Barquero: change the filter to add only Model, Model $ adjustment and Model % adjustment
08/12/2021 - Hairo Barquero: Only filter by ptas_valuemethodcalculation = Model and delete filter "ptas_percentadjustment <> 0"
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/


/*
These are the only characteristics with adjustments currently:
591500000 --Development Rights
668020004 --Environmental Restriction
668020005 --Designation
668020006 --Nuisance
**668020003 --View / I´m adding View type becasue seems like they are going to have adjustments in some point
*/
select plvc._ptas_landid_value, ptas_characteristictype AS CharacteristicTypeId, ptas_designationtype			AS CharacteristicTypeSubtypeId, 0 AS AdjustmentAttributeId, ptas_percentadjustment FROM [dynamics].[ptas_landvaluecalculation] plvc	 WHERE  plvc.ptas_characteristictype = 591500000
AND ptas_valuemethodcalculation = 668020005 	--Model
union all
select plvc._ptas_landid_value, ptas_characteristictype AS CharacteristicTypeId, ptas_environmentalrestriction AS CharacteristicTypeSubtypeId,0 AS AdjustmentAttributeId, ptas_percentadjustment FROM [dynamics].[ptas_landvaluecalculation] plvc	 WHERE  plvc.ptas_characteristictype = 668020004
AND ptas_valuemethodcalculation = 668020005 	--Model
union all
select plvc._ptas_landid_value, ptas_characteristictype AS CharacteristicTypeId, ptas_designationtype			AS CharacteristicTypeSubtypeId,0 AS AdjustmentAttributeId, ptas_percentadjustment FROM [dynamics].[ptas_landvaluecalculation] plvc	 WHERE  plvc.ptas_characteristictype = 668020005
AND ptas_valuemethodcalculation = 668020005 	--Model
union all
select plvc._ptas_landid_value, ptas_characteristictype AS CharacteristicTypeId, ptas_nuisancetype				AS CharacteristicTypeSubtypeId,COALESCE(ptas_noiselevel,0) AS AdjustmentAttributeId, ptas_percentadjustment FROM [dynamics].[ptas_landvaluecalculation] plvc	 WHERE  plvc.ptas_characteristictype = 668020006
AND ptas_valuemethodcalculation = 668020005 	--Model
UNION ALL
select plvc._ptas_landid_value, ptas_characteristictype AS CharacteristicTypeId, ptas_viewtype					AS CharacteristicTypeSubtypeId,COALESCE(ptas_quality,0)    AS AdjustmentAttributeId, COALESCE(ptas_percentadjustment,0) AdjustmentPct FROM [dynamics].[ptas_landvaluecalculation] plvc	 WHERE  plvc.ptas_characteristictype = 668020003
AND ptas_valuemethodcalculation = 668020005 	--Model
--There are types that I´m not sure if will have an adjustment in some point
/*
668020000	Zoning
668020001	Submerged Land
668020002	Waterfront
668020003	View
668020007	Land Schedule
668020008	Manual Adjustment
*/

GO -- 2. Create Lands Adjustments view

CREATE FUNCTION [cus].[FN_GetNonModelAdjs]
(
    @major NVARCHAR(6),
    @minor NVARCHAR(4),
    @landValue FLOAT
)
RETURNS FLOAT
WITH SCHEMABINDING
AS 
BEGIN
/*
Author: Jairo Barquero
Date Created:  11/18/2020
Description: Returns the $ sum of all the non-model adjustments for a parcel. (Value method different than “model”)

IMPORTANT NOTE: Currently for Traffic Noise there is NO VALUE in ptas_dollaradjustment, the values are dirrectly in ptas_adjustedvalue
				for that reaon I´m adding the third CASE statement to sumarize the corresponding value.

Modifications:
mm/dd/yyyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/

	Declare @LandGuid as uniqueidentifier,
			@AdjustmentsValue Float

	select @LandGuid = _ptas_landid_value from dynamics.ptas_parceldetail
	where ptas_snapshottype is NULL AND statecode = 0AND statuscode = 1
	and ptas_major = @major
	and ptas_minor = @minor

	
	SELECT	@AdjustmentsValue = SUM
								(
								CASE 
									WHEN ptas_percentadjustment <> 0 THEN (@LandValue * ptas_percentadjustment) / 100
									ELSE 0
								END
								+
								CASE 
									WHEN ptas_dollaradjustment <> 0 THEN ptas_dollaradjustment
									ELSE 0
								END
								+
								CASE 
									WHEN ptas_adjustedvalue <> 0 AND ptas_dollaradjustment IS NULL AND  ptas_percentadjustment IS NULL THEN ptas_adjustedvalue 
									ELSE 0
								END
								)
	   FROM dynamics.ptas_landvaluecalculation
	  WHERE ptas_valuemethodcalculation in(668020000,668020001) --% adjustment / $ adjustment
		AND ptas_adjustedvalue <> 0
		AND _ptas_landid_value = @LandGuid

	RETURN COALESCE(@AdjustmentsValue, 0)
END

GO -- 3. Create [FN_GetNonModelAdjs] function
CREATE FUNCTION [cus].[FN_ShouldApplyModelAdj]
(
    @major NVARCHAR(10),
    @minor NVARCHAR(10),
    @characteristicTypeId int,							-- View / Nuisance etc
	@characteristicType NVARCHAR(MAX) = NULL,			-- Not Used
    @characteristicTypeSubtypeId int,					-- What type of View or Nuisance
	@characteristicTypeSubtype NVARCHAR(MAX) = NULL,	-- Not Used
	@adjustmentAttributeName NVARCHAR(MAX) = NULL,		-- Not Used
    @adjustmentAttributeId int = 0,      				-- The intensity of the char
	@adjustmentAttribute NVARCHAR(MAX) = NULL			-- Not Used
)
RETURNS BIT
AS 
BEGIN
	DECLARE @AdjustmentExists BIT
		/*   ,@AdjustmentValue Float
	
	SELECT top 1 @AdjustmentValue	= ptas_percentadjustment
	  FROM dynamics.vw_LandModelAdjustments
	 WHERE CharacteristicTypeId			= @characteristicTypeId			
	   AND CharacteristicTypeSubtypeId	= @characteristicTypeSubtypeId	
	   --AND AdjustmentAttributeId		= @adjustmentAttributeId		
	   AND _ptas_landid_value			= @LandGuid
	*/
	Declare @LandGuid as uniqueidentifier
	--		@AdjustmentValue Float

	select @LandGuid = _ptas_landid_value from dynamics.ptas_parceldetail
	where ptas_snapshottype is NULL AND statecode = 0AND statuscode = 1
	and ptas_major = @major
	and ptas_minor = @minor

	SELECT @AdjustmentExists =	CASE		
									WHEN (	SELECT top 1  ptas_percentadjustment
											  FROM dynamics.vw_LandModelAdjustments
											 WHERE CharacteristicTypeId			= @characteristicTypeId			
											   AND CharacteristicTypeSubtypeId	= @characteristicTypeSubtypeId	
											   --AND AdjustmentAttributeId		= @adjustmentAttributeId		
											   AND _ptas_landid_value			= @LandGuid
											) IS NULL THEN 0
									ELSE 1
								END 
	RETURN @AdjustmentExists

END


GO -- 4. Create [FN_ShouldApplyModelAdj] function

DROP INDEX IF EXISTS [IX_RScriptModel_IsDeleted] ON [cus].[RScriptModel]
EXEC SP_DropColumn_With_Constraints  '[cus].[RScriptModel]', 'IsDeleted'
ALTER TABLE [cus].[RScriptModel] ADD [IsDeleted] BIT NOT NULL DEFAULT 0

CREATE INDEX [IX_RScriptModel_IsDeleted] ON [cus].[RScriptModel] ([IsDeleted])


GO -- 5. Add [IsDeleted] column to [cus].[RScriptModel] table

DROP TABLE IF EXISTS [cus].[DynamicSqlLog]

CREATE TABLE [cus].[DynamicSqlLog]
(
	[DynamicSqlLogId] INT NOT NULL IDENTITY,     
    [UserId] UNIQUEIDENTIFIER NOT NULL,
	[JobId] INT NULL,
	[SqlLog] NVARCHAR(MAX) NULL,
	[Parameters] NVARCHAR(MAX) NULL,
	[LogContext] NVARCHAR(MAX) NULL,
	[CreatedTimestamp] DateTime NOT NULL DEFAULT (getdate()),
	

	CONSTRAINT [PK_DynamicSqlLog] PRIMARY KEY ([DynamicSqlLogId])
)

CREATE INDEX [IX_DynamicSqlLog_UserId] ON [cus].[DynamicSqlLog] ([UserId])
CREATE INDEX [IX_DynamicSqlLog_JobId] ON [cus].[DynamicSqlLog] ([JobId])
CREATE INDEX [IX_DynamicSqlLog_CreatedTimestamp] ON [cus].[DynamicSqlLog] ([CreatedTimestamp])

GO -- 6. Add [cus].[DynamicSqlLog] table