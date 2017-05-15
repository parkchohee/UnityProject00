using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    CharacterInfo characterInfo = null;
    GameObject characterModel = null;

    Animation BoxAnimation = null;

    public Button btnStart;
    public Button btnCreate;
    public Button btnDelete;

    public int slotNum;

	void Start ()
    {
        btnStart.gameObject.SetActive(false);
        btnCreate.gameObject.SetActive(false);
        btnDelete.gameObject.SetActive(false);

        BoxAnimation = this.gameObject.GetComponentInChildren<Animation>();
        BoxAnimation.Stop();
    }
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject PickObj = GameManager.MousePick;

            if (PickObj == this.gameObject)
                SelectSlot();
            else
                BoxAnimation.Stop();
        }
    }

    void SelectSlot()
    {
        BoxAnimation.Play();

        if (characterInfo == null)
        {
            btnCreate.gameObject.SetActive(true);
            btnStart.gameObject.SetActive(false);
            btnDelete.gameObject.SetActive(false);
        }
        else
        {
            btnStart.gameObject.SetActive(true);
            btnDelete.gameObject.SetActive(true);
            btnCreate.gameObject.SetActive(false);
        }

        GameManager.Instance.SelectSlotNum = slotNum;
    }

    public void SetCharacterInfo(CharacterInfo _characterInfo)
    {
        characterInfo = _characterInfo;
    }

    public CharacterInfo GetCharacterInfo()
    {
        return characterInfo;
    }

    public void SetCharacterModel(string _modelName)
    {
        GameObject itemObj = Resources.Load<GameObject>("Prefabs/Select/" + _modelName);
        characterModel = Instantiate(itemObj, this.gameObject.transform.position, Quaternion.Euler(0.0f, 180.0f, 0.0f)) as GameObject;
        characterModel.transform.SetParent(this.gameObject.transform);
    }
}
