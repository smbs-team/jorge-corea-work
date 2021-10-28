// <copyright file="ICacheManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
  using System;

  /// <summary>
  /// Generic cache manager interface.
  /// </summary>
  /// <typeparam name="T">Type to hold in cache.</typeparam>
  public interface ICacheManager<T>
    where T : class
  {
    /// <summary>
    /// Get an item from cache or fetch and cache.
    /// </summary>
    /// <param name="key">cache key.</param>
    /// <param name="minutes">minutes to expiration.</param>
    /// <param name="getResult">function to invoke on result needed.</param>
    /// <returns>Object found in cache.</returns>
    T Get(string key, double minutes, Func<T> getResult);

    /// <summary>
    /// Removes an item from cache.
    /// </summary>
    /// <param name="cacheKey">Key to the cache item.</param>
    void RemoveItem(string cacheKey);
  }
}