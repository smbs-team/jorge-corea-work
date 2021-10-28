﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CustomSearchesValuationEFLibrary.Model
{
    public partial class ptas_lowincomehousingunits
    {
        public Guid ptas_lowincomehousingunitsid { get; set; }
        public DateTimeOffset? createdon { get; set; }
        public decimal? exchangerate { get; set; }
        public int? importsequencenumber { get; set; }
        public DateTimeOffset? modifiedon { get; set; }
        public DateTimeOffset? overriddencreatedon { get; set; }
        public decimal? ptas_iac_capitalization_rate { get; set; }
        public decimal? ptas_iac_effectivegrossincome { get; set; }
        public decimal? ptas_iac_effectivegrossincome_base { get; set; }
        public decimal? ptas_iac_expenseamount { get; set; }
        public decimal? ptas_iac_expenseamount_base { get; set; }
        public decimal? ptas_iac_expensespct { get; set; }
        public decimal? ptas_iac_expensespct_base { get; set; }
        public decimal? ptas_iac_indicatedvalue { get; set; }
        public decimal? ptas_iac_indicatedvalue_base { get; set; }
        public decimal? ptas_iac_netoperatingincome { get; set; }
        public decimal? ptas_iac_netoperatingincome_base { get; set; }
        public decimal? ptas_iac_otherincome { get; set; }
        public decimal? ptas_iac_otherincome_base { get; set; }
        public decimal? ptas_iac_potentialgrossincome { get; set; }
        public decimal? ptas_iac_potentialgrossincome_base { get; set; }
        public decimal? ptas_iac_totalgrossincome { get; set; }
        public decimal? ptas_iac_totalgrossincome_base { get; set; }
        public decimal? ptas_iac_vacancyandcreditlossamount { get; set; }
        public decimal? ptas_iac_vacancyandcreditlossamount_base { get; set; }
        public decimal? ptas_iac_vacancyandcreditlosspercent { get; set; }
        public decimal? ptas_indicatedrent_1bedroom { get; set; }
        public decimal? ptas_indicatedrent_1bedroom_base { get; set; }
        public decimal? ptas_indicatedrent_2bedroom { get; set; }
        public decimal? ptas_indicatedrent_2bedroom_base { get; set; }
        public decimal? ptas_indicatedrent_2bedroom2bath { get; set; }
        public decimal? ptas_indicatedrent_2bedroom2bath_base { get; set; }
        public decimal? ptas_indicatedrent_3bedroom { get; set; }
        public decimal? ptas_indicatedrent_3bedroom_base { get; set; }
        public decimal? ptas_indicatedrent_3bedroom2bath { get; set; }
        public decimal? ptas_indicatedrent_3bedroom2bath_base { get; set; }
        public decimal? ptas_indicatedrent_3bedroom3bath { get; set; }
        public decimal? ptas_indicatedrent_3bedroom3bath_base { get; set; }
        public decimal? ptas_indicatedrent_4bedroom { get; set; }
        public decimal? ptas_indicatedrent_4bedroom_base { get; set; }
        public decimal? ptas_indicatedrent_5plusbedroom { get; set; }
        public decimal? ptas_indicatedrent_5plusbedroom_base { get; set; }
        public decimal? ptas_indicatedrent_sleepingroom { get; set; }
        public decimal? ptas_indicatedrent_sleepingroom_base { get; set; }
        public decimal? ptas_indicatedrent_studio { get; set; }
        public decimal? ptas_indicatedrent_studio_base { get; set; }
        public string ptas_name { get; set; }
        public int? ptas_nbrof100setasideunits { get; set; }
        public int? ptas_nbrof120setasideunits { get; set; }
        public int? ptas_nbrof20setasideunits { get; set; }
        public int? ptas_nbrof30setasideunits { get; set; }
        public int? ptas_nbrof35setasideunits { get; set; }
        public int? ptas_nbrof40setasideunits { get; set; }
        public int? ptas_nbrof45setasideunits { get; set; }
        public int? ptas_nbrof50setasideunits { get; set; }
        public int? ptas_nbrof60setasideunits { get; set; }
        public int? ptas_nbrof70setasideunits { get; set; }
        public int? ptas_nbrof80setasideunits { get; set; }
        public int? ptas_numberofmarketunits { get; set; }
        public int? ptas_numof1bedunits { get; set; }
        public int? ptas_numof2bedunits { get; set; }
        public int? ptas_numof3bedunits { get; set; }
        public int? ptas_numof4bedunits { get; set; }
        public int? ptas_numof5bedplusunits { get; set; }
        public int? ptas_numofstudiounits { get; set; }
        public decimal? ptas_percentofunitsat100 { get; set; }
        public decimal? ptas_percentofunitsat120 { get; set; }
        public decimal? ptas_percentofunitsat20 { get; set; }
        public decimal? ptas_percentofunitsat30 { get; set; }
        public decimal? ptas_percentofunitsat35 { get; set; }
        public decimal? ptas_percentofunitsat40 { get; set; }
        public decimal? ptas_percentofunitsat45 { get; set; }
        public decimal? ptas_percentofunitsat50 { get; set; }
        public decimal? ptas_percentofunitsat60 { get; set; }
        public decimal? ptas_percentofunitsat70 { get; set; }
        public decimal? ptas_percentofunitsat80 { get; set; }
        public decimal? ptas_percentofunitsatmarket { get; set; }
        public decimal? ptas_percentofunitsatrestrictedrent { get; set; }
        public decimal? ptas_restrictedrent_1bed { get; set; }
        public decimal? ptas_restrictedrent_1bed_base { get; set; }
        public decimal? ptas_restrictedrent_2bed { get; set; }
        public decimal? ptas_restrictedrent_2bed_base { get; set; }
        public decimal? ptas_restrictedrent_3bed { get; set; }
        public decimal? ptas_restrictedrent_3bed_base { get; set; }
        public decimal? ptas_restrictedrent_4bed { get; set; }
        public decimal? ptas_restrictedrent_4bed_base { get; set; }
        public decimal? ptas_restrictedrent_5bedplus { get; set; }
        public decimal? ptas_restrictedrent_5bedplus_base { get; set; }
        public decimal? ptas_restrictedrent_sleepingroom { get; set; }
        public decimal? ptas_restrictedrent_sleepingroom_base { get; set; }
        public decimal? ptas_restrictedrent_studio { get; set; }
        public decimal? ptas_restrictedrent_studio_base { get; set; }
        public decimal? ptas_rra_1bedroom1bathrent { get; set; }
        public decimal? ptas_rra_1bedroom1bathrent_base { get; set; }
        public decimal? ptas_rra_2bedroom1bathrent { get; set; }
        public decimal? ptas_rra_2bedroom1bathrent_base { get; set; }
        public decimal? ptas_rra_2bedroom2bathrent { get; set; }
        public decimal? ptas_rra_2bedroom2bathrent_base { get; set; }
        public decimal? ptas_rra_3bedroom1bathrent { get; set; }
        public decimal? ptas_rra_3bedroom1bathrent_base { get; set; }
        public decimal? ptas_rra_3bedroom2bathrent { get; set; }
        public decimal? ptas_rra_3bedroom2bathrent_base { get; set; }
        public decimal? ptas_rra_3bedroom3bathrent { get; set; }
        public decimal? ptas_rra_3bedroom3bathrent_base { get; set; }
        public decimal? ptas_rra_4bedroomrent { get; set; }
        public decimal? ptas_rra_4bedroomrent_base { get; set; }
        public decimal? ptas_rra_5bedroomplusrent { get; set; }
        public decimal? ptas_rra_5bedroomplusrent_base { get; set; }
        public decimal? ptas_rra_commercialrent { get; set; }
        public decimal? ptas_rra_commercialrent_base { get; set; }
        public int? ptas_rra_commercialrentsqft { get; set; }
        public int? ptas_rra_nbrof1bedroom1bathunits { get; set; }
        public int? ptas_rra_nbrof2bedroom1bathunits { get; set; }
        public int? ptas_rra_nbrof2bedroom2bathunits { get; set; }
        public int? ptas_rra_nbrof3bedroom1bathunits { get; set; }
        public int? ptas_rra_nbrof3bedroom2bathunits { get; set; }
        public int? ptas_rra_nbrof3bedroom3bathunits { get; set; }
        public int? ptas_rra_nbrof4bedroomunits { get; set; }
        public int? ptas_rra_nbrof5bedroomplusunits { get; set; }
        public int? ptas_rra_nbrofopen1bedroomunits { get; set; }
        public int? ptas_rra_nbrofopen2bedroomunits { get; set; }
        public int? ptas_rra_nbrofsleepingroomunits { get; set; }
        public int? ptas_rra_nbrofstudiounits { get; set; }
        public decimal? ptas_rra_open1bedroomrent { get; set; }
        public decimal? ptas_rra_open1bedroomrent_base { get; set; }
        public decimal? ptas_rra_open2bedroomrent { get; set; }
        public decimal? ptas_rra_open2bedroomrent_base { get; set; }
        public decimal? ptas_rra_otherincomeperunit { get; set; }
        public decimal? ptas_rra_otherincomeperunit_base { get; set; }
        public decimal? ptas_rra_sleepingroomrent { get; set; }
        public decimal? ptas_rra_sleepingroomrent_base { get; set; }
        public decimal? ptas_rra_studiorent { get; set; }
        public decimal? ptas_rra_studiorent_base { get; set; }
        public DateTimeOffset? ptas_ruc_assessmentdate { get; set; }
        public decimal? ptas_ruc_discountrate { get; set; }
        public decimal? ptas_ruc_discountrate_base { get; set; }
        public decimal? ptas_ruc_leaseholdreversionvalue { get; set; }
        public decimal? ptas_ruc_leaseholdreversionvalue_base { get; set; }
        public decimal? ptas_ruc_presentvalueofleasholdreversion { get; set; }
        public decimal? ptas_ruc_presentvalueofleasholdreversion_base { get; set; }
        public decimal? ptas_ruc_restrictedleasedfeevalue { get; set; }
        public decimal? ptas_ruc_restrictedleasedfeevalue_base { get; set; }
        public DateTimeOffset? ptas_ruc_terminationdateplustwoyears { get; set; }
        public decimal? ptas_ruc_totalrestrictedusevalue { get; set; }
        public decimal? ptas_ruc_totalrestrictedusevalue_base { get; set; }
        public decimal? ptas_ruc_unrestrictedmarketvalue { get; set; }
        public decimal? ptas_ruc_unrestrictedmarketvalue_base { get; set; }
        public int? ptas_totalnumberofrestrictedrentunits { get; set; }
        public int? ptas_totalnumberofunits { get; set; }
        public int? ptas_utilityprogram { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public long? versionnumber { get; set; }
        public Guid? _createdby_value { get; set; }
        public Guid? _createdonbehalfby_value { get; set; }
        public Guid? _modifiedby_value { get; set; }
        public Guid? _modifiedonbehalfby_value { get; set; }
        public Guid? _ownerid_value { get; set; }
        public Guid? _owningbusinessunit_value { get; set; }
        public Guid? _owningteam_value { get; set; }
        public Guid? _owninguser_value { get; set; }
        public Guid? _ptas_assessmentyearid_value { get; set; }
        public Guid? _ptas_lowincomehousingprogramid_value { get; set; }
        public Guid? _ptas_projectid_value { get; set; }
        public Guid? _transactioncurrencyid_value { get; set; }
    }
}
