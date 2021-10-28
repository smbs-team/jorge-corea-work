namespace PTASketchFileMigratorConsoleApp
{
    using System.Collections.Generic;

    /// <summary>Drawing types and entities ids data.</summary>
    public class FileType
    {
        /// <summary>Gets or sets the accessory drawing ids.</summary>
        /// <value>The accessory drawing ids.</value>
        public string[] AccessoryDrawingIDs { get; set; }

        /// <summary>Gets or sets the drawing ids.</summary>
        /// <value>The drawing ids.</value>
        public Dictionary<string, string> DrawingIds { get; set; }
    }
}
