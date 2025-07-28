using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    private Entity m_entity;

    private void Awake()
    {
        if (gameObject.GetComponentInParent<Entity>() != null)
        {
            m_entity = gameObject.GetComponentInParent<Entity>();
            m_entity.m_EntityComponents.Add(this);
        }
        else
        {
            m_entity = gameObject.GetComponent<Entity>();
            m_entity.m_EntityComponents.Add(this);
        }
    }
}
