using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RingType
{
    Normal, Serial, Finish,Extra
}
public class Ring : MonoBehaviour
{
    public RingType ringType;
    public Material unsafeMaterial;
    public Material finishMaterial;
    public GameObject cube;
    public TMPro.TextMeshProUGUI text;
    private GameObject child;
    private Rigidbody childRigid;
    private Transform center;

    public void CreateRing(RingType ringType,Vector3 vector)
    {
        this.ringType = ringType;
        transform.position = vector;
        switch (ringType)
        {
            case RingType.Normal:
                CreateNormalRing();
                break;
            case RingType.Serial:
                CreateSerialRing();
                break;
            case RingType.Finish:
                CreateFinishRing();
                break;
            case RingType.Extra:
                CreateExtraRing();
                break;
            default:
                break;
        }


    }
    private void CreateNormalRing()
    {

        int spacePlatform = Random.Range(0, 8);
        int unsafePlatform = Random.Range(0, 8);
        while (unsafePlatform == spacePlatform)
        {
            unsafePlatform = Random.Range(0, 8);
        }
        Destroy(gameObject.transform.GetChild(spacePlatform).gameObject);
        ChangeUnsafePlatform(transform.GetChild(unsafePlatform).gameObject);
        CreateWall();
    }
    public void CreateSerialRing()
    {
        int rand = Random.Range(0, 3);
        Destroy(gameObject.transform.GetChild(rand).gameObject);
        Destroy(gameObject.transform.GetChild(rand + 1).gameObject);
    }


    private void CreateFinishRing()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material = finishMaterial;
            transform.GetChild(i).tag = "Finish Platform";

        }
    }

    public void CreateExtraRing()
    {
        float red = Random.Range(0F,1F);
        float green = Random.Range(0F,1F);
        float blue = Random.Range(0F,1F);
        Color color = new Color(red, green, blue,1.0F);
        for (int i = 0; i < 4; i++)
        {
            Destroy(transform.GetChild(Random.Range(0, transform.childCount)).gameObject);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).tag = "Extra Platform";
            transform.GetChild(i).GetComponent<Renderer>().material.color = color;
        }
    }

    public void ChangeUnsafePlatform(GameObject platform)
    {
        platform.GetComponent<Renderer>().material = unsafeMaterial;
        platform.tag = "Unsafe Platform";
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            DestroyChilds();
        }
    }

    private void CreateWall()
    {
        int r = Random.Range(0, 8);
        GameObject wall= Instantiate(cube, transform);
        wall.transform.SetParent(transform);
        wall.transform.rotation = transform.GetChild(r).transform.rotation;
    }

    public void DestroyChilds()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {

            child = transform.GetChild(0).gameObject;
            child.transform.SetParent(null);

            if (child.tag!="Speed Cube")
            {
                child.tag = "Untagged";
                center = child.transform.Find("Center");
                childRigid = child.AddComponent<Rigidbody>();
                childRigid.freezeRotation = true;
                childRigid.useGravity = true;
                childRigid.AddForce((-transform.position + center.position).normalized * 1000);
                Destroy(child, 1F);
            }
        }
        //En sonki ring 
        Destroy(gameObject);

    }

}

/*
  
  public class RingManager {
  
  private IRing ring;
  
  public RingManager(IRing ring){
  this.ring = ring;
  }
  
  
  public void CreateRing(Vector3 vector){
  ring.CreateRing(vector);
 }
 
  }
  
  
  
  
  
  
  public interface Ring{
  void CreateRing();
  }
  
  public class NormalRing : IRing {
 
  public GameObject CreateRing(Vector3 vector){
  
  
   return Instantiate(transform,vector)
  
  } 
  
  public class SerialRing : IRing{
  
   * public GameObject CreateRing(Vector3 vector){
  
  
   return Instantiate(transform,vector)
  
  } 
  
  
  }
  
  
  public class FinishRing : IRing{
  
   * public GameObject CreateRing(Vector3 vector){
  
  
   return Instantiate(transform,vector)
  
  } 
  
  
  }
  
  public class ExtraRing : IRing{
  
   public GameObject CreateRing(Vector3 vector){
  
  
   return Instantiate(transform,vector)
  
  } 
  
  
  }
  
  
  
  
  
  
  
 
  
 * */
