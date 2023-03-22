using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ProgressBar : MonoBehaviour
{
    public static int maximum;
    public static float current;
    public Image fill;
    public static int streak;
    public TextMeshProUGUI streakText;
    public TextMeshProUGUI bonus;
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

    void GetCurrentFill() {
        if(streak == maximum) {
            StartCoroutine(reachedBonus());
            maximum+=3;
            currentBonus+=500;
        }
        float fillAmount = current / (float) maximum;
        fill.transform.localScale = new Vector3(fillAmount, 1, 1);
    }

    IEnumerator reachedBonus() {
        bonus.text = "Bonus: +" + currentBonus;
        bonus.enabled = true;
        yield return new WaitForSeconds(2f);
        bonus.enabled = false;
        //still need to increase score
    }
}
