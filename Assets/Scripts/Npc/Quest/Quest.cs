using System;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [TextArea]
    public string questDescription;

    public List<ItemSO> requiredItems;
    public List<int> requiredQuantities;

    public List<ItemSO> rewardItems;
    public List<int> rewardQuantities;
}