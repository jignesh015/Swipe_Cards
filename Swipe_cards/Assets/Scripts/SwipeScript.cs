using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeScript : MonoBehaviour
{
    //Game variables
    public GameObject frontCardObj;
    public GameObject backCardObj;
    public GameObject canvas;
    public GameObject touchIcon;
    public List<Sprite> cardSprites;

    //Private game variables
    private Animator swipeAnimator;
    private GameObject frontCard;
    private GameObject backCard;
    private Vector3 swipeStartPos;
    private Vector3 swipeMovePos;
    private Vector3 swipeEndPos;
    private Vector3 frontCardPos;
    private Vector3 backCardPos;
    private Image frontCardSprite;
    private Image backCardSprite;
    private List<Sprite> dontKnowCards = new List<Sprite>();

    //Private flags
    private bool touchCardFlag = false;
    private string swipeDirection;
    private bool swipeCompleteFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        InstatiateCards();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchCardFlag) {
            if (Input.touchCount > 0) {
               
                Touch cardTouch = Input.GetTouch(0);

                if (cardTouch.phase == TouchPhase.Began)
                {
                    swipeStartPos = cardTouch.position;
                    touchIcon.SetActive(true);
                    
                }

                touchIcon.transform.position = cardTouch.position;
                if (swipeStartPos.x > cardTouch.position.x)
                {
                    SwipeLeft();
                }
                else if (swipeStartPos.x < cardTouch.position.x)
                {
                    SwipeRight();
                }

                if (cardTouch.phase == TouchPhase.Ended)
                {
                    touchIcon.SetActive(false);
                    touchCardFlag = false;
                }
            }
            
        }

        if (!touchCardFlag && !swipeCompleteFlag)
        {
            if (swipeDirection == "left") {
                swipeAnimator.SetBool("SwipeLeftComplete", true);
                swipeAnimator.SetBool("SwipeLeftFlag", false);
                swipeCompleteFlag = true;

                //Add left swiped card to Don't know list
                dontKnowCards.Add(cardSprites[0]);
                StartCoroutine(SwipeComplete());
            }
            else if (swipeDirection == "right")
            {
                swipeAnimator.SetBool("SwipeRightComplete", true);
                swipeAnimator.SetBool("SwipeRightFlag", false);
                swipeCompleteFlag = true;
                StartCoroutine(SwipeComplete());
            }
        }
    }

    private void InstatiateCards() {

        if (cardSprites.Count > 0)
        {
            if (cardSprites.Count > 1)
            {
                backCard = Instantiate(backCardObj, canvas.transform);
                backCard.SetActive(true);
                backCardPos = backCard.transform.position;
                backCardSprite = backCard.GetComponent<Image>();
                backCardSprite.sprite = cardSprites[1];
            }
            frontCard = Instantiate(frontCardObj, canvas.transform);
            frontCard.SetActive(true);
            frontCardPos = frontCard.transform.position;
            frontCardSprite = frontCard.GetComponent<Image>();
            frontCardSprite.sprite = cardSprites[0];
            
            swipeAnimator = frontCard.GetComponent<Animator>();
        }
        else {
            CheckDontKnowCards();
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

    IEnumerator SwipeComplete() {
        yield return new WaitForSeconds(0.35f);

        Destroy(frontCard);
        Destroy(backCard);

        cardSprites.RemoveAt(0);

        InstatiateCards();

    }

    private void CheckDontKnowCards() {
        if (dontKnowCards.Count > 0)
        {
            cardSprites = dontKnowCards;
            InstatiateCards();
        }
        else {
            NoCardsLeft();
        }
    }

    private void NoCardsLeft() {
        Debug.Log(dontKnowCards.Count);
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
        swipeCompleteFlag = false;
    }

    public void TouchExitCard() {
        //Debug.Log("Touch Exit");
        //touchCardFlag = false;
    }
}
