using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieSpawner : MonoBehaviour
{
    public Transform Root;
    public GameObject Origin        ;
    public int PieCount;
    public float Space;
    void Start()
    {
        var originPosition = Origin.transform.position;
        for (var i = 1; i <= PieCount; ++i)
        {
            var pitObj = GameObject.Instantiate(Origin);
            pitObj.transform.position = new Vector3(originPosition.x , originPosition.y - i * Space, originPosition.z);
            pitObj.SetActive(true);
            pitObj.transform.SetParent(Root);
        }
    }

    
    
}
