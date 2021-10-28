using System.IO;
using Newtonsoft.Json;
using PTASMobileSketch;

namespace PTASSketchFromVCADD
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = args[0];
            var jsonOutput = args[1];
            var svgOutput = args[2];
            var xml = File.ReadAllText(input);
            var sketch = SketchFromVCADD.Read(xml);
            var json = JsonConvert.SerializeObject(sketch, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var svg = SketchToSVG.Write(sketch);
            File.WriteAllText(jsonOutput, json);
            File.WriteAllText(svgOutput, svg);
        }
    }
}
