// <copyright file="ImageAnalysisHelper.cs" company="King County">
//  Copyright (c) King County. All rights reserved.
// </copyright>

namespace PTASIlinxService.Classes
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
  using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
  using PTASILinxConnectorHelperClasses.Models;
  using PTASLinxConnectorHelperClasses.Models;

  /// <summary>
  /// Helper class interface to image analysis.
  /// </summary>
  public class ImageAnalysisHelper : IImageAnalysisHelper
  {
    private readonly string endpoint;
    private readonly string subscriptionKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageAnalysisHelper"/> class.
    /// </summary>
    /// <param name="config">Configuration parameters.</param>
    public ImageAnalysisHelper(IConfigParams config)
    {
      this.endpoint = config.CognitiveEndPoint;
      this.subscriptionKey = config.CognitiveSubscriptionKey;
    }

    /// <summary>
    /// Decide if an image is text and doesn't have adult content.
    /// </summary>
    /// <param name="buffer">Physical representation of the image.</param>
    /// <returns>True if image is acceptable, false otherwise.</returns>
    public async Task<(bool, string)> ImageIsAcceptable(byte[] buffer)
    {
      using (ComputerVisionClient client = Authenticate(this.endpoint, this.subscriptionKey))
      {
        var text = await AnalyzeOCR(buffer, client);
        if (!text.Regions.Any())
        {
          return (false, "Image does not contain text.");
        }

        var analisys = await AnalyzeImage(buffer, client);
        return analisys.Adult.IsAdultContent || analisys.Adult.IsRacyContent ? (false, "Image has adult or racy content.") : (true, string.Empty);
      }
    }

    private static Task<ImageAnalysis> AnalyzeImage(byte[] buffer, ComputerVisionClient client)
    {
      return client.AnalyzeImageInStreamAsync(new MemoryStream(buffer), new List<VisualFeatureTypes>()
            {
          VisualFeatureTypes.Adult,
            });
    }

    private static Task<OcrResult> AnalyzeOCR(byte[] buffer, ComputerVisionClient client)
    {
      return client.RecognizePrintedTextInStreamAsync(true, new MemoryStream(buffer));
    }

    private static ComputerVisionClient Authenticate(string endpoint, string key) =>
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
  }
}