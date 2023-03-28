using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct typeColorButton
    {
        public Color SelectButton;
        public Color DefaultButton;
        public Color CorrectButton;
        public Color IncorrectButton;
    }


    [Header("Other")]
    public List<Question> allQuestions = new List<Question>();
    public List<GameObject> pageObj = new List<GameObject>();
    private int IndexPage;
    public int indexPage
    {
        get => IndexPage;
        set
        {
            IndexPage = value;
            for (int i = 0; i < pageObj.Count; i++)
                pageObj[i].SetActive((i == IndexPage) ? true : false);
        }
    }

    [Header("Page 1: SelectHero")]
    public float changeOfSizeTime;
    public Button startGameButton;
    public GameObject[] heroPref = new GameObject[2];


    [Header("Page 2: Information")]
    public GameObject panelWithHero;
    public TextMeshProUGUI NameHero;
    public TextMeshProUGUI Information;
    public TextMeshProUGUI Description;
    public GameObject[] heroObjInPanelDescription = new GameObject[2];
    private SelectHero selectHeroObj;


    [Header("Page 3: Questions")]
    public Transform answerButtonPerent;
    public GameObject answerButtonPref;
    public typeColorButton answerButtonColor;
    public List<GameObject> answerPanel = new List<GameObject>();
    public List<GameObject> paintZone = new List<GameObject>();

    private int IndexQuestion;
    public int indexQuestion
    {
        get => IndexQuestion;
        set
        {
            IndexQuestion = value;
            if(selectHeroObj.numberHero == 2 && IndexQuestion < allQuestions.Count || selectHeroObj.numberHero == 1 && IndexQuestion < 5)
            {
                for (int i = 0; i < answerPanel.Count; i++) answerPanel[i].SetActive((i == 0) ? true : false);
                CreateAnswerButton();
            }
            else
            {
                for (int i = 0; i < answerPanel.Count; i++) answerPanel[i].SetActive((i == 3) ? true : false);
            }
        }
    }
    private List<GameObject> allAnswerButton = new List<GameObject>();

    public void OnPointerClick(SelectHero hero)
    {
        if (selectHeroObj != null && selectHeroObj != hero) ChangeOfSize(selectHeroObj, false);
        ChangeOfSize(hero, true);

        selectHeroObj = hero;

        NameHero.text = hero.heroName;
        Information.text = hero.informationText;
        Description.text = hero.descriptionText;

        

        //“Ó ˜ÚÓ ÌËÊÂ ÔÂÂÔËÒ‡Ú¸ ÊÂÎ‡ÚÂÎ¸ÌÓ
        if (hero.numberHero == 1)
        {
            heroPref[0].SetActive(true);
            heroPref[1].SetActive(false);

            heroObjInPanelDescription[0].SetActive(true);
            heroObjInPanelDescription[1].SetActive(false);

            //Debug.Log($"{answerPanel[1].GetComponentInChildren<TextMeshProUGUI>().text.Contains("Ã¿ —»Ã”")}");
            for (int i = 1; i < answerPanel.Count; i++)
                answerPanel[i].GetComponentInChildren<TextMeshProUGUI>().text = answerPanel[i].GetComponentInChildren<TextMeshProUGUI>().text.Replace("Ã¿ —»Ã”", "¿ÕÕ≈");
        }
        else
        {
            heroPref[1].SetActive(true);
            heroPref[0].SetActive(false);

            heroObjInPanelDescription[1].SetActive(true);
            heroObjInPanelDescription[0].SetActive(false);

            //Debug.Log($"{answerPanel[1].GetComponentInChildren<TextMeshProUGUI>().text.Contains("¿ÕÕ≈")}");
            for (int i = 1; i < answerPanel.Count; i++)
                answerPanel[i].GetComponentInChildren<TextMeshProUGUI>().text = answerPanel[i].GetComponentInChildren<TextMeshProUGUI>().text.Replace("¿ÕÕ≈", "Ã¿ —»Ã”");
        }
    }
    private async Task ChangeOfSize(SelectHero hero, bool isExpand)
    {
        if (isExpand)
        {
            startGameButton.GetComponent<RectTransform>().localPosition = new Vector3(hero.GetComponent<RectTransform>().localPosition.x, hero.GetComponent<RectTransform>().localPosition.y - 327, 0);
            startGameButton.interactable = true;
        }

        Vector2 startSize = hero.GetComponent<RectTransform>().sizeDelta;
        Vector2 newSize = new Vector2((isExpand) ? 225 : 200, hero.GetComponent<RectTransform>().sizeDelta.y);
        for (float t = 0; t < changeOfSizeTime; t += Time.deltaTime)
        {
            hero.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(startSize, newSize, t / changeOfSizeTime);
            await Task.Delay(1);
        }
    }
    public void StartGame() 
    {
        indexPage++;
        panelWithHero.SetActive(true);

        var a = heroPref[selectHeroObj.numberHero-1].gameObject.GetComponentsInChildren<Image>();
        foreach (var item in a)
        {
            if (item.name == "PaintZone") paintZone.Add(item.gameObject);
        }
    }
    public void StartQuestions()
    {
        foreach (var zone in paintZone)
            Destroy(zone.GetComponent<EventTrigger>());

        indexPage++;
        indexQuestion = (selectHeroObj.numberHero == 1) ? 0 : 5;
        //CreateAnswerButton();
    }



    private void CreateAnswerButton()
    {
        answerButtonPerent.GetComponentInChildren<TextMeshProUGUI>().text = allQuestions[indexQuestion].questionText;
        for (int i = 1; i <= allQuestions[indexQuestion].countVariableAnswer; i++)
        {
            var obj = Instantiate(answerButtonPref, answerButtonPerent);
            obj.GetComponent<Answer>().thisAnswerIndex = i;// allQuestions[indexQuastion].
            obj.GetComponent<RectTransform>().localPosition = 
                new Vector2((allQuestions[indexQuestion].countVariableAnswer > 3)?
                    175 + (i % 2) * -350 : 
                        (i > 2)? 0:
                        175 + (i % 2) * -350
                , (i < 3) ? -80 : -160);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"¬¿–»¿Õ“ {i}";
            allAnswerButton.Add(obj);
        }
    }

    public void SetMyAnswer(Answer myAnswer)
    {
        var oldSelectButton = allAnswerButton.Find(x => x.GetComponent<Image>().color == answerButtonColor.SelectButton);
        if(oldSelectButton != null) oldSelectButton.GetComponent<Image>().color = answerButtonColor.DefaultButton;

        allQuestions[indexQuestion].selectAnswer = myAnswer.thisAnswerIndex;
        myAnswer.GetComponent<Image>().color = answerButtonColor.SelectButton;
    }
    public void SendAnswer() => SendAnswerTask();
    private async Task SendAnswerTask()
    {
        foreach (var button in allAnswerButton)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Image>().color = (allQuestions[indexQuestion].answer == button.GetComponent<Answer>().thisAnswerIndex) ?
                answerButtonColor.CorrectButton :
                answerButtonColor.IncorrectButton;
        }


        allAnswerButton[allQuestions[indexQuestion].selectAnswer-1].GetComponent<Animator>().SetBool("Change",true);
        await Task.Delay(2000);
        allAnswerButton[allQuestions[indexQuestion].selectAnswer - 1].GetComponent<Animator>().SetBool("Change", false);


        for (int i = 0; i < answerPanel.Count; i++) answerPanel[i].SetActive((i == (CheckOnCorrectAnswer()?1:2))?true:false);

        foreach (var button in allAnswerButton) 
            Destroy(button);
        allAnswerButton.Clear();
    }
    private bool CheckOnCorrectAnswer() => allQuestions[indexQuestion].selectAnswer == allQuestions[indexQuestion].answer;
    public void NextQuestion()
    {
        var removPaintZon = paintZone[Random.Range(0, paintZone.Count)];
        removPaintZon.GetComponent<Animator>().SetBool("destroy",true);

        indexQuestion++;
    }

    public void BackQuestion() => indexQuestion = indexQuestion;



    public void EndGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
