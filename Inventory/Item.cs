using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Generic Item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemID;
    public Sprite itemIcon;
    public GameObject itemDropPrefab;

}
