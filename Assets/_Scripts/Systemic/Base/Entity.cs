using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType
    {
        Player,
        Monster,
        Item
    };
    public EntityType m_EntityType;
    public List<EntityComponent> m_EntityComponents;
    public bool isVisible = true;

}
