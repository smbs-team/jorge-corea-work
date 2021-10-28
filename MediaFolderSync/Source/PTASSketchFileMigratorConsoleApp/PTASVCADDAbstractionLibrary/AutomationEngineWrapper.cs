namespace PTASVCADDAbstractionLibrary
{
    using VCadd32;

    /// <summary>AutomationEngine class wrapper.</summary>
    /// <seealso cref="PTASVCADDAbstractionLibrary.AutomationEngineBase" />
    public class AutomationEngineWrapper : AutomationEngineBase
    {
        private AutomationEngine vcadd;

        /// <summary>Initializes a new instance of the <see cref="AutomationEngineWrapper"/> class.</summary>
        public AutomationEngineWrapper()
        {
            this.vcadd = new AutomationEngine();
        }

        /// <summary>Initiates command to clear the current drawing of all entities. Will not prompt user to verify the command.</summary>
        /// <param name="hW">The world handle to reference open drawing worlds.</param>
        public override void ClearDrawingNoPrompt(int hW)
        {
            this.vcadd.ClearDrawingNoPrompt(hW);
        }

        /// <summary>Destroys a drawing world and frees allocated memory.</summary>
        /// <param name="hW">The world handle of the world to be destroyed.</param>
        public override void DestroyWorld(int hW)
        {
            this.vcadd.DestroyWorld(hW);
        }

        /// <summary>Export an XML drawing file.</summary>
        /// <param name="pStruct">Pointer to a VCXMLSettings structure.</param>
        /// <param name="path">File name for the exported file.</param>
        /// <param name="selectedOnly">True to export only selected entities, false to export all entities.</param>
        public override void ExportXML(object pStruct, string path, short selectedOnly)
        {
            this.vcadd.ExportXML(pStruct, path, selectedOnly);
        }

        /// <summary>Returns or sets the Visual CADD™ world handle of the current drawing world.</summary>
        /// <returns>The Visual CADD™ world handle returned when VCNewWorld is used.</returns>
        public override int GetCurrWorld()
        {
            return this.vcadd.GetCurrWorld();
        }

        /// <summary>
        ///   <para>Get the current XML import/export settings as a structure</para>
        /// </summary>
        /// <returns>The number of bytes required for the structure.</returns>
        public override object GetXMLStruct()
        {
            return this.vcadd.GetXMLStruct();
        }

        /// <summary>Used to initialize the Visual CADD™ DLL in order to access the functionality from an external application.</summary>
        public override void Init()
        {
            this.vcadd.Init();
        }

        /// <summary>Loads a Visual CADD™ native file.</summary>
        /// <param name="path">The path and file name for saving the drawing.</param>
        public override void LoadVCDFromFile(string path)
        {
            this.vcadd.LoadVCDFromFile(path);
        }

        /// <summary>Creates another instance of a "world" for Visual CADD™ to create or modify a drawing.</summary>
        /// <param name="x">  The world handle for the object to be used as the new world.</param>
        /// <returns>A long representing a Visual CADD™ world handle.</returns>
        public override int NewWorld(int x)
        {
            return this.vcadd.NewWorld(x);
        }

        /// <summary>  Sets the Visual CADD™ world handle of the current drawing world.</summary>
        /// <param name="x">The Visual CADD™ world handle returned when VCNewWorld is used.</param>
        public override void SetCurrWorld(int x)
        {
            this.vcadd.SetCurrWorld(x);
        }

        /// <summary>
        /// If a drawing being loaded has layers which are hidden, then Visual CADD™ will display a dialog box message alerting the user. This warning was implemented as it is disconcerting not to be warned when all of one's data is not shown. However, VCGetDisplayHiddenLayersMessage and VCSetDisplayHiddenLayersMessage allows for direct reading and setting the value of this setting.
        /// </summary>
        /// <param name="x">Toggle setting:<br />0 - Off (Unchecked)<br />1 - On (Checked)<br />2 - Restore previous state.</param>
        public override void SetDisplayHiddenLayersMessage(short x)
        {
            this.vcadd.SetDisplayHiddenLayersMessage(x);
        }

        /// <summary>Used to remove the Visual CADD™ DLL from memory.</summary>
        public override void Terminate()
        {
            this.vcadd.Terminate();
        }
    }
}
