IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SRCH_R_CondoAreaData')
	DROP PROCEDURE [cus].[SRCH_R_CondoAreaData]  
GO
CREATE PROCEDURE [cus].[SRCH_R_CondoAreaData]  
												 @Nbhd			[varchar](500),  
												 @Major			[char](6) = NULL,  
												 @SalesFrom		[smalldatetime] = NULL,  
												 @SalesTo		[smalldatetime] = NULL,  
												 @Population	[char](3) = 'N'  
WITH EXECUTE AS CALLER  
AS  
BEGIN
/*  
Author: Mario Umaña  
Date Created:  18/01/2021  
Description:    SP that pulls all records  filtered by parameter @Nbhd,@Mayor,@SalesFrom ,@SalesTo and  @Population from condos and sales.
  
Modifications:  
mm/dd/yyy - [CREATED BY] : [DETAILED DESCRIPTION OF THE CHANGE]  
*/    
SET NOCOUNT ON  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED  
  
  
  
DECLARE    
  @AssmtYr   smallint  
 ,@chNghbrhd smallint  
 ,@chStartDate varchar(6)  
 ,@leadZero  tinyint  
 ,@Error  int  
 ,@RowCount  int  
  
SELECT @AssmtYr = 2020--RPAssmtYr FROM AssmtYr  
  
 
SET @Nbhd=REPLACE(@Nbhd,' ','')--elim extra spaces  
SET @Nbhd=REPLACE(@Nbhd,';',',')--convert semicolons  
SET @Nbhd=@Nbhd+','  

 
--Parse neighborhood string  
CREATE Table #NbhdParam (NeighborhoodId INT PRIMARY KEY)  
IF LEN(@Nbhd)>0  
BEGIN  
	INSERT INTO #NbhdParam  
	SELECT DISTINCT value  
	FROM STRING_SPLIT (@Nbhd,',')  
	WHERE RTRIM(value) <> '';
END  


IF OBJECT_ID('Tempdb..#PropType') IS NOT NULL
DROP TABLE Tempdb..#PropType;

CREATE TABLE #PropType
(
	 ptas_propertyTypeId UNIQUEIDENTIFIER PRIMARY KEY
	,PropType NVARCHAR(1)
)

INSERT INTO #PropType
SELECT  ptas_propertyTypeId
       ,SUBSTRING(pt.ptas_name,1,1) 
FROM dynamics.ptas_propertytype AS pt

IF OBJECT_ID(N'tempdb..#SASSaleTable') IS NOT NULL
BEGIN
	DROP TABLE #SASSaleTable
END;

CREATE TABLE #SASSaleTable  (
	[CUnit_ParcelID] [UNIQUEIDENTIFIER],
	[SalesId]	[UNIQUEIDENTIFIER] NULL,
	[Major]		[NVARCHAR](6) NULL,
	[Minor]		[NVARCHAR](4) NULL,
	[RG]		TINYINT NULL, --Range
	[TW]		TINYINT NULL, --Township
	[SC]		TINYINT NULL, --Section
	[Q]			CHAR(2) NULL, --QuarterSection
	[Folio]		[NVARCHAR](7) NULL,
	[SpecArea]	SMALLINT NULL,
	[Nbhd]		SMALLINT NULL, --NeighborhoodId
	[SuperGroup]VARCHAR(50) DEFAULT '' ,
	[HouseNo]	[NVARCHAR](5) NULL, --StreetNbr
	[FractN]	[NVARCHAR](3) NULL, --NbrFraction
	[StrPfx]	[NVARCHAR](2) DEFAULT '' , --DirPrefix
	[StrName]	[NVARCHAR](250) NULL, --StreetName
	[StrType]	[NVARCHAR](4) NULL,--StreetType
	[Suffix]	[NVARCHAR](2) NULL, --DirSuffix
	[Descript]	[NVARCHAR](80) NULL, --ComplexName
	[LotSize]	[INT] NULL, --SqFtLot
	[PrevRollYr]	[SMALLINT] NOT NULL,
	[PrevApprLand]	[FLOAT] NULL,
	[PrevApprImp]	[FLOAT] NULL,
	[PrevApprTot]	[FLOAT] NULL,
	[NewRollYr]		[SMALLINT] NOT NULL,
	[NewSelLand]	[INT]    DEFAULT 0 ,
	[NewSelImps]	[INT]    DEFAULT 0,
	[NewSelTot]		[INT]    DEFAULT 0,
	[SelectMethod]	[VARCHAR](30) NULL,
	[SelectReason]	[NVARCHAR](30) NULL,
	[SelectDate]	[DATETIME] NULL,
	[NewConstrVal]	[INT] NULL,
	[SelectAppr]	[VARCHAR](50) NULL,
	[PriorYrAppr]	[VARCHAR](50) NULL,
	[TransferWarning] [VARCHAR](120) NOT NULL,
	[MFInterfaceFlag]	[SMALLINT] NULL,
	[PostingStatus]		[VARCHAR](45) NULL,
	[ExciseTaxNbr]		[INT] DEFAULT 0,
	[SalePrice]			[INT] DEFAULT 0,
	[SaleDate]			[SMALLDATETIME] NULL,
	[VerifiedAtMarket]	[NVARCHAR](1) NULL,
	[VerifiedBy]		[NVARCHAR](100) NULL,
	[VerifDate]			[SMALLDATETIME] NULL,
	[TaxpayerName]		[NVARCHAR](max) NULL,
	[NewTaxpayerName]	[NVARCHAR](200) DEFAULT '',
	[SellerName]		[NVARCHAR](200) DEFAULT '',
	[BuyerName]			[NVARCHAR](200) DEFAULT '',
	[PclsInSale]		[SMALLINT]  DEFAULT 0, --PropCnt
	[SaleWarnings]		[VARCHAR](150) DEFAULT '',
	[NewSaleWarning]	[VARCHAR](150) DEFAULT '',
	[BankSale]			[VARCHAR](1) DEFAULT '',
	[PropType]			[NVARCHAR](100) DEFAULT 0,
	[PrinUse]			[NVARCHAR](100) DEFAULT 0,
	[PropClass]			[NVARCHAR](100) DEFAULT 0,
	[Reason]			[NVARCHAR](100) DEFAULT 0,
	[Instr]				[NVARCHAR](100) DEFAULT 0,
	[Verif]				[NVARCHAR](100) DEFAULT 0,
	[WftAcc]			[TINYINT] NULL,
	[YrBlt]				[SMALLINT] NULL,
	[EffYr]				[SMALLINT] NULL,
	[PctComp]			[TINYINT] NULL, --PcntComplete
	[NbrStry]			[TINYINT] NULL, --NbrStories
	[Location]			[SMALLINT] NULL,
	[CurbAppl]			[SMALLINT] NULL,
	[Scty]				[VARCHAR](3) NULL, --SectySystem
	[Convert]			[VARCHAR](3) NULL, --AptConversion
	[NbrUnits]			[SMALLINT] NULL,
	[Lndry]				[SMALLINT] NULL, --Laundry
	[BldgQual]			[SMALLINT] NULL, --BldgQuality
	[BldgClss]			[SMALLINT] NULL, --ConstrClass
	[Elvtr]				[VARCHAR](3) NULL,
	[UnitID]			[NVARCHAR](10) NULL, --UnitNbr
	[PctLandVal]		[DECIMAL] (9,4) NULL, --PcntLandVal
	[PctOwnrShip]		[DECIMAL] (9,4) NULL, --PcntOwnership
	[LivArea]			[INT] NULL, --Footage
	[Bdrms]				[VARCHAR](1) NULL, --NbrBedrooms
	[ViewMtn]			[SMALLINT] NULL, --ViewMountain
	[ViewLkRv]			[SMALLINT] NULL, --ViewLakeRiver
	[ViewCity]			[SMALLINT] NULL, --ViewCityTerritorial
	[ViewSnd]			[SMALLINT] NULL, --ViewPugetSound
	[ViewWaSamm]		[SMALLINT] NULL, --ViewLakeWaSamm
	[PkgOpen]			[TINYINT] NULL,
	[PkgCp]				[TINYINT] NULL,
	[PkgBsmt]			[TINYINT] NULL,
	[PkgBsmtTand]		[TINYINT] NULL,
	[PkgGar]			[TINYINT] NULL,
	[PkgGarTand]		[TINYINT] NULL,
	[PkgOther]			[NVARCHAR](10) NULL,
	[UnitType]			[NVARCHAR](100) NULL,
	[UnitLoc]			[SMALLINT] NULL,
	[UnitQual]			[SMALLINT] NULL,
	[BthFull]			[SMALLINT] NULL,
	[BthHalf]			[SMALLINT] NULL,
	[Bth3Qtr]			[SMALLINT] NULL,
	[FP]				[NVARCHAR](3) NULL, --Fireplace
	[TopFlr]			[NVARCHAR](3) NULL, --TopFloor
	[NoRegress]			[TINYINT] NULL, --Regression
	[BldgNbr]			[NVARCHAR](5) NULL,
	[BldgCond]			[SMALLINT] NULL, --BldgCondition
	[UnitCondo]			[SMALLINT] NULL, --UnitCondition
	[OthRoom]			[NVARCHAR](10) DEFAULT '0', --OtherRoom
	[EndUnit]			[NVARCHAR](3) NULL,
	[FloorNbr]			[NVARCHAR](2) NULL,
	[NoteText]			[NVARCHAR](255) NULL, --NoteText
	[PermitNbr]			[NVARCHAR](30) NULL,
	[PermitDate]		[SMALLDATETIME] NULL,
	[PermitValue]		[INT]     DEFAULT 0 NULL,
	[PermitType]		[TINYINT] NULL,
	[PermTypeDescr]		[NVARCHAR](100) DEFAULT '',
	[EconomicUnitName]	[NVARCHAR](50) NULL,
	[EconomicUnit]		[NVARCHAR](25) NULL,
	[EconomicUnitParcelList] [NVARCHAR](max) DEFAULT '' ,
	[PIN]				[NVARCHAR](10) NULL,
	[MapPin]			[FLOAT] NULL
) ;
  
/*  
Need to restrict length of notes fields  
Warning: The table '#SASSaleTable' has been created but its maximum row size (9674) exceeds the maximum number of bytes  
per row (8060). INSERT or UPDATE of a row in this table will fail if the resulting row length exceeds 8060 bytes.  
*/  
  
  
  
/*  codes rather than descriptions?  
, PropType    tinyint    DEFAULT 0  --rpsc  
, PrinUse    tinyint    DEFAULT 0  --rpsc  
, PropClass    tinyint    DEFAULT 0  --rpsc  
, Reason    tinyint    DEFAULT 0  --rpsc  
, Instr     tinyint    DEFAULT 0  --rpsc  
, Verif     tinyint    DEFAULT 0  --rpsc  
*/  
  
--Insert main temp table #SASSaleTable:  
IF ISNULL(@Major,'')<>'' AND ISNULL(@Major,'')<>'0' AND ISNULL(@Major,'')<>'000000'  
BEGIN  
    --Need to consider revising this next part so that entire pop is not selected if only sold units are required  
INSERT INTO #SASSaleTable
SELECT cu._ptas_parcelid_value	AS 'CUnit_ParcelID'
,dps.ptas_salesid				AS 'SalesId'
,pd.ptas_major					AS Major
,cu.ptas_minornumber			AS Minor
,qs.ptas_range					AS RG--Range
,qs.ptas_township				AS TW--township
,qs.ptas_section				AS SC --Section
,[dynamics].[fn_GetValueFromStringMap]('ptas_qstr','ptas_quartersection',	 qs.ptas_quartersection		) AS Q --QUARTERSECTION
,pdcu.ptas_folio				AS Folio
,spec.ptas_areanumber			AS SpecArea
,COALESCE(psn.ptas_nbhdnumber,'')	AS Nbhd				--Mario comment: All values for those are currencly null in ptas_parcelDetail for ptas_applgroup = 'K'
,COALESCE(sg.ptas_name, 'Unassigned') AS SuperGroup --Mario comment: Seems the correct table to link but it is empty.
,COALESCE(cu.ptas_addr1_streetnumber,'')	AS HouseNo
,COALESCE(cu.ptas_addr1_streetnumberfraction,'')	AS FractN
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_addr1_directionprefix',	 cu.ptas_addr1_directionprefix		),'')  	AS StrPfx
,stn.ptas_name					AS StrName
,stt.ptas_name					AS StrType
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_addr1_directionsuffix',	 cu.ptas_addr1_directionsuffix		),'')  	AS Suffix
,cc.ptas_name					AS Descript
,pd.ptas_sqftlot				AS LotSize
,@AssmtYr						AS PrevRollYr 
,ah.ptas_landvalue				AS PrevApprLand		
,ah.ptas_impvalue				AS PrevApprImp		
,ah.ptas_totalvalue				AS PrevApprTot		
,@AssmtYr+1						AS NewRollYr	
,0								AS NewSelLand	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,0								AS NewSelImps	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,0								AS NewSelTot	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,''								AS SelectMethod --Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,''						 		AS SelectReason
,''								AS SelectDate	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,0								AS NewConstrVal --Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,''								AS SelectAppr
,ah.ptas_apprname				AS PriorYrAppr
,''								AS TranferingWarning -- Mario Comment: Check the value.
,0								AS MFInterfaceFlag
,''								AS PostingStatus --Mario Comment: The values seemed to be related to ptas_interfaceflag
,COALESCE(dps.ptas_excisetaxnumber,0)		AS ExciseTaxNbr
,dps.ptas_saleprice				AS SalePrice
,dps.ptas_saledate				AS SaleDate
,COALESCE(dps.ptas_atmarket	,'')			AS VerifiedAtMarket
,COALESCE(dps.ptas_verifiedby,'')			AS VerifiedBy
,COALESCE(dps.ptas_verifiedbydate,'')		AS VerifDate
,COALESCE(ta.ptas_taxpayerName, '')			AS TaxpayerName
,COALESCE(dps.ptas_taxlastname, '')			AS NewTaxpayerName
,COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') /*COALESCE(dps.ptas_sellerName, '')*/			AS SellerName 
,COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')/*COALESCE(dps.ptas_granteelastName, '')*/		AS BuyerName 
,COALESCE(dps.ptas_nbrparcels, '')			AS PclsInSale
,''								AS SaleWarnings	
,''								AS NewSaleWarning
,''								AS BankSale
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_affidavitpropertytype', dps.ptas_affidavitpropertytype),'0') 	AS PropType -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salesprincipleuse'	 , dps.ptas_salesprincipleuse	 ),'0')		AS PrintUse -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salepropertyclass'	 , dps.ptas_salepropertyclass	 ),'0') 	AS PropClass -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_reason'				 , dps.ptas_reason				 ),'0') 	AS Reason -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_instrument'			 , dps.ptas_instrument			 ),'0')		AS Instr -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_levelofverification'	 , dps.ptas_levelofverification	 ),'0')		AS Verif -- IF the value is '' it must be zero in the final resultset
,COALESCE(vw_LvWF.WF_Id,'0')		AS WftAcc
,ccyb.ptas_name						AS YrBlt
,ccye.ptas_name						AS EffYr
,cc.ptas_percentcomplete			AS PctComp
,cc.ptas_numberofstories			AS NbrStry
,cc.ptas_projectlocation			AS Location
,cc.ptas_projectappeal				AS CurbAppl
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condocomplex','ptas_securitysystem'	 , cc.ptas_securitysystem	 ),'0')					AS Scty 
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condocomplex','ptas_apartmentconversion', cc.ptas_apartmentconversion	 ),'0')				AS [Convert] 
,cc.ptas_numberofunits				AS NbrUnits
,cc.ptas_laundry					AS Lndry
,cc.ptas_buildingquality			AS BldgQual
,cc.ptas_constructionclass			AS BldgClss
,[dynamics].[fn_GetValueFromStringMap]('ptas_condocomplex','ptas_elevators', cc.ptas_elevators	 )									AS Elvtr
,REPLACE(cu.ptas_unitnumbertext,'#','')				AS UnitID
,cu.ptas_percentlandvaluedecimal					AS PctLandVal
,cu.ptas_percentownershipdecimal					AS PctOwnrShip
,cu.ptas_totalliving								AS LivArea
,cu.ptas_numberofbedrooms							AS Bdrms
,COALESCE(cu.ptas_viewmountain,'0')					AS ViewMtn
,COALESCE(cu.ptas_viewlakeorriver ,'0')				AS ViewLkRv
,COALESCE(cu.ptas_viewcityorterritorial,'0')		AS ViewCity
,COALESCE(cu.ptas_viewpugetsound,'0')				AS ViewSnd
,COALESCE(cu.ptas_viewlakewashingtonorlakesammamish,'0') AS ViewWaSamm
,COALESCE(cu.ptas_numberofopenparkingspaces,'0')	AS PkgOpen
,COALESCE(cu.ptas_numberofcarportspaces,'0')		AS PkgCp
,COALESCE(cu.ptas_numberofbasementparkingspaces,'0')AS PkgBsmt
,COALESCE(cu.ptas_numberofbasementtandemspaces,'0') AS PkgBsmtTand
,COALESCE(cu.ptas_numberofgarageparkingspaces,'0')	AS PkgGar
,COALESCE(cu.ptas_numberofgaragetandemspaces,'0')	AS PkgGarTand
,COALESCE(cu.ptas_otherparking,'0')					AS PkgOther
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_unittype', cu.ptas_unittype)	AS UnitType	
,cu.ptas_unitlocation								AS UnitLoc
,cu.ptas_unitquality								AS UnitQual
,cu.ptas_numberoffullbaths							AS BthFull
,cu.ptas_numberof12baths							AS BthHalf
,cu.ptas_numberof34baths							AS Bth3Qtr
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_fireplace', cu.ptas_fireplace)	AS FP	
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_topfloor', cu.ptas_topfloor)		AS TopFlr
,cu.ptas_regressionexclusionreason					AS NoRegress
,cu.ptas_buildingnumber								AS BldgNbr
,cc.ptas_buildingcondition							AS BldgCond
,cu.ptas_condounitcondition							AS UnitCondo
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_otherrooms', cu.ptas_otherrooms),'0')		AS OthRoom
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_endunit', cu.ptas_endunit)		AS EndUnit
,cu.ptas_floornumber								AS FloorNbr	
,COALESCE(cun.ptas_notetext,'')						AS NoteText
,COALESCE(per.ptas_name,'')							AS PermitNbr
,per.ptas_issueddate								AS PermitDate
,COALESCE(per.ptas_permitvalue,'0')					AS PermitValue
,COALESCE(per.ptas_permittype,'0')					AS PermitType
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_permit','ptas_permittype', per.ptas_permittype),'')		AS PermTypeDescr
,COALESCE(euni.ptas_name,'')						AS EconomicUnitName
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_parceleconomicunit','ptas_type', peuni.ptas_type),'')		AS EconomicUnit
,COALESCE(
    STUFF(
        (select ('; ' + ptas_name + '')
        from dynamics.ptas_parceleconomicunit od (nolock)
        where od._ptas_economicunitid_value = euni.ptas_economicunitid
        FOR XML PATH('')), 1, 2, ''
    ),'') EconomicUnitParcelList
,pdcu.ptas_Major+pdcu.ptas_Minor			AS PIN
,CASE
	WHEN pt.PropType = 'K' 
	THEN CONVERT(float, pdcu.ptas_major + '0000')
	ELSE CONVERT(float, pdcu.ptas_major + pdcu.ptas_minor)
END	AS MapPin
FROM		dynamics.ptas_condocomplex				AS cc
JOIN		dynamics.ptas_parceldetail				AS pd	ON pd.ptas_parceldetailid = cc._ptas_parcelid_value AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = '' --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed
JOIN		dynamics.ptas_condounit					AS cu	ON cu._ptas_complexid_value = cc.ptas_condocomplexid
JOIN		dynamics.ptas_parceldetail				AS pdcu	ON pdcu.ptas_parceldetailid = cu._ptas_parcelid_value AND COALESCE(pdcu.ptas_splitcode,0) = 0 AND (pdcu.statecode  = 0 AND pdcu.statuscode = 1) AND COALESCE(pdcu.ptas_snapshottype,'') = ''
LEFT JOIN	dynamics.ptas_specialtyneighborhood		AS psn  ON psn.ptas_specialtyneighborhoodid = pdcu._ptas_specialtynbhdid_value
JOIN		dynamics.ptas_taxaccount				AS ta	ON ta.ptas_taxaccountid = cu._ptas_taxaccountid_value
LEFT JOIN   dynamics.vw_LandValueWaterFront			AS vw_LvWF  ON vw_LvWF._ptas_landid_value = pd._ptas_landid_value  
JOIN		dynamics.ptas_year						AS ccyb	ON ccyb.ptas_yearid = cc._ptas_yearbuiltid_value
JOIN		dynamics.ptas_year						AS ccye	ON ccye.ptas_yearid = cc._ptas_effectiveyearid_value
LEFT JOIN   dynamics.ptas_economicunit				AS euni ON pdcu._ptas_economicunit_value = euni.ptas_economicunitid
LEFT JOIN   dynamics.ptas_parceleconomicunit		AS peuni ON peuni._ptas_economicunitid_value = euni.ptas_economicunitid and cu.ptas_condounitid = peuni._ptas_parcelId_Value --and peuni.ptas_type = 1
LEFT JOIN   dynamics.ptas_camanotes					AS cun	ON cun._ptas_parcelid_value = cc._ptas_parcelid_value AND cun._ptas_minorparcelid_value = cu.ptas_condounitid AND ptas_attachedentitydisplayname = 'Condo Unit'
JOIN		#PropType								AS pt	ON pt.ptas_propertytypeid = pdcu._ptas_propertytypeid_value
LEFT JOIN	ptas.ptas_appraisalhistory				AS ah	ON ah.ptas_parcelid		= cu._ptas_parcelid_value and ah.ptas_taxyearidname  = @AssmtYr   and ah.ptas_revalormaint = 'R'
LEFT JOIN	dynamics.ptas_streetname				AS stn	ON cu._ptas_addr1_streetnameid_value = stn.ptas_streetnameid
LEFT JOIN	dynamics.ptas_streettype				AS stt	ON cu._ptas_addr1_streettypeid_value = stt.ptas_streettypeid
LEFT JOIN	dynamics.ptas_specialtyarea				AS spec ON spec.ptas_specialtyareaid = pd._ptas_specialtyareaid_value  
JOIN		dynamics.ptas_qstr						AS qs	ON pdcu._ptas_qstrid_value = qs.ptas_qstrid
LEFT JOIN	dynamics.ptas_supergroup				AS sg	ON sg.ptas_supergroupid = pd._ptas_supergroupdid_value  
LEFT JOIN	dynamics.ptas_sales_parceldetail_parcelsinsale AS spdps          
INNER JOIN  dynamics.ptas_sales						AS dps  ON dps.ptas_salesid = spdps.ptas_salesid AND dps.ptas_saleprice > 0 AND dps.ptas_saledate BETWEEN @SalesFrom AND @SalesTo
															ON spdps.ptas_parceldetailid = pdcu.ptas_parceldetailid
LEFT JOIN  dynamics.ptas_geoarea					AS pga	ON pga.ptas_geoareaid = pd._ptas_geoareaid_value
LEFT JOIN  dynamics.ptas_geoneighborhood			AS pgn  ON pgn.ptas_geoneighborhoodid = pd._ptas_geonbhdid_value
OUTER APPLY(Select TOP 1 p.ptas_name,p.ptas_issueddate,p.ptas_permitvalue,p.ptas_permittype
			FROM dynamics.ptas_permit p
			Where p._ptas_parcelid_value = pdcu.ptas_parceldetailid
			ORDER BY ptas_issueddate DESC
			) per
WHERE pdcu.ptas_applgroup  = 'K' 
  AND spec.ptas_areanumber = 700 
  AND pd.ptas_major        =  @Major 
ORDER BY Major,Minor
  
    SELECT @Error      = @@ERROR  
       IF @Error      <> 0  
       BEGIN  
     RAISERROR (@Error, 11, 1)  
     RETURN (0)  
       END  
END --END IF ISNULL(@Major,'')<>''  AND ISNULL(@Major,'')<>'0' AND ISNULL(@Major,'')<>'000000'  
  
  
  
IF ISNULL(@Major,'')='' OR ISNULL(@Major,'')<>'0' OR ISNULL(@Major,'')<>'000000'  
BEGIN  
    --Need to consider revising this next part so that entire pop is not selected if only sold units are required  
INSERT INTO #SASSaleTable
SELECT cu._ptas_parcelid_value	AS 'CUnit_ParcelID'
,dps.ptas_salesid				AS 'SalesId'
,pd.ptas_major					AS Major
,cu.ptas_minornumber			AS Minor
,qs.ptas_range					AS RG--Range
,qs.ptas_township				AS TW--township
,qs.ptas_section				AS SC --Section
,[dynamics].[fn_GetValueFromStringMap]('ptas_qstr','ptas_quartersection',	 qs.ptas_quartersection		) AS Q --QUARTERSECTION
,pdcu.ptas_folio				AS Folio
,spec.ptas_areanumber			AS SpecArea
,COALESCE(psn.ptas_nbhdnumber,'')	AS Nbhd				--Mario comment: All values for those are currencly null in ptas_parcelDetail for ptas_applgroup = 'K'
,COALESCE(sg.ptas_name, 'Unassigned') AS SuperGroup --Mario comment: Seems the correct table to link but it is empty.
,COALESCE(cu.ptas_addr1_streetnumber,'')	AS HouseNo
,COALESCE(cu.ptas_addr1_streetnumberfraction,'')	AS FractN
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_addr1_directionprefix',	 cu.ptas_addr1_directionprefix		),'')  	AS StrPfx
,stn.ptas_name					AS StrName
,stt.ptas_name					AS StrType
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_addr1_directionsuffix',	 cu.ptas_addr1_directionsuffix		),'')  	AS Suffix
,cc.ptas_name					AS Descript
,pd.ptas_sqftlot				AS LotSize
,@AssmtYr						AS PrevRollYr 
,ah.ptas_landvalue				AS PrevApprLand		
,ah.ptas_impvalue				AS PrevApprImp		
,ah.ptas_totalvalue				AS PrevApprTot		
,@AssmtYr+1						AS NewRollYr	
,0								AS NewSelLand	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,0								AS NewSelImps	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,0								AS NewSelTot	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,''								AS SelectMethod --Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,''						 		AS SelectReason
,''								AS SelectDate	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,0								AS NewConstrVal --Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
,''								AS SelectAppr
,ah.ptas_apprname				AS PriorYrAppr
,''								AS TranferingWarning -- Mario Comment: Check the value.
,0								AS MFInterfaceFlag
,''								AS PostingStatus --Mario Comment: The values seemed to be related to ptas_interfaceflag
,COALESCE(dps.ptas_excisetaxnumber,0)		AS ExciseTaxNbr
,dps.ptas_saleprice				AS SalePrice
,dps.ptas_saledate				AS SaleDate
,COALESCE(dps.ptas_atmarket	,'')			AS VerifiedAtMarket
,COALESCE(dps.ptas_verifiedby,'')			AS VerifiedBy
,COALESCE(dps.ptas_verifiedbydate,'')		AS VerifDate
,COALESCE(ta.ptas_taxpayerName, '')			AS TaxpayerName
,COALESCE(dps.ptas_taxlastname, '')			AS NewTaxpayerName
,COALESCE(dps.ptas_grantorfirstname,'') +' '+COALESCE(dps.ptas_grantorlastname,'') /*COALESCE(dps.ptas_sellerName, '')*/			AS SellerName 
,COALESCE(dps.ptas_granteefirstname,'') +' '+COALESCE(dps.ptas_granteelastname,'')/*COALESCE(dps.ptas_granteelastName, '')*/		AS BuyerName 
,COALESCE(dps.ptas_nbrparcels, '')			AS PclsInSale
,''								AS SaleWarnings	
,''								AS NewSaleWarning
,''								AS BankSale
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_affidavitpropertytype', dps.ptas_affidavitpropertytype),'0') 	AS PropType -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salesprincipleuse'	 , dps.ptas_salesprincipleuse	 ),'0')		AS PrintUse -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_salepropertyclass'	 , dps.ptas_salepropertyclass	 ),'0') 	AS PropClass -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_reason'				 , dps.ptas_reason				 ),'0') 	AS Reason -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_instrument'			 , dps.ptas_instrument			 ),'0')		AS Instr -- IF the value is '' it must be zero in the final resultset
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_sales','ptas_levelofverification'	 , dps.ptas_levelofverification	 ),'0')		AS Verif -- IF the value is '' it must be zero in the final resultset
,COALESCE(vw_LvWF.WF_Id,'0')		AS WftAcc
,ccyb.ptas_name						AS YrBlt
,ccye.ptas_name						AS EffYr
,cc.ptas_percentcomplete			AS PctComp
,cc.ptas_numberofstories			AS NbrStry
,cc.ptas_projectlocation			AS Location
,cc.ptas_projectappeal				AS CurbAppl
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condocomplex','ptas_securitysystem'	 , cc.ptas_securitysystem	 ),'0')					AS Scty 
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condocomplex','ptas_apartmentconversion', cc.ptas_apartmentconversion	 ),'0')				AS [Convert] 
,cc.ptas_numberofunits				AS NbrUnits
,cc.ptas_laundry					AS Lndry
,cc.ptas_buildingquality			AS BldgQual
,cc.ptas_constructionclass			AS BldgClss
,[dynamics].[fn_GetValueFromStringMap]('ptas_condocomplex','ptas_elevators', cc.ptas_elevators	 )									AS Elvtr
,REPLACE(cu.ptas_unitnumbertext,'#','')				AS UnitID
,cu.ptas_percentlandvaluedecimal					AS PctLandVal
,cu.ptas_percentownershipdecimal					AS PctOwnrShip
,cu.ptas_totalliving								AS LivArea
,cu.ptas_numberofbedrooms							AS Bdrms
,COALESCE(cu.ptas_viewmountain,'0')					AS ViewMtn
,COALESCE(cu.ptas_viewlakeorriver ,'0')				AS ViewLkRv
,COALESCE(cu.ptas_viewcityorterritorial,'0')		AS ViewCity
,COALESCE(cu.ptas_viewpugetsound,'0')				AS ViewSnd
,COALESCE(cu.ptas_viewlakewashingtonorlakesammamish,'0') AS ViewWaSamm
,COALESCE(cu.ptas_numberofopenparkingspaces,'0')	AS PkgOpen
,COALESCE(cu.ptas_numberofcarportspaces,'0')		AS PkgCp
,COALESCE(cu.ptas_numberofbasementparkingspaces,'0')AS PkgBsmt
,COALESCE(cu.ptas_numberofbasementtandemspaces,'0') AS PkgBsmtTand
,COALESCE(cu.ptas_numberofgarageparkingspaces,'0')	AS PkgGar
,COALESCE(cu.ptas_numberofgaragetandemspaces,'0')	AS PkgGarTand
,COALESCE(cu.ptas_otherparking,'0')					AS PkgOther
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_unittype', cu.ptas_unittype)	AS UnitType	
,cu.ptas_unitlocation								AS UnitLoc
,cu.ptas_unitquality								AS UnitQual
,cu.ptas_numberoffullbaths							AS BthFull
,cu.ptas_numberof12baths							AS BthHalf
,cu.ptas_numberof34baths							AS Bth3Qtr
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_fireplace', cu.ptas_fireplace)	AS FP	
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_topfloor', cu.ptas_topfloor)		AS TopFlr
,cu.ptas_regressionexclusionreason					AS NoRegress
,cu.ptas_buildingnumber								AS BldgNbr
,cc.ptas_buildingcondition							AS BldgCond
,cu.ptas_condounitcondition							AS UnitCondo
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_otherrooms', cu.ptas_otherrooms),'0')		AS OthRoom
,[dynamics].[fn_GetValueFromStringMap]('ptas_condounit','ptas_endunit', cu.ptas_endunit)		AS EndUnit
,cu.ptas_floornumber								AS FloorNbr	
,COALESCE(cun.ptas_notetext,'')						AS NoteText
,COALESCE(per.ptas_name,'')							AS PermitNbr
,per.ptas_issueddate								AS PermitDate
,COALESCE(per.ptas_permitvalue,'0')					AS PermitValue
,COALESCE(per.ptas_permittype,'0')					AS PermitType
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_permit','ptas_permittype', per.ptas_permittype),'')		AS PermTypeDescr
,COALESCE(euni.ptas_name,'')						AS EconomicUnitName
,COALESCE([dynamics].[fn_GetValueFromStringMap]('ptas_parceleconomicunit','ptas_type', peuni.ptas_type),'')		AS EconomicUnit
,COALESCE(
    STUFF(
        (select ('; ' + ptas_name + '')
        from dynamics.ptas_parceleconomicunit od (nolock)
        where od._ptas_economicunitid_value = euni.ptas_economicunitid
        FOR XML PATH('')), 1, 2, ''
    ),'') EconomicUnitParcelList
,pdcu.ptas_Major+pdcu.ptas_Minor			AS PIN
,CASE
	WHEN pt.Proptype = 'K' 
	THEN CONVERT(float, pdcu.ptas_major + '0000')
	ELSE CONVERT(float, pdcu.ptas_major + pdcu.ptas_minor)
END	AS MapPin
FROM		dynamics.ptas_condocomplex				AS cc
JOIN		dynamics.ptas_parceldetail				AS pd	ON pd.ptas_parceldetailid = cc._ptas_parcelid_value AND COALESCE(pd.ptas_splitcode,0) = 0 AND (pd.statecode  = 0 AND pd.statuscode = 1) AND COALESCE(pd.ptas_snapshottype,'') = '' --This get the ACTIVE parcels. When "statecode  = 1 AND dpd.statuscode = 2" the parcel is Killed
JOIN		dynamics.ptas_condounit					AS cu	ON cu._ptas_complexid_value = cc.ptas_condocomplexid
JOIN		dynamics.ptas_parceldetail				AS pdcu	ON pdcu.ptas_parceldetailid = cu._ptas_parcelid_value AND COALESCE(pdcu.ptas_splitcode,0) = 0 AND (pdcu.statecode  = 0 AND pdcu.statuscode = 1) AND COALESCE(pdcu.ptas_snapshottype,'') = ''
LEFT JOIN	dynamics.ptas_specialtyneighborhood		AS psn  ON psn.ptas_specialtyneighborhoodid = pdcu._ptas_specialtynbhdid_value
JOIN		#NbhdParam								AS nh   ON nh.NeighborhoodId = psn.ptas_nbhdnumber
JOIN		dynamics.ptas_taxaccount				AS ta	ON ta.ptas_taxaccountid = cu._ptas_taxaccountid_value
LEFT JOIN   dynamics.vw_LandValueWaterFront			AS vw_LvWF  ON vw_LvWF._ptas_landid_value = pd._ptas_landid_value  
JOIN		dynamics.ptas_year						AS ccyb	ON ccyb.ptas_yearid = cc._ptas_yearbuiltid_value
JOIN		dynamics.ptas_year						AS ccye	ON ccye.ptas_yearid = cc._ptas_effectiveyearid_value
LEFT JOIN   dynamics.ptas_economicunit				AS euni ON pdcu._ptas_economicunit_value = euni.ptas_economicunitid
LEFT JOIN   dynamics.ptas_parceleconomicunit		AS peuni ON peuni._ptas_economicunitid_value = euni.ptas_economicunitid and cu.ptas_condounitid = peuni._ptas_parcelId_Value --and peuni.ptas_type = 1
LEFT JOIN   dynamics.ptas_camanotes					AS cun	ON cun._ptas_parcelid_value = cc._ptas_parcelid_value AND cun._ptas_minorparcelid_value = cu.ptas_condounitid AND ptas_attachedentitydisplayname = 'Condo Unit'
JOIN		#PropType								AS pt	ON pt.ptas_propertytypeid = pdcu._ptas_propertytypeid_value
LEFT JOIN	ptas.ptas_appraisalhistory				AS ah	ON ah.ptas_parcelid		= cu._ptas_parcelid_value and ah.ptas_taxyearidname  = @AssmtYr   and ah.ptas_revalormaint = 'R'
LEFT JOIN	dynamics.ptas_streetname				AS stn	ON cu._ptas_addr1_streetnameid_value = stn.ptas_streetnameid
LEFT JOIN	dynamics.ptas_streettype				AS stt	ON cu._ptas_addr1_streettypeid_value = stt.ptas_streettypeid
LEFT JOIN	dynamics.ptas_specialtyarea				AS spec ON spec.ptas_specialtyareaid = pd._ptas_specialtyareaid_value  
JOIN		dynamics.ptas_qstr						AS qs	ON pd._ptas_qstrid_value = qs.ptas_qstrid
LEFT JOIN	dynamics.ptas_supergroup				AS sg	ON sg.ptas_supergroupid = pd._ptas_supergroupdid_value  
LEFT JOIN	dynamics.ptas_sales_parceldetail_parcelsinsale AS spdps          
INNER JOIN  dynamics.ptas_sales						AS dps  ON dps.ptas_salesid = spdps.ptas_salesid AND dps.ptas_saleprice > 0 AND dps.ptas_saledate BETWEEN @SalesFrom AND @SalesTo
															ON spdps.ptas_parceldetailid = pdcu.ptas_parceldetailid
LEFT JOIN  dynamics.ptas_geoarea					AS pga	ON pga.ptas_geoareaid = pd._ptas_geoareaid_value
LEFT JOIN  dynamics.ptas_geoneighborhood			AS pgn  ON pgn.ptas_geoneighborhoodid = pd._ptas_geonbhdid_value
OUTER APPLY(Select TOP 1 p.ptas_name,p.ptas_issueddate,p.ptas_permitvalue,p.ptas_permittype
			FROM dynamics.ptas_permit p
			Where p._ptas_parcelid_value = pdcu.ptas_parceldetailid
			ORDER BY ptas_issueddate DESC
			) per
WHERE pdcu.ptas_applgroup  = 'K' 
  AND spec.ptas_areanumber = 700 

ORDER BY Major,Minor

    SELECT @Error      = @@ERROR  
       IF @Error      <> 0  
       BEGIN  
     RAISERROR (@Error, 11, 1)  
     RETURN (0)  
       END  
END --END IF ISNULL(@Major,'')='' OR ISNULL(@Major,'')<>'0' OR ISNULL(@Major,'')<>'000000  
  
CREATE NONCLUSTERED INDEX [IX_SASSaleTable_ParcelID] ON #SASSaleTable ([CUnit_ParcelID]);
CREATE NONCLUSTERED INDEX [IX_SASSaleTable_SalesId]  ON #SASSaleTable ([SalesId])        
      
--The only other item that might be of help is the appraiser ID of who selected last year's numbers.  Thanks, Craig Johnson  

--New Select Values  
UPDATE #SASSaleTable    
SET  NewSelLand		= COALESCE(ahn.ptas_landvalue,0) 	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,NewSelImps		= COALESCE(ahn.ptas_impvalue,0)	 	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,NewSelTot		= COALESCE(ahn.ptas_totalvalue,0)	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,SelectMethod	= ahn.ptas_method					--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,SelectReason	= [dynamics].[fn_GetValueFromStringMap]('ptas_appraisalhistory','ptas_valuationreason',	 ahn.ptas_valuationreason) 		 
	,SelectDate		= ahn.createdon						--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,NewConstrVal	= ahn.ptas_newconstruction		  --Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,SelectAppr		= ahn.ptas_apprname				 
	,MFInterfaceFlag= ahn.ptas_interfaceflag			
	,PostingStatus	= [dynamics].[fn_GetValueFromStringMap]('ptas_appraisalhistory','ptas_interfaceflag',	 ahn.ptas_interfaceflag		)  --Mario Comment: The values seemed to be related to ptas_interfaceflag
FROM  #SASSaleTable sst 
INNER JOIN ptas.ptas_appraisalhistory				AS ahn	ON ahn.ptas_parcelid	= sst.CUnit_ParcelID 
WHERE ahn.ptas_taxyearidname = @AssmtYr+1 
  AND ahn.ptas_revalormaint = 'R' 
  AND ahn.ptas_impvalue > 0 

--M over R  
UPDATE #SASSaleTable    
SET  NewSelLand		= COALESCE(ahn.ptas_landvalue,0) 	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,NewSelImps		= COALESCE(ahn.ptas_impvalue,0)	 	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,NewSelTot		= COALESCE(ahn.ptas_totalvalue,0)	--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,SelectMethod	= ahn.ptas_method					--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,SelectReason	= [dynamics].[fn_GetValueFromStringMap]('ptas_appraisalhistory','ptas_valuationreason',	 ahn.ptas_valuationreason) 		 
	,SelectDate		= ahn.createdon						--Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,NewConstrVal	= ahn.ptas_newconstruction		  --Mario Comment: The value of these columns needs to be double check, former value took from CondoApplHist vs ptas.ptas_appraisalhistory now.
	,SelectAppr		= ahn.ptas_apprname				 
	,MFInterfaceFlag= ahn.ptas_interfaceflag			
	,PostingStatus	= [dynamics].[fn_GetValueFromStringMap]('ptas_appraisalhistory','ptas_interfaceflag',	 ahn.ptas_interfaceflag		)  --Mario Comment: The values seemed to be related to ptas_interfaceflag
FROM  #SASSaleTable sst 
INNER JOIN ptas.ptas_appraisalhistory				AS ahn	ON ahn.ptas_parcelid	= sst.CUnit_ParcelID 
WHERE ahn.ptas_taxyearidname = @AssmtYr+1 
  AND ahn.ptas_revalormaint = 'M'  

  
/*
************************************* THE VALUE dOES NOT EXITS IN TABLE stringmap ********************************
UPDATE #SASSaleTable    
SET PostingStatus = (SELECT LUItemShortDesc   
                   FROM LUItem2   
                  WHERE LUTypeId = 109   
                   AND LUItemId = MFInterfaceFlag)  
*/  

--Create warnings where the Parcel recently was transferred to the Res Condo crew.    
--Want to know if the transferred parcel has already been valued when developing annual update values.  

UPDATE sst
SET TransferWarning = ISNULL( 'Transferred from PropType = '+ CONVERT(CHAR(1),ch.displayValueNew) + ' ' + CONVERT(CHAR(12),ch.EventDate),'')
From [ptas].[ptas_changehistory] ch
JOIN #SASSaleTable sst ON sst.CUnit_ParcelID = ch.parcelGuid
Where EventDate > + CONVERT(CHAR(4),2020-3) 
AND ch.attribDispName = 'PropType' 
AND ch.displayValueNew <> 'K'
AND ch.entityDispName = 'ptas_parceldetail'

UPDATE sst
SET TransferWarning = ISNULL( 'Transferred from ApplGroup = '+ CONVERT(CHAR(1),ch.displayValueNew) + ' ' + CONVERT(CHAR(12),ch.EventDate),'')
From [ptas].[ptas_changehistory] ch
JOIN #SASSaleTable sst ON sst.CUnit_ParcelID = ch.parcelGuid
Where EventDate > + CONVERT(CHAR(4),2020-3) 
AND ch.attribDispName = 'ApplGroup' 
AND ch.displayValueNew <> 'K'
AND ch.entityDispName = 'ptas_parceldetail'
  
 
  
--Get Sale Warnings and BankSale 
UPDATE #SASSaleTable
SET SaleWarnings = COALESCE( [dynamics].[fn_ConcatSalesNotes](SalesId,3,'7',150),'')
  , BankSale	 = CASE WHEN [dynamics].[fn_ConcatSalesNotes](SalesId,3,'60,61',150) IS NOT NULL THEN 'Y' ELSE '' END
Where SalesId IS NOT NULL              
  
  
--More Possible Bank Sales  - added 9/10/2013 Don G  
UPDATE #SASSaleTable  
SET BankSale = 'Y'  
WHERE BankSale = ''  
  AND (SellerName LIKE '%BANK%'  
   OR SellerName LIKE '%MORTGAGE%'  
   OR SellerName LIKE '%FUNDING%'  
   OR SellerName LIKE '%FNMA%'  
   OR SellerName LIKE '%MRTG%'  
   OR SellerName LIKE '%MTG%'  
   OR SellerName LIKE '%FANNIE MAE%'  
   OR SellerName LIKE '%HOUSING AND URBAN DEVELOPMENT%'  
   OR SellerName LIKE '%HOUSING & URBAN DEVELOPMENT%'  
   OR SellerName = '%HUD%'  
   OR SellerName LIKE '%CREDIT UNION%'  
   OR BuyerName LIKE '%BANK%'  
   OR BuyerName LIKE '%MORTGAGE%'  
   OR BuyerName LIKE '%FUNDING%'  
   OR BuyerName LIKE '%FNMA%'  
   OR BuyerName LIKE '%MRTG%'  
   OR BuyerName LIKE '%MTG%'  
   OR BuyerName LIKE '%FANNIE MAE%'  
   OR BuyerName LIKE '%HOUSING AND URBAN DEVELOPMENT%'  
   OR BuyerName LIKE '%HOUSING & URBAN DEVELOPMENT%'  
   OR BuyerName = '%HUD%'  
   OR BuyerName LIKE '%CREDIT UNION%'  
   )  
  AND SellerName NOT LIKE '% Banks%'   
  AND SellerName NOT LIKE '%Banks %'   
  AND SellerName NOT LIKE '% Banks %'  
  AND BuyerName NOT LIKE '% Banks%'   
  AND BuyerName NOT LIKE '%Banks %'   
  AND BuyerName NOT LIKE '% Banks %'                                                         
  

  
/*  
  
The transfer warning seems to work from what I have check, but I think the ParcelNoteID and ParcelNote columns need to come back out of the query (if that's possible). The queries take way longer now than they did just a week or two ago. I think we took 
those columns out a couple of years ago because of this very problem. Some of the queries are so large that there is no way to get all the data before it times out. Also, most of the parcel notes are irrelevant and very few of the transferred parcels actu
ally get a note. So, can you take those columns out again? I think with the transfer warning column, I might be able to weed out what I need. The only other item that might be of help is the appraiser ID of who selected last years numbers.   
  
Thanks, Craig Johnson  4/24/06  
  
  
  
Notes:  
RealProp Notes: are for the entire complex (see CondoUnit.NoteText above for condo unit notes).  
Sale Notes: There can be ShortNote(s) and LongNote(s), and multiple notes (NoteInstance) for a single   
SaleId/ExciseTaxNbr.     
  
  
UPDATE #SASSaleTable  
  SET ParcelNoteId = rp.NoteId  
 FROM #SASSaleTable sst INNER JOIN RealProp rp (NOLOCK) ON (sst.Major = rp.Major AND rp.Minor = '0000')  
  
  
DECLARE @NICntr tinyint  
  
SET @NICntr = 0  
  
BEGIN  
  
  
  INSERT #ParcelNoteInstance (ParcelNoteId, NoteInstance, AssmtEntityId, UpdateDate, ShortNote, LongNote, RowUniqueId)  
  SELECT ni.NoteId, ni.NoteInstance, ni.AssmtEntityId, ni.UpdateDate, ni.ShortNote, ni.LongNote, ni.RowUniqueId  
    FROM NoteInstance ni (NOLOCK) INNER JOIN #SASSaleTable sst (NOLOCK) ON (ni.NoteId = sst.ParcelNoteId)  
   WHERE sst.ParcelNoteId > 0  
  
  WHILE @NICntr < 3  
  BEGIN  
    INSERT #ParcelNoteId (ParcelNoteId, MaxNoteInstance)  
    SELECT ParcelNoteId, Max(NoteInstance)  
      FROM #ParcelNoteInstance (NOLOCK)  
  GROUP BY ParcelNoteId  
  
    IF @NICntr = 2  
      UPDATE #SASSaleTable  
         SET ParcelNotes = ParcelNotes + ' AND OTHER NOTES;'  
        FROM #SASSaleTable sst  INNER JOIN #ParcelNoteId nid (NOLOCK) ON (sst.ParcelNoteId = nid.ParcelNoteId)  
                                INNER JOIN #ParcelNoteInstance ni (NOLOCK) ON (sst.ParcelNoteId = ni.ParcelNoteId   
                                  AND nid.MaxNoteInstance = ni.NoteInstance)  
    ELSE  
      UPDATE #SASSaleTable  
         SET ParcelNotes = ParcelNotes + CONVERT(Varchar(600),(CASE WHEN ShortNote IS NULL   
                           THEN SUBSTRING(LongNote,1,600) ELSE ShortNote END)) + '(' + convert(char(11),ni.UpdateDate)  
                           + ' ' + ni.AssmtEntityId + ');'  
        FROM #SASSaleTable sst  INNER JOIN #ParcelNoteId nid (NOLOCK) ON (sst.ParcelNoteId = nid.ParcelNoteId)  
                                INNER JOIN #ParcelNoteInstance ni (NOLOCK) ON (sst.ParcelNoteId = ni.ParcelNoteId   
                                  AND nid.MaxNoteInstance = ni.NoteInstance)  
   
    DELETE #ParcelNoteInstance  
      FROM #ParcelNoteInstance ni INNER JOIN #ParcelNoteId nid   
        ON (ni.ParcelNoteId = nid.ParcelNoteId AND ni.NoteInstance = nid.MaxNoteInstance)  
  
    TRUNCATE TABLE #ParcelNoteId  
    SET @NICntr = @NICntr + 1  
  END  
END  
  
UPDATE #SASSaleTable   --added due to QMAS error: "Unknown Error 1004" etc. (Excel doesn't like leading = sign in cell)  
SET ParcelNotes = STUFF(ParcelNotes, 1, 1, 'Equals ') where left(ParcelNotes,1)='='  
*/  
  
/*
-- Mario Comment: The value has been included in the result but the current table is empty.

UPDATE #SASSaleTable  
SET SuperGroup = csg.SuperGroupName  
FROM #SASSaleTable sst INNER JOIN CommercialSuperGroupSub csgs ON sst.NeighborhoodId = csgs.SubArea   
                       INNER JOIN CommercialSuperGroup csg ON csgs.SuperGroupGuid = csg.SuperGroupGuid  
WHERE csg.Descr = 'ResCondo'                                            
*/                                                                                         
  --SELECT 'F',SalePrice,*  FROM #SASSaleTable   
--Return final result set  
  
IF   @Population  = 'Y'  
BEGIN  
SELECT
  sst.Major			as 'Major'  
 ,sst.Minor			as 'Minor'  
 ,sst.rg			as 'RG'  
 ,sst.tw			as 'TW'  
 ,sst.SC			as 'SC'  
 ,sst.Q				as 'Q'  
 ,sst.Folio			as 'Folio'  
 ,sst.SpecArea		as 'SpecArea'  
 ,sst.Nbhd			as 'Nbhd'  
 ,sst.SuperGroup	as 'SuperGroup'  
 ,sst.HouseNo		as 'HouseNo'  
 ,sst.Fractn		as 'Fractn'  
 ,sst.StrPfx		as 'StrPfx'  
 ,sst.StrName		as 'StrName'  
 ,sst.StrType		as 'StrType'  
 ,sst.Suffix		as 'Suffix'  
 ,sst.Descript		as 'Descript' --ComplexName  
 ,sst.LotSize		as 'LotSize'  --SqFtLot
 ,PrevRollYr =  @AssmtYr  
 ,sst.PrevApprLand	AS 'PrevApprLand'
 ,sst.PrevApprImp   AS 'PrevApprImp'  
 ,sst.PrevApprTot	as 'PrevApprTot'
 ,NewRollYr =  @AssmtYr+1  
 ,sst.NewSelLand    
 ,sst.NewSelImps    
 ,sst.NewSelTot    
 ,sst.SelectMethod   
 ,sst.SelectReason   
 ,sst.SelectDate     
 ,sst.NewConstrVal   
 ,sst.SelectAppr     
 ,sst.PriorYrAppr  
 ,sst.TransferWarning  
 ,sst.MFInterfaceFlag   
 ,sst.PostingStatus  
 ,ISNULL(sst.ExciseTaxNbr,'')	as 'ExciseTaxNbr'  
 ,sst.SalePrice					AS 'SalePrice'   
 ,sst.SaleDate					AS 'SaleDate'   
 ,sst.VerifiedAtMarket			as 'VerifiedAtMarket'  
 ,sst.VerifiedBy				as 'VerifiedBy'  
 ,sst.VerifDate					as 'VerifDate'  
 ,sst.TaxpayerName   
 ,sst.NewTaxpayerName   
 ,sst.SellerName   
 ,sst.BuyerName   
 ,sst.PclsInSale  AS 'PclsInSale'  
  --  ,sst.NoteId   Still need to get sales notes for given id  
 ,sst.SaleWarnings AS 'SaleWarnings'  
 ,'NewSaleWarning' = ''  
 ,sst.BankSale   -- added 9/10/2013 Don G  
 ,sst.PropType       
 ,sst.PrinUse       
 ,sst.PropClass      
 ,sst.Reason      
 ,sst.Instr        
 ,ISNULL(sst.Verif,'')	AS 'Verif' 
     
 ,sst.WftAcc			as 'WftAcc'  
 ,sst.YrBlt				as 'YrBlt'  
 ,sst.EffYr    
 ,sst.PctComp			as 'PctComp'  
 ,sst.NbrStry			as 'NbrStry'  
 ,sst.Location			as 'Location'  
 ,sst.CurbAppl			as 'CurbAppl'  
 ,sst.Scty				as 'Scty'  
 ,sst.[Convert]			as 'Convert'  
 ,sst.NbrUnits			as 'NbrUnits'  
 ,sst.Lndry				as 'Lndry'  
 ,sst.BldgQual			as 'BldgQual'  
 ,sst.BldgClss			as 'BldgClss'  
 ,sst.Elvtr				as 'Elvtr'  
 ,sst.UnitID			as 'UnitID'  
 ,sst.PctLandVal		as 'PctLandVal'  
 ,sst.PctOwnrShip		as 'PctOwnrShip'  
 ,sst.LivArea			as 'LivArea'  
 ,sst.Bdrms				as 'Bdrms'  
 ,sst.ViewMtn			as 'ViewMtn'  
 ,sst.ViewLkRv			as 'ViewLkRv'  
 ,sst.ViewCity			as 'ViewCity'  
 ,sst.ViewSnd			as 'ViewSnd'  
 ,sst.ViewWaSamm		as 'ViewWaSamm'  
 ,sst.PkgOpen			as 'PkgOpen'  
 ,sst.PkgCp				as 'PkgCp'  
 ,sst.PkgBsmt			as 'PkgBsmt'  
 ,sst.PkgBsmtTand		as 'PkgBsmtTand'  
 ,sst.PkgGar			as 'PkgGar'  
 ,sst.PkgGarTand		as 'PkgGarTand'  
 ,sst.PkgOther			as 'PkgOther'  
 ,sst.UnitType			as 'UnitType'  
 ,sst.UnitLoc			as 'UnitLoc'  
 ,sst.UnitQual			as 'UnitQual'  
 ,sst.BthFull			as 'BthFull'  
 ,sst.BthHalf			as 'BthHalf'  
 ,sst.Bth3Qtr			as 'Bth3Qtr'  
 ,sst.Fp				as 'Fp'  
 ,sst.TopFlr			as 'TopFlr'  
 ,sst.NoRegress			as 'NoRegress'  
 ,sst.BldgNbr			as 'BldgNbr'  
 ,sst.BldgCond			as 'BldgCond'  
 ,sst.UnitCondo			as 'UnitCond'  
 ,sst.OthRoom			as 'OthRoom'  
 ,sst.EndUnit			as 'EndUnit'  
 ,sst.FloorNbr			as 'FloorNbr'  
 ,sst.NoteText			as 'CondoUnitNote'  
-- ,sst.ParcelNoteId  
-- ,sst.ParcelNotes   
  
,sst.PermitNbr  
,ISNULL(CONVERT (VARCHAR(8), sst.PermitDate, 1 ),'') AS 'PermitDate'    --Style = 1 returns mm/dd/yy  
,sst.PermitValue  
,sst.PermitType  
,sst.PermTypeDescr  
,sst.EconomicUnitName  
,sst.EconomicUnit  
,sst.EconomicUnitParcelList  
,sst.Pin  
,sst.MapPin  
,ROW_NUMBER() OVER (ORDER BY (SELECT 1))	AS RecId 
FROM  
 #SASSaleTable sst  
ORDER BY sst.Major,sst.Minor,sst.SaleDate desc,sst.ExciseTaxNbr desc   --sst.RecId  
END  
  
ELSE  
BEGIN  
SELECT
  sst.Major			as 'Major'  
 ,sst.Minor			as 'Minor'  
 ,sst.rg			as 'RG'  
 ,sst.tw			as 'TW'  
 ,sst.SC			as 'SC'  
 ,sst.Q				as 'Q'  
 ,sst.Folio			as 'Folio'  
 ,sst.SpecArea		as 'SpecArea'  
 ,sst.Nbhd			as 'Nbhd'  
 ,sst.SuperGroup	as 'SuperGroup'  
 ,sst.HouseNo		as 'HouseNo'  
 ,sst.Fractn		as 'Fractn'  
 ,sst.StrPfx		as 'StrPfx'  
 ,sst.StrName		as 'StrName'  
 ,sst.StrType		as 'StrType'  
 ,sst.Suffix		as 'Suffix'  
 ,sst.Descript		as 'Descript' --ComplexName  
 ,sst.LotSize		as 'LotSize'  --SqFtLot
 ,PrevRollYr =  @AssmtYr  
 ,sst.PrevApprLand	AS 'PrevApprLand'
 ,sst.PrevApprImp   AS 'PrevApprImp'  
 ,sst.PrevApprTot	as 'PrevApprTot'
 ,NewRollYr =  @AssmtYr+1  
 ,sst.NewSelLand    
 ,sst.NewSelImps    
 ,sst.NewSelTot    
 ,sst.SelectMethod   
 ,sst.SelectReason   
 ,sst.SelectDate     
 ,sst.NewConstrVal   
 ,sst.SelectAppr     
 ,sst.PriorYrAppr  
 ,sst.TransferWarning  
 ,sst.MFInterfaceFlag   
 ,sst.PostingStatus  
 ,ISNULL(sst.ExciseTaxNbr,'')	as 'ExciseTaxNbr'  
 ,sst.SalePrice					AS 'SalePrice'   
 ,sst.SaleDate					AS 'SaleDate'   
 ,sst.VerifiedAtMarket			as 'VerifiedAtMarket'  
 ,sst.VerifiedBy				as 'VerifiedBy'  
 ,sst.VerifDate					as 'VerifDate'  
 ,sst.TaxpayerName   
 ,sst.NewTaxpayerName   
 ,sst.SellerName   
 ,sst.BuyerName   
 ,sst.PclsInSale  AS 'PclsInSale'  
  --  ,sst.NoteId   Still need to get sales notes for given id  
 ,sst.SaleWarnings AS 'SaleWarnings'  
 ,'NewSaleWarning' = ''  
 ,sst.BankSale   -- added 9/10/2013 Don G  
 ,sst.PropType       
 ,sst.PrinUse       
 ,sst.PropClass      
 ,sst.Reason      
 ,sst.Instr        
 ,ISNULL(sst.Verif,'')	AS 'Verif' 
     
 ,sst.WftAcc			as 'WftAcc'  
 ,sst.YrBlt				as 'YrBlt'  
 ,sst.EffYr    
 ,sst.PctComp			as 'PctComp'  
 ,sst.NbrStry			as 'NbrStry'  
 ,sst.Location			as 'Location'  
 ,sst.CurbAppl			as 'CurbAppl'  
 ,sst.Scty				as 'Scty'  
 ,sst.[Convert]			as 'Convert'  
 ,sst.NbrUnits			as 'NbrUnits'  
 ,sst.Lndry				as 'Lndry'  
 ,sst.BldgQual			as 'BldgQual'  
 ,sst.BldgClss			as 'BldgClss'  
 ,sst.Elvtr				as 'Elvtr'  
 ,sst.UnitID			as 'UnitID'  
 ,sst.PctLandVal		as 'PctLandVal'  
 ,sst.PctOwnrShip		as 'PctOwnrShip'  
 ,sst.LivArea			as 'LivArea'  
 ,sst.Bdrms				as 'Bdrms'  
 ,sst.ViewMtn			as 'ViewMtn'  
 ,sst.ViewLkRv			as 'ViewLkRv'  
 ,sst.ViewCity			as 'ViewCity'  
 ,sst.ViewSnd			as 'ViewSnd'  
 ,sst.ViewWaSamm		as 'ViewWaSamm'  
 ,sst.PkgOpen			as 'PkgOpen'  
 ,sst.PkgCp				as 'PkgCp'  
 ,sst.PkgBsmt			as 'PkgBsmt'  
 ,sst.PkgBsmtTand		as 'PkgBsmtTand'  
 ,sst.PkgGar			as 'PkgGar'  
 ,sst.PkgGarTand		as 'PkgGarTand'  
 ,sst.PkgOther			as 'PkgOther'  
 ,sst.UnitType			as 'UnitType'  
 ,sst.UnitLoc			as 'UnitLoc'  
 ,sst.UnitQual			as 'UnitQual'  
 ,sst.BthFull			as 'BthFull'  
 ,sst.BthHalf			as 'BthHalf'  
 ,sst.Bth3Qtr			as 'Bth3Qtr'  
 ,sst.Fp				as 'Fp'  
 ,sst.TopFlr			as 'TopFlr'  
 ,sst.NoRegress			as 'NoRegress'  
 ,sst.BldgNbr			as 'BldgNbr'  
 ,sst.BldgCond			as 'BldgCond'  
 ,sst.UnitCondo			as 'UnitCond'  
 ,sst.OthRoom			as 'OthRoom'  
 ,sst.EndUnit			as 'EndUnit'  
 ,sst.FloorNbr			as 'FloorNbr'  
 ,sst.NoteText			as 'CondoUnitNote'  
-- ,sst.ParcelNoteId  
-- ,sst.ParcelNotes   
  
,sst.PermitNbr  
,ISNULL(CONVERT (VARCHAR(8), sst.PermitDate, 1 ),'') AS 'PermitDate'    --Style = 1 returns mm/dd/yy  
,sst.PermitValue  
,sst.PermitType  
,sst.PermTypeDescr  
,sst.EconomicUnitName  
,sst.EconomicUnit  
,sst.EconomicUnitParcelList  
,sst.Pin  
,sst.MapPin  
,ROW_NUMBER() OVER (ORDER BY (SELECT 1))	AS RecId
FROM  
 #SASSaleTable sst  
WHERE  sst.SalePrice > 0  
ORDER BY sst.Major,sst.Minor,sst.SaleDate desc,sst.ExciseTaxNbr desc   --sst.RecId  
END  
  
  
SELECT @Error   = @@ERROR  
   IF @Error   <> 0  
   BEGIN  
 RAISERROR (@Error, 11, 1)  
 RETURN (0)  
   END  
  
RETURN (0)  
END  
GO
--SET STATISTICS TIME ON
--SET STATISTICS IO ON
--GO
--EXEC   [cus].[SRCH_R_CondoAreaData]  '010',NULL,NULL,NULL,'Y'  
--GO
--SET STATISTICS TIME OFF
--SET STATISTICS IO OFF
--GO  