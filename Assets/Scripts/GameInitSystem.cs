using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public partial struct GameInitSystem : ISystem
{
    void OnCreate(ref SystemState state){
   //        PlayerAuthoring.BulletPrefab=Resources.Load<GameObject>("Prefabs/bullet");
       //     TankSpawningPointAuthoring.playerPrefab=Resources.Load<GameObject>("Prefabs/player");
    }
}
