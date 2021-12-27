using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Inventory Settings
    [Header("Inventory settings")]
    [SerializeField]
    int inventorySize = 10;
    public static int maxSlotQuantity = 99;
    [HideInInspector]
    public List<InventorySlot> inv = new List<InventorySlot>();
    public ItemManager mainItemManager;
    [SerializeField]
    [Tooltip("What should be done with dropped items")]
    DropMode dropMethod;
    [SerializeField]
    Vector3 dropOffset;
    [SerializeField]
    float dropForce = 5f;

    //xxxNFSxxx NFS = Not Fully Stacked
    #endregion

    #region Inventory Manipulation
    public void AddItem(Item item, int value = 1){
        int toAdd = value; //we rarely have a perfectly stacked items so we keep value of that item left to add to put it into multiple slots. 
        if(ExistsNFSSlot(item)){
            int slotIndex = GetNFSSlotIndex(item); //grab reference for faster execution
            int slotq = inv[GetNFSSlotIndex(item )].slotQuantity; //grab reference for faster execution
            if((maxSlotQuantity - slotq) < toAdd){ //if we have enough value of that item to fully stack that slot
                toAdd -= (maxSlotQuantity - slotq);
                inv[slotIndex].slotQuantity = maxSlotQuantity; 
            }else{ //if we don't have enough then just add the items and the adding is complete.
                inv[slotIndex].slotQuantity += toAdd;
                toAdd = 0;
            }
        }else{//if there isn't a single not fully stacked slot, then create one if there's space.
             if(inv.Count < inventorySize) //if there's space left.
            {
                if(toAdd >= maxSlotQuantity){ //if you can make a fully stacked slot from the beginning
                    InventorySlot temp = new InventorySlot(Instantiate(item),maxSlotQuantity);
                    inv.Add(temp);
                    toAdd -=  maxSlotQuantity;
                }else if(toAdd < maxSlotQuantity){ //if the created slot won't be full
                    InventorySlot temp = new InventorySlot(Instantiate(item),toAdd);
                    inv.Add(temp);
                    toAdd = 0;
                }
            }else{
                Debug.LogWarning("No space to add"); //Implement dropping items from eq when overfilled
            }
        }
        if(toAdd > 0 && inv.Count < inventorySize) //if there still is something left to add, just run the AddItem again.
            AddItem(item,toAdd);
    }

    public void RemoveItem(Item item, int value = 1){
        int toRemove = value;
         if(inv.Count > 0 && GetQuantity(item) >= toRemove && toRemove >  0){ //check if we the player has enough items to fullfill the remove command.
            if(ExistsNFSSlot(item)){
                int slotIndex = GetNFSSlotIndex(item);
                int slotq = inv[slotIndex].slotQuantity;
                if(slotq > toRemove){ //slot is more than enough
                    inv[slotIndex].slotQuantity -= toRemove;
                    toRemove = 0;
                }
                if(slotq == toRemove){ //slot is just enough
                    toRemove = 0;
                    inv.RemoveAt(slotIndex);
                }
                if(slotq < toRemove){ //slot is not enough
                    toRemove -= slotq;
                    inv.RemoveAt(slotIndex);
                }

            }else{ //if there doesn't exist a not full stacked slot
                int slotIndex = GetLastStackIndex(item);
                int slotq = inv[slotIndex].slotQuantity;
                if(slotq > toRemove){ //slot is more than enough
                    inv[slotIndex].slotQuantity-= toRemove;
                    toRemove = 0;
                }
                if(slotq == toRemove){ //slot is just enough
                    toRemove = 0;
                    inv.RemoveAt(slotIndex);
                }
                if(slotq < toRemove){ //slot is not enough
                    toRemove -= slotq;
                    inv.RemoveAt(slotIndex);
                }
            }
            if(toRemove > 0){
             RemoveItem(item,toRemove); 
            }
         }else{
             Debug.Log("Not enough Items.");
         }
    }

    public void DropItem(int index){
        switch(dropMethod){
            case DropMode.Destroy:
                inv.RemoveAt(index);
            break;
            case DropMode.Drop:
                GameObject droppedItem = Instantiate(mainItemManager.registeredItems.Find(x => x.itemID == inv[index].slotItem.itemID).itemDropPrefab,transform.position + dropOffset,Quaternion.identity);
                Pickupable droppedItemScript = droppedItem.GetComponent<Pickupable>();
                droppedItemScript.heldItem = inv[index].slotItem;
                droppedItemScript.heldItemValue = inv[index].slotQuantity;
                droppedItem.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce);
                droppedItem = null;
                droppedItemScript = null;
                inv.RemoveAt(index);
            break;
        }
    }
    public int GetQuantity(Item item){
        int allQ = 0; //quantity of this item in inventory.
        if(inv.Count > 0){
            foreach(InventorySlot x in inv){
                if(x.slotItem.itemID == item.itemID){
                    allQ += x.slotQuantity;
                }
            }
            return allQ;
        }else return 0;
    }
    int GetNFSSlotIndex(Item item){
        for(int i=0;i < inv.Count;i++){
            if(inv[i].slotItem.itemID == item.itemID && !inv[i].isMaxStacked){
                return i;
            }
        }
        return -1;
    }
    bool ExistsNFSSlot(Item item){
        if(inv.Exists(x => x.slotItem.itemID == item.itemID && x.isMaxStacked == false)){
            return true;
        }else return false;
    }

    int GetLastStackIndex(Item item){
        for(int i=0;i< inv.Count;i++){
            if(inv[i].slotItem.itemID == item.itemID && inv[i].isMaxStacked){
                return i;
            }
        }
        return -1;
    } 
    #endregion
        
}

public enum DropMode{
    Destroy,
    Drop
}

[System.Serializable]
public class InventorySlot{

    int m_slotQuantity;
    private int maxQuantity = Inventory.maxSlotQuantity;
    public Item slotItem {get;set;}
    public int slotQuantity 
    {
        get{
            return m_slotQuantity;
        }
        set{
            if((value) == maxQuantity){
                m_slotQuantity = value;
                isMaxStacked = true;
            }else{
                m_slotQuantity= value;
                isMaxStacked = false;
            }
        }
    }
    public bool isMaxStacked {get;set;}

    public InventorySlot(Item _item, int _value){
        slotItem = _item;
        slotQuantity = _value;
    }
}
