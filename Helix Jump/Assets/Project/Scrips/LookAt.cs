using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform Center;
    public Transform Target;
    public float UpdateRange;
    public float Distance;
    public float OffsetY;
    Vector3 _Target;
    private Vector3 _Start;

    // Start is called before the first frame update
    void Start()
    {
        _Target = Target.position;
        _Start = _Target;
    }

    public void ResetY()
    {
        _Target = _Start;
    }

    // Update is called once per frame
    void Update()
    {
        var target = Target.position;
        var delta = target - _Target;
        if ( Mathf.Abs(delta.x) > UpdateRange)
        {            
            var x = _Target.x+  UnityEngine.Time.deltaTime * delta.x;
            _Target = new Vector3(x, _Target.y, _Target.z);
        }
        if (Mathf.Abs(delta.y) > UpdateRange)
        {
            var y = _Target.y + UnityEngine.Time.deltaTime * delta.y;
            _Target = new Vector3(_Target.x, y, _Target.z);
        }
        if (Mathf.Abs(delta.z) > UpdateRange)
        {
            var z = _Target.z + UnityEngine.Time.deltaTime * delta.z;
            _Target = new Vector3(_Target.x, _Target.y, z);
        }
        
        

        var vec = new Vector2(Target.position.x, Target.position.z) - new Vector2(Center.position.x , Center.position.z);

        var pos = vec.normalized * Distance;
        
        transform.position = new Vector3(pos.x , _Target.y + OffsetY +3, pos.y);
        transform.LookAt(new Vector3(_Target.x, _Target.y - OffsetY, _Target.z));
        

    }
}
