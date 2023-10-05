using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
public readonly partial struct PlayerMoveAspect : IAspect
{
    public readonly RefRW<PlayerData> PlayerDataSelected;
    public readonly RefRW<LocalTransform> transform;
    public void Move(float2 input,float deltaTime){
        
        transform.ValueRW.Position+=(transform.ValueRW.Up()*input.y*deltaTime);
        PlayerDataSelected.ValueRW.PlayerBound.Center= transform.ValueRW.Position;
    }
    public void RotateTank(float deltaTime,float value){
         transform.ValueRW=transform.ValueRW.RotateZ(value*deltaTime);
    }
  
}
