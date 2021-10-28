namespace PTASVCADDAbstractionLibrary
{
    /// <summary>Declares automation engine methods.</summary>
    public interface IAutomationEngine
    {
        /// <inheritdoc cref = "AutomationEngineWrapper.ClearDrawingNoPrompt(int)"/>
        void ClearDrawingNoPrompt(int hW);

        /// <inheritdoc cref = "AutomationEngineWrapper.DestroyWorld(int)"/>
        void DestroyWorld(int hW);

        /// <inheritdoc cref = "AutomationEngineWrapper.ExportXML(object, string, short)"/>
        void ExportXML(object pStruct, string path, short selectedOnly);

        /// <inheritdoc cref = "AutomationEngineWrapper.Init"/>
        void Init();

        /// <inheritdoc cref = "AutomationEngineWrapper.LoadVCDFromFile(string)"/>
        void LoadVCDFromFile(string path);

        /// <inheritdoc cref = "AutomationEngineWrapper.NewWorld(int)"/>
        int NewWorld(int x);

        /// <inheritdoc cref = "AutomationEngineWrapper.SetCurrWorld(int)"/>
        void SetCurrWorld(int x);

        /// <inheritdoc cref = "AutomationEngineWrapper.SetDisplayHiddenLayersMessage(short)"/>
        void SetDisplayHiddenLayersMessage(short x);

        /// <inheritdoc cref = "AutomationEngineWrapper.Terminate"/>
        void Terminate();

        /// <inheritdoc cref = "AutomationEngineWrapper.GetCurrWorld"/>
        int GetCurrWorld();

        /// <inheritdoc cref = "AutomationEngineWrapper.GetXMLStruct"/>
        object GetXMLStruct();
    }
}