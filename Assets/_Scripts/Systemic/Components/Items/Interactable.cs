using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : EntityComponent
{
    public InteractFunction function;
    public bool isInteractable = true;
    public void Interact(GameObject player)
    {
        if (isInteractable)
        {
            function.InteractAction(player);
        }
    }

}
