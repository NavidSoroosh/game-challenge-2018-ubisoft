using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : EntityComponent
{
    public ToTrigger TriggerTarget;
    public bool isTriggered = false;

    private void FixedUpdate()
    {
        if (isTriggered == true)
        {
            TriggerTarget.isTriggered = true;
        }
        else
        {
            TriggerTarget.isTriggered = false;
        }
    }


}
