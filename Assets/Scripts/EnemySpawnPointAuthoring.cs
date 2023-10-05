using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class EnemySpawnPointAuthoring : MonoBehaviour
{
   public GameObject enemyPrefab;
 
     class Baker:Baker<EnemySpawnPointAuthoring>{
        public override void Bake(EnemySpawnPointAuthoring pa){
 
      var entityPrefab=GetEntity(pa.enemyPrefab,TransformUsageFlags.Dynamic);
        var entity=GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity,new EnemySpawnPoint{
            pos=pa.transform.position
        });
         AddComponent(entity,new EnemyEntityPrefabData{
         EnemyPrefabEntity=entityPrefab
        });
    }
  }
}
public struct EnemySpawnPoint:IComponentData{
    public float3 pos;
}
public struct EnemyEntityPrefabData:IComponentData{
    public Entity EnemyPrefabEntity;
}