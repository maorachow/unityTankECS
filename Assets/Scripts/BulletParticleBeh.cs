using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Entities;
public class BulletParticleBeh : MonoBehaviour
{
    public static BulletParticleBeh instance;
    public NativeQueue<float3> particlePosQueue;
    public ParticleSystem ps;
    ParticleSystem.EmitParams psParams;
    // Start is called before the first frame update
    void Start()
    {
        instance=this;
        particlePosQueue=new NativeQueue<float3>(Allocator.Persistent);
        ps=GetComponent<ParticleSystem>();
    }
    void FixedUpdate(){
        while(particlePosQueue.Count>0){
            float3 posf3;
            particlePosQueue.TryDequeue(out posf3);
            Vector3 pos=new Vector3(posf3.x,posf3.y,posf3.z);
         //   transform.position=pos;
         psParams.position=pos;
            ps.Emit(psParams,1);
        }
    }
   
 
}
