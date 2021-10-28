// <copyright file="IContactManager.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASDynamicsCrudHelperClasses.Interfaces
{
  using System.Threading.Tasks;
  using PTASDynamicsCrudHelperClasses.JSONMappings;

  /// <summary>
  /// Generic contact manager.
  /// </summary>
  public interface IContactManager
  {
    /// <summary>
    /// Gets a contact based on the email.
    /// </summary>
    /// <param name="email">email to look for.</param>
    /// <returns>Contact if found, null if not.</returns>
    Task<DynamicsContact> GetContactFromEmail(string email);

    /// <summary>
    /// Get contact info by contact Id.
    /// </summary>
    /// <param name="contactId">Id of the contact to search for.</param>
    /// <returns>The contact if found or null.</returns>
    Task<DynamicsContact> GetContactFromContactId(string contactId);
  }
}