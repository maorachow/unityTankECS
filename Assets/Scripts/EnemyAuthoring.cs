using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class EnemyAuthoring : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform BulletPos;
   
 
    class Baker:Baker<EnemyAuthoring>{
    public override void Bake(EnemyAuthoring ea){
         ea.BulletPos=ea.transform.GetChild(0).GetChild(0);
   
    
        var entity=GetEntity(TransformUsageFlags.Dynamic);
        var shooterEntity=GetEntity(GetChild(0),TransformUsageFlags.Dynamic);
        AddComponent(entity,new EnemyData{
            EnemyHealth=20,
            ShooterTransEntity=shooterEntity,
            BulletEntity=GetEntity(ea.BulletPrefab,TransformUsageFlags.Dynamic),
            BulletPos=GetEntity(ea.BulletPos,TransformUsageFlags.Dynamic),
            EnemyBound=new AABB{Center=ea.transform.position,Extents=new Vector3(0.16f,0.16f,0.1f),
          },  BulletCD=0f
        });
   //     AddComponent(shooterEntity,new ShooterTag{});
    }
  }
}
public struct EnemyData:IComponentData{
    public int EnemyHealth;
    public Entity ShooterTransEntity;
    public Entity BulletEntity;
    public Entity BulletPos;
    public AABB EnemyBound;
    public float BulletCD;
}
public struct EnemyShooterTag:IComponentData{

}