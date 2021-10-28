using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PTAS_Sketch
{
    [Route("api/[controller]")]
    public class SketchToJSONController : Controller
    {
        [HttpGet]
        public ContentResult Get(string route)
        {
            var filename = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "xml", route);
            filename = filename.Substring(0, filename.Length - 4) + ".json";
            string result;
            if (System.IO.File.Exists(filename))
            {
                result = System.IO.File.ReadAllText(filename);
            }
            else
            {
                filename = filename.Substring(0, filename.Length - 5) + ".vcd.xml";
                var xml = System.IO.File.ReadAllText(filename);
                var sketch = PTASMobileSketch.SketchFromVCADD.Read(xml);
                result = JsonConvert.SerializeObject(sketch, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            System.Console.WriteLine(route + ": " + System.Environment.NewLine + result);
            return Content(result, "application/json");
        }

        [HttpPost]
        public async void Post(string route)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var text = await reader.ReadToEndAsync();
                System.Console.WriteLine(route + ": " + System.Environment.NewLine + text);
            }
        }
    }
}
