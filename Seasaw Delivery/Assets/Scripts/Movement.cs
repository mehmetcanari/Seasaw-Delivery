using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Movement : MonoBehaviour
{
    #region Variables
    private Vector2 startPos;
    private Vector2 deltaPos;

    public Animator anim;
    
    public float moveSmoother;
    public float moveSpeed;
    public float impulsePower;
    public float speed;
    
    private bool isClear = false;
    public bool isThrowed = false;
    public bool balance = false;
    public bool win = false;
    bool oneTime = true;
    bool isMoving = false;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera finalCam;
    public GameObject gemImage;
    public List<GameObject> boxes = new List<GameObject>();
    public Rigidbody rb;
    #endregion
    

    void Start()
    {
        #region Calling Functions
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        #endregion
    }


    void Update()
    {
        if (win && oneTime)
        {          
            transform.Rotate(new Vector3(0, 180, 0));
            oneTime = false;
            return;
        }
        #region Movement
        if (isMoving && !balance && !win)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
        #endregion

        #region Swerve
        if (Input.GetMouseButtonDown(0)) 
        {
            startPos = Input.mousePosition;
            anim.SetBool("Stop", true);
        }
        
        else
        {
            anim.SetBool("Stop", false);
        }

        if (Input.GetMouseButton(0) && !win)
        {
            isMoving = false;
            deltaPos = (Vector2)Input.mousePosition - startPos;
            deltaPos.y = 0;
            var movement = new Vector3(Mathf.Lerp(rb.transform.position.x, rb.transform.position.x + (deltaPos.x / Screen.width) * moveSpeed, Time.deltaTime * moveSmoother), transform.position.y, transform.position.z);
            rb.transform.position = movement;
            startPos = Input.mousePosition;
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, 4.80f, -4.80f), transform.position.y, transform.position.z);
        }
        else
        {
            isMoving = true;
        }
        #endregion

        #region Drop Mechanic
        if (Input.GetMouseButtonUp(0) && !isThrowed && balance)
        {
            isThrowed = true;
            if (!isClear)
            {
                Debug.Log("Bıraktın");
                //Instantiate(, new Vector3(character.transform.position.x - 0.5f, character.transform.position.y, character.transform.position.z), Quaternion.identity
                boxes[boxes.Count - 1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                boxes[boxes.Count - 1].GetComponent<Rigidbody>().AddForce(impulsePower, 0, 0, ForceMode.Impulse);
                Invoke("FalseReturner", 1f);
                boxes[boxes.Count - 1].transform.parent = null;
                boxes.Remove(boxes[boxes.Count - 1]);

                if (boxes.Count == 0 || boxes == null)
                {
                    isClear = true;
                    anim.SetBool("Idle", true);
                }
                else
                {
                    isClear = false;
                    anim.SetBool("Idle", false);
                }
            }
        }
        #endregion
    }

    #region TriggerEnter&Exit
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Balance")
        {
            balance = true;
        }
        if(other.gameObject.tag == "Gem")
        {
            //Instantiate(gemImage, new Vector3 (other.gameObject.transform.position.x, other.gameObject.transform.position.y, -5), Quaternion.identity);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        if(other.gameObject.tag == "Win")
        {
            win = true;
        }
        if (other.gameObject.tag == "Cam")
        {
            mainCam.gameObject.SetActive(false);
            finalCam.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Balance")
        {
            balance = false;
        }
    }
    #endregion

    #region Methods
    public void FalseReturner()
    {
        isThrowed = false;
    }
    #endregion
}
