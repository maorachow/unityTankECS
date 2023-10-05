using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public partial struct PlayerTankRotatingSystem : ISystem
{
    public float3 ToEuler( quaternion quaternion) {
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


        foreach(var shooterTrans in SystemAPI.Query<ShooterMoveAspect>()){
            Vector3 ms = Input.mousePosition;
        ms = Camera.main.ScreenToWorldPoint(ms);//获取鼠标相对位置

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
        }
    }
}





/*public partial struct PlayerRotateJob:IJobEntity{
    quaternion q;
    void Execute(PlayerMoveAspect aspect){
        aspect.RotateTank(q);
    }
}*/