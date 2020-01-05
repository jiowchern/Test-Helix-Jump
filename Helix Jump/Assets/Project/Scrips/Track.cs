using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    /*
        *   x1   =   x0   +   r   *   cos(ao   *   3.14   /180   ) 
            y1   =   y0   +   r   *   sin(ao   *   3.14   /180   ) 
     */
    public Transform Center;
    Vector2 _Center;
    private float _Distance;
    public float Position ;
    void Start()
    {
        _Center = new Vector2(Center.position.x , Center.position.z);
        var pos =new Vector2( gameObject.transform.position.x , gameObject.transform.position.z);
        
        _Distance = Vector2.Distance(_Center, pos);
    }

    
    void Update()
    {
        Position %= 360;
        var x = _Center.x + _Distance * Mathf.Cos(Position * Mathf.PI / 180);
        var z = _Center.y + _Distance * Mathf.Sin(Position * Mathf.PI / 180);

        var y = transform.position.y;
        transform.position = new Vector3(x, y, z); 
    }
}
