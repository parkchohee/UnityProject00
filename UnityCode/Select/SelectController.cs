using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using LitJson;

public class SelectController : MonoBehaviour
{
    public GameObject[] CharacterSlot = null;

    public List<CharacterJobInfo> CharacterJobInfoList = new List<CharacterJobInfo>();

    void Start ()
    {
        for (int i = 0; i < CharacterSlot.Length; i++)
        {
            CharacterSlot[i].GetComponent<CharacterSlot>().slotNum = i;
        }

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForJobInit;
        httpHelper.get(100, "/AllCharacterJobs");
    }
	
	void Update ()
    {

    }

    void OnHttpRequestForJobInit(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log(www.text);
        }
        else
        {
            JsonData characterData = JsonMapper.ToObject(www.text);
            CharacterSettings(characterData);
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForJobInit;
    }

    void CharacterSettings(JsonData characterData)
    {
        for (int i = 0; i < characterData.Count; i++)
        {
            CharacterJobInfoList.Add(new CharacterJobInfo(
                int.Parse(characterData[i]["JobId"].ToString()),
                characterData[i]["PrefabName"].ToString()
                ));
        }
        
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("USER_ID", GameManager.Instance.LoginId);

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForSlotInit;
        httpHelper.post(100, "/SlotInit", data);
    }

    void OnHttpRequestForSlotInit(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log(www.text);
        }
        else
        {
            Debug.Log(www.text);
            JsonData SlotData = JsonMapper.ToObject(www.text);
            CharacterSlotSetting(SlotData);
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForSlotInit;
    }

    void CharacterSlotSetting(JsonData SlotData)
    {
        for (int i = 0; i < SlotData.Count; i++)
        {
            CharacterSlot characterSlot = null;
            characterSlot = CharacterSlot[int.Parse(SlotData[i]["SlotNum"].ToString())].GetComponent<CharacterSlot>();
            characterSlot.SetCharacterInfo(new CharacterInfo( SlotData[i]["UserId"].ToString(),
                       int.Parse(SlotData[i]["SlotNum"].ToString()),
                       int.Parse(SlotData[i]["CharacterNum"].ToString()),
                       SlotData[i]["Name"].ToString(),
                       int.Parse(SlotData[i]["Job"].ToString()),
                       int.Parse(SlotData[i]["Exp"].ToString()), int.Parse(SlotData[i]["MaxExp"].ToString()),
                       int.Parse(SlotData[i]["Mp"].ToString()), int.Parse(SlotData[i]["MaxMp"].ToString()),
                       int.Parse(SlotData[i]["Hp"].ToString()), int.Parse(SlotData[i]["MaxHp"].ToString()),
                       int.Parse(SlotData[i]["Level"].ToString()),
                       int.Parse(SlotData[i]["SkillLev1"].ToString()), int.Parse(SlotData[i]["SkillLev2"].ToString()), int.Parse(SlotData[i]["SkillLev3"].ToString())));

            characterSlot.GetComponentInChildren<Animation>().gameObject.SetActive(false);
        }

        for (int i = 0; i < CharacterSlot.Length; i++)
        {
            CharacterSlot slot = CharacterSlot[i].GetComponent<CharacterSlot>();

            if (slot.GetCharacterInfo() == null)
                continue;

            for (int jobInfoIndex = 0; jobInfoIndex < CharacterJobInfoList.Count; jobInfoIndex++)
            {
                if (slot.GetCharacterInfo().JobId == CharacterJobInfoList[jobInfoIndex].JobId)
                {
                    slot.SetCharacterModel(CharacterJobInfoList[jobInfoIndex].PrefabName);
                }
            }
        }
    }

    void ExitBtn()
    {
        SceneManager.LoadScene("LoginScene");
    }

    void CreateBtn()
    {
        GameManager.Instance.NextSceneName = "CharacterCreateScene";
        SceneManager.LoadScene("LoadingScene");
    }

    void StartBtn()
    {
        GameManager.Instance.CharacterNum = CharacterSlot[GameManager.Instance.SelectSlotNum].GetComponent<CharacterSlot>().GetCharacterInfo().CharacterNum;
        GameManager.Instance.NextSceneName = "PlayScene";
        SceneManager.LoadScene("LoadingScene");
    }

    void DeleteBtn()
    {
        Debug.Log("DeleteBtn");
    }
}
