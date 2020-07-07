using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractebleTrigger : MonoBehaviour
{
    private bool playerInTrigger = false;
    private Player player;

    public Player.PlayerState triggerType = Player.PlayerState.Shoping;

    private void Start()
    {
        LOBEvents.current.onInteracting += Interaction;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    public void Interaction()
    {
        if (playerInTrigger && player.currentState == Player.PlayerState.Walking)
        {
            Debug.Log("Player in Trigger : " + playerInTrigger);
            player.currentState = triggerType;
            if (triggerType == Player.PlayerState.Shoping)
            {
                transform.parent.gameObject.GetComponent<Shop>().ShopStart();
            }
            if (triggerType == Player.PlayerState.Learning)
            {
                transform.parent.gameObject.GetComponent<Bibliothek>().BibliothekStart();
            }
            if (triggerType == Player.PlayerState.DoorOpening)
            {
                transform.parent.gameObject.GetComponent<Lair>().OpenDoor();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !other.isTrigger)
        {
            // display "press F"
            playerInTrigger = true;
            Debug.Log("Player enter Trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            // Stop display "press F"
            playerInTrigger = false;
            Debug.Log("Player exit Trigger");
        }
    }
}
