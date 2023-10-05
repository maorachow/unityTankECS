using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
public partial struct EnemyMoveSystem:ISystem{
    void OnUpdate(ref SystemState state){
        new EnemyMoveJob{deltaTime=SystemAPI.Time.DeltaTime}.ScheduleParallel();
    }
}

public partial struct EnemyMoveJob:IJobEntity{
    public float deltaTime;
    void Execute(EnemyMoveAspect aspect){
        aspect.Move(0.3f,deltaTime);
    }
}

public partial struct EnemyRotatingSystem:ISystem{
    void OnUpdate(ref SystemState state){
        if(GameManagerBeh.instance.playerEntity!=Entity.Null){
             float3 targetPos=state.EntityManager.GetComponentData<LocalTransform>(GameManagerBeh.instance.playerEntity).Position;
        new EnemyRotatingJob{deltaTime=SystemAPI.Time.DeltaTime,value=targetPos}.ScheduleParallel();
        }
       
    }
}

public partial struct EnemyTankShooterRotatingSystem : ISystem
{
    public static float3 ToEuler( quaternion quaternion) {
            float4 q = quaternion.value;
            double3 res;
 
            double sinr_cosp = +2.0 * (q.w * q.x + q.y * q.z);
            double cosr_cosp = +1.0 - 2.0 * (q.x * q.x + q.y * q.y);
            res.x = math.atan2(sinr_cosp, cosr_cosp);
 
            double sinp = +2.0 * (q.w * q.y - q.z * q.x);
            if (math.abs(sinp) >= 1) {
                res.y = math.PI / 2 * math.sign(sinp);
            } else {
                res.y = math.asin(sinp);
            }
 
            double siny_cosp = +2.0 * (q.w * q.z + q.x * q.y);
            double cosy_cosp = +1.0 - 2.0 * (q.y * q.y + q.z * q.z);
            res.z = math.atan2(siny_cosp, cosy_cosp);
 
            return (float3) res;
        }
    void OnUpdate(ref SystemState state){
  if(GameManagerBeh.instance.playerEntity!=Entity.Null){

        foreach(var shooterTrans in SystemAPI.Query<EnemyShooterMoveAspect>()){
            Vector3 ms =SystemAPI.GetComponent<LocalTransform>(GameManagerBeh.instance.playerEntity).Position ;
   

        Vector3 gunPos = new Vector3(shooterTrans.transformW.ValueRW.Position.x,shooterTrans.transformW.ValueRW.Position.y,shooterTrans.transformW.ValueRW.Position.z);
        
        float fireangle;
   
        Vector2 targetDir = ms - gunPos;
        fireangle = Vector2.Angle(targetDir, Vector3.up);
        if (ms.x > gunPos.x)
        {
            fireangle = -fireangle;
        }
        LocalTransform playerTrans= SystemAPI.GetComponent<LocalTransform>(shooterTrans.curParent.ValueRW.Value);
       float3 eulerW=ToEuler(playerTrans.Rotation);
        quaternion q=quaternion.EulerXYZ(new float3(0,0,fireangle * Mathf.Deg2Rad)-eulerW);
   
      //  quaternion qt=tankTrans.transform.ValueRW.TransformRotation(q);

                shooterTrans.RotateTankShooter(q);
        }}
    }
}

public readonly partial struct EnemyShooterMoveAspect:IAspect{
    public readonly RefRW<Parent> curParent;
    public readonly RefRW<LocalTransform> transform;
    public readonly RefRW<LocalToWorld> transformW;
    public readonly RefRO<EnemyShooterTag> tag;
    public void RotateTankShooter(quaternion q){
         transform.ValueRW.Rotation=q;
    }
}

public partial struct EnemyRotatingJob:IJobEntity{
    public float3 value;
    public float deltaTime;
    void Execute(EnemyMoveAspect aspect){
        aspect.RotateTankTowards(deltaTime,value);
    }
}
public readonly partial struct EnemyMoveAspect:IAspect{
    readonly RefRW<EnemyData> data;
    readonly RefRW<LocalTransform> transform;
    
    public void Move(float value,float deltaTime){
        transform.ValueRW.Position+=(transform.ValueRW.Up()*value*deltaTime);
        data.ValueRW.EnemyBound.Center=transform.ValueRW.Position;
    }
    public void RotateTankTowards(float deltaTime,float3 f3){
        Vector3 ms =new Vector3(f3.x,f3.y,f3.z);
        Vector3 gunPos = new Vector3(transform.ValueRW.Position.x,transform.ValueRW.Position.y,transform.ValueRW.Position.z);   
        float fireangle;
        Vector2 targetDir = ms - gunPos;
        fireangle = Vector2.Angle(targetDir, Vector3.up);
        if (ms.x > gunPos.x)
        {
            fireangle = -fireangle;
        }
        float fireangleRad=fireangle*Mathf.Deg2Rad;
        quaternion q=quaternion.EulerXYZ(new float3(0,0,fireangleRad));
         transform.ValueRW.Rotation=math.slerp(transform.ValueRW.Rotation,q,deltaTime*5f);
    }
}


public partial struct EnemyTankShootingSystem : ISystem
{
  void OnUpdate(ref SystemState state){
    if(GameManagerBeh.instance.playerEntity==Entity.Null){
        return;
    }
    float3 targetPos=state.EntityManager.GetComponentData<LocalTransform>(GameManagerBeh.instance.playerEntity).Position;
    
        foreach(var enemyData in SystemAPI.Query<EnemyShootingAspect>()){
              enemyData.data.ValueRW.BulletCD-=SystemAPI.Time.DeltaTime;
            if(math.distance(targetPos,SystemAPI.GetComponent<LocalTransform>(enemyData.enemyEntity).Position)<10f&&enemyData.data.ValueRW.BulletCD<=0f){
                LocalTransform shooterPosTransform=SystemAPI.GetComponent<LocalTransform>(enemyData.enemyEntity);
                Entity instance = state.EntityManager.Instantiate(enemyData.data.ValueRW.BulletEntity);

                state.EntityManager.SetComponentData(instance, new LocalTransform
                {
                    Position = SystemAPI.GetComponent<LocalToWorld>(enemyData.data.ValueRW.BulletPos).Position,
                    Rotation = SystemAPI.GetComponent<LocalToWorld>(enemyData.data.ValueRW.ShooterTransEntity).Rotation,
                    Scale = 1f
                });
            enemyData.data.ValueRW.BulletCD=0.5f;
            }
         
            
                
       
        }
    
    
  }
}
public partial struct EnemyDespawnSystem : ISystem
{
  void OnUpdate(ref SystemState state){
         var ecb2=new EntityCommandBuffer(Allocator.TempJob);
        var parallelWriter2 = ecb2.AsParallelWriter(); 
        var jbce=new DestroyTankJob{ecb=parallelWriter2};
        var bulletCollisionEnemyJobHandle = jbce.Schedule(state.Dependency); 
        bulletCollisionEnemyJobHandle.Complete();
         ecb2.Playback(state.EntityManager);
        ecb2.Dispose();
  }
}
public partial struct DestroyTankJob:IJobEntity{
    public EntityCommandBuffer.ParallelWriter ecb;
    void Execute([EntityIndexInQuery] int index,EnemyShootingAspect aspect){
        aspect.DestroyTank(index,ecb);
    }
}
public readonly partial struct EnemyShootingAspect:IAspect{
    public readonly RefRW<EnemyData> data;
    public readonly Entity enemyEntity;
    public readonly RefRW<LocalTransform> transform;
    public void DestroyTank(int index,EntityCommandBuffer.ParallelWriter ecb){
        if(data.ValueRW.EnemyHealth<=0){
         ecb.DestroyEntity(index,enemyEntity);
        BulletParticleBeh.instance.particlePosQueue.Enqueue(transform.ValueRW.Position);   
        }
        
    }
}