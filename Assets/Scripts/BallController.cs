using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float jumpSpeed;
    public Transform topTransform;
    public GameObject splashPrefab;
    private Ring ring;
    private CanvasController canvasController;
    private bool control = true;
    private bool control2 = true;
    private CylinderController cylinder;
    private int serialRing=0;
    public ParticleSystem particle;
    private int myRing = 0;



    void Start()
    {
        cylinder = GameObject.FindObjectOfType<CylinderController>();

        canvasController = GameObject.FindObjectOfType<CanvasController>();
    }



    void Update()
    {
        if (topTransform.hasChanged&&control2)
        {
            transform.position = topTransform.position + new Vector3(0, 0, -1.5F);
            control2 = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (control)
        {            
            if (collision.gameObject.CompareTag("Safe Platform"))
            {
               
                StartCoroutine(jump(collision.transform));
                if (serialRing >= 2)
                {
                    SerialTouch(collision.gameObject.transform.parent.GetComponent<Ring>());
                }

            }
            else if (collision.gameObject.CompareTag("Unsafe Platform"))
            {
                if (serialRing >= 2)
                    SerialTouch(collision.gameObject.transform.parent.GetComponent<Ring>());
                else
                    canvasController.RestartGame();
            }
            else if (collision.gameObject.CompareTag("Last Platform"))
            {
                StartCoroutine(jump(collision.transform));

                int level = PlayerPrefs.GetInt("level");
                canvasController.NextLevelSetActiveButton();
                PlayerPrefs.SetInt("level", level + 1);

            }
            else if (collision.gameObject.CompareTag("Finish Platform"))
            {
                StartCoroutine(jump(collision.transform));
                if (serialRing >= 2)
                {
                    SerialTouch(collision.gameObject.transform.parent.GetComponent<Ring>());

                }
                else
                {
                    collision.gameObject.transform.parent.GetComponent<Ring>().DestroyChilds();
                }
                serialRing++;

            }
            else if(collision.gameObject.CompareTag("Extra Platform"))
            {
                int level = PlayerPrefs.GetInt("level");
                Time.timeScale = 0.0F;
                canvasController.NextLevelSetActiveButton();
                PlayerPrefs.SetInt("level", level + 1);
                canvasController.AddScore(serialRing*canvasController.score);
            }

        }
    }
    

    private void SerialTouch(Ring ring)
    {
        myRing++;
        canvasController.UpdateSlider(cylinder.ringCount,myRing);
        Instantiate(particle, new Vector3(transform.position.x, transform.position.y, 0.0F),transform.rotation);
        ring.DestroyChilds();
        int extraScore = 0;
        for (int i = 1; i <= serialRing; i++)
        {
            extraScore = extraScore + i*10;
        }
        serialRing = 0;
        canvasController.AddScore(extraScore);
        canvasController.AddScore(20);



    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Speed Cube"))
        {
            //Buradaki forun 2 ile baþlamasýnýn sebebi cylinder içerisinde ilk 2 child ringlerden oluþmamasýndan dolayýdýr.
            for (int i = 2; i < 4; i++)
            {

                myRing++;
                canvasController.UpdateSlider(cylinder.ringCount,myRing);
                serialRing++;
                Destroy(other.gameObject);
                if(ring = cylinder.transform.GetChild(i).GetComponent<Ring>())
                {
                    if (ring.transform.GetChild(0).tag != "Finish Platform")
                    {
                        ring.DestroyChilds();
                        canvasController.AddScore(20);
                    }

                }
                

            }
        } else if(other.gameObject.CompareTag("Ring")) 
        {
            myRing++;
            serialRing++;
            canvasController.AddScore(20);
            canvasController.UpdateSlider(cylinder.ringCount,myRing);
        }
    }



    IEnumerator jump(Transform collision)
    {
        control = false;
        GameObject splash = Instantiate(splashPrefab, transform.position + new Vector3(0, -0.21F, 0), transform.rotation);
        splash.transform.SetParent(collision);
        rigidBody.AddForce(Vector3.up * jumpSpeed);
        yield return new WaitForSecondsRealtime(0.1F);
        serialRing = 0;
        control = true;

    }

}
