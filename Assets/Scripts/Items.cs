using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Squares/Item", order = 100)]
public class Items : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int id;
}
