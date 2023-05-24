using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ParallaxGroup : MonoBehaviour
{
    public float relativeSpeed;
    public Sprite groupSprite;
    public Transform targetTransform;
    public bool debugInfo;
    float startXpos;
    [SerializeField]
    float length;

    public void Setup(int _zOrder, float yOffset)
    {
        if(targetTransform == null) targetTransform = Camera.main.transform;//grab target transform if null
        //spawn parallaxObjects;
        for(int i=0;i < 3; i++)
        {
            GameObject parallaxImage = new GameObject("Parallax Image", typeof(SpriteRenderer));
            parallaxImage.transform.SetParent(transform);
            parallaxImage.GetComponent<SpriteRenderer>().sprite = groupSprite;
            parallaxImage.GetComponent<SpriteRenderer>().sortingOrder = _zOrder;
            parallaxImage.transform.localScale = new Vector3(18, 10.2f, 1f);
        }
        length = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        transform.GetChild(0).transform.position = new Vector3(startXpos - length, yOffset, 0f);
        transform.GetChild(1).transform.position = new Vector3(0f, yOffset, 0f);
        transform.GetChild(2).transform.position = new Vector3(startXpos + length, yOffset, 0f);
        startXpos = transform.position.x;
    }

    private void Update()
    {
        float diviation = (targetTransform.position.x * (1 - relativeSpeed));
        float moveDistance = (targetTransform.position.x * relativeSpeed);

        transform.position = new Vector3(startXpos + moveDistance, transform.position.y, transform.position.z);

        if (debugInfo) Debug.Log("diviation: " + diviation);

        if (diviation > startXpos + length) startXpos += length;
        else if (diviation < startXpos - length) startXpos -= length;
    }

}