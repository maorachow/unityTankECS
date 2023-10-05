using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Transforms;
public class ShooterAuthoring : MonoBehaviour
{
     class Baker:Baker<ShooterAuthoring>{
    public override void Bake(ShooterAuthoring pa){
       //  pa.BulletPos=pa.transform.GetChild(0).GetChild(0);
   //      pa.ShooterPos=pa.transform.GetChild(0);
        var entity=GetEntity(TransformUsageFlags.Dynamic);
   //     var shooterEntity=GetEntity(GetChild(0),TransformUsageFlags.Dynamic);
   
        AddComponent(entity,new ShooterTag{});
    }
  }
}
