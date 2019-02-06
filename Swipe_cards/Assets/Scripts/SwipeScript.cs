using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeScript : MonoBehaviour
{
    public float swipeTime = 0.5f;
    public float minSwipeDistance = 50f;

    public GameObject touchIcon;

    public Animator swipeAnimator;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeDistance;

    private Vector3 swipeStartPos;
    private Vector3 swipeMovePos;
    private Vector3 swipeEndPos;

    private bool touchCardFlag = false;
    private string swipeDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (touchCardFlag) {
            if (Input.touchCount > 0) {
               
                Touch cardTouch = Input.GetTouch(0);

                if (cardTouch.phase == TouchPhase.Began)
                {
                    swipeStartTime = Time.time;
                    swipeStartPos = cardTouch.position;

                    touchIcon.SetActive(true);
                    
                }

                touchIcon.transform.position = cardTouch.position;

                //Debug.Log(cardTouch.position);
                if (swipeStartPos.x > cardTouch.position.x)
                {
                    SwipeLeft();
                }
                else if (swipeStartPos.x < cardTouch.position.x)
                {
                    SwipeRight();
                }

                //if (cardTouch.phase == TouchPhase.Moved)
                //{
                //    swipeMovePos = cardTouch.position;
                //    if (swipeStartPos.x > swipeMovePos.x)
                //    {
                //        SwipeLeft();
                //    }
                //    else if (swipeStartPos.x < swipeMovePos.x) {
                //        SwipeRight();
                //    }
                //}

                if (cardTouch.phase == TouchPhase.Ended)
                {
                    touchIcon.SetActive(false);

                    touchCardFlag = false;
                }
            }
            
        }

        if (!touchCardFlag)
        {
            if (swipeDirection == "left") {
                swipeAnimator.SetBool("SwipeLeftComplete", true);
                swipeAnimator.SetBool("SwipeLeftFlag", false);
            }else if (swipeDirection == "right")
            {
                swipeAnimator.SetBool("SwipeRightComplete", true);
                swipeAnimator.SetBool("SwipeRightFlag", false);
            }
        }
    }

    private void SwipeLeft()
    {
        swipeDirection = "left";
        swipeAnimator.SetBool("SwipeLeftFlag", true);
        swipeAnimator.SetBool("SwipeRightFlag", false);

        
    }

    private void SwipeRight() {
        swipeDirection = "right";
        swipeAnimator.SetBool("SwipeRightFlag", true);
        swipeAnimator.SetBool("SwipeLeftFlag", false);
    }

    public void CrossClick() {
        Debug.Log("Cross clicked");
    }

    public void TickClick() {
        Debug.Log("Tick Clicked");
    }

    public void TouchEnterCard() {
        //Debug.Log("Touch enter");
        touchCardFlag = true;
    }

    public void TouchExitCard() {
        //Debug.Log("Touch Exit");
        //touchCardFlag = false;
    }
}
