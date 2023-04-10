using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Streakbar Class
public class ProgressBar : MonoBehaviour
{
    //streak goal variable
    public static int maximum;
    //current streak shared with other scripts
    public static float current;
    public Image fill;
    public static int streak;
    //ui game objects
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI bonus;
    //bonus if user hits streak
    public static int currentBonus;
    // Start is called before the first frame update
    void Start()
    {
        maximum = 3;
        streak = 0;
        bonus.enabled = false;
        currentBonus = 500;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
        streakText.text = streak.ToString();
    }

    //fill the streak bar accordingly
    void GetCurrentFill() {
        //if reached maximum
        if(streak == maximum) {
            StartCoroutine(reachedBonus());
            //make new maximum
            maximum+=3;
            //give bonus points
            currentBonus+=500;
        }
        //find the fill amount
        float fillAmount = current / (float) maximum;
        //fill the object
        fill.transform.localScale = new Vector3(fillAmount, 1, 1);
    }

    //coroutine to display message when user hits streak
    IEnumerator reachedBonus() {
        bonus.text = "Bonus: +" + currentBonus;
        bonus.enabled = true;
        //for two secons 
        yield return new WaitForSeconds(2f);
        bonus.enabled = false;
    }
}
