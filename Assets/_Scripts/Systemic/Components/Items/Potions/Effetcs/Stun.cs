using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stun : AEffect
{
    override public void AddDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            if (!p.isShield)
            {
                p.isStun = true;
                if (NetworkManager.GetInstance() != null)
                {
                    if (NetworkManager.GetInstance().PhoneIsConnected())
                        NetworkManager.GetInstance().SendMessage("stun-" + p.ID + "-" + p.type);
                }
                
            }
        }
    }

    override public void RemoveDot(GameObject t_target)
    {
        Player p = t_target.GetComponent<Player>();
        if (p != null)
        {
            p.isStun = false;
            if (NetworkManager.GetInstance() != null)
                NetworkManager.GetInstance().SendMessage("destun-" + p.ID);
        }
    }
}
