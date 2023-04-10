using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Image letter;
    public Vector3 origin;
    

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
        letter.color = new Color32(123, 0, 226, 100);
    }
    public void OnEndDrag(PointerEventData eventData) {
        letter.color = new Color32(123, 0, 226, 150);
        bool foundDash = false;
        for(int i = 0; i < MainPlayScript.dashes.Length; i++) {
            if(dashChecker(MainPlayScript.dashes[i])) {
                //checks if letter is in same spot
                replaceLetter(i);
                letter.transform.position = new Vector3(MainPlayScript.dashes[i].transform.position.x, MainPlayScript.dashes[i].transform.position.y + 150, 0);
                foundDash = true;
            }
        }
        if(!foundDash) {
            letter.transform.position = origin;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        letter = GetComponent<Image>();
        origin = letter.transform.position;
    }
    

    private bool dashChecker(GameObject dash) {
        bool xCheck = (letter.transform.position.x >= dash.transform.position.x && letter.transform.position.x <= dash.transform.position.x + 150) || (letter.transform.position.x <= dash.transform.position.x && letter.transform.position.x + 150 >= dash.transform.position.x);
        bool yCheck = (letter.transform.position.y >= dash.transform.position.y && letter.transform.position.y <= dash.transform.position.y + 225) || (letter.transform.position.y <= dash.transform.position.y && letter.transform.position.y + 150 >= dash.transform.position.y);
        if(xCheck && yCheck) {
            return true;
        }
        return false;
    }

    private void replaceLetter(int dashIndex) {
        for(int i = 0; i < MainPlayScript.letters.Length; i++) {
            if(MainPlayScript.letters[i].transform.position == new Vector3(MainPlayScript.dashes[dashIndex].transform.position.x, MainPlayScript.dashes[dashIndex].transform.position.y + 150, 0)) {
                MainPlayScript.letters[i].transform.position = MainPlayScript.letters[i].GetComponent<DragAndDrop>().origin;
            }
        }
    }
}
