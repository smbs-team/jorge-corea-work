using System;
using PTASILinxConnectorHelperClasses.Models;

namespace PTASILinxService_UnitTests
{
  public class MoqCacheMananger<T> : ICacheManager<T>
    where T : class
  {
    private T found = null;
    public T Get(string key, double minutes, Func<T> getResult)
    {
      if (this.found == null)
      {
        this.found = getResult?.Invoke();
      }
      return this.found;
    }

    public void RemoveItem(string cacheKey)
    {
    }
  }
}
