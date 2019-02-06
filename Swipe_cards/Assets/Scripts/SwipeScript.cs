using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeScript : MonoBehaviour
{
    //Game variables
    public GameObject frontCardObj;
    public GameObject backCardObj;
    public GameObject canvas;
    public GameObject gameOverCanvas;
    public GameObject touchIconObj;
    public List<Sprite> cardSprites;

    //Text variables
    public GameObject IDontKnowTitle;
    public TextMeshProUGUI IDontKnowScore;
    public TextMeshProUGUI IKnowScore;

    //Private game variables
    private Animator swipeAnimator;
    private GameObject frontCard;
    private GameObject backCard;
    private GameObject touchIcon;
    private Vector3 swipeStartPos;
    private Vector3 swipeMovePos;
    private Vector3 swipeEndPos;
    private Vector3 frontCardPos;
    private Vector3 backCardPos;
    private Image frontCardSprite;
    private Image backCardSprite;
    private List<Sprite> dontKnowCards = new List<Sprite>();

    //Private flags & counters
    private bool touchCardFlag = false;
    private string swipeDirection;
    private bool swipeCompleteFlag = false;
    private int IKnowCounter = 0;
    private bool crossClicked = false;
    private bool tickClicked = false;

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
                    Destroy(touchIcon);
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
                swipeDirection = "";

                //Add left swiped card to Don't know list
                dontKnowCards.Add(cardSprites[0]);
                StartCoroutine(SwipeComplete());
            }
            else if (swipeDirection == "right")
            {
                swipeAnimator.SetBool("SwipeRightComplete", true);
                swipeAnimator.SetBool("SwipeRightFlag", false);
                swipeCompleteFlag = true;
                swipeDirection = "";

                //Update right swipe counter
                IKnowCounter += 1;
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

        touchCardFlag = false;

        Destroy(frontCard);
        Destroy(backCard);
        Destroy(touchIcon);

        cardSprites.RemoveAt(0);

        //Update score
        IDontKnowScore.text = dontKnowCards.Count.ToString();
        IKnowScore.text = IKnowCounter.ToString();

        InstatiateCards();
        
    }

    private void CheckDontKnowCards() {
        if (dontKnowCards.Count > 0)
        {
            IDontKnowTitle.SetActive(true);
            cardSprites = dontKnowCards;
            InstatiateCards();
        }
        else {
            NoCardsLeft();
        }
    }

    private void NoCardsLeft() {
        touchCardFlag = false;
        swipeCompleteFlag = true;
        Destroy(canvas);
        gameOverCanvas.SetActive(true);
    }


    public void CrossClick() {
        Debug.Log("Clicked cross");
        touchCardFlag = false;
        swipeCompleteFlag = false;
        swipeAnimator.SetBool("SwipeLeftFlag", true);
        swipeAnimator.SetBool("SwipeRightFlag", false);

        StartCoroutine(SetSwipeDirection("left"));
    }

    public void TickClick() {
        touchCardFlag = false;
        swipeCompleteFlag = false;
        swipeAnimator.SetBool("SwipeRightFlag", true);
        swipeAnimator.SetBool("SwipeLeftFlag", false);

        StartCoroutine(SetSwipeDirection("right"));
    }

    IEnumerator SetSwipeDirection(string direction){
        yield return new WaitForSeconds(0.32f);
        swipeDirection = direction;
    }

    public void TouchEnterCard() {
        Destroy(touchIcon);
        touchIcon = Instantiate(touchIconObj, canvas.transform);
        touchCardFlag = true;
        swipeCompleteFlag = false;
    }

    public void TouchExitCard() {
    }
}
