using System;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

using HarmonyLib;

using Il2CppInterop.Runtime.Injection;

using UnityEngine;

namespace Mod
{
    // Mod injection, not needed to change anything of this

    [BepInPlugin("GUID", "NAME", "VERSION")]
    public class Plugin : BasePlugin
    {
        internal new static ManualLogSource Log;

        public override void Load()
        {
            Log = base.Log;

            TryLoad();
        }

        private void TryLoad()
        {
            try
            {
                ClassInjector.RegisterTypeInIl2Cpp<BaseClass>();
                var plugin = new GameObject(typeof(BaseClass).FullName);
                UnityEngine.Object.DontDestroyOnLoad(plugin);
                plugin.AddComponent<BaseClass>();
                plugin.hideFlags = HideFlags.HideAndDontSave;
                new Harmony("GUID").PatchAll();

                Log.LogInfo("Plugin has been loaded!");

                TryPatch();
            }
            catch (Exception e)
            {
                Log.LogError("Error loading the plugin!");
                Log.LogError(e);
            }
        }

        private void TryPatch()
        {
            try
            {
                Log.LogInfo($"Attempting to patch...");

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

                Log.LogInfo($"Successfully patched!");
            }
            catch (Exception e)
            {
                Log.LogError($"Error registering patch!");
                Log.LogError(e);
            }
        }
    }

    // How to use Harmony with IL2CPP

    [HarmonyPatch(typeof(Class), nameof(Clas.Method), MethodType.Normal)]
    class PatchOnClick
    {
        [HarmonyPrefix]
        static void Prefix(SelectionButton __instance)
        {

        }
    }

    // Program the mod here

    public class BaseClass : MonoBehaviour
    {
        private void Start()
        {
            Plugin.Log.LogInfo("Working!");
        }
    }
}
