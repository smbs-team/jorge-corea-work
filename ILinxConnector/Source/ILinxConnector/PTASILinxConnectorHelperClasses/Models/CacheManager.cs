// <copyright file="CacheManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASILinxConnectorHelperClasses.Models
{
  using System;
  using System.Web;
  using System.Web.Caching;

  /// <summary>
  /// Generic http cache management.
  /// </summary>
  /// <typeparam name="T">Type to manage.</typeparam>
  public class CacheManager<T> : ICacheManager<T>
    where T : class
  {
    /// <inheritdoc/>
    public T Get(string key, double minutes, Func<T> getResult)
    {
      if (HttpContext.Current.Cache[key] is T found)
      {
        return found;
      }

      if (getResult != null)
      {
        T toReturn = getResult();
        if (toReturn != null)
        {
          HttpContext.Current.Cache.Add(key, toReturn, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes), CacheItemPriority.Normal, null);
          return toReturn;
        }
      }

      return default;
    }

    /// <inheritdoc/>
    public void RemoveItem(string cacheKey)
    {
      HttpContext.Current.Cache.Remove(cacheKey);
    }
  }
}
