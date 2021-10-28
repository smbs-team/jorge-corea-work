namespace PTASTileStorageWorkerLibrary.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Take json and png as source and split the png in separated files base on json mapbox script definition, finally copy to target directory.
    /// </summary>
    public class SpriteSplitter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteSplitter"/> class.
        /// </summary>
        /// <param name="container">Blob target container.</param>
        public SpriteSplitter(Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Gets the target blob container. It is set on the contructor only.
        /// </summary>
        public CloudBlobContainer Container { get; private set; }

        /// <summary>
        /// Split the png at spritePath using jsonPath configuration in targetPath.
        /// </summary>
        /// <param name="jsonPath">Path to the json configuration.</param>
        /// <param name="spritePath">Path to sprite image to split.</param>
        /// <param name="targetPath">Path to target directoy to where to copy the splitted images.</param>
        /// <param name="layerName">Name of map layer for build path for sprite.</param>
        /// <returns> a dictionary with the mapped sprite image name to new target file.</returns>
        public async Task<Dictionary<string, string>> SplitToFolder(string jsonPath, string spritePath, string targetPath, string layerName)
        {
            Dictionary<string, string> splittedMap = new Dictionary<string, string>();
            using (Image bitmap = Bitmap.FromFile(spritePath))
            {
                Newtonsoft.Json.Linq.JObject jdata = null;
                using (var str = new StreamReader(jsonPath))
                {
                    var jDataText = str.ReadToEnd();
                    if (!string.IsNullOrEmpty(jDataText))
                    {
                        jdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jDataText);
                    }
                }

                var tmpFolder = System.IO.Path.GetTempPath();
                if (!Directory.Exists(tmpFolder))
                {
                    Directory.CreateDirectory(tmpFolder);
                }

                var targetDirectory = this.Container.GetDirectoryReference(targetPath);

                if (jdata != null)
                {
                    int i = 0;
                    foreach (var p in jdata.Properties())
                    {
                        splittedMap.Add(p.Name, $"v1.0/sprites/{layerName}/sprite_{i++}.png");
                        var value = p.Value as Newtonsoft.Json.Linq.JObject;
                        Rectangle sourceRect = new Rectangle(value.Value<int>("x"), value.Value<int>("y"), value.Value<int>("width"), value.Value<int>("height"));
                        var tempPathFile = Path.Combine(tmpFolder, "sprint_{i++}.png");
                        using (Bitmap newBmp = new Bitmap(sourceRect.Width, sourceRect.Height, bitmap.PixelFormat))
                        {
                            using (Graphics g = Graphics.FromImage(newBmp))
                            {
                                g.DrawImage(bitmap, new Rectangle(0, 0, sourceRect.Width, sourceRect.Height), sourceRect, GraphicsUnit.Pixel);
                                g.Flush();
                            }

                            if (!Directory.Exists(targetPath))
                            {
                                Directory.CreateDirectory(targetPath);
                            }

                            newBmp.Save(tempPathFile, System.Drawing.Imaging.ImageFormat.Png);
                        }

                        CloudBlockBlob blockBlob = this.Container.GetBlockBlobReference(Path.Combine(targetPath, $"sprite_{i}.png"));
                        blockBlob.Properties.ContentType = "image/png";
                        await blockBlob.UploadFromFileAsync(tempPathFile);
                        File.Delete(tempPathFile);
                    }
                }
            }

            return splittedMap;
        }
    }
}
