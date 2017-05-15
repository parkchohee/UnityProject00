using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public GameObject NPCPopup;
    public GameObject PlayerPopup;

    bool IsEnabled = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!IsEnabled)
            return;

        if (NPCPopup == null)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NPCPopup.SetActive(true);

            if (PlayerPopup != null)
            {
                PlayerPopup.SetActive(true);
                PlayerPopup.transform.position = NPCPopup.transform.position 
                    + new Vector3(NPCPopup.GetComponent<RectTransform>().rect.width / 2, 0, 0)
                    + new Vector3(PlayerPopup.GetComponent<RectTransform>().rect.width / 2, 0, 0);
            }
        }
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsEnabled = false;
        }
    }
}
