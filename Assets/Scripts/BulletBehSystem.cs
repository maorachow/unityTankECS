using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
public partial struct BulletBehSystem : ISystem
{
   void OnUpdate(ref SystemState state){
        var ecb=new EntityCommandBuffer(Allocator.TempJob);
        var parallelWriter = ecb.AsParallelWriter(); 
        var jb=new BulletMoveJob{ecb=parallelWriter,deltaTime=SystemAPI.Time.DeltaTime};
        var bulletMovementJobHandle = jb.ScheduleParallel(state.Dependency); 
        bulletMovementJobHandle.Complete();
         ecb.Playback(state.EntityManager);
        ecb.Dispose();
 
   }
}

[UpdateAfter(typeof(BulletBehSystem))]
public partial struct BulletCollideWithPlayerSystem : ISystem
{
   void OnUpdate(ref SystemState state){
         var ecb2=new EntityCommandBuffer(Allocator.TempJob);
        var parallelWriter2 = ecb2.AsParallelWriter(); 
        var jbcp=new BulletCollisionJob{ecb=parallelWriter2,players=state.EntityManager.CreateEntityQuery(typeof(PlayerData)).ToComponentDataArray<PlayerData>(Allocator.Persistent)};
        var bulletCollisionPlayerJobHandle = jbcp.Schedule(state.Dependency); 
        bulletCollisionPlayerJobHandle.Complete();
         ecb2.Playback(state.EntityManager);
        ecb2.Dispose();
   }
}


[UpdateAfter(typeof(BulletCollideWithPlayerSystem))]
public partial struct BulletCollideWithEnemySystem : ISystem
{
   void OnUpdate(ref SystemState state){
         var ecb2=new EntityCommandBuffer(Allocator.TempJob);
        var parallelWriter2 = ecb2.AsParallelWriter(); 
        var jbce=new BulletCollideWithEnemyJob{ecb=parallelWriter2,enemies=state.EntityManager.CreateEntityQuery(typeof(EnemyData)).ToComponentDataArray<EnemyData>(Allocator.Persistent)};
        var bulletCollisionEnemyJobHandle = jbce.Schedule(state.Dependency); 
        bulletCollisionEnemyJobHandle.Complete();
         ecb2.Playback(state.EntityManager);
        ecb2.Dispose();
   }
}

public partial struct BulletCollisionJob:IJobEntity{
   public NativeArray<PlayerData> players;
   public EntityCommandBuffer.ParallelWriter ecb;
   void Execute([EntityIndexInQuery] int index,BulletBehAspect aspect){
    aspect.CollideWithPlayer(players,ecb,index);
   }
}

public partial struct BulletCollideWithEnemyJob:IJobEntity{
   public NativeArray<EnemyData> enemies;
   public EntityCommandBuffer.ParallelWriter ecb;
   void Execute([EntityIndexInQuery] int index,BulletBehAspect aspect){
    aspect.CollideWithEnemy(enemies,ecb,index);
   }
}
//[BurstCompile]
public partial struct BulletMoveJob:IJobEntity{
 
    public EntityCommandBuffer.ParallelWriter ecb;
    public float deltaTime;
    void Execute([EntityIndexInQuery] int index,BulletBehAspect aspect){
        aspect.Move(ecb,deltaTime,index);
       
    }
}