using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHero : MonoBehaviour
{
    public int numberHero;
    public string heroName;
    [TextArea(5, 5)]
    public string informationText;
    [TextArea(5, 5)]
    public string descriptionText;

    public void OnPointerClick() => FindObjectOfType<GameController>().OnPointerClick(this); 
}
