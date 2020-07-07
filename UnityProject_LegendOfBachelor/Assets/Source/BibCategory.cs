using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BibCategory : MonoBehaviour
{
    public GameObject Category1;
    public GameObject Category2;
    public GameObject Category3;
    public GameObject Category4;

    public int choiceMade;

    private void Update()
    {

    }

    public void ChoiceOption1()
    {
        choiceMade = 1;
        Debug.Log("Button 1 geklickt");
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }

    public void ChoiceOption2()
    {
        choiceMade = 2;
        Debug.Log("Button 2 geklickt");
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }

    public void ChoiceOption3()
    {
        choiceMade = 3;
        Debug.Log("Button 3 geklickt");
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }

    public void ChoiceOption4()
    {
        choiceMade = 4;
        Debug.Log("Button 4 geklickt");
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }
}