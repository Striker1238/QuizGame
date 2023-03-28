using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintZone : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 newPos;
    public void RemoveZone()
    {
        FindObjectOfType<GameController>().paintZone.Remove(gameObject);
        Destroy(gameObject);
    }

    public void OnPointerDown()
    {
        startPos = transform.position;
    }
    public void OnPointerUp()
    {
        newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (newPos.x >= startPos.x + 1) FindObjectOfType<GameController>().StartQuestions();
    }
}
