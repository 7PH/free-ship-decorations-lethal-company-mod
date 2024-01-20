using HarmonyLib;
using System.Collections.Generic;

namespace LCTutorialMod.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        /**
         * Alter ship decorations to be free and limit the number of decorations to 1
         */
        [HarmonyPatch("RotateShipDecorSelection")]
        [HarmonyPostfix]
        static void changeShownDecorationPrices(ref List<TerminalNode> ___ShipDecorSelection)
        {
            // Remove all decorations but the first
            if (___ShipDecorSelection.Count > 1)
            {
                ___ShipDecorSelection.RemoveRange(1, ___ShipDecorSelection.Count - 1);
            }
            // Alter price
            foreach (TerminalNode node in ___ShipDecorSelection)
            {
                LCModFreeShipDecorations.ModeBase.Instance.mls.LogInfo("Overriding the price of ship decoration (" + node.ToString() + ")");
                node.itemCost = 0;
            }
        }

        /**
         * Override the ship decoration price
         */
        [HarmonyPatch("LoadNewNodeIfAffordable")]
        [HarmonyPrefix]
        static void changeDecorationPrices(TerminalNode node, ref List<TerminalNode> ___ShipDecorSelection)
        {
            // Determine whether the wanted item (during initial selection or confirm step) is a ship decoration
            UnlockableItem item = node.shipUnlockableID == -1 ? null : StartOfRound.Instance.unlockablesList.unlockables[node.shipUnlockableID];
            bool isDecoration = item != null && ___ShipDecorSelection.Contains(item.shopSelectionNode);

            // Only override the item cost to 0 if it's a decoration
            if (isDecoration)
            {
                LCModFreeShipDecorations.ModeBase.Instance.mls.LogInfo("Overriding the price of ship decoration (" + node.ToString() + ")");
                node.itemCost = 0;
            }
        }
    }
}
