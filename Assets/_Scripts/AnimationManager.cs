using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject ClerContainer;
    public GameObject InfContainer;
    public GameObject MedContainer;

    public GameObject GetContainerFromTeam(Player.PlayerType team)
    {
        switch(team)
        {
            case Player.PlayerType.Priest:
                return ClerContainer;
            case Player.PlayerType.Doctor:
                return MedContainer;
            case Player.PlayerType.Infected:
                return InfContainer;
            default:
                return null;
        }
    }
}
