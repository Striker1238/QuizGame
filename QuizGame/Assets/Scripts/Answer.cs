using UnityEngine;
public class Answer : MonoBehaviour
{
    [HideInInspector]
    public int thisAnswerIndex;
    public void OnPointerClick() => FindObjectOfType<GameController>().SetMyAnswer(this);
}
