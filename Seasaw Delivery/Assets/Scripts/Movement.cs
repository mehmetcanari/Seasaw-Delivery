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

    public float moveSmoother;
    public float moveSpeed;
    public float impulsePower;
    public float speed;
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
    public bool instantiated = false;

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
    public TextMeshProUGUI perfect;
    #endregion


    void Start()
    {
        #region Calling Functions
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isAnimated = true;
        isStarted = false;
        isEnded = false;
        #endregion
    }


    void Update()
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
                var movement = new Vector3(Mathf.Lerp(rb.transform.position.x, rb.transform.position.x + (deltaPos.x / Screen.width) * moveSpeed, Time.deltaTime * moveSmoother), transform.position.y, transform.position.z);
                rb.transform.position = movement;
                startPos = Input.mousePosition;

                if (isAnimated)
                {
                    anim.SetBool("Stop", true);
                }
            }
            else
            {
                isMoving = true;
                if (isAnimated)
                {
                    anim.SetBool("Stop", false);
                }
            }
            #endregion

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

        if (rewardCounter == 4)
        {
            perfect.gameObject.SetActive(true);
        }

        if (rewardCounter == 9)
        {
            perfect.gameObject.SetActive(true);
        }

        if (rewardCounter == 14)
        {
            perfect.gameObject.SetActive(true);
        }
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Balance")
        {
            balance = true;
            balanceTutorial.gameObject.SetActive(true);
            balanceTutorial.gameObject.transform.DOScale(Vector2.one, 0.5f);
        }
        else if (other.gameObject.tag == "Gem")
        {
            Instantiate(gemParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject.transform.parent.gameObject);
            scoreCount++;
            puan.text = " " + scoreCount;
            scoreImage.transform.DOScale(new Vector2(5, 5), 0.1f);
            scoreImage.transform.DOScale(new Vector2(2.5f, 2.5f), 0.2f);
            Debug.Log(scoreCount);
            //Debug.Log("Gem");
        }
        else if (other.gameObject.tag == "Win")
        {
            win = true;

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
        }
        if (other.gameObject.tag == "Fall")
        {
            rewardCounter += 1;
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
    #endregion
}
