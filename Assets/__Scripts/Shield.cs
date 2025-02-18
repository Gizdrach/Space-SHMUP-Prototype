using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPesSecond = 0.1f;

    [Header("Set in Dynamically")]
    public int levelShown = 0;

    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;   
    }

    void Update()
    {
        int currLevel = Mathf.FloorToInt(Hero.S.shildeLevel);
        if (levelShown != currLevel) 
        {
            levelShown = currLevel;
            mat.mainTextureOffset = new Vector2(0.2f*levelShown, 0);
        }

        float rZ = -(rotationsPesSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
