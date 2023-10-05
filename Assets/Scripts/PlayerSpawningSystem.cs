using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Collections;
using Unity.Transforms;
using Unity.Jobs;
[UpdateAfter(typeof(GameInitSystem))]
public partial struct PlayerSpawningSystem : ISystem
{
    void OnUpdate(ref SystemState state){

        if(GameManagerBeh.instance.isCreatingPlayer==true){

             state.EntityManager.DestroyEntity(state.EntityManager.CreateEntityQuery(typeof(PlayerData)).ToEntityArray(Allocator.Persistent));
         //   foreach(var spawningPoint in SystemAPI.Query<TankSpawnPoint>())
   /*         foreach(var spawnPoint in SystemAPI.Query<TankSpawningAspect>()){
         //   Debug.Log("Spawn");
            Entity instance=state.EntityManager.Instantiate(spawnPoint.entityData.ValueRW.PlayerPrefab);
            state.EntityManager.SetComponentData(instance,LocalTransform.FromPosition(spawnPoint.pointData.ValueRW.pos));
        }*/
        EntityCommandBuffer ecb=new EntityCommandBuffer(Allocator.TempJob);

       new SpawnEntityJob{ecb=ecb}.Run();
     //   jh.Complete();
        ecb.Playback(state.EntityManager);
        GameManagerBeh.instance.UpdateList();
        GameManagerBeh.instance.PlayerLifeCount--;
        Debug.Log(GameManagerBeh.instance.PlayerLifeCount);
        GameManagerBeh.instance.isCreatingPlayer=false;
        }
    }
}
public partial struct SpawnEntityJob:IJobEntity{
    public EntityCommandBuffer ecb;
    void Execute(TankSpawningAspect aspect){
        aspect.Spawn(ecb);
    }
}