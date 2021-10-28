namespace PTASTileStorageWorkerLibrary.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.Gis.Model;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Utility to convert mapbox styles to renderes json for import in database.
    /// </summary>
    public class MapboxStyleConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapboxStyleConverter"/> class.
        /// </summary>
        /// <param name="container">Blob target container.</param>
        public MapboxStyleConverter(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Gets the target blob container. It is set on the contructor only.
        /// </summary>
        public CloudBlobContainer Container { get; private set; }

        /// <summary>
        /// Read style for the path and convert and save the new renderer definition in json format to target path.
        /// </summary>
        /// <param name="stylePath">Path to the style.</param>
        /// <param name="spriteMap">Sprite to map dictionary for aumatically set on renders definition.</param>
        /// <param name="targetPath">Target path for json renderer.</param>
        /// <param name="layerName">Name for target layer.</param>
        /// <returns>Async task to allow await.</returns>
        public async Task ConvertStyle(string stylePath, Dictionary<string, string> spriteMap, string targetPath, string layerName)
        {
            // convert and save the new renderer definition in json format
            using (StreamReader reader = new StreamReader(stylePath))
            {
                var styleAsText = reader.ReadToEnd();
                var styleData = JsonConvert.DeserializeObject<JObject>(styleAsText);
                var layers = styleData.Value<JArray>("layers");
                var layerFile = new StringBuilder();

                layerFile.Append("[");
                for (int i = 0; i < layers.Count; i++)
                {
                    var paint = layers[i].Value<JObject>("paint");
                    var layer = layers[i] as JObject;
                    layer.Remove("source");
                    layer.Add("source", layerName + "Source");
                    layer.Add("minzoom", 0);
                    layer.Add("maxzoom", 22);
                    if (paint.ContainsKey("fill-pattern"))
                    {
                        var currentValue = paint.Value<string>("fill-pattern");
                        paint.Remove("fill-pattern");
                        paint.Add("fill-pattern", spriteMap[currentValue]);
                    }

                    if (i > 0)
                    {
                        layerFile.Append(",");
                    }

                    var jsonObj = JsonConvert.SerializeObject(layers[i]);
                    layerFile.Append(jsonObj);
                }

                layerFile.Append("]");

                var targetDirectory = this.Container.GetDirectoryReference(targetPath);
                var tmpFile = Path.GetTempFileName();

                using (StreamWriter writer = new StreamWriter(tmpFile)) // Path.Combine(targetPath, $"renderer_{i}.json")))
                {
                    writer.Write(layerFile.ToString());
                    writer.Flush();
                }

                CloudBlockBlob blockBlob = this.Container.GetBlockBlobReference(Path.Combine(targetPath, $"defaultMapLayers.json"));
                blockBlob.Properties.ContentType = "application/json";
                await blockBlob.UploadFromFileAsync(tmpFile);
                File.Delete(tmpFile);
            }
        }
    }
}
