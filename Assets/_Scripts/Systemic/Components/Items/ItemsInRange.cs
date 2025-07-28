using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInRange : EntityComponent
{
    public List<Item> items;

    private void OnTriggerEnter(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null)
        {
            Item i = e.GetComponent<Item>();
            if (i != null)
                items.Add(i);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null)
        {
            Item i = e.GetComponent<Item>();
            if (i != null)
                items.Remove(i);
        }
    }

}
