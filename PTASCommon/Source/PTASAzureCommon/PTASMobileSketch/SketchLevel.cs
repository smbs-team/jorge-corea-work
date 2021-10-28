using System.Collections.Generic;

namespace PTASMobileSketch
{
    public class SketchLevel
    {
        public string uniqueIdentifier;

        public string name;

        public bool visible;

        public bool isScratchpad;

        public List<SketchLevelLayer> layers;
    }
}
