using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickFunction : InteractFunction
{
    public override void InteractAction(GameObject player)
    {
        ItemsInRange ir = player.GetComponent<ItemsInRange>();
        Inventory inventory = player.GetComponent<Inventory>();

        if (ir != null && inventory != null)
        {
            Item item = GetComponent<Item>();
            if (item != null)
            {
                inventory.AddItem(item);
                ir.items.Remove(item);
                GetComponent<PotionController>().IsPicked();
                Destroy(this.gameObject);
            }
        }
    }

}
