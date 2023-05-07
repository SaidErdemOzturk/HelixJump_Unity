using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderController : MonoBehaviour
{
    public float rotateSpeed;
    public RingType ringType;
    public GameObject canvas;
    private float moveX;
    public Transform topTransform;
    public GameObject ring;
    public GameObject speedCube;
    public int ringCount;
    private int level = 1;
    private List<GameObject> ringList = new List<GameObject>();
    private bool mouseDown;
    void Start()
    {
        if (PlayerPrefs.GetInt("level") == 0)
        {
            PlayerPrefs.SetInt("level", 1); 
        }
        level = PlayerPrefs.GetInt("level");
        ringCount = 10+level;
        //localposition parent e göre alýnan pozisyondur.
        topTransform.localPosition = new Vector3(0, 1, 0);
        transform.localScale = new Vector3(2, 3 * ringCount+66,2);
        if (level % 5 == 0)
        {
            for (int i = 1; i <= ringCount; i++)
            {

                CreateRing((i * 6), RingType.Serial);
            }
        }
        else
        {
            for (int i = 1; i <= ringCount; i++)
            {
                CreateRing((i * 6), RingType.Normal);
            }
        }
        CreateRing((ringCount * 6) + 6, RingType.Finish);

        for (int i = 1; i <= 10; i++)
        {
            
            canvas.transform.GetChild(i-1).transform.position = topTransform.position - new Vector3(0, (ringCount * 6) + 6 + (i * 6), 1);
            canvas.transform.GetChild(i-1).GetComponent<TMPro.TextMeshProUGUI>().text = (i).ToString() + "x";
            CreateRing((ringCount * 6) + 6 + (i* 6), RingType.Extra);
        }


        //Ýlk önce Ringleri oluþturuyoruz sonra SpeedCube leri oluþturuyoruz. Çünkü GetChild kullanýrken speedCube objemizin üst sýrada olmamasý gerekli
        for (int i = 0; i < ringList.Count; i++)
        {
            CreateSpeedCube(ringList[i].transform);
        }

    }

    private void CreateRing(float ringTransformY,RingType ringType)
    {
        GameObject create = Instantiate(ring);
        create.transform.SetParent(transform);
        create.GetComponent<Ring>().CreateRing(ringType, topTransform.position - new Vector3(0, ringTransformY, 0));
        if (Random.Range(0, 8) == 0)
        {
            ringList.Add(create);
        }
    }

    private void CreateSpeedCube(Transform ringTransform)
    {
        GameObject cube = Instantiate(speedCube);
        cube.transform.SetParent(transform);
        cube.transform.position = ringTransform.position + new Vector3(0, 2F,0);
        cube.transform.rotation = Quaternion.Euler(0, Random.Range(0,360),0);
    }


    private void FixedUpdate()
    {

        if (mouseDown)
            transform.Rotate(0f, moveX * rotateSpeed * Time.deltaTime, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Mouse X");
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }else if (Input.GetMouseButtonUp(0))
        {
            mouseDown=false;
        }
    }
}