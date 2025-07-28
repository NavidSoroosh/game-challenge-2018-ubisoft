using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityComponent
{
    public enum PlayerType
    {
        Infected,
        Doctor,
        Priest
    }
    public int ID = 0;
    public PlayerType type;
    public float speed;
    public float zoneRadius;
    [HideInInspector]
    public float BaseSpeed;
    [HideInInspector]
    public float BaseRadius;
    [HideInInspector]
    public bool isStun = false;
    [HideInInspector]
    public bool isShield = false;

    public AnimationManager animManager;

    private GameObject AnimContainer;
    [HideInInspector]
    public GameObject FaceAnim;
    [HideInInspector]
    public GameObject LeftAnim;
    [HideInInspector]
    public GameObject RightAnim;
    [HideInInspector]
    public GameObject ImoAnim;

    private void Start()
    {
        BaseSpeed = speed;
        BaseRadius = zoneRadius;

        AnimContainer = animManager.GetContainerFromTeam(type);
        GameObject RootAnimContainer = Instantiate(AnimContainer, transform.position, transform.rotation);
        RootAnimContainer.transform.parent = transform;
        RootAnimContainer.transform.localPosition = Vector3.zero;

        for (int i = 0; i < RootAnimContainer.transform.childCount; i++)
        {
            GameObject tmp = RootAnimContainer.transform.GetChild(i).gameObject;
            if (tmp.name.Contains("Droite"))
                RightAnim = tmp;
            else if (tmp.name.Contains("Gauche"))
                LeftAnim = tmp;
            else if (tmp.name.Contains("Face"))
                FaceAnim = tmp;
            else if (tmp.name.Contains("Immobile"))
                ImoAnim = tmp;
        }
    }
}
