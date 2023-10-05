using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
public readonly partial struct ShooterMoveAspect : IAspect
{
    public readonly RefRW<Parent> curParent;
    public readonly RefRW<LocalTransform> transform;
    public readonly RefRW<LocalToWorld> transformW;
    public readonly RefRO<ShooterTag> tag;
    public void RotateTankShooter(quaternion q){
         transform.ValueRW.Rotation=q;
    }
}
