using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxManager : MonoBehaviour
{
    public List<ParallaxGroupSetupConfiguration> groups = new();
    public float yOffset;
    public bool updateOffset;

    private void Start()
    {
        for (int i=0;i<groups.Count;i++)
        {
            GameObject tempGroup = new GameObject("Parallax Group " + i,typeof(ParallaxGroup));
            tempGroup.transform.SetParent(transform);
            tempGroup.transform.position = new Vector3(0f,yOffset,0f);
            ParallaxGroup temp = tempGroup.GetComponent<ParallaxGroup>();
            temp.relativeSpeed = -groups[i].relativeSpeed;
            temp.groupSprite= groups[i].groupSprite;
            temp.Setup(i,yOffset);
            tempGroup = null;
        }
    }

    private void Update()
    {
        if(updateOffset)
        transform.position = transform.GetComponentInParent<Transform>().position + new Vector3(0f,yOffset,0f);
    }
}

[System.Serializable]
public struct ParallaxGroupSetupConfiguration
{
    public Sprite groupSprite;
    public float relativeSpeed;
}