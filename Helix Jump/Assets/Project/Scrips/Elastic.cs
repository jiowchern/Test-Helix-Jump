using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elastic : MonoBehaviour
{
    public Vector3 Force;
    private Vector3 _StartPosition;
    readonly Regulus.Utility.TimeCounter _CollisionCooldown;

    readonly static long _CooldownTicks = (long)(((double)Regulus.Utility.TimeCounter.SecondTicks) * 0.2);
    
    public Elastic()
    {    
        _CollisionCooldown = new Regulus.Utility.TimeCounter();
        
    }

    public void Start()
    {
        _StartPosition = transform.position;
    }
    public void ResetPosition()
    {
        gameObject.transform.position = _StartPosition;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_CollisionCooldown.Ticks < _CooldownTicks)
            return;

        var rig = gameObject.GetComponent<Rigidbody>();
        rig.velocity = Vector3.zero;         
        rig.AddForce(Force, ForceMode.Impulse);
        
        _CollisionCooldown.Reset();


    }



    
}
