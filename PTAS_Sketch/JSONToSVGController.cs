using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace PTAS_Sketch
{
    [Route("api/[controller]")]
    public class JSONToSVGController : Controller
    {
        [HttpPost]
        public async Task<ContentResult> Post()
        {
            string json = null;
            using (var reader = new StreamReader(Request.Body))
            {
                json = await reader.ReadToEndAsync();
            }
            var sketch = (PTASMobileSketch.SketchControl)JsonConvert.DeserializeObject(json, typeof(PTASMobileSketch.SketchControl), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var svg = PTASMobileSketch.SketchToSVG.Write(sketch);
            return Content(svg, "image/svg+xml");
        }
    }
}
