// <copyright file="BlobJsonGetter.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Web;
  using Microsoft.WindowsAzure.Storage.Blob;
  using PTASIlinxService.Classes.Exceptions;

  /// <summary>
  /// Class to manage blob to json.
  /// </summary>
  public static class BlobJsonGetter
  {
    /// <summary>
    /// Try to get content.
    /// </summary>
    /// <param name="container">Container to search for.</param>
    /// <param name="route">Route to get.</param>
    /// <param name="recursive">Do we need to go into the subfolders.</param>
    /// <returns>List of json strings.</returns>
    public static async Task<IEnumerable<JsonBlob>> GetAllContentsAsync(CloudBlobContainer container, string route, bool recursive = true)
    {
      try
      {
        var blob = container.GetDirectoryReference(route);
        IEnumerable<IListBlobItem> blobs = blob.ListBlobs(recursive);
        var blobTasks = blobs
          .Select(b => b as CloudBlockBlob)
          .Where(b => b != null)
          .Select(b => new
          {
            task = b.DownloadTextAsync(),
            uri = b.Uri.LocalPath.Replace($"/{container.Name}/", string.Empty),
          })
          .ToArray();
        var jsonStrings =
          await Task
          .WhenAll(blobTasks.Select(x => x.task).ToArray());
        return jsonStrings.Select((itm, idx) => new JsonBlob { Content = itm, Route = blobTasks[idx].uri });
      }
      catch (Exception ex)
      {
        throw new GetAllContentsFailException(ex);
      }
    }

    /// <summary>
    /// Changes a blob to another container.
    /// </summary>
    /// <param name="contents">Contents to save.</param>
    /// <param name="outputContainer">Where to save to.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal static async Task CopyToContainer(IEnumerable<JsonBlob> contents, CloudBlobContainer outputContainer)
    {
      var t = contents.Select(c =>
      {
        var r = c.Route.StartsWith("/") ? c.Route.Substring(1) : c.Route;
        var rf = outputContainer.GetBlockBlobReference(r);
        return rf.UploadTextAsync(c.Content);
      }).ToArray();
      await Task.WhenAll(t);
    }

    /// <summary>
    /// Changes a blob to another container.
    /// </summary>
    /// <param name="routes">Routes to delete.</param>
    /// <param name="srcContainer">Where to save to.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal static async Task DeleteFromContainer(IEnumerable<string> routes, CloudBlobContainer srcContainer)
    {
      foreach (var route in routes.ToArray())
      {
        var rf = srcContainer.GetBlockBlobReference(route);
        if (await rf.ExistsAsync())
        {
          await rf.DeleteAsync();
        }
      }
    }
  }
}