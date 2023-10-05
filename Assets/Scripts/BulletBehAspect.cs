using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
public readonly partial struct BulletBehAspect : IAspect
{
   public readonly RefRW<BulletData> data;
   public readonly RefRW<LocalTransform> transform;
   
   public readonly Entity entity;
   public void CollideWithPlayer(NativeArray<PlayerData> players,EntityCommandBuffer.ParallelWriter ecb,int index){
    for(int i=0;i<players.Length;i++){
        PlayerData player=players[i];
        if(AABBExtensions.ToBounds(player.PlayerBound).Intersects(AABBExtensions.ToBounds(data.ValueRW.bulletBounds))){
            player.PlayerHealth--;
             BulletParticleBeh.instance.particlePosQueue.Enqueue(transform.ValueRW.Position);
            ecb.DestroyEntity(index,entity);

        }
    }
   }

    public void CollideWithEnemy(NativeArray<EnemyData> enemies,EntityCommandBuffer.ParallelWriter ecb,int index){
    for(int i=0;i<enemies.Length;i++){
        EnemyData enemy=enemies[i];
        if(AABBExtensions.ToBounds(enemy.EnemyBound).Intersects(AABBExtensions.ToBounds(data.ValueRW.bulletBounds))){
            enemy.EnemyHealth--;
            
             BulletParticleBeh.instance.particlePosQueue.Enqueue(transform.ValueRW.Position);
            ecb.DestroyEntity(index,entity);

        }
    }
   }
  

   
   public void Move(EntityCommandBuffer.ParallelWriter ecb,float deltaTime,int index){
            transform.ValueRW=transform.ValueRW.Translate(data.ValueRW.bulletSpeed*transform.ValueRW.Up()*deltaTime);
            data.ValueRW.bulletBounds.Center= transform.ValueRW.Position;
            data.ValueRW.lifeTime+=deltaTime;
            if( data.ValueRW.lifeTime>=4f){
                BulletParticleBeh.instance.particlePosQueue.Enqueue(transform.ValueRW.Position);
                ecb.DestroyEntity(index,entity);
            }
    }
}
