using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using LitJson;

public class CreateSceneController : MonoBehaviour {

    public List<CharacterJobInfo> CharacterJobInfoList = new List<CharacterJobInfo>();

    public Text JobName;
    public Text JobDescription;
    public InputField InputName;

    public SkillDescription skill1;
    public SkillDescription skill2;
    public SkillDescription skill3;

    public GameObject CharacterModel;

    int CharNum = 0;

    void Start () {
        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForJobInit;
        httpHelper.get(100, "/AllCharacterJobs");
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
                characterData[i]["Name"].ToString(),
                characterData[i]["PrefabName"].ToString(),
                characterData[i]["Description"].ToString()
                ));
        }

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForSkillInit;
        httpHelper.get(100, "/AllCharacterSkills");
    }

    void OnHttpRequestForSkillInit(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log(www.text);
        }
        else
        {
            JsonData SkillData = JsonMapper.ToObject(www.text);
            SkillSettings(SkillData);
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForSkillInit;
    }

    void SkillSettings(JsonData SkillData)
    {
        for (int i = 0; i < SkillData.Count; i++)
        {
            for (int jobListIndex = 0; jobListIndex < CharacterJobInfoList.Count; jobListIndex++)
            {
                if (CharacterJobInfoList[jobListIndex].JobId != int.Parse(SkillData[i]["JobId"].ToString()))
                    continue;

                CharacterJobInfoList[jobListIndex].AddSkill(new CharacterSkill(
                    int.Parse(SkillData[i]["Id"].ToString()),
                    SkillData[i]["Name"].ToString(),
                    SkillData[i]["Description"].ToString(), // TODO : Description으로 변경
                    SkillData[i]["ImageName"].ToString(),
                    int.Parse(SkillData[i]["Skilltype"].ToString()),
                    int.Parse(SkillData[i]["SkillPower"].ToString()),
                    int.Parse(SkillData[i]["JobId"].ToString()),
                    int.Parse(SkillData[i]["SlotNum"].ToString()),
                    int.Parse(SkillData[i]["Mp"].ToString())
                    ));
            }
        }

        SetCharacter(CharNum);
    }

    void SetCharacter(int charNum)
    {
        JobName.text = CharacterJobInfoList[charNum].Name;
        JobDescription.text = CharacterJobInfoList[charNum].Description;

        skill1.SkillImage.image.sprite = Resources.Load<Sprite>("Images/Skills/" + CharacterJobInfoList[charNum].CharacterSkillList[0].ImageName);
        skill2.SkillImage.image.sprite = Resources.Load<Sprite>("Images/Skills/" + CharacterJobInfoList[charNum].CharacterSkillList[1].ImageName);
        skill3.SkillImage.image.sprite = Resources.Load<Sprite>("Images/Skills/" + CharacterJobInfoList[charNum].CharacterSkillList[2].ImageName);

        skill1.Name.text = CharacterJobInfoList[charNum].CharacterSkillList[0].Name;
        skill2.Name.text = CharacterJobInfoList[charNum].CharacterSkillList[1].Name;
        skill3.Name.text = CharacterJobInfoList[charNum].CharacterSkillList[2].Name;

        skill1.Desc.text = CharacterJobInfoList[charNum].CharacterSkillList[0].Description ;
        skill2.Desc.text = CharacterJobInfoList[charNum].CharacterSkillList[1].Description;
        skill3.Desc.text = CharacterJobInfoList[charNum].CharacterSkillList[2].Description;

        if (CharacterModel != null)
            Destroy(CharacterModel);

        GameObject itemObj = Resources.Load<GameObject>("Prefabs/Create/" + CharacterJobInfoList[charNum].PrefabName);
        CharacterModel = Instantiate(itemObj, new Vector3(0f, 0f, 0f), Quaternion.Euler(0.0f, 180.0f, 0.0f)) as GameObject;
    }

    void PrevBtn()
    {
        CharNum--;

        if (CharNum < 0)
            CharNum = CharacterJobInfoList.Count - 1;

        SetCharacter(CharNum);
    }

    void NextBtn()
    {
        CharNum++;

        if (CharNum >= CharacterJobInfoList.Count)
            CharNum = CharNum % CharacterJobInfoList.Count;

        SetCharacter(CharNum);
    }

    void CharacterCreateBtn()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("JOB", CharacterJobInfoList[CharNum].JobId.ToString());
        data.Add("NAME", InputName.text);
        data.Add("SLOT_NUM", GameManager.Instance.SelectSlotNum.ToString());
        data.Add("LOGIN_ID", GameManager.Instance.LoginId);

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCreateCharacter;
        httpHelper.post(100, "/CharacterCreate", data);
    }

    void OnHttpRequestForCreateCharacter(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log(www.text);
            // >> :TODO 에러 메세지 팝업
        }
        else
        {
            Debug.Log(www.text);
            // >> : TODO slot selectSceneㅇ로 넘어간다
            GameManager.Instance.NextSceneName = "SlotSelectScene";
            SceneManager.LoadScene("LoadingScene");
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForCreateCharacter;
    }
}
