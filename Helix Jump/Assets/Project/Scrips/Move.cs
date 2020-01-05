using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Track Track;
    public float Force;

    readonly Regulus.Utility.TimeCounter _CollisionCooldown;

    readonly static long _CooldownTicks = (long)(((double)Regulus.Utility.TimeCounter.SecondTicks) * 0.2);

    public Move()
    {
        _CollisionCooldown = new Regulus.Utility.TimeCounter();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_CollisionCooldown.Ticks < _CooldownTicks)
            return;
        var direction = collision.gameObject.GetComponent<MoveDirection>();
        if (direction == null )
        {    
            return;
        }
        
        Force = direction.Force;
        _CollisionCooldown.Reset();

    }

    public void Update()
    {
        var deltaForce = UnityEngine.Time.deltaTime * Force;
        Track.Position += deltaForce;
        Force -= deltaForce;
        if(Mathf.Abs(Force) < 1)
        {
            Force = 0;
        }
    }
}

