using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
public partial struct PlayerTankShootingSystem : ISystem
{
  void OnUpdate(ref SystemState state){
    if(Input.GetKey(KeyCode.Space)){
        foreach(var playerData in SystemAPI.Query<PlayerData>()){
        
             LocalTransform shooterPosTransform=SystemAPI.GetComponent<LocalTransform>(playerData.BulletPos);
        Entity instance = state.EntityManager.Instantiate(playerData.BulletEntity);

                state.EntityManager.SetComponentData(instance, new LocalTransform
                {
                    Position = SystemAPI.GetComponent<LocalToWorld>(playerData.BulletPos).Position,
                    Rotation = SystemAPI.GetComponent<LocalToWorld>(playerData.ShooterTransEntity).Rotation,
                    Scale = 1f
                });
          
       
        }
    }
    
  }
}

public readonly partial struct PlayerShootingAspect:IAspect{
    public readonly RefRW<PlayerData> data;
}
