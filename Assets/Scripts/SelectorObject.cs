using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Chance", menuName = "ScriptableObjects/Obstacle Chance", order = 1)]
public class SelectorObject : ScriptableObject
{
    public int numerator, denominator;
    public GameObject myObject;
}
