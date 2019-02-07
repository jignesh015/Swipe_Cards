using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwipeScript : MonoBehaviour
{
    //Game variables
    public GameObject frontCardObj;
    public GameObject backCardObj;
    public GameObject canvas;
    public GameObject gameOverCanvas;

    //Sprite variables
    public List<Sprite> cardSpritesList;
    public Sprite tickSprite;
    public Sprite crossSprite;

    //Text variables
    public GameObject IDontKnowTitle;
    public TextMeshProUGUI IDontKnowScore;
    public TextMeshProUGUI IKnowScore;

    //Sprite name array
    public string[] cardSpriteName;
    public string[] dontKnowSpriteName;
    public string dontKnowFlag;

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
    private List<Sprite> cardSprites = new List<Sprite>();
    private List<Sprite> dontKnowCards = new List<Sprite>();

    //Private flags & counters
    private bool touchCardFlag = false;
    private string swipeDirection;
    private bool swipeCompleteFlag = false;
    private int IKnowCounter;

    void Start()
    {
        //Check for saved Data
        CheckSavedData();
    }

    void Update()
    {
        if (touchCardFlag) {
            if (Input.touchCount > 0) {
               
                Touch cardTouch = Input.GetTouch(0);

                if (cardTouch.phase == TouchPhase.Began)
                {
                    swipeStartPos = cardTouch.position;
                }

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
                IKnowCounter = IKnowCounter + 1;
                StartCoroutine(SwipeComplete());
            }
        }
    }

    //Instatiate the cards that are yet to be marked "I know"
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
        frontCardSprite.sprite = crossSprite;
    }

    private void SwipeRight() {
        swipeDirection = "right";
        swipeAnimator.SetBool("SwipeRightFlag", true);
        swipeAnimator.SetBool("SwipeLeftFlag", false);
        frontCardSprite.sprite = tickSprite;
    }

    IEnumerator SwipeComplete() {
        yield return new WaitForSeconds(0.35f);

        touchCardFlag = false;
        Destroy(frontCard);
        Destroy(backCard);
        cardSprites.RemoveAt(0);

        //Update score
        IDontKnowScore.text = dontKnowCards.Count.ToString();
        IKnowScore.text = IKnowCounter.ToString();

        //Save data after every swipe
        UpdateSavedData();
        InstatiateCards();
        
    }

    private void UpdateSavedData() {
        cardSpriteName = new string[20];
        dontKnowSpriteName = new string[20];

        //Save dont know cards
        if (dontKnowCards.Count > 0) {
            for (var i = 0; i < dontKnowCards.Count; i++) {
                dontKnowSpriteName[i] = dontKnowCards[i].name;
            }
        }

        //save remaining cards
        if (cardSprites.Count > 0)
        {
            for (var i = 0; i < cardSprites.Count; i++)
            {
                cardSpriteName[i] = cardSprites[i].name;
            }
        }

        //After all cards are swiped either "I know" or "I don't know"
        if (cardSprites.Count == 0) {
            dontKnowFlag = "allCardsSwiped";
        }

        SaveScript.SaveData(this);
    }

    private void CheckSavedData() {
        SavedData swipeData = SaveScript.LoadData();
        if (swipeData == null)      //If save file doesn't exist
        {
            cardSprites = cardSpritesList;
            IKnowCounter = 0;
            dontKnowFlag = "cardsRemaining";
            InstatiateCards();
        }
        else {                      //If save file exists
            //Fetch cards sprite
            for (var i = 0; i < swipeData.cardSpriteName.Length; i++) {
                if (swipeData.cardSpriteName[i] == null) {
                    break;
                }
                for (var j = 0; j < cardSpritesList.Count; j++) {
                    if (cardSpritesList[j].name == swipeData.cardSpriteName[i]) {
                        cardSprites.Add(cardSpritesList[j]);
                    }
                }
            }

            //Fetch Dont know sprites
            for (var i = 0; i < swipeData.dontKnowSpriteName.Length; i++) {
                if (swipeData.dontKnowSpriteName[i] == null) {
                    break;
                }
                for (var j = 0; j < cardSpritesList.Count; j++)
                {
                    if (cardSpritesList[j].name == swipeData.dontKnowSpriteName[i])
                    {
                        dontKnowCards.Add(cardSpritesList[j]);
                    }
                }
            }


            //If all cards are swiped either "I know" or "I don't know", only show the "I don't know" cards
            if (swipeData.dontKnowFlag == "allCardsSwiped") {
                cardSprites = new List<Sprite>();
            }

            //Fetch scores
            IDontKnowScore.text = swipeData.dontKnowScore;
            IKnowScore.text = swipeData.knowScore;
            IKnowCounter = int.Parse(swipeData.knowScore);
            dontKnowFlag = swipeData.dontKnowFlag;

            InstatiateCards();
        }
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
        touchCardFlag = false;
        swipeCompleteFlag = false;
        swipeAnimator.SetBool("SwipeLeftFlag", true);
        swipeAnimator.SetBool("SwipeRightFlag", false);

        frontCardSprite.sprite = crossSprite;

        StartCoroutine(SetSwipeDirection("left"));
    }

    public void TickClick() {
        touchCardFlag = false;
        swipeCompleteFlag = false;
        swipeAnimator.SetBool("SwipeRightFlag", true);
        swipeAnimator.SetBool("SwipeLeftFlag", false);

        frontCardSprite.sprite = tickSprite;

        StartCoroutine(SetSwipeDirection("right"));
    }

    IEnumerator SetSwipeDirection(string direction){
        yield return new WaitForSeconds(0.32f);
        swipeDirection = direction;
    }

    public void TouchEnterCard() {
        touchCardFlag = true;
        swipeCompleteFlag = false;
    }

    public void TouchExitCard() {
    }

    public void GoToMainScreen() {
        SceneManager.LoadScene("MainScreen");
    }
}
