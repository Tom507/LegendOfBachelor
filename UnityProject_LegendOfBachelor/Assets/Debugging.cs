using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Debugging : MonoBehaviour
{
    public Player player;
    public Player.PlayerState playerState;
    public UIMachine.StateUi stateUi;
    [Header("_________________")]

    public bool overrite = true;

    [Header("PLayer stats Override")]

    [Tooltip("-1 = no Change")]
    public float playerHealth = -1;
    [Tooltip("-1 = no Change")]
    public float playerMoney = -1;
    [Tooltip("-1 = no Change")]
    public float playerSpeed = -1;
    [Tooltip("-1 = no Change")]
    public float playerAccellaration = -1;
    [Tooltip("-1 = no Change")]
    public float playerDamage = -1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && overrite)
        {
            playerState = player.currentState;
            stateUi = UIMachine.currentState;

            if (playerMoney != -1)
            {
                player.inventory.money = (int)playerMoney;
            }

            if (playerHealth != -1)
            {
                player.currentHealth = playerHealth;
            }

            NavMeshAgent navMeshAgent = player.GetComponent<NavMeshAgent>();
            if(playerSpeed != -1)
            {
                navMeshAgent.speed = playerSpeed;
            }

            if (playerAccellaration != -1)
            {
                navMeshAgent.acceleration = playerAccellaration;
            }

            if(playerDamage != -1)
            {
                player.inventory.itemAttackPoints = playerDamage;
            }
        }
    }
}
