using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class BulletAuthoring : MonoBehaviour
{
   class Baker:Baker<BulletAuthoring>{
    public override void Bake(BulletAuthoring ba){
        AddComponent(GetEntity(TransformUsageFlags.Dynamic),new BulletData{bulletSpeed=UnityEngine.Random.Range(1.5f,4.5f),lifeTime=0f,bulletBounds=new AABB{Center=ba.transform.position,Extents=new float3(0.1f,0.1f,0.1f)}});
    }
  }
}
public partial struct BulletData:IComponentData{
    public float bulletSpeed;
    public float lifeTime;
    public AABB bulletBounds;
}
