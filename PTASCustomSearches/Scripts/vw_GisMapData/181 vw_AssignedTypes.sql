IF EXISTS (SELECT * FROM sys.objects WHERE type = 'V' AND name = 'vw_AssignedTypes')
    DROP VIEW dynamics.vw_AssignedTypes ;  
GO 

CREATE VIEW dynamics.vw_AssignedTypes
WITH SCHEMABINDING
AS
/*
Author: Jairo Barquero
Date Created:  01/13/2021
Description:    This View returns the Assigned types

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SELECT  pa.RpGuid AS PArcelId
	   ,pa.AssmtYr
	   ,pa.AssignmentTypeItemId
	   ,su.ptas_legacyid
  FROM [rp].[ParcelAssignment] pa
 INNER JOIN [dynamics].[systemuser] su 	ON pa.AssignedToGuid = SU.systemuserid 

