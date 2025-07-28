using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : EntityComponent
{
    public GameObject stun;
    public GameObject shield;
    public GameObject SpeedUp;
    private Player m_player;

    private void Start()
    {
        m_player = GetComponent<Player>();
    }

    private void Update()
    {
        stun.SetActive(m_player.isStun);
        shield.SetActive(m_player.isShield);

        if (m_player.speed > m_player.BaseSpeed)
            SpeedUp.SetActive(true);
        else
            SpeedUp.SetActive(false);
    }
}
