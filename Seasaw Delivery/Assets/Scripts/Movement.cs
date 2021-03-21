using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
    #region Variables
    private Vector2 startPos;
    private Vector2 deltaPos;
    private Vector2 finalRot = new Vector2(x: 0, y: 180);
    private Vector3 playerRot = new Vector3(0, 0, 0);

    public float moveSmoother;
    public float moveSpeed;
    public float impulsePower;
    public float speed;
    public float lerpTime;
    public float smoothTime;
    private int scoreCount;
    public int rewardCounter;
    private bool balance = false;
    private bool win = false;
    private bool isThrowed = false;
    private bool oneTime = true;
    private bool isMoving = false;
    private bool isClear = false;
    private bool isAnimated = false;
    private bool isStarted = false;
    private bool isEnded = false;
    private bool move = false;
    private bool tutorialBool = false;
    private bool isTutorialDestroyed = false;
    private bool isPerfect = false;
    public bool instantiated = false;
    private bool ragdoll = false;

    public GameObject playerTransform;
    public Collider[] col;
    public Collider playerCol;
    public Rigidbody[] ragdollRb;
    public Animator anim;
    public ParticleSystem gemParticle;
    public ParticleSystem boxDestroy;
    public CinemachineVirtualCamera playerCamCM;
    public CinemachineVirtualCamera rotateCamCM;
    public List<GameObject> boxes = new List<GameObject>();
    public Rigidbody rb;
    public GameObject tutorial;
    public GameObject scoreImage;
    public TextMeshProUGUI puan;
    public TextMeshProUGUI tapToStart;
    public TextMeshProUGUI balanceTutorial;
    public TextMeshProUGUI good;
    public TextMeshProUGUI amazing;
    public TextMeshProUGUI perfect;
    public GameObject Retry;
    public GameObject NextLevel;
    public GameObject kutusayi;
    public TextMeshPro kutusayitext;
    #endregion


    private void Awake()
    {
        DisableRagdoll();
    }

    void Start()
    {
        #region Calling Functions
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isAnimated = true;
        isStarted = false;
        isEnded = false;
        kutusayitext = kutusayi.GetComponent<TextMeshPro>();
        #endregion
    }

    private void FixedUpdate()
    {
        #region Drop Mechanic
        if (Input.GetMouseButtonUp(0) && !isThrowed && balance)
        {
            isThrowed = true;
            if (!isClear)
            {
                //Debug.Log("Bıraktın");
                //Instantiate(, new Vector3(character.transform.position.x - 0.5f, character.transform.position.y, character.transform.position.z), Quaternion.identity
                boxes[boxes.Count - 1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                boxes[boxes.Count - 1].GetComponent<Rigidbody>().AddForce(impulsePower, 0, 0, ForceMode.Impulse);
                Invoke("FalseReturner", 1f);
                boxes[boxes.Count - 1].transform.parent = null;
                boxes.Remove(boxes[boxes.Count - 1]);

                //if (boxes.Count == 0 || boxes == null)
                //{
                //    isClear = true;
                //    anim.SetBool("Idle", true);
                //}
                //else
                //{
                //    isClear = false;
                //    anim.SetBool("Idle", false);
                //}
            }
        }
        #endregion
    }

    void Update()
    {
        #region BoxesList
        for (var i = boxes.Count - 1; i > -1; i--)
        {
            if(boxes[i] == null)
            {
                boxes.Remove(boxes[i]);
            }
        }
        if (boxes.Count != 0)
        {
            kutusayi.transform.position = new Vector3(boxes[0].transform.position.x + 2, boxes[0].transform.position.y + 0.6f, boxes[0].transform.position.z + 0.2f);
        }

        #endregion
        
        if (!ragdoll)
        {
            #region Final Movement
            if (win && oneTime)
            {
                isAnimated = false;
                if (!isAnimated)
                {
                    isEnded = true;
                    anim.SetBool("Stop", true);
                    isStarted = false;
                }
                else
                {
                    return;
                }

                transform.DORotate(finalRot, 0.5f);
                oneTime = false;
                Invoke("EndGame", 1);
            }
            #endregion

            #region Movement
            if (isMoving && !balance && !win && isStarted && !tutorialBool)
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
            else if (!isStarted)
            {
                isAnimated = false;
                anim.SetBool("Stop", true);
            }
            else
            {
                isAnimated = true;
                anim.SetBool("Stop", false);
            }
            if (tutorialBool)
            {
                isAnimated = false;
                anim.SetBool("Stop", true);
            }
            #endregion

            #region Swerve
            if (!isEnded)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (tapToStart != null)
                    {
                        tapToStart.gameObject.transform.DOMoveY(-50, 0.5f);
                        Destroy(tapToStart.gameObject, 1);
                    }
                    isStarted = true;
                    startPos = Input.mousePosition;
                }

                if (isStarted && !move)
                {
                    anim.SetBool("Stop", false);
                }

                if (Input.GetMouseButton(0) && !win && move)
                {
                    tutorialBool = false;

                    if (!isTutorialDestroyed)
                    {
                        tutorial.gameObject.transform.DOScale(Vector2.zero, 0.5f);
                        Destroy(tutorial, 1f);
                        isTutorialDestroyed = true;
                    }

                    isMoving = false;
                    deltaPos = (Vector2)Input.mousePosition - startPos;
                    deltaPos.y = 0;
                    var movement = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + (deltaPos.x / Screen.width) * moveSpeed, Time.deltaTime * moveSmoother), transform.position.y, transform.position.z);
                    transform.position = movement;
                    startPos = Input.mousePosition;

                    if (deltaPos.x > 0)
                    {
                        Debug.Log(deltaPos);
                        playerTransform.transform.DORotate(new Vector2(0, 70), 0.5f);
                       
                    }
                    else if (deltaPos.x < 0)
                    {
                        Debug.Log(deltaPos);
                        playerTransform.transform.DORotate(new Vector2(0, -70), 0.5f);
                    }

                    else
                    {
                        deltaPos.x = 0;
                    }

                    if (isAnimated)
                    {
                        anim.SetBool("Stop", false);
                    }
                }
                else
                {
                    isMoving = true;
                    if (isAnimated)
                    {
                        anim.SetBool("Stop", false);
                    }
                    
                    playerTransform.transform.Rotate(new Vector3(0, 0, 0));
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("Mouse'ı kaldırdın");
                    transform.DORotate(new Vector3(0,0,0),0.5f);
                }
            }
            #endregion
        }

        #region Reward
        if (rewardCounter == 4)
        {
            if (!isPerfect)
            {
                good.gameObject.SetActive(true);
                good.gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
            }
        }

        else if (rewardCounter == 8)
        {
            if (!isPerfect)
            {
                amazing.gameObject.SetActive(true);
                amazing.gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
            }
        }

        else if (rewardCounter == 14)
        {
            if (!isPerfect)
            {
                perfect.gameObject.SetActive(true);
                perfect.gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
            }
        }

        else
        {
            good.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.25f);
            amazing.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.25f);
            perfect.gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.25f);
        }
        #endregion
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Balance")
        {
            for (var i = boxes.Count - 1; i > -1; i--)
            {
                boxes[i].GetComponent<Rigidbody>().isKinematic = false;
                boxes[i].GetComponent<Collider>().enabled = true;
            }

            balance = true;
            balanceTutorial.gameObject.SetActive(true);
            balanceTutorial.gameObject.transform.DOScale(Vector2.one, 0.5f);
        }

        else if (other.gameObject.tag == "Gem")
        {
            Instantiate(gemParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject.transform.parent.gameObject);
            scoreCount++;
            if (!win)
            {
                puan.text = " " + scoreCount;
            }
            scoreImage.transform.DOScale(new Vector2(5, 5), 0.1f);
            scoreImage.transform.DOScale(new Vector2(2.5f, 2.5f), 0.2f);
            Debug.Log(scoreCount);
            //Debug.Log("Gem");
        }

        if (other.gameObject.tag == "Win")
        {
            if (win)
            {
                scoreCount = boxes.Count * scoreCount;
                puan.text = "" + scoreCount;
            }
            win = true;
            NextLevel.SetActive(true);
            NextLevel.gameObject.transform.DOScale(new Vector2(1, 1), 0.3f);
            kutusayitext.SetText("" + boxes.Count + "x");

            if (win)
            {
                isAnimated = false;
            }
        }

        else if (other.gameObject.tag == "CamRotate")
        {
            playerCamCM.gameObject.SetActive(false);
            rotateCamCM.gameObject.SetActive(true);
        }

        if (other.gameObject.tag == "Move")
        {
            move = true;
            tutorialBool = true;
            tutorial.SetActive(true);
            tutorial.gameObject.transform.DOScale(new Vector2(0.5f, 0.5f), 0.5f);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Destroy")
        {
            playerCamCM.Follow = null;
            Retry.SetActive(true);
            ActivateRagdoll();
        }

        if (other.gameObject.tag == "Surface")
        {
            //lerpTime = Mathf.Clamp(lerpTime + Time.deltaTime * smoothTime, 0f, 1f);
            isPerfect = false;
            rewardCounter += 1;
            //playerCamCM.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(playerCamCM.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, new Vector3(playerCamCM.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x, playerCamCM.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y, playerCamCM.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z - 100f), lerpTime * Time.deltaTime);
            //Invoke("DelayOffset", 1f);
        }

        if (other.gameObject.tag == "SurfaceAnvil")
        {
            Time.fixedDeltaTime = 0.005f;
            Debug.Log(Time.fixedDeltaTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Balance")
        {
            balance = false;
            balanceTutorial.gameObject.transform.DOScale(Vector2.zero, 0.5f);
            //Destroy(balanceTutorial.gameObject, 1f);
        }

        if (other.gameObject.tag == "Surface")
        {
            isPerfect = true;
            perfect.gameObject.transform.DOScale(new Vector2(0, 0), 0.5f);
            //rewardCounter = 0;
        }

        if (other.gameObject.tag == "SurfaceAnvil")
        {
            Debug.Log("SurfaceAnvil");
            Time.fixedDeltaTime = 0.02f;
            Debug.Log(Time.fixedDeltaTime);
        }
    }


    #endregion

    #region Collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Invoke("RagdollDeath", 1.5f);
            ActivateRagdoll();
        }
    }
    #endregion

    #region Methods
    public void FalseReturner()
    {
        isThrowed = false;
    }

    public void EndGame()
    {
        if (boxes.Count != 0)
        {
            for (var i = boxes.Count - 1; i > -1; i--)
            {
                boxes[i].transform.position = Vector3.Lerp(boxes[i].transform.position, new Vector3(boxes[i].transform.position.x, boxes[i].transform.position.y, boxes[i].transform.position.z - 10), 1 * Time.deltaTime);
                boxes[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Instantiate(boxDestroy, transform.position, Quaternion.identity);
                //boxes.Remove(boxes[i]);
                isClear = true;
                instantiated = true;
            }
        }

        isStarted = false;
        isAnimated = false;
        anim.SetBool("Dance", true);
    }

    public void RagdollDeath()
    {
        Retry.SetActive(true);
    }

    public void ActivateRagdoll()
    {
        playerCamCM.gameObject.SetActive(false);
        for (var i = col.Length - 1; i > 0; i--)
        {
            //ragdollJoint[i].enableCollision = true;
            col[i].enabled = true;
            ragdollRb[i].isKinematic = false;
            ragdollRb[i].mass = 0f;
        }
        
        for (var i = boxes.Count - 1; i > -1; i--)
        {
            //boxes[i].transform.position = Vector3.Lerp(boxes[i].transform.position, new Vector3(boxes[i].transform.position.x, boxes[i].transform.position.y, boxes[i].transform.position.z - 10), 1 * Time.deltaTime);
            boxes[i].transform.parent = null;
            boxes[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        playerCol.enabled = false;
        anim.enabled = false;
        ragdoll = true;
    }

    public void DisableRagdoll()
    {
        ragdollRb = GetComponentsInChildren<Rigidbody>();
        col = GetComponentsInChildren<Collider>();
        for (var i = col.Length - 1; i > 0; i--)
        {
            col[i].enabled = false;
            ragdollRb[i].isKinematic = true;
        }
    }
    #endregion
}
