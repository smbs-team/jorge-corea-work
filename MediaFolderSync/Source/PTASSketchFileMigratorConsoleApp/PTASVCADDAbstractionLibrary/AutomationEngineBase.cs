namespace PTASVCADDAbstractionLibrary
{
    /// <summary>Defines the methods need of automation engine class.</summary>
    /// <seealso cref="PTASVCADDAbstractionLibrary.IAutomationEngine" />
    public abstract class AutomationEngineBase : IAutomationEngine
    {
        /// <summary>Initializes a new instance of the <see cref="AutomationEngineBase"/> class.</summary>
        internal AutomationEngineBase()
        {
        }

        /// <inheritdoc />
        public abstract void Init();

        /// <inheritdoc />
        public abstract int NewWorld(int x);

        /// <inheritdoc />
        public abstract void SetCurrWorld(int x);

        /// <inheritdoc />
        public abstract void SetDisplayHiddenLayersMessage(short x);

        /// <inheritdoc />
        public abstract void DestroyWorld(int hW);

        /// <inheritdoc />
        public abstract void Terminate();

        /// <inheritdoc />
        public abstract void LoadVCDFromFile(string path);

        /// <inheritdoc />
        public abstract void ExportXML(object pStruct, string path, short selectedOnly);

        /// <inheritdoc />
        public abstract void ClearDrawingNoPrompt(int hW);

        /// <inheritdoc />
        public abstract int GetCurrWorld();

        /// <inheritdoc />
        public abstract object GetXMLStruct();
    }
}
