using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
public class CameraMoveBeh : MonoBehaviour
{
    public Camera curCam;
    public float targetOrthoSize=5f;
    public Vector3 targetPos=new Vector3(0f,0f,-10f);
    // Start is called before the first frame update
    void Start()
    {
        curCam=GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManagerBeh.instance.playerEntity!=Entity.Null){
            LocalTransform transformE= World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(GameManagerBeh.instance.playerEntity);
            targetPos=new Vector3(transformE.Position.x,transformE.Position.y,-10f);
           
        }else{
            Debug.Log("null");
           targetPos=new Vector3(0f,0f,-10f); 
        }
        targetOrthoSize+=Input.GetAxis("Mouse ScrollWheel")*5f;
        targetOrthoSize=Mathf.Clamp(targetOrthoSize,3f,80f);
        curCam.orthographicSize=Mathf.Lerp(curCam.orthographicSize,targetOrthoSize,Time.deltaTime*5f);
        transform.position=Vector3.Lerp(transform.position,targetPos,5f*Time.deltaTime);
    } 
}
