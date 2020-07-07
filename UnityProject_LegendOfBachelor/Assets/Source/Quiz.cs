using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Linq;

public class Quiz : MonoBehaviour
{
    public Monster currentAtackingMonster;
    [Header("learning Vars")]
    public bool learning = false;
    public QuestionType learningQuestionType = QuestionType.Allgemein;

    [Header("UI Objects")]
    public GameObject uiQuestion;
    public List<GameObject> uiAnswers = new List<GameObject>();

    private Question currentQuestion;

    public static Quiz current;
    private void Start()
    {
        // Subscribtions
        LOBEvents.current.onQuizToggle += Toggled;
        LOBEvents.current.onFightStarted += NextQuestion;
        LOBEvents.current.onFightEnded += EndQuiz;
        LOBEvents.current.onQuestionAnswered += QuestionAnswered;
        setup();
    }

    #region Quiz
    // vars
    private Text questionText;
    private List<Text> answersTexts = new List<Text>();

    public void NextQuestion()
    {
        QuestionType questionType = QuestionType.Allgemein;
        if (currentAtackingMonster != null)
        {
            questionType = currentAtackingMonster.questionTypes[Mathf.CeilToInt( Random.Range(0, currentAtackingMonster.questionTypes.Count))];
        }
        else
        {
            questionType = QuestionType.Allgemein;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentState == Player.PlayerState.Fighting)
            AskQuestion(questions.GetRandomQuestion(questionType)); // mit richtiger liste abhängig von monster fragentyp aufrufen
    }

    public void NextQuestion(Monster monster)
    {
        currentAtackingMonster = monster;
        var questionType = monster.questionTypes[Mathf.CeilToInt(Random.Range(0, monster.questionTypes.Count))];

        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentState == Player.PlayerState.Fighting)
            AskQuestion(questions.GetRandomQuestion(questionType)); //TODO: mit richtiger liste abhängig von monster fragentyp aufrufen
    }

    public void NextLearningQuestion(QuestionType questionType)
    {
        learning = true;
        learningQuestionType = questionType;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentState == Player.PlayerState.Learning)
            AskQuestion(questions.GetRandomQuestion(questionType));
    }

    private void AskValidQuestion(QuestionType questionType, bool learningQuestion)
    {
        if (learningQuestion)
        {
            Question q = questions.GetRandomQuestionUsingPoints(questionType);
            if(q != null)
            {
                AskQuestion(q);
            }else
            {
                // TODO :: behaviour for 
            }
        }
        else
        {

        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="q"></param>
    public void AskQuestion (Question q) 
    {
        questionText.text = q.question;
        currentQuestion = q;

        var slots = new List<Text>(answersTexts);
        var answers = new List<string>(q.answers);

        var ansRslot = Mathf.FloorToInt(Random.value * slots.Count);
        slots[ansRslot].text = q.answers[0];
        slots.Remove(slots[ansRslot]);

        LobUtilities.Shuffle(answers);

        int j = 0;
        foreach(string ans in answers)
        {
            answersTexts[j].text = ans;
            j++;
        }
        if (!learning)
        {
            UIMachine.StateChanged(UIMachine.StateUi.Quiz);
        }
        else
        {
            UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
        }

    }

    /// <summary>
    /// deprecated use AskQuestion(Question q) instead
    /// </summary>
    /// <param name="number"></param>
    public void AskQuestion(int number)
    {
        // find question by number
        Question q = questions.fragenAllgemein.Find(x => x.number == number);
        questionText.text = q.question;
        currentQuestion = q;

        var slots = new List<Text>(answersTexts);
        var answers = new List<string>(q.answers);

        var ansRslot = Mathf.FloorToInt(Random.value * slots.Count);
        slots[ansRslot].text = q.answers[0];
        slots.Remove(slots[ansRslot]);

        LobUtilities.Shuffle(answers);

        int j = 0;
        foreach (string ans in answers)
        {
            answersTexts[j].text = ans;
            j++;
        }

        if (learning)
        {
            UIMachine.StateChanged(UIMachine.StateUi.LibraryQuestion);
        }
        else
        {
            UIMachine.StateChanged(UIMachine.StateUi.Quiz);
        }
    }

    public void EndQuiz()
    {
        UIMachine.StateChanged(UIMachine.StateUi.Game);
        if (learning)
        {
            learning = false;
        }
    }

    public void QuestionAnswered()
    {
        if (!learning)
        {
            NextQuestion();
        }
        else
        {
            NextLearningQuestion(learningQuestionType);
        }
    }

    public void Toggled(GameObject toggler)
    {
        if (learning)
        {
            if (toggler.GetComponentInChildren<Text>().text == currentQuestion.answers[0])
            {
                Debug.Log("Antwort richtig");
                StartCoroutine(UIMachine.flashRight());
                currentQuestion.questionPoints--;
            }
            else
            {
                Debug.Log("Antwort falsch");
                StartCoroutine(UIMachine.flashWrong());
                currentQuestion.questionPoints++;
            }
        }
        else
        {
            if (toggler.GetComponentInChildren<Text>().text == currentQuestion.answers[0])
            {
                Debug.Log("Antwort richtig");
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DealDamage(); // TODO: unify finding
                StartCoroutine(UIMachine.flashRight());
            }
            else
            {
                Debug.Log("Antwort falsch");
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage();
                StartCoroutine(UIMachine.flashWrong());
            }
        }
    }
    #endregion

    public enum QuestionType
    {
        Allgemein,
        Scrum,
        VModell,
        WModell
    }

    #region setup and Reading 

    [Header("fragen leser")]
    public TextAsset fragen;
    public XDocument xDocFragen = new XDocument(new XElement("root"));
    public Questions questions = new Questions();

    private List<Toggle> toggles = new List<Toggle>();
    private void setup()
    {
        current = this;
        // vars setup
        questionText = uiQuestion.GetComponent<Text>();
        foreach (GameObject uia in uiAnswers)
        {
            answersTexts.Add(uia.GetComponentInChildren<Text>());
            toggles.Add(uia.GetComponentInChildren<Toggle>());
        }

        // load Questions
        List<string> fileNames = new List<string> {
            "Fragen on Note Wasserfall-Model",
            "Fragen on Note V-Model",
            "Fragen on Note Scrum",
            "Fragen on Note Allgemein"
        };

        XDocument xDocumentAllgemein = XDocument.Parse(Resources.Load<TextAsset>("Fragen on Note Allgemein").text);
        XDocument xDocumentScrum = XDocument.Parse(Resources.Load<TextAsset>("Fragen on Note Scrum").text);
        XDocument xDocumentVModell = XDocument.Parse(Resources.Load<TextAsset>("Fragen on Note V-Model").text);
        XDocument xDocumentWModell = XDocument.Parse(Resources.Load<TextAsset>("Fragen on Note Wasserfall-Model").text);

        questions.fragenAllgemein = Read("FragenAllgemein", xDocumentAllgemein);
        questions.fragenScrum = Read("FragenScrum", xDocumentScrum);
        questions.fragenVModell = Read("FragenV-Modell", xDocumentVModell);
        questions.fragenWModell = Read("FragenW-Modell", xDocumentWModell);
    }

    public List<Question> Read( string questionsType, XDocument xDocument)
    {
        List<Question> questionsList = new List<Question>();

        var qus = xDocument.Element(questionsType).Elements("Frage");
        foreach(var qu in qus)
        {
            List<string> answers = new List<string>();
            answers.Add(qu.Element("AntwortR").Value);

            var ansF = qu.Elements("AntwortF");
            foreach(var ans in ansF)
            {
                answers.Add(ans.Value);
            }
            
            questionsList.Add(new Question(int.Parse( qu.Element("number").Value), qu.Element("_Frage").Value, answers));
        }
        return questionsList;
    }
    #endregion
}
[System.Serializable]
public class Question
{
    public int number;
    public string question;
    public List<string> answers;
    public int questionPoints = 2;

    public Question(int number, string question, List<string> answers)
    {
        this.number = number;
        this.question = question;
        this.answers = answers;
    }
}

public class Questions
{
    public List<Question> fragenAllgemein;
    public List<Question> fragenScrum;
    public List<Question> fragenVModell;
    public List<Question> fragenWModell;

    public Question GetQuestion (Quiz.QuestionType questionType, int number)
    {
        switch (questionType)
        {
            case Quiz.QuestionType.Allgemein:
                return fragenAllgemein[number];
            case Quiz.QuestionType.Scrum:
                return fragenScrum[number];
            case Quiz.QuestionType.VModell:
                return fragenVModell[number];
            case Quiz.QuestionType.WModell:
                return fragenWModell[number];
            default:
                return fragenAllgemein[number];
        }
    }
    public Question GetRandomQuestion(Quiz.QuestionType questionType)
    {
        switch (questionType)
        {
            case Quiz.QuestionType.Allgemein:
                return fragenAllgemein[(int)Random.Range(0, (float)fragenAllgemein.Count)];
            case Quiz.QuestionType.Scrum:
                return fragenScrum[(int)Random.Range(0, (float)fragenScrum.Count )];
            case Quiz.QuestionType.VModell:
                return fragenVModell[(int)Random.Range(0, (float)fragenVModell.Count)];
            case Quiz.QuestionType.WModell:
                return fragenWModell[(int)Random.Range(0, (float)fragenWModell.Count)];
            default:
                return fragenAllgemein[(int)Random.Range(0, (float)fragenAllgemein.Count)];
        }
    }

    public Question GetRandomQuestionUsingPoints(Quiz.QuestionType questionType)
    {
        int rand = 0;

        switch (questionType)
        {
            case Quiz.QuestionType.Allgemein:
                rand = (int)Random.Range(1, (float)fragenAllgemein.Count + 1);
                return fragenAllgemein.Find(x => x.number == rand && x.questionPoints >= 0);

            case Quiz.QuestionType.Scrum:
                rand = (int)Random.Range(1, (float)fragenScrum.Count + 1);
                return fragenScrum.Find(x => x.number == rand && x.questionPoints >= 0);

            case Quiz.QuestionType.VModell:
                rand = (int)Random.Range(1, (float)fragenVModell.Count + 1);
                return fragenVModell.Find(x => x.number == rand && x.questionPoints >= 0);

            case Quiz.QuestionType.WModell:
                rand = (int)Random.Range(1, (float)fragenWModell.Count + 1);
                return fragenWModell.Find(x => x.number == rand && x.questionPoints >= 0);

            default:
                rand = (int)Random.Range(1, (float)fragenAllgemein.Count + 1);
                return fragenAllgemein.Find(x => x.number == rand && x.questionPoints >= 0);
        }
    }
}
