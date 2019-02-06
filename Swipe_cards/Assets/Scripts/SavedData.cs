using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedData {
    public string dontKnowScore;
    public string knowScore;
    public string[] cardSpriteName;
    public string[] dontKnowSpriteName;

    public SavedData(SwipeScript swipeData) {
        dontKnowScore = swipeData.IDontKnowScore.text;
        knowScore = swipeData.IKnowScore.text;

        cardSpriteName = swipeData.cardSpriteName;
        dontKnowSpriteName = swipeData.dontKnowSpriteName;
    }

}
