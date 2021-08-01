using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomItem
{
    public Item item;

    [SerializeField] private AnimationCurve amount= new AnimationCurve(new Keyframe[] { new Keyframe(0,0), new Keyframe(100, 1)});

    public int GetAmount(float change)
    {
        return Mathf.RoundToInt(amount.Evaluate(change));
    }
}
