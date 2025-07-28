using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFunction : EntityComponent
{
    virtual public void InteractAction(GameObject player)
    {
        print("[ERROR] You Need to Override InteractAction From InteractFunction");
    }
}
