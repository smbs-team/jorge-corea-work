// <copyright file="CreateSignDocumentController.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PTASIlinxService.App_Start.SwaggerFilters;
    using PTASIlinxService.Classes;
    using PTASLinxConnectorHelperClasses.Models;
    using PTASServicesCommon.CloudStorage;
    using Swashbuckle.Swagger.Annotations;
    using static PTASIlinxService.ExceptionFilter;

    /// <summary>
    /// Controller for signing documents.
    /// </summary>
    public class CreateSignDocumentController : ApiController
  {
    private const string FileName = "default-template";
    private const string Template = "template";
    private static readonly IDictionary<string, string> Files = new Dictionary<string, string>();

    private readonly IConfigParams config;
    private readonly CloudBlobContainer container;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSignDocumentController"/> class.
    /// </summary>
    /// <param name="provider">Cloud provider.</param>
    /// <param name="config">Configuration.</param>
    public CreateSignDocumentController(ICloudStorageProvider provider, IConfigParams config)
    {
      if (provider is null)
      {
        throw new ArgumentNullException(nameof(provider));
      }

      this.container = provider.GetCloudBlobContainer("templates");
      this.config = config ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// Posts data for document.
    /// </summary>
    /// <returns>Result of post.</returns>
    [SwaggerResponse(HttpStatusCode.OK, "No Error", typeof(HtmlDocumentResponse))]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "Unhandled error", typeof(ErrorResponse))]
    public async Task<HtmlDocumentResponse> Post(/*[FromBody] dynamic pp*/)
    {
      var requestContent = await this.GetRequestContent();
      var postValues = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(requestContent);
      var htmlResponse = this.ExtractItem(postValues, FileName).RemoveDoubleSpaces().RemoveEmptyTemplateVars().RemoveNewLines();
      return new HtmlDocumentResponse { HtmlResponse = htmlResponse };
    }

    /// <summary>
    /// Gets string form actual storage.
    /// </summary>
    /// <param name="fileName">File to load.</param>
    /// <returns>String content of file.</returns>
    protected virtual string GetFromStorage(string fileName)
    {
      if (this.config.LoadDocusignHtmlFromBlob)
      {
        return this.container.GetBlockBlobReference($"{fileName.ToLower()}.html").DownloadText();
      }

      fileName = $"~/templates/{fileName.ToLower()}.html";
      string path = HttpContext.Current.Server.MapPath(fileName);
      return File.ReadAllText(path);
    }

    /// <summary>
    /// Get content from post body.
    /// </summary>
    /// <returns>Content posted in body.</returns>
    protected virtual Task<string> GetRequestContent()
    {
      return this.Request.Content.ReadAsStringAsync();
    }

    private string ContentFromArray(string key, JArray jArray)
          => jArray
        .Children<JObject>()
        .Select(itm
          => itm.Properties().ToDictionary(valuePair => valuePair.Name, k => k.Value))
        .Aggregate(string.Empty, (prior, properties)
          => prior + this.ExtractItem(properties, key));

    private string ContentFromObject(string key, JObject jObj)
      => this.ExtractItem(jObj.Properties().ToDictionary(valuePair => valuePair.Name, valuePair => valuePair.Value), key);

    private string ExtractItem(Dictionary<string, JToken> properties, string defaultFile)
    {
      var fileName = properties.TryGetValue(Template, out JToken templateObj)
             ? templateObj.Value<string>()
             : defaultFile;
      return this.ReplaceProperties(fileName, properties);
    }

    private string GetFileContents(string fileName)
    {
      if (!Files.TryGetValue(fileName, out string content))
      {
        content = this.GetFromStorage(fileName);
        Files.Add(fileName, content);
      }

      return content;
    }
    ////private string GetFromStorage(string fileName)
    ////{
    ////  var readContent = File
    ////    .ReadAllText(GetPath(fileName))
    ////    .Replace(Environment.NewLine, " ");
    ////  Files.Add(fileName, readContent);
    ////  return readContent;
    ////}

    private string ReplaceProperties(string fileName, Dictionary<string, JToken> values)
    {
      string seed = this.GetFileContents(fileName);
      return values
             .Aggregate(seed, (inpt, jtokenValuePair) =>
             {
               string toReplace = $"%{jtokenValuePair.Key}%";
               switch (jtokenValuePair.Value)
               {
                 case JObject jObj:
                   return inpt.Replace(toReplace, this.ContentFromObject(jtokenValuePair.Key, jObj));
                 case JArray jArray:
                   return inpt.Replace(toReplace, this.ContentFromArray(jtokenValuePair.Key, jArray));
                 default:
                   return inpt.Replace(toReplace, jtokenValuePair.Value.ToString() ?? jtokenValuePair.ToString());
               }
             });
    }
  }
}
