// <copyright file="FormInput.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Classes
{
  /// <summary>
  /// Abstract form input for all crud.
  /// </summary>
  public abstract class FormInput
  {
    /// <summary>
    /// Object must be able to set it's own ID.
    /// </summary>
    public abstract void SetId();
  }
}
