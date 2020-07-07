using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEditor;

public class Player : MonoBehaviour
{
    #region systemic Vars
    public NavMeshAgent agent;
    private Vector3 input;
    private GameObject playerModel;

    private Camera ShopCamera;
    private Camera MainCamera;
    private Camera LearnCamera;
    #endregion

    [Header("Stats")]
    public float health = 100f;
    public float maxHealth = 100;
    public float currentHealth;
    public float baseAttackPoints = 10f;
    public float baseDefencePoints = 0f;
    public Inventory inventory;

    [Header("UI Ellements")]
    public HealthBar healthBar;

    [Header("FightsTech")]
    public int layerMaskMonster;

    public PlayerState currentState = PlayerState.Walking;

    public List<Monster> attackingMonsters = new List<Monster>();



    void Start()
    {
        #region Inits
        agent = GetComponent<NavMeshAgent>();
        var dubidamdamdamdubidabadibidam = 42; // objigatorische antwort auf alle Fragen
        layerMaskMonster = LayerMask.GetMask("Monster");

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        InventorySetup();
        playerModel = transform.GetChild(0).gameObject;

        // Camera Inits
        List<GameObject> allObjects = getAllGameobjectsInScene();
        ShopCamera = allObjects.Find(x => x.name ==  "Shop Camera").GetComponent<Camera>();
        MainCamera = allObjects.Find(x => x.name == "Main Camera").GetComponent<Camera>();
        LearnCamera = allObjects.Find(x => x.name == "LearnCamera").GetComponent<Camera>();

        UIMachine.StateChanged(UIMachine.StateUi.Game);
        #endregion


        //Subscribtions
        LOBEvents.current.onPlayerSpotted += PlayerSpoted;
        LOBEvents.current.onEscaping += EscapingToPause;

        // temp Invfill
        inventory.money = 10;
    }

    /// <summary>
    /// shows ALL Objects, disabled objects included.
    /// </summary>
    List<GameObject> getAllGameobjectsInScene()
    {
        {
            List<GameObject> objectsInScene = new List<GameObject>();

            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                    objectsInScene.Add(go);
            }

            return objectsInScene;
        }
    }
    void Update()
    {
        switch (currentState)
        {
            case PlayerState.Walking:
                LearnCamera.gameObject.SetActive(false);
                ShopCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(true);

                //Input / Movement
                input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                agent.SetDestination(transform.position + input);
                break;
            case PlayerState.Fighting:
                LearnCamera.gameObject.SetActive(false);
                ShopCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(true);
                break;
            case PlayerState.Shoping:
                LearnCamera.gameObject.SetActive(false);
                ShopCamera.gameObject.SetActive(true);
                MainCamera.gameObject.SetActive(false);
                break;
            case PlayerState.Pause:
                LearnCamera.gameObject.SetActive(false);
                ShopCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(true);
                break;
            case PlayerState.Learning:
                LearnCamera.gameObject.SetActive(true);
                ShopCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(false);
                break;
            case PlayerState.DoorOpening:
                LearnCamera.gameObject.SetActive(false);
                ShopCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(true);
                break;
        }
    
        if (!(currentState == PlayerState.Pause)) 
        {
            // Player Death
            if (health <= 0f)
            {
                LOBEvents.current.PlayerDeath();
                playerModel.SetActive(false);
            }

            // End Fight
            if(attackingMonsters.Count == 0 && currentState == PlayerState.Fighting)
            {
                LOBEvents.current.FightEnded();
                currentState = PlayerState.Walking;
            }

            // Interaktionen
            if (Input.GetButtonDown("Interaction"))
            {
                LOBEvents.current.Interacting();
            }
        }
        
        // Abbruch
        if (Input.GetButtonDown("Escape"))
        {
            LOBEvents.current.Escaping();
        }

        //Pause Delay counter
        if(pauseDelay > 0)
            pauseDelay -= Time.deltaTime;
    }

    public void TakeDamage()
    {
        float damage = 0f;
        foreach (Monster m in attackingMonsters)
        {
            damage += m.attackPoints;
        }
        currentHealth -= (damage - baseDefencePoints - inventory.itemDefencePoints);
        healthBar.SetHealth(currentHealth);
    }

    public void DealDamage()
    {
        foreach(Monster m in attackingMonsters)
        {
            Debug.Log(m.currentHealth + " : " + baseAttackPoints + " Item : " + inventory.itemAttackPoints);
            m.TakeDamage(baseAttackPoints + inventory.itemAttackPoints);
        }
    }

    // Monster enters Fighting Trigger 
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMaskMonster) != 0)
        {
            Debug.Log("Trigger enter : " + other.gameObject);

            Monster m = other.GetComponent<Monster>();
            attackingMonsters.Add(m);
            m.currentState = Monster.AiState.Fighting;
            currentState = PlayerState.Fighting;

            LOBEvents.current.FightStarted(m);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMaskMonster) != 0)
        {
            //TODO interactable löschen on exit
            Monster m = other.GetComponent<Monster>();
            attackingMonsters.Remove(m);
            m.currentState = Monster.AiState.Attacking;
        }
    }

    private void UseKey()
    {
        // TODO: Doors, open
    }
    #region Events
    private void PlayerSpoted(Monster m)
    {

    }

    public float pauseDelay = 0.5f;
    public float pauseDelayMax = 0.5f;
    private void EscapingToPause()
    {
        if (currentState == PlayerState.Walking && pauseDelay <= 0)
        {
            currentState = Player.PlayerState.Pause;
            LOBEvents.current.Pause();
            Debug.Log(currentState);
        }
        else
        {
            if (currentState == PlayerState.Pause)
            {
                currentState = Player.PlayerState.Walking;
                LOBEvents.current.Resume();
                Debug.Log(currentState);
            }
        }
    }

    #endregion

    public enum PlayerState
    {
        Walking,
        Fighting,
        Shoping,
        Pause,
        Learning,
        DoorOpening,
    }

    #region Inventory
    private void InventorySetup()
    {
        inventory = new Inventory(this);

    }
    [System.Serializable]
    public class Inventory
    {
        public Inventory(Player player)
        {
            this.player = player;
        }
        private Player player;

        [Header("Inventory Slots")]
        public int healthPotions = 0;
        public int keys = 0;
        public int money = 0;
        public float itemAttackPoints = 0f;
        public float itemDefencePoints = 0f;

        [Header("Item Values")]
        float healthPotionValue = 20f;

        public void ItemPickup(Item item)
        {
            switch (item.itemType)
            {
                case Item.ItemType.Sword:
                    if(item.itemLevel > itemAttackPoints)
                    {
                        itemAttackPoints = item.itemLevel;
                    }
                    break;
                case Item.ItemType.Shield:
                    if (item.itemLevel > itemDefencePoints)
                    {
                        itemDefencePoints = item.itemLevel;
                    }
                    break;
                case Item.ItemType.Armor:
                    if (item.itemLevel > itemDefencePoints)
                    {
                        itemDefencePoints = item.itemLevel;
                    }
                    break;
                case Item.ItemType.HealthPotion:
                    healthPotions += 1;
                    break;
                case Item.ItemType.Key:
                    keys += 1;
                    break;
                default:
                    break;
            }
        }
        public void UseItem(Item item)
        {
            switch (item.itemType)
            {
                case Item.ItemType.HealthPotion:
                    player.currentHealth += healthPotionValue;
                    healthPotions -= 1;
                    break;
                case Item.ItemType.Key:
                    player.UseKey();
                    keys -= 1;
                    break;
                default:
                    Debug.Log("can't use this item");
                    break;
            }
        }
    }

    [System.Serializable]
    public class Item
    {
        public ItemType itemType;
        public int itemLevel;

        public Item(ItemType itemType, int itemLevel)
        {
            this.itemType = itemType;
            this.itemLevel = itemLevel;
        }

        public enum ItemType
        {
            Sword,
            Shield,
            Armor,
            HealthPotion,
            Key
        }
    }
    #endregion


}
