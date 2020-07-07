using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lair : MonoBehaviour
{

    public GameObject door;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (Input.GetKeyDown(KeyCode.F))
            door.SetActive(false);
        player.currentState = Player.PlayerState.Walking;

    }
}
