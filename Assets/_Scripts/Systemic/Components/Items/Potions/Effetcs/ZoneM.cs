using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZoneM : AEffect
{
    public float Multiplier = 0f;

    override public void AddDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            if (!p.isShield)
               p.zoneRadius = 0;
        }
    }

    override public void RemoveDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            p.zoneRadius = p.BaseRadius;
        }
    }
}
