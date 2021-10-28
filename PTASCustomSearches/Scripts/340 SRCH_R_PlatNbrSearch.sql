
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_PlatNbrSearch')
	DROP PROCEDURE [cus].[SRCH_R_PlatNbrSearch]  
GO

CREATE PROCEDURE [cus].[SRCH_R_PlatNbrSearch]  	@PlatNbr	char(6)
											   ,@PlatLot	char(14) = NULL
											   ,@PlatBlock	char(7)  = NULL

AS BEGIN
/*
Author: Jairo Barquero
Date Created:  11/12/2020
Description:    SP that pulls all records  filtered by parameter @PlatNbr, @PlatLot or @PlatBlock

Modifications:
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]
*/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
/****************** Create Temp Table *************** */

CREATE TABLE #PropType
(
ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY ,
PropType NVARCHAR(1)
)
INSERT INTO #PropType
SELECT  ptas_propertyTypeId       
		,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt

Create Table #PlatNbrSearch
	( RecId int Identity(1,1)
    ,ParcelId  uniqueidentifier 
	,Major char(6) Null 
	,Minor char(4) Null 
    ,Area smallint Null
    ,SubArea smallint Null
	,QuarterSection char(2) Null 
	,Section tinyint Null 
	,Township tinyint Null 
	,Range tinyint Null 
	,LevyCode char(4) Null
	,Folio varchar(7) Null
	,TaxpayerName nvarchar(max) Null 
	,StreetNbr nvarchar(max) Null 
	,NbrFraction nvarchar(max) Null 
	,DirPrefix char(2) Null 
	,StreetName nvarchar(max) Null 
	,StreetType char(4) Null 
	,DirSuffix char(2) Null 
	,UnitDescr nvarchar(max) Null
	,ZipCode varchar(10)
	,PropType varchar(30) Null  --Hairo comment changed from "char(1)" to varchar(30) due "Mobile Home" and "Floating Home", when finish this search I need to know if I´m not including these in the resultset.
	,PlatLot char(14) Null
	,PlatBlock char(7) Null
	,PlatTypeCode varchar(10)	--Hairo Comment changed from "tinyint Null" to varchar(19) duo the 'NOT FOUND' 
	,PlatName nvarchar(max) Null
    ,Pin char(10) Null
    ,MapPin float Null)

Create Index Ind1 ON #PlatNbrSearch (ParcelId, Major)


/********** End Create Table *************************************/



/* Determine what kind of search it is */
/* Per bb have to allow either lot or block or both.
** Don't think I can use dynamic Where because
** EXEC won't work with temp table created here
** so just have  a lot of options. */
Declare @IncludesLot Bit
Declare @IncludesBlock Bit
Declare @PlatExists Bit


IF ISNULL(@PlatLot,'') <> ''
	SELECT @IncludesLot = 1
ELSE	
	SELECT @IncludesLot = 0

IF ISNULL(@PlatBlock,'') <> ''
	SELECT @IncludesBlock = 1
ELSE	
	SELECT @IncludesBlock = 0

IF ISNULL(@PlatNbr,'') <> ''
	SELECT @PlatExists = 1
ELSE	
	SELECT @PlatExists = 0

/*This is a double check, @BadLotBlock should be always = 0, that means the @PlatNbr always come with a value*/
DECLARE @BadLotBlock Bit
SELECT @BadLotBlock = 0
If (@IncludesLot = 1 or @IncludesBlock = 1) And @PlatExists <> 1 
	SELECT @BadLotBlock = 1

IF @BadLotBlock = 0 
BEGIN

IF @PlatExists = 1

  IF @IncludesLot  = 0 And @IncludesBlock = 0 
  BEGIN	
     INSERT 
       INTO #PlatNbrSearch (ParcelId)
     SELECT dpd.ptas_parceldetailid
       FROM [dynamics].[ptas_parceldetail] dpd 
      WHERE	dpd.ptas_major = TRIM(@PlatNbr)
	    AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
        AND COALESCE(dpd.ptas_snapshottype,'') = '' 
	    AND COALESCE(dpd.ptas_splitcode,0) = 0
  END
  
  IF @IncludesLot = 1 AND @IncludesBlock = 1
  BEGIN	
     INSERT 
       INTO #PlatNbrSearch (ParcelId)
     SELECT dpd.ptas_parceldetailid
       FROM [dynamics].[ptas_parceldetail] dpd 
      WHERE	dpd.ptas_major = TRIM(@PlatNbr)
        AND	dpd.ptas_platlot   Like ISNULL(TRIM(@PlatLot),'')   + '%'
        AND	dpd.ptas_platblock Like ISNULL(TRIM(@PlatBlock),'') + '%'	
	    AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
        AND COALESCE(dpd.ptas_snapshottype,'') = '' 
	    AND COALESCE(dpd.ptas_splitcode,0) = 0		
  END
   
  IF @IncludesLot = 1 AND @IncludesBlock = 0
  BEGIN	
     INSERT 
       INTO #PlatNbrSearch (ParcelId)
     SELECT dpd.ptas_parceldetailid
       FROM [dynamics].[ptas_parceldetail] dpd 
      WHERE	dpd.ptas_major = TRIM(@PlatNbr)
        AND	dpd.ptas_platlot   Like ISNULL(TRIM(@PlatLot),'')   + '%'
	    AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
        AND COALESCE(dpd.ptas_snapshottype,'') = '' 
	    AND COALESCE(dpd.ptas_splitcode,0) = 0		
  END
    
  If @IncludesLot = 0 AND @IncludesBlock = 1
  BEGIN	
     INSERT 
       INTO #PlatNbrSearch (ParcelId)
     SELECT dpd.ptas_parceldetailid
       FROM [dynamics].[ptas_parceldetail] dpd 
      WHERE	dpd.ptas_major = TRIM(@PlatNbr)
        AND	dpd.ptas_platblock Like ISNULL(TRIM(@PlatBlock),'') + '%'	
	    AND (dpd.statecode  = 0 AND dpd.statuscode = 1)
        AND COALESCE(dpd.ptas_snapshottype,'') = '' 
	    AND COALESCE(dpd.ptas_splitcode,0) = 0		
  END


END /*@BadLotBlock */

UPDATE #PlatNbrSearch
   SET  Major          = dpd.ptas_major
       ,Minor          = dpd.ptas_minor
       ,Area           =  CASE WHEN dpd.ptas_proptype = 'R' THEN  COALESCE(dpd.ptas_resarea,'')
	                     	   WHEN dpd.ptas_proptype = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN  dpd.ptas_commarea
	                     	   ELSE dpd.ptas_commarea
	                      END
       ,SubArea        =  CASE WHEN dpd.ptas_proptype = 'R' THEN  COALESCE(dpd.ptas_ressubarea,'')
	                     	   WHEN dpd.ptas_proptype = 'K' AND COALESCE(dpd.ptas_minor,'') = '0000' THEN dpd.ptas_commsubarea
	                     	   ELSE dpd.ptas_commsubarea
	                      END
       ,QuarterSection = COALESCE(dsm.value,'')
       ,Section        = pqstr.ptas_section	
       ,Township       = pqstr.ptas_township
       ,Range          = pqstr.ptas_range		
       ,LevyCode       = dpd.ptas_levyCode
       ,Folio          = dpd.ptas_folio
       ,TaxpayerName   = CASE WHEN SUBSTRING(dpd.ptas_acctnbr,11,1) >= '0' THEN SUBSTRING(dpd.ptas_namesonaccount,1,256) ELSE '' END
       ,StreetNbr      = COALESCE(dpd.ptas_addr1_streetnumber,'')
       ,NbrFraction    = COALESCE(dpd.ptas_nbrfraction,'')
       ,DirPrefix      = COALESCE(dsm02.value,'')
       ,StreetName     = SUBSTRING(dpd.ptas_streetname, 1,256)
       ,StreetType     = dpd.ptas_streettype
       ,DirSuffix      = dpd.ptas_dirsuffix
       ,UnitDescr      = SUBSTRING(dpd.ptas_addr1_line2,1,100)		
       ,ZipCode        = COALESCE(dpd.ptas_zipcode,'')
       ,PropType       = dpd.ptas_proptype
       ,PlatLot        = COALESCE(dpd.ptas_platlot,'')
       ,PlatBlock      = COALESCE(dpd.ptas_platblock,'')
       --,PlatTypeCode   = 'NOT FOUND'	  --p.PlatTypeCode Hairo Comment, NOT FOUND, check tables related to ptas_abstractproject / ptas_majornumberindex /ptas_majornumberdetail
       --,PlatName       = 'NOT FOUND'	  --p.PlatName 	   Hairo Comment, NOT FOUND, check tables related to ptas_abstractproject / ptas_majornumberindex /ptas_majornumberdetail
       ,Pin            = COALESCE(dpd.ptas_major,'') + COALESCE(dpd.ptas_minor,'')
       ,MapPin         = CASE WHEN dpd.ptas_proptype = 'K' then convert(float, dpd.ptas_major + '0000')
                              ELSE convert(float, dpd.ptas_major + dpd.ptas_minor)
                         END
  FROM dynamics.ptas_parceldetail dpd 
 INNER JOIN #PlatNbrSearch pns
    ON pns.ParcelId = dpd.ptas_parceldetailid  
  LEFT JOIN [dynamics].[ptas_qstr] pqstr
    ON pqstr.[ptas_qstrid] = dpd._ptas_qstrid_value  
  LEFT JOIN [dynamics].[stringmap]	dsm
    ON dsm.attributevalue = pqstr.ptas_quartersection 
   AND dsm.objecttypecode = 'ptas_qstr'
   AND dsm.attributename  = 'ptas_quartersection'	

  LEFT JOIN [dynamics].[stringmap]	dsm02
    ON dsm02.attributevalue = dpd.ptas_addr1_directionprefix
   AND dsm02.objecttypecode = 'ptas_parceldetail'
   AND dsm02.attributename  = 'ptas_addr1_directionprefix'	


 SELECT ParcelId
    	,Major
    	,Minor
    	,TaxpayerName
    	,Street = ISNULL((LTRIM(LTRIM(LTRIM(LTRIM(LTRIM(RTRIM(StreetNbr) + ' ' + RTRIM(NbrFraction) + ' ' + RTRIM(DirPrefix) + ' ' + RTRIM(StreetName)) + ' ' + RTRIM(StreetType)) + ' ' + RTRIM(DirSuffix))))),'') 
    	,StreetNbr   = ISNULL(StreetNbr,'')   
    	,NbrFraction = ISNULL(NbrFraction,'') 
    	,DirPrefix   = ISNULL(DirPrefix,'')   
    	,StreetName  = ISNULL(StreetName,'')  
    	,StreetType  = ISNULL(StreetType,'')  
    	,DirSuffix   = ISNULL(DirSuffix,'')   
    	,UnitDescr   = ISNULL(UnitDescr,'')   
    	,ZipCode     = ISNULL(ZipCode,'') 
        ,Area
        ,SubArea
    	,QuarterSection
    	,Section
    	,Township
    	,Range
    	,LevyCode
    	,Folio
    	,PT = PropType
    	,PlatLot
    	,PlatBlock
    	,PlatTypeCode
    	,PlatName
        ,Pin
        ,MapPin 
        ,RecId
   FROM	#PlatNbrSearch pn
  ORDER BY Major, Minor

RETURN(@@ERROR)
END




GO


