using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Panacee : AEffect
{
    public float MultiplierSpeed = 0.5f;
    public float MultiplierZone = 0.5f;
    public float sizeScale = 1.5f;

    override public void AddDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            p.zoneRadius += p.BaseRadius * MultiplierZone;
            p.speed += p.BaseSpeed * MultiplierSpeed;
            p.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);
        }
    }

    override public void RemoveDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            p.zoneRadius -= p.BaseRadius * MultiplierZone;
            p.speed -= p.BaseSpeed * MultiplierSpeed;
            p.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
