using BlasII.Randomizer.Items;

namespace BlasII.Randomizer.MapTracker
{
    internal class InventoryHandler
    {
        private Blas2Inventory _currentInventory;
        private bool _needsRefresh = true;

        public Blas2Inventory CurrentInventory
        {
            get
            {
                if (_needsRefresh)
                {
                    _needsRefresh = false;
                    CalculateInventory();
                }

                return _currentInventory;
            }
        }

        /// <summary>
        /// Force the inventory to be recalculated next time it is needed
        /// </summary>
        public void Refresh() => _needsRefresh = true;

        /// <summary>
        /// Recalculate the inventory of the playthrough
        /// </summary>
        private void CalculateInventory()
        {
            Main.MapTracker.Log("Calculating new inventory");

            var settings = Main.Randomizer.CurrentSettings;
            _currentInventory = new Blas2Inventory(settings, Main.Randomizer.Data.DoorDictionary);
            _currentInventory.AddItem(Main.Randomizer.Data.GetItem(GetStartingWeaponId(settings)));

            foreach (string locationId in Main.Randomizer.ItemHandler.CollectedLocations)
            {
                Item item = Main.Randomizer.ItemHandler.GetItemAtLocation(locationId);
                if (item.progression)
                {
                    _currentInventory.AddItem(item);
                    Main.MapTracker.LogWarning("Adding " + item.id);
                }
            }

            //foreach (var item in Main.Randomizer.Data.ItemList)
            //{
            //    // Only check progression items
            //    if (!item.progression)
            //        continue;

            //    // Find the amount the player has of this item
            //    int amount;
            //    if (item.subItems == null)
            //    {
            //        amount = Main.Randomizer.ItemHandler.CollectedItems.Count(x => x == item.id);
            //    }
            //    else
            //    {
            //        amount = item.subItems.Count(x => Main.Randomizer.ItemHandler.IsItemCollected(x));
            //    }

            //    // Add it to the inventory
            //    for (int i = 0; i < amount; i++)
            //    {
            //        _currentInventory.AddItem(item);
            //    }
            //    if (amount > 0)
            //        Main.MapTracker.LogWarning($"Adding {item.id} {amount} times!");
            //}
        }

        /// <summary>
        /// Calculates the item id of the chosen starting weapon
        /// </summary>
        private string GetStartingWeaponId(RandomizerSettings settings)
        {
            return "WE0" + (settings.RealStartingWeapon + 1);
        }
    }
}
