using System;
using System.Configuration;
using System.Linq;
using CommandLine;
using Microsoft.WindowsAzure.Storage.Blob;
using PTASServicesCommon.CloudStorage;

namespace JsonLister
{

  public class Options
  {

    [Option('r', "route", Required = false, HelpText = "Route in the container.")]
    public string Route { get; set; } = "";

    [Option('c', "container", Required = false, HelpText = "Container name, default json-store-processed.")]
    public string Container { get; set; } = null;

    [Option('w', "wait", Required = false, HelpText = "Wait at the end?")]
    public bool Wait { get; set; } = false;

  }

  class Program
  {
    static void Main(string[] args)
    {
      var wait = false;
      Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
      {
        var config = new CloudStorageConfigurationProvider(ConfigurationManager.AppSettings["BlobStorage"]);
        var provider = new CloudStorageProvider(config);
        string result = LoadJson(o, provider);

        string str = Beautify(result);
        wait = o.Wait;
        Console.Out.WriteLine(str);
      })
        .WithNotParsed(e 
          => e.ToList().ForEach(ie => Console.Out.WriteLine(ie.ToString())));
      if (wait)
      {
        Console.ReadLine();
      }
    }

    private static string Beautify(string result)
      => Newtonsoft.Json.JsonConvert.SerializeObject(
        Newtonsoft.Json.JsonConvert.DeserializeObject(result),
        Newtonsoft.Json.Formatting.Indented);

    private static string LoadJson(Options o, CloudStorageProvider provider)
    {
      var container = provider.GetCloudBlobContainer(o.Container ?? "json-store-processed");
      var directory = container.GetDirectoryReference(o.Route);
      var files = directory.ListBlobs(true).ToArray();
      var r = files.Select(b => b as CloudBlockBlob)
         .Where(b => b != null)
         .Select(f =>
         {
           string text = f.DownloadText();
           return $"{{\"url\" : \"{f.Uri.LocalPath}\", \"content\" : {text}}}";
         });
      var json = string.Join(",", r);
      var result = $"[{json}]";
      return result;
    }
  }
}
