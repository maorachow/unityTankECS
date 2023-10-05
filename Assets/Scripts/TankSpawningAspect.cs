using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public readonly partial struct TankSpawningAspect:IAspect{
    public readonly RefRW<TankSpawnPoint> pointData;
    public readonly RefRW<PlayerEntityPrefabData> entityData;
    public void Spawn(EntityCommandBuffer ecb){
        Entity instance = ecb.Instantiate(entityData.ValueRW.PlayerPrefab); 
        ecb.SetComponent(instance, LocalTransform.FromPosition(pointData.ValueRW.pos));
    }
}
