using UnityEngine;
[System.Serializable]
public class Question
{
    [TextArea(5,10)]
    public string questionText;
    public int countVariableAnswer;
    public int answer;
    [HideInInspector]
    public int selectAnswer = 0;
}
