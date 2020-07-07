using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIMachine
{
    // Alle Elemente
    public static List<GameObject> all;

    // Game
    public static List<GameObject> GameUi;

    //Quiz UI
    public static List<GameObject> QuizUi;
    public static List<GameObject> WrongUi;
    public static List<GameObject> RightUi;

    //Shop UI
    public static List<GameObject> ShopHintUi;
    public static List<GameObject> LackOfMoneyUi;
    public static List<GameObject> ItemBoughtUi;

    //Library UI
    public static List<GameObject> LibraryCategoryUi;
    public static List<GameObject> LibraryQuestionUi;


    public static StateUi currentState = StateUi.Game;

    public static void StateChanged(UIMachine.StateUi state)
    {
        Debug.Log("State Changed : " + state);
        foreach (GameObject obj in all) // Alle Ui Elemente werden ausgeblendet
        {
            obj.SetActive(false);
        }

        switch (state) // Die richtigen Elemente werden wieder eingeblendet
        {
            case (UIMachine.StateUi.Game):
                foreach (GameObject obj in GameUi)
                {
                    obj.SetActive(true);
                }
                break;

            case (UIMachine.StateUi.Quiz):
                foreach (GameObject obj in QuizUi)
                {
                    obj.SetActive(true);
                }
                break;

            case (UIMachine.StateUi.Shop):
                foreach (GameObject obj in ShopHintUi)
                {
                    obj.SetActive(true);
                }
                break;
            case (UIMachine.StateUi.LibraryCategory):
                foreach (GameObject obj in LibraryCategoryUi)
                {
                    obj.SetActive(true);
                }
                break;
            case (UIMachine.StateUi.LibraryQuestion):
                foreach (GameObject obj in LibraryQuestionUi)
                {
                    obj.SetActive(true);
                }
                break;

        }
        UIMachine.currentState = state;
    }

    public static IEnumerator flashWrong()
    {
        foreach (GameObject obj in WrongUi)
        {
            obj.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        foreach (GameObject obj in WrongUi)
        {
            obj.SetActive(false);
        }
        LOBEvents.current.QuestionAnswered();
    }

    public static IEnumerator flashRight()
    {
        foreach (GameObject obj in RightUi)
        {
            obj.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        foreach (GameObject obj in RightUi)
        {
            obj.SetActive(false);
        }
        LOBEvents.current.QuestionAnswered();
    }

    public static IEnumerator flashLackOfMoney()
    {
        foreach (GameObject obj in LackOfMoneyUi)
        {
            obj.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        foreach (GameObject obj in LackOfMoneyUi)
        {
            obj.SetActive(false);
        }
        LOBEvents.current.LackOfMoney();
    }

    public static IEnumerator flashItemBought()
    {
        foreach (GameObject obj in ItemBoughtUi)
        {
            obj.SetActive(true);
        }

        yield return new WaitForSeconds(2);

        foreach (GameObject obj in ItemBoughtUi)
        {
            obj.SetActive(false);
        }
        LOBEvents.current.ItemBought();
    }

    public enum StateUi
    {
        Quiz,
        Shop,
        Game,
        LibraryCategory,
        LibraryQuestion
    }
}
