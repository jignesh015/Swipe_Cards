using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeScript : MonoBehaviour
{
    public float swipeTime = 0.5f;
    public float minSwipeDistance = 50f;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeDistance;

    private Vector3 swipeStartPos;
    private Vector3 swipeEndPos;

    private bool touchCardFlag = false;

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
                }

                if (cardTouch.phase == TouchPhase.Ended)
                {
                    swipeEndTime = Time.time;
                    swipeEndPos = cardTouch.position;

                    swipeDistance = (swipeEndPos - swipeStartPos).magnitude;
                    Debug.Log(swipeDistance);

                    if (swipeDistance > minSwipeDistance)
                    {
                        Swipe();
                    }

                    touchCardFlag = false;
                }
            }
            
        }
    }

    private void Swipe() {
        Debug.Log("Swipe");
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
