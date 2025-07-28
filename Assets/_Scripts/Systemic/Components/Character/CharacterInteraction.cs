using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemsInRange))]
public class CharacterInteraction : EntityComponent
{
    private ItemsInRange ir;

    private void Start()
    {
        ir = GetComponent<ItemsInRange>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (ir.items.Count > 0)
            {
                Item toInteract = GetClosest();
                if (toInteract != null)
                {
                    toInteract.GetComponent<Interactable>().Interact(gameObject);
                }
            }
        }
    }

    private Item GetClosest()
    {
        Item toReturn = null;
        float minDist = -1;
        foreach (Item it in ir.items)
        {
            foreach (Item i in ir.items)
            {
                if (i.GetComponent<Interactable>() != null)
                {
                    float dist = (transform.position - i.transform.position).magnitude;
                    if (dist < minDist || minDist == -1)
                    {
                        minDist = dist;
                        toReturn = i;
                    }
                }
            }
        }
        return (toReturn);
    }
}
