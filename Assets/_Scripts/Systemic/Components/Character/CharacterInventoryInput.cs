using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(ItemsInRange))]
public class CharacterInventoryInput : EntityComponent
{
    public string next = "NextP1";
    public string prev = "PrevP1";
    public string pick = "PickItemP1";
    public string drop = "DropItemP1";

    private Inventory m_inventory;
    private ItemsInRange m_iir;

    private void Start()
    {
        m_inventory = GetComponent<Inventory>();
        m_iir = GetComponent<ItemsInRange>();
    }
    void Update ()
    {
		if (Input.GetButtonDown(next))
        {
            m_inventory.NextSelectedItem();
        }
        else if (Input.GetButtonDown(prev))
        {
            m_inventory.PreviousSelectedItem();
        }
        else if (Input.GetButtonDown(pick))
        {
            Item closest = GetClosestInteractable();
            if (closest != null)
                closest.GetComponent<Interactable>().Interact(this.gameObject);
        }
        else if (Input.GetButtonDown(drop))
        {
            m_inventory.RemoveItem();
        }
	}

    Item GetClosestInteractable()
    {
        float min = -1;
        Item o = null;
        foreach (Item i in m_iir.items)
        {
            if (i.gameObject.GetComponent<Interactable>() != null)
            {
                float dist = (transform.position - i.transform.position).magnitude;
                if (min == -1 || dist < min)
                {
                    min = dist;
                    o = i;
                }
            }
        }
        return (o);
    }

}
