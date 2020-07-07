using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionCounter : MonoBehaviour
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
        gameObject.GetComponent<Text>().text = "" + player.inventory.healthPotions;
    }
    public void UpdateText(Player.Item.ItemType itemType)
    {
        Debug.Log("updateText :: " + itemType + player.inventory.healthPotions);
        if (itemType == Player.Item.ItemType.HealthPotion)
        {
            gameObject.GetComponent<Text>().text = "" + player.inventory.healthPotions;
        }
    }
}
