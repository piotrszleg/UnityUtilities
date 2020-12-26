using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundParticles : AutoFieldsExecutor
{
    [Pool(50)]
    public PoolObject particlesPrefab;
    [GetComponent]
    CharacterController controller = null;
    public Vector3 particlesOffset;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (enabled && !controller.isGrounded && hit.gameObject.tag=="Ground")
        {
            Pool.instance.Request(particlesPrefab, transform.position + particlesOffset, Quaternion.identity);
        }
    }
}
