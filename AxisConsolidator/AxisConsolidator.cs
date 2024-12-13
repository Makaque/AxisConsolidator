using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;

namespace AxisConsolidator
{
    public class AxisConsolidator : ModBehaviour
    {
        public static AxisConsolidator Instance;
        public bool useShipLookDirection = false;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"{nameof(AxisConsolidator)} is loaded!", MessageType.Success);
            useShipLookDirection = ModHelper.Config.GetSettingsValue<bool>("Use Ship Look Direction");
            new Harmony("Makaque.AxisConsolidator").PatchAll(Assembly.GetExecutingAssembly());
        }

        public override void Configure(IModConfig config)
        {
            useShipLookDirection = config.GetSettingsValue<bool>("Use Ship Look Direction");
            ModHelper.Console.WriteLine($"Changed 'Use Ship Look Direction' setting to {useShipLookDirection}!");
        }

    }

    [HarmonyPatch]
    public class PlayerAxisConsolidation
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BaseInputManager), nameof(BaseInputManager.UpdateInversion))]
        public static bool BaseInputManager_UpdateInversion_Prefix()
        {
            if (AxisConsolidator.Instance.useShipLookDirection)
            {
                if (BaseInputManager._pendingInputMode == InputMode.ShipCockpit || BaseInputManager._pendingInputMode == InputMode.ModelShip)
                {
                    InputLibrary.pitch.InversionFactor = PlayerData.GetShipLookInversionFactor();
                    InputLibrary.look.InversionFactor = PlayerData.GetLookInversionFactor();
                    InputLibrary.freeLook.InversionFactor = PlayerData.GetLookInversionFactor();
                    return false;
                }
                InputLibrary.pitch.InversionFactor = PlayerData.GetShipLookInversionFactor();
                InputLibrary.look.InversionFactor = PlayerData.GetLookInversionFactor();
                InputLibrary.freeLook.InversionFactor = PlayerData.GetLookInversionFactor();
                return false;
            } else
            {
                if (BaseInputManager._pendingInputMode == InputMode.ShipCockpit || BaseInputManager._pendingInputMode == InputMode.ModelShip)
                {
                    InputLibrary.pitch.InversionFactor = PlayerData.GetShipLookInversionFactor();
                    InputLibrary.look.InversionFactor = PlayerData.GetLookInversionFactor();
                    InputLibrary.freeLook.InversionFactor = PlayerData.GetLookInversionFactor();
                    return false;
                }
                InputLibrary.pitch.InversionFactor = PlayerData.GetLookInversionFactor() * -1;
                InputLibrary.look.InversionFactor = PlayerData.GetLookInversionFactor();
                InputLibrary.freeLook.InversionFactor = PlayerData.GetLookInversionFactor();
                return false;
            }
        }
    }

}
