using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class EnemyShooterAuthoring : MonoBehaviour
{
     class Baker:Baker<EnemyShooterAuthoring>{
    public override void Bake(EnemyShooterAuthoring pa){
       //  pa.BulletPos=pa.transform.GetChild(0).GetChild(0);
   //      pa.ShooterPos=pa.transform.GetChild(0);
        var entity=GetEntity(TransformUsageFlags.Dynamic);
   //     var shooterEntity=GetEntity(GetChild(0),TransformUsageFlags.Dynamic);
   
        AddComponent(entity,new EnemyShooterTag{});
    }
  }
}
