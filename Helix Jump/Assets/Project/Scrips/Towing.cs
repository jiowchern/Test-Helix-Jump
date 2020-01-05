using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towing : MonoBehaviour
{
    public Transform Center;
    Vector2 _Center;
    private float _Distance;
    void Start()
    {
        _Center = new Vector2(Center.position.x, Center.position.z);
        var pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        _Distance = Vector2.Distance(_Center, pos);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var pos =new Vector2( gameObject.transform.position.x  , gameObject.transform.position.z);
        var distance = Vector2.Distance(_Center, pos);
        var scale = _Distance / distance;
        var y = gameObject.transform.position.y;
        gameObject.transform.position = new Vector3(pos.x * scale, y , pos.y * scale);
    }
}
