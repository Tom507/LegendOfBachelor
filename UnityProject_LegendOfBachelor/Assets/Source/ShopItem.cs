using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public Player.Item.ItemType itemType = Player.Item.ItemType.HealthPotion;
    public int itemPrice = 1;
    public int itemLevel = 1;

    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public bool buy()
    {
        if(player.inventory.money >= itemPrice && gameObject.activeSelf)
        {
            player.inventory.money -= itemPrice;
            player.inventory.ItemPickup(new Player.Item(itemType, itemLevel));
            if(itemType != Player.Item.ItemType.HealthPotion)
            {
                gameObject.SetActive(false);
            }
            return true;
        }
        else
        {
            Debug.Log("You dont have enough money");
            return false;
        }
    }
}
