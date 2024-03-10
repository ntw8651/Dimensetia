using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectionUI : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public GameObject selectionBackground;
    public GameObject selectionText;
    public GameObject dialogController;

    public void OnPointerClick(PointerEventData eventData)
    {
        char name = transform.name[transform.name.Length-1];
        dialogController.GetComponent<DialogController>().DF_ViewSelectionsReturn((int) char.GetNumericValue(name));
    }

    //그럼 UI에다가 raycastGraphic컨트롤러를 만들어두고 UI오브젝트들에는 Interface 상속시켜주는 식으로 할까? 아니 이거 있는데 이미?
}
