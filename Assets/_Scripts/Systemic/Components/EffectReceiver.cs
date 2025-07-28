using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReceiver : EntityComponent
{
    public List<Effect> m_effects = new List<Effect>();

    public void AddEffect(AEffect t_effect, float t_duration)
    {
        bool reset = false;
        foreach(Effect e in m_effects)
        {
            if (e.effect.GetType() == t_effect.GetType())
            {
                reset = true;
            }
        }
        if (!reset)
        {
            m_effects.Add(new Effect(t_effect, t_duration, Time.time));
            t_effect.AddDot(this.gameObject);
        }
    }

    private void Update()
    {
        List<int> toremove = new List<int>();
        for (int i = 0; i < m_effects.Count; i++)
        {
            if (m_effects[i].isEnded())
                toremove.Add(i);
        }
        if (toremove.Count > 0)
        {
            foreach (int i in toremove)
            {
                m_effects[i].effect.RemoveDot(this.gameObject);
                m_effects.RemoveAt(i);
            }
        }
    }

}

[System.Serializable]
public class Effect
{
    public AEffect effect;
    public float duration;
    public float startTime;

    public Effect(AEffect t_effect, float t_duration, float t_startTime)
    {
        effect = t_effect;
        duration = t_duration;
        startTime = t_startTime;
    }

    public bool isEnded()
    {
        return (Time.time - startTime > duration);
    }
}