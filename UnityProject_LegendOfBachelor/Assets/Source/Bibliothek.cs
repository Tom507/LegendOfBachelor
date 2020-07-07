using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bibliothek : MonoBehaviour
{
    public Quiz quiz;
    public Quiz.QuestionType currentQuestionType;
    public GameObject playerLearningPos;
    private bool bibliothekStarted = false;
    private Player player;
    /// <summary>
    /// Die Bibliothek soll von einem InteractableTrigger getriggert werden können
    /// </summary>
    // Start is called before the first frame update
    public void Start()
    {
        LOBEvents.current.onEscaping += Escaping;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log(player);
        quiz = GameObject.Find("Quiz").GetComponent<Quiz>();
    }


    public void BibliothekStart()  // called by interactable Trigger
    {
        UIMachine.StateChanged(UIMachine.StateUi.LibraryCategory);
        player.currentState = Player.PlayerState.Learning;
        bibliothekStarted = true;

        player.agent.SetDestination(playerLearningPos.transform.position);
    }

    private void startLearningQuiz()
    {
        quiz.NextLearningQuestion(currentQuestionType);
    }

    public void BibliothekEnd()
    {
        if (bibliothekStarted)
        {
            player.currentState = Player.PlayerState.Walking;
            UIMachine.StateChanged(UIMachine.StateUi.Game);
            bibliothekStarted = false;
        }
    }

    private void Update()
    {
        if (bibliothekStarted)
        {
            player.pauseDelay = player.pauseDelayMax;
        }
    }

    public void Escaping()
    {
        if (UIMachine.currentState == UIMachine.StateUi.LibraryCategory)
        {
            BibliothekEnd();
        }
        if(UIMachine.currentState == UIMachine.StateUi.LibraryQuestion)
        {
            UIMachine.StateChanged(UIMachine.StateUi.LibraryCategory);
        }
    }

    public void ChoiceOptionScrum()
    {
        currentQuestionType = Quiz.QuestionType.Scrum;
        Debug.Log("Scrum geklickt");
        startLearningQuiz();
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }

    public void ChoiceOptionVModell()
    {
        currentQuestionType = Quiz.QuestionType.VModell;
        Debug.Log("VModell geklickt");
        startLearningQuiz();
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }

    public void ChoiceOptionWModell()
    {
        currentQuestionType = Quiz.QuestionType.WModell;
        Debug.Log("WModell geklickt");
        startLearningQuiz();
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }

    public void ChoiceOptionAllgemein()
    {
        currentQuestionType = Quiz.QuestionType.Allgemein;
        Debug.Log("Allgmein geklickt");
        startLearningQuiz();
        UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
    }

}
