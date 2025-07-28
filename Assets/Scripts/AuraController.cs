using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraController : MonoBehaviour
{
	public int numFaction;
	public GameController gameController;

    private SphereCollider m_sc;
    private Player m_p;

	void OnTriggerEnter(Collider other){

		GameObject pnj = other.gameObject;

        if (pnj.tag.Contains("PNJ"))
        {
            PNJController pnjController = pnj.GetComponent<PNJController>();

            if (numFaction != pnjController.GetFaction())
            {
                gameController.ChangeScore(pnjController.GetFaction(), numFaction);
                gameController.ChangeFaction(numFaction, pnj);

            }
        }
	}

    private void Start()
    {
        m_sc = GetComponent<SphereCollider>();
        m_p = GetComponentInParent<Player>();
    }

    private void Update()
    {
        m_sc.radius = m_p.zoneRadius;
    }
}
