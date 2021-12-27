using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemManager : ScriptableObject
{
    public List<Item> registeredItems = new List<Item>();

    [ContextMenu("Assign Item IDs")]
    void AssignIDs(){
        for(int i=0;i<registeredItems.Count;i++){
            registeredItems[i].itemID = i;
        }
    }
}
