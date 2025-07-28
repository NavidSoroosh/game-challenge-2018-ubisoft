using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : EntityComponent
{
    public float m_Range;
    public float m_FOV = 0.707f;
    public List<GameObject> EntitiesInSight;

    private RaycastHit[] hits;

    private void Start()
    {
        SphereCollider sc = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        sc.isTrigger = true;
        sc.radius = m_Range;
    }

    private void OnTriggerStay(Collider other)
    {
        Entity e = other.gameObject.GetComponent<Entity>();
        if (e != null)
        {
            Vector3 direction = Vector3.Normalize(other.transform.position - transform.position);
            float dot = Vector3.Dot(direction, transform.forward);
            if (e.isVisible)
            {
                hits = Physics.RaycastAll(transform.position, (other.transform.position - transform.position));
                if (hits.Length > 0)
                {
                    bool canSee = true;
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject.layer != 2 && hit.transform.gameObject.GetInstanceID() != other.gameObject.GetInstanceID())
                            canSee = false;
                        Debug.DrawRay(transform.position, hit.point, Color.red);
                        if (hit.transform.gameObject.GetInstanceID() == other.gameObject.GetInstanceID() && canSee && !EntitiesInSight.Exists(x => x.GetInstanceID() == other.gameObject.GetInstanceID()) && dot > m_FOV)
                        {
                            EntitiesInSight.Add(other.gameObject);
                        }
                        else if (hit.transform.gameObject.GetInstanceID() == other.gameObject.GetInstanceID() && (!canSee || dot <= m_FOV))
                        {
                            EntitiesInSight.Remove(other.gameObject);
                        }
                    }

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity e = other.gameObject.GetComponent<Entity>();
        if (e != null)
        {
            if (e.isVisible && EntitiesInSight.Exists(x => x.GetInstanceID() == other.gameObject.GetInstanceID()))
                EntitiesInSight.Remove(other.gameObject);
        }
    }
}
