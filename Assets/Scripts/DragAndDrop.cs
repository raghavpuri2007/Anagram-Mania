using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    //letter game object
    private Image letter;
    //variable that holds the original coordinates of current letter
    public Vector3 origin;
    public float height;
    public float width;
    
    //method that runs when the letter is being dragged
    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
        letter.color = new Color32(238, 221, 187, 200);
    }
    //method that runs when the letter is done dragging
    public void OnEndDrag(PointerEventData eventData) {
        letter.color = new Color32(238, 221, 187, 255);
        bool foundDash = false;
        //for each dash check to make sure it is not over another dash
        for(int i = 0; i < MainPlayScript.dashes.Length; i++) {
            if(dashChecker(MainPlayScript.dashes[i])) {
                Debug.Log("HELLO: " + height);
                //checks if letter is in same spot
                replaceLetter(i);
                float yPos = MainPlayScript.dashes[i].transform.position.y + (height/12f);
                if(height < 1000) {
                    yPos = MainPlayScript.dashes[i].transform.position.y + (height/10f);
                }
                letter.transform.position = new Vector3(MainPlayScript.dashes[i].transform.position.x, yPos, 0);
                Debug.Log("Letter Pos: " + letter.transform.position);
                foundDash = true;
            }
        }
        //if letter is not within good distance of dash, then send it back to start
        if(!foundDash) {
            letter.transform.position = origin;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        letter = GetComponent<Image>();
        origin = letter.transform.position;
        height = Screen.height;
        width = Screen.width;
    }
    
    //method that checks if a letter is within good distance of a dash
    private bool dashChecker(GameObject dash) {
        //checks for a good x distance of letter & dash
        bool xCheck = (letter.transform.position.x >= dash.transform.position.x && letter.transform.position.x <= dash.transform.position.x + (width/16)) || (letter.transform.position.x <= dash.transform.position.x && letter.transform.position.x + (width/16) >= dash.transform.position.x);
        //checks for a good y distance of letter & dash
        bool yCheck = (letter.transform.position.y >= dash.transform.position.y && letter.transform.position.y <= dash.transform.position.y + (height/7)) || (letter.transform.position.y <= dash.transform.position.y && letter.transform.position.y + (height/11) >= dash.transform.position.y);
        if(xCheck && yCheck) {
            return true;
        }
        return false;
    }
    //method that makes sure no two letters are in the same position
    private void replaceLetter(int dashIndex) {
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            //if two letters share location
            float yPos = MainPlayScript.dashes[i].transform.position.y + (height/12f);
            if(height < 1000) {
                yPos = MainPlayScript.dashes[i].transform.position.y + (height/10f);
            }
            if(MainPlayScript.letters[i].transform.position.ToString("F8").Equals(new Vector3(MainPlayScript.dashes[dashIndex].transform.position.x, yPos, 0).ToString("F8"))) {
                //send letter back to start
                MainPlayScript.letters[i].transform.position = MainPlayScript.letters[i].GetComponent<DragAndDrop>().origin;
            }
        }
    }
}
