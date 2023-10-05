using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Transforms;
using Unity.Mathematics;
public class PlayerAuthoring : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform BulletPos;
   
 
    class Baker:Baker<PlayerAuthoring>{
    public override void Bake(PlayerAuthoring pa){
         pa.BulletPos=pa.transform.GetChild(0).GetChild(0);
   
    
        var entity=GetEntity(TransformUsageFlags.Dynamic);
        var shooterEntity=GetEntity(GetChild(0),TransformUsageFlags.Dynamic);
        AddComponent(entity,new PlayerData{
            PlayerHealth=20,
            ShooterTransEntity=shooterEntity,
            BulletEntity=GetEntity(pa.BulletPrefab,TransformUsageFlags.Dynamic),
            BulletPos=GetEntity(pa.BulletPos,TransformUsageFlags.Dynamic),
            PlayerBound=new AABB{Center=pa.transform.position,Extents=new Vector3(0.16f,0.16f,0.1f)}
        });
  //     AddComponent(shooterEntity,new ShooterTag{});
    }
  }
}
public struct PlayerData:IComponentData{
    public AABB PlayerBound;
    public Entity BulletPos;
    public Entity ShooterTransEntity;
    public Entity BulletEntity;
    public int PlayerHealth;
}
public struct ShooterTag:IComponentData{
    
}
