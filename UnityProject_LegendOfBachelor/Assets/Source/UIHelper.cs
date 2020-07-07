using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIHelper : MonoBehaviour
{
    [Header("States")]

    public List<GameObject> UiQuiz;
    public List<GameObject> UiGame;

    public List<GameObject> UiShopHint;

    public List<GameObject> UiLibraryCategory;
    public List<GameObject> UiLibraryQuestion;

    [Header("Flash Screens")]

    public List<GameObject> UiLackOfMoney;
    public List<GameObject> UiItemBought;
    public List<GameObject> UiRight;
    public List<GameObject> UiWrong;

    private void Awake()
    {
        UIMachine.GameUi = UiGame;

        UIMachine.QuizUi = UiQuiz;
        UIMachine.RightUi = UiRight;
        UIMachine.WrongUi = UiWrong;

        UIMachine.ShopHintUi = UiShopHint;
        UIMachine.LackOfMoneyUi = UiLackOfMoney;
        UIMachine.ItemBoughtUi = UiItemBought;

        UIMachine.LibraryCategoryUi = UiLibraryCategory;
        UIMachine.LibraryQuestionUi = UiLibraryQuestion;

        UIMachine.all = UiGame.Concat(UiQuiz).Concat(UiRight).Concat(UiWrong).Concat(UiShopHint).Concat(UiLackOfMoney).Concat(UiItemBought).Concat(UiLibraryCategory).Concat(UiLibraryQuestion).ToList();
    }
}
