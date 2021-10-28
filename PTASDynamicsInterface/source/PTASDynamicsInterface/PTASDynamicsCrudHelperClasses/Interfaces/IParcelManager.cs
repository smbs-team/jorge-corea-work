// <copyright file="IParcelManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PTASDynamicsCrudHelperClasses.JSONMappings;

    /// <summary>
    /// Generic Parcel manager.
    /// </summary>
    public interface IParcelManager
    {
        /// <summary>
        /// Get the top 10 list of parcels Info that contains parcelid.
        /// </summary>
        /// <param name="parcelId">Id to search.</param>
        /// <returns>Parcels look up data or null.</returns>
        Task<List<ParcelLookupResult>> GetParcelFromParcelId(string parcelId);

        /// <summary>
        /// Gets the top 10 list of parcels Info that contains it's Parcel account number.
        /// </summary>
        /// <param name="accountNumber">Parcel Account Number  to look for.</param>
        /// <returns>Parcels look up data or null.</returns>
        Task<List<ParcelLookupResult>> GetParcelFromAccountNumber(string accountNumber);

        /// <summary>
        ///  Gets the top list of parcels Info that contains the address parts.
        /// </summary>
        /// <param name="addressParts">array of address parts to look for.</param>
        /// <returns>Parcels look up data or null.</returns>
        Task<List<ParcelLookupResult>> GetParcelFromAddress(string[] addressParts);

        /// <summary>
        ///  Gets the top list of parcels Info that contains the address parts.
        /// </summary>
        /// <param name="searchParts">array of parts to look for.</param>
        /// <returns>Parcels look up data or null.</returns>
        Task<List<ParcelLookupResult>> GetParcelFromCommercialProject(string[] searchParts);

        /// <summary>
        /// Get the top 10 parcel info by SearchValue, if is a single word try by parcelId first if fail, try by accountNumber and if fail try by address, if is more than one word try by address.
        /// </summary>
        /// <param name="searchValue">Value to search for address, ParcelId or Account Number.</param>
        /// <returns>Parcels look up data or null.</returns>
        Task<List<ParcelLookupResult>> LookupParcel(string searchValue);

        /// <summary>
        /// Gets a list of matches for the address by geocoding of the addres using mapbox service.
        /// </summary>
        /// <param name="searchValue">serach criteria to lookup.</param>
        /// <returns>list of FormattedAddress with the resulting parced features of mapbox, or null if not found any. This method return the 3 closest matches.</returns>
        Task<List<FormattedAddress>> LookupAddress(string searchValue);
    }
}