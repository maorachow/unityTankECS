using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public partial struct PlayerMoveSystem : ISystem
{
   void OnUpdate(ref SystemState state){
    float rad=Input.GetAxis("Horizontal")*100* Mathf.Deg2Rad;
    new TankRotateJob{deltaTime=SystemAPI.Time.DeltaTime,value=rad}.Schedule();
    new PlayerMoveJob{inputVec=new float2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")),deltaTime=SystemAPI.Time.DeltaTime}.Schedule();
   }
}
public partial struct TankRotateJob:IJobEntity{
       public float deltaTime;
       public float value;
       void Execute(PlayerMoveAspect aspect){
        aspect.RotateTank(deltaTime,value);
       }
}
public partial struct PlayerMoveJob:IJobEntity{
    public float2 inputVec;
    public float deltaTime;
    void Execute(PlayerMoveAspect aspect){
   //     Debug.Log("move");
        aspect.Move(inputVec,deltaTime);
    }
}