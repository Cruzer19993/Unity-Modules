using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Pickupable : MonoBehaviour
{
    //class used for storing pickupable item on a gameobject
    public Item heldItem;
    public int heldItemValue;
}
