using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Questions", menuName = "ScriptableObjects/Questions", order = 2)]
public class Questions : ScriptableObject
{
    public SimpleQuestion[] questions;
}

[Serializable]
public class SimpleQuestion
{
    public Sprite questionAnswerSprite;
    public bool isRight;
}