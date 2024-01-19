using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LCTutorialMod.Patches;

namespace LCModFreeShipDecorations
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class ModeBase : BaseUnityPlugin
    {
        private const string modGUID = "7ph.dev.lcmodfreeshipdecorations";
        private const string modName = "LC Mod - Free Ship Decorations";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        public static ModeBase Instance;

        internal ManualLogSource mls;

        public ManualLogSource Mls
        {
            get { return mls; }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            // Initialize logging
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo(modName + " initialized");

            // Apply patches
            harmony.PatchAll(typeof(ModeBase));
            harmony.PatchAll(typeof(TerminalPatch));
        }
    }
}
