using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    #region Shop
    [Header("Shop Vars")]
    private Player player;
    public GameObject shopItemsParent;
    public GameObject selectedItem;
    public List<GameObject> shopItemsAll = new List<GameObject>();
    private bool shopStarted = false;

    public bool shopping = false;
    private bool shoppingStarted = false;

    public int selectedItemCoordinates;

    public GameObject playerShoppingPos;

    [Header("Input")]
    public Vector2 controllInput;
    public float controllDeadspot = 0.1f;


    private void ShopSetup()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        LOBEvents.current.onEscaping += ShopEnd;
        LOBEvents.current.onInteracting += BuySelectedItem;

        for (int i = 0; i < shopItemsParent.transform.childCount; i++)
        {
            shopItemsAll.Add( shopItemsParent.transform.GetChild(i).gameObject);
        }
        // temporary
        selectedItem = shopItemsAll[0];
    }

    public void ShopStart()
    {
        shopping = true;

        player.agent.SetDestination(playerShoppingPos.transform.position);

        UIMachine.StateChanged( UIMachine.StateUi.Shop);
        shopStarted = true;
    }
    public void ShopEnd()
    {
        if (shoppingStarted)
        {
            shopping = false;
            shoppingStarted = false;
            foreach (GameObject go in shopItemsAll)
            {
                go.transform.GetChild(0).gameObject.SetActive(false);
                selectedItemCoordinates = 0;
            }

            player.currentState = Player.PlayerState.Walking;
            UIMachine.StateChanged(UIMachine.StateUi.Game);
        }
    }

    private void BuySelectedItem()
    {
        if (shopping)
        {
            var itemToBuy = selectedItem.GetComponent<ShopItem>();
            if (shoppingStarted)
            {
                if (itemToBuy.buy())
                {
                    UIMachine.flashItemBought();
                    Debug.Log("Item Bought");
                }
                else
                {
                    UIMachine.flashLackOfMoney();
                    Debug.Log("Item not Bought");
                }
            }
            shoppingStarted = true;
        }
    }

    private bool lastInputAccepted = false;
    private bool ShopUpdate()
    {
        #region selectionControlls
        controllInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Mathf.Abs( controllInput.x) > controllDeadspot && !lastInputAccepted)
        {
            // Input abs addieren / subtrahieren
            if(controllInput.x > 0f)
            {
                selectedItemCoordinates += 1;
            }
            else
            {
                selectedItemCoordinates -= 1;
            }

            var length = shopItemsAll.Count-1;
            // wrap around 
            if(selectedItemCoordinates > length)
            {
                selectedItemCoordinates = 0;
            }
            if(selectedItemCoordinates < 0f)
            {
                selectedItemCoordinates = length;
            }

            lastInputAccepted = true;
            Controllchanged();
        }
        // nicht angenommen
        if(Mathf.Abs(controllInput.x) < controllDeadspot)
        {
            lastInputAccepted = false;
        }
        return true;
        #endregion

        
    }

    private void Controllchanged()
    {
        selectedItem.transform.GetChild(0).gameObject.SetActive(false);
        selectedItem = shopItemsAll[selectedItemCoordinates];
        selectedItem.transform.GetChild(0).gameObject.SetActive(true);
    }
    #endregion

    #region Highligting Objects
    [Header("Highlighted Objs")]

    public Color fadingColor = Color.blue;

    List<ColoredObject> coloredObjects = new List<ColoredObject>();
    private void ObjHighlightingSetup()
    {
        foreach(GameObject go in shopItemsAll)
        {
            var coloObj = new ColoredObject(go);
            coloredObjects.Add(coloObj);
            Material[] objMats = go.GetComponent<Renderer>().materials;
            foreach (Material m in objMats)
            {
                coloObj.colors.Add(m.color);
            }
        }
    }

    private class ColoredObject
    {
        public ColoredObject(GameObject go)
        {
            this.go = go;
        }
        public GameObject go;
        public List<Color> colors = new List<Color>();
    }

    private void ObjHighlitingUpdate()
    {
        foreach(GameObject go in shopItemsAll)
        {
            Material[] objMats = selectedItem.GetComponent<Renderer>().materials;
            for (int i = 0; i < objMats.Length; i++)
            {
                Material m = objMats[i];
                var colObj = coloredObjects.Find(x => x.go == go);
                m.color = Color.Lerp( m.color, colObj.colors[i] + 0.5f * fadingColor, 0.01f);
            }
        }

    }
    #endregion

    private void Start()
    {
        ShopSetup();
        
    }
    private void Update()
    {
        if (shopping)
        {
            ShopUpdate();
            if (shopStarted)
        {
            player.pauseDelay = player.pauseDelayMax;
        }
            //ObjHighlitingUpdate();
        }

    }

}
