using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class TankSpawningPointAuthoring : MonoBehaviour
{
    public GameObject playerPrefab;
 
     class Baker:Baker<TankSpawningPointAuthoring>{
    public override void Bake(TankSpawningPointAuthoring pa){
 
      var entityPrefab=GetEntity(pa.playerPrefab,TransformUsageFlags.Dynamic);
        var entity=GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity,new TankSpawnPoint{
            pos=pa.transform.position
        });
         AddComponent(entity,new PlayerEntityPrefabData{
         PlayerPrefab=entityPrefab
        });
    }
  }
}

public struct TankSpawnPoint:IComponentData{

        public float3 pos;
        
}
public struct PlayerEntityPrefabData:IComponentData{

        public Entity PlayerPrefab;
        
}
