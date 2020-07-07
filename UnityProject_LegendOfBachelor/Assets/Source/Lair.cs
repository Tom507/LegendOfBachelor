using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lair : MonoBehaviour
{

    public GameObject door;
    public Player player;

    public void OpenDoor()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.F) && player.inventory.keys >= 1)
        {
            door.SetActive(false);
            player.inventory.keys--;
        }
        player.currentState = Player.PlayerState.Walking;

    }
}
