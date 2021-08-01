using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    public delegate void StatCall(float value, float maxValue);
    public event StatCall OnValueChanged = delegate { };
    public event StatCall OnValueEmpty = delegate { };
    public event StatCall OnValueFull = delegate { };
    public event StatCall OnValueNoLongerEmpty = delegate { };
    public event StatCall OnValueNoLongerFull = delegate { };

    private float statValue;
    [SerializeField] private float maxStatValue;
    private bool empty = false;
    private bool full = false;

    private void Start()
    {
        StatValue = MaxStatValue;
    }

    public float StatValue
    {
        get => statValue;
        set
        {
            statValue = Mathf.Clamp(value, 0, maxStatValue);
            OnValueChanged(statValue, maxStatValue);
            if (statValue <= 0 && !empty)
            {
                OnValueEmpty(statValue, maxStatValue);
                empty = true;
            }
            else if (statValue >= maxStatValue && !full)
            {
                OnValueFull(statValue, maxStatValue);
                full = true;
            }
            else if (statValue > 0 && empty)
            {
                OnValueNoLongerEmpty(statValue, maxStatValue);
                empty = false;
            }
            else if (statValue < maxStatValue && full)
            {
                OnValueNoLongerFull(statValue, maxStatValue);
                full = false;
            }
        }
    }

    public float MaxStatValue
    {
        get => maxStatValue;
        set => maxStatValue = value;
    }
}
