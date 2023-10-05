using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
public class GameManagerBeh : MonoBehaviour
{
public static GameManagerBeh instance;
public int PlayerLifeCount=10;
public Entity[] playerEntities;
public Entity playerEntity;
public bool isCreatingPlayer=false;
public bool isSpawningEnemy=false;
    void InvokeCreateEnemy(){
        isSpawningEnemy=true;
    }
    void Awake(){
        instance=this;
    }
    void Start(){
        InvokeRepeating("InvokeCreateEnemy",1.0f,10f);
    }
    // Update is called once per frame
    public void UpdateList(){
        playerEntities = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerData)).ToEntityArray(Allocator.Persistent).ToArray();
    //    Debug.Log(playerEntities.Length);
    }
   void FixedUpdate(){

    playerEntities = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerData)).ToEntityArray(Allocator.Persistent).ToArray();
   //  Debug.Log(playerEntities.Length);
    if(playerEntities.Length>0&&PlayerLifeCount>0){
        playerEntity=playerEntities[0];
    }else{
        playerEntity=Entity.Null;
        if(PlayerLifeCount>0){
         isCreatingPlayer=true;   
        } 
        
    }
     if(playerEntities.Length==0&&PlayerLifeCount<=0){
        Debug.Log("lose");
     }
     if(Input.GetKeyDown(KeyCode.F)){
          
                World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(PlayerData)).ToEntityArray(Allocator.Persistent));
           
              playerEntity=Entity.Null; 
     }
   //  playerEntities.Dispose();
   }
}
