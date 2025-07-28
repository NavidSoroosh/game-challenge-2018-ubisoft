using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZoneP : AEffect
{
    public float Multiplier = 0.5f;

    override public void AddDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            p.zoneRadius += p.BaseRadius * Multiplier;
        }
    }

    override public void RemoveDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            p.zoneRadius -= p.BaseRadius * Multiplier;
        }
    }
}
