using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] private Direction direction = Direction.Vertical;
    [SerializeField] private RectTransform statBar;
    [SerializeField] private RectTransform statBarFill;
    [SerializeField] StatSystem statSystem;

    private enum Direction
    {
        Vertical,
        Horizontal
    }

    // Start is called before the first frame update
    void Awake()
    {
        statSystem.OnValueChanged += UpdateDisplay;
    }

    void UpdateDisplay(float value, float maxValue)
    {
        switch (direction)
        {
            case Direction.Vertical:
                statBarFill.sizeDelta = new Vector2(statBar.rect.width, statBar.rect.height * (value / maxValue));
                break;
            case Direction.Horizontal:
                statBarFill.sizeDelta = new Vector2(statBar.rect.width * (value / maxValue), statBar.rect.height);
                break;
            default:
                statBarFill.sizeDelta = new Vector2(statBar.rect.width * (value / maxValue), statBar.rect.height);
                break;
        }
    }
}
