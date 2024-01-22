using HarmonyLib;
using System.Collections.Generic;

namespace LCTutorialMod.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        /**
         * Make all ship decorations available and mark them free in the terminal listing
         */
        [HarmonyPatch("RotateShipDecorSelection")]
        [HarmonyPostfix]
        static void changeShownDecorationPrices(ref List<TerminalNode> ___ShipDecorSelection)
        {
            // Empty the list
            ___ShipDecorSelection.Clear();

            // Re-add all unlockables that are ship decorations
            for (int index = 0; index < StartOfRound.Instance.unlockablesList.unlockables.Count; ++index)
            {
                UnlockableItem obj = StartOfRound.Instance.unlockablesList.unlockables[index];
                if (obj.shopSelectionNode != null && !obj.alwaysInStock) {
                    ___ShipDecorSelection.Add(obj.shopSelectionNode);
                }
            }
            
            // Alter price to 0
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
