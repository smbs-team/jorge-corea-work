namespace PTASMediaHelperClasses
{
  using System;

  /// <summary>
  /// item returns an initial date
  /// must also update to current date.
  /// </summary>
  public interface IAssetDateManager
  {
    /// <summary>
    /// Gets a date from the object.
    /// </summary>
    /// <returns>The fetched date.</returns>
    DateTime GetDate();

    /// <summary>
    /// Save the date to current.
    /// </summary>
    void UpdateDate();
  }
}
