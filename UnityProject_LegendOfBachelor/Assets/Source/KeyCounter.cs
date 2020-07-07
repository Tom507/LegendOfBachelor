using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCounter : MonoBehaviour
{
    Player player;
    private void Start()
    {
        LOBEvents.current.onItemBought += UpdateText;
        LOBEvents.current.onItemPickup += UpdateText;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        UpdateText();
    }

    public void UpdateText()
    {
        gameObject.GetComponent<Text>().text = "" + player.inventory.keys;
    }
    public void UpdateText(Player.Item.ItemType itemType)
    {
        if(itemType == Player.Item.ItemType.Key)
        {
            gameObject.GetComponent<Text>().text = "" + player.inventory.keys;
            Debug.Log("UpdateText");
        }
    }
}
