using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AEffect
{
    virtual public void ApplyEffect(GameObject t_target, float t_duration)
    {
        EffectReceiver er = t_target.GetComponent<EffectReceiver>();
        if (er != null)
        {
            er.AddEffect(this, t_duration);
        }
    }

    virtual public void AddDot(GameObject t_target)
    {

    }

    virtual public void RemoveDot(GameObject t_target)
    {

    }
}
