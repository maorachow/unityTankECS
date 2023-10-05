using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
public partial struct EnemySpawningSystem:ISystem{
    void OnUpdate(ref SystemState state){
        if(GameManagerBeh.instance.isSpawningEnemy==true){
            EntityCommandBuffer ecb=new EntityCommandBuffer(Allocator.TempJob);
            new EnemySpawningJob{ecb=ecb}.Run();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            GameManagerBeh.instance.isSpawningEnemy=false;
        }
    }
}
public partial struct EnemySpawningJob:IJobEntity{
    public EntityCommandBuffer ecb;
    void Execute(EnemySpawningAspect aspect){
        aspect.Spawn(ecb);
    }
}
public readonly partial struct EnemySpawningAspect:IAspect{
    readonly RefRW<EnemySpawnPoint> point;
    readonly RefRW<EnemyEntityPrefabData> data;
    public void Spawn(EntityCommandBuffer ecb){
       Entity instance= ecb.Instantiate(data.ValueRW.EnemyPrefabEntity);
        ecb.SetComponent(instance,LocalTransform.FromPosition(point.ValueRW.pos));
    }
} 