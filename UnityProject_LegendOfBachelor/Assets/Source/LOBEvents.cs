using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOBEvents : MonoBehaviour
{
    public static LOBEvents current;
    private void Awake()
    {
        current = this;
    }

    public event Action<Monster> onFightStarted;
    public void FightStarted(Monster monster)
    {
        if (onFightStarted != null)
        {
            onFightStarted(monster);
        }
    }

    public event Action onFightEnded;
    public void FightEnded()
    {
        if (onFightEnded != null)
        {
            onFightEnded();
        }
    }

    public event Action onPlayerDeath;
    public void PlayerDeath()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    public event Action<Monster> onMonsterDeath;
    public void MonsterDeath(Monster monster)
    {
        if (onMonsterDeath != null)
        {
            onMonsterDeath(monster);
        }
    }

    public event Action onPause;
    public void Pause()
    {
        if (onPause != null)
        {
            onPause();
        }
    }
    public event Action onResume;
    public void Resume()
    {
        if (onResume != null)
        {
            onResume();
        }
    }

    public event Action<GameObject> onQuizToggle;
    public void QuizToggle(GameObject button)
    {
        if(onQuizToggle != null)
        {
            onQuizToggle(button);
        }
    }

    public event Action onQuestionAnswered;
    public void QuestionAnswered()
    {
        if (onQuestionAnswered != null)
        {
            onQuestionAnswered();
        }
    }

    // unused
    public event Action<Monster> onPlayerSpotted;
    public void PlayerSpoted(Monster m)
    {
        if (onPlayerSpotted != null)
        {
            onPlayerSpotted(m);
        }
    }

    //Shop

    public event Action onLackOfMoney;
    public void LackOfMoney()
    {
        if (onLackOfMoney != null)
        {
            onLackOfMoney();
        }
    }

    public event Action onItemBought;
    public void ItemBought()
    {
        if (onItemBought != null)
        {
            onItemBought();
        }
    }

    public event Action<Player.Item.ItemType> onItemUsed;
    public void ItemUsed(Player.Item.ItemType itemType)
    {
        if (onItemUsed != null)
        {
            onItemUsed(itemType);
        }
    }

    public event Action<Player.Item.ItemType> onItemPickup;
    public void ItemPickup(Player.Item.ItemType itemType)
    {
        if (onItemPickup != null)
        {
            onItemPickup(itemType);
        }
    }

    // Interaction

    public event Action onInteracting;
    public void Interacting()
    {
        if (onInteracting != null)
        {
            onInteracting();
        }
    }

    // Abbruch

    public event Action onEscaping;
    public void Escaping()
    {
        if (onEscaping != null)
        {
            onEscaping();
        }
    }
    
}
