using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Interactions : MonoBehaviour
{
    
    [SerializeField]
    float interactionRange;
    [SerializeField]
    KeyCode interactKey = KeyCode.E;
    int layermask = 1 << 7;
    Inventory eq;
    Camera cam; //main camera that shoots a ray from the cursor
    // Update is called once per frame

    void Start(){
        eq = GetComponent<Inventory>();
        cam = Camera.main;
    }
    void Update()
    {
        RaycastHit PickupableHit;
        Ray _ray = cam.ScreenPointToRay(Input.mousePosition); //since the cursor is locked to the center we can use that to shoot a ray from the center
        Pickupable item;
        if(Physics.Raycast(_ray,out PickupableHit,interactionRange,layermask)){
            item = PickupableHit.collider.gameObject.GetComponent<Pickupable>();
        }else{
            item = null;
        }
        if(Input.GetKeyDown(interactKey)){
            if(item != null){
                PickupItem(item);
            }
        }

        #if UNITY_EDITOR
            Debug.DrawRay(cam.transform.position,cam.transform.forward * interactionRange,Color.green,Time.deltaTime);
        #endif
    }

    void PickupItem(Pickupable item){
        eq.AddItem(item.heldItem,item.heldItemValue);
    }
}
