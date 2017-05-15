using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LitJson;

public class PlaySceneController : MonoBehaviour
{
    int LoadingPercent = 0;

    GameObject player;
    CharacterInfo characterInfo;
    PlaySceneUIController uiController;

    public List<Item> allItemList;
    public List<Item> playerItemList;

    public List<EnemyInfo> EnemyInfoList;

    void Start ()
    {
        uiController = gameObject.GetComponent<PlaySceneUIController>();

        allItemList = new List<Item>();
        playerItemList = new List<Item>();
        EnemyInfoList = new List<EnemyInfo>();

        LoadData();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void LoadData()
    {
        // >> : characternum을 이용해 정보를 가져온다.
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCharacterInit;
        httpHelper.post(100, "/CharacterInfoByCharacterNum", data);
    }

    void OnHttpRequestForCharacterInit(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else
        {
            JsonData characterData = JsonMapper.ToObject(www.text);
            SetCharacterInfo(characterData);
           
            // >> : jobID 이용해 정보를 가져온다.
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("JOB_ID", characterData[0]["Job"].ToString());

            Http httpHelper = Http.Instance;
            httpHelper.OnHttpRequest += OnHttpRequestForJob;
            httpHelper.post(100, "/JobInfoByJobId", data);
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForCharacterInit;

    }

    void OnHttpRequestForJob(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else
        {
            Debug.Log(www.text);
            JsonData jobData = JsonMapper.ToObject(www.text);
            SetJobInfo(jobData);
          
          
            // >> : jobID 이용해 스킬 정보를 가져온다.
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("JOB_ID", jobData[0]["JobId"].ToString());

            Http httpHelper = Http.Instance;
            httpHelper.OnHttpRequest += OnHttpRequestForSkill;
            httpHelper.post(100, "/SkillInfoByJobId", data);

        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForJob;

    }

    void OnHttpRequestForSkill(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else
        {
            Debug.Log(www.text);
            JsonData skillData = JsonMapper.ToObject(www.text);
            SetSkills(skillData);
           
            Http httpHelper = Http.Instance;
            httpHelper.OnHttpRequest += OnHttpRequestForAllItemList;
            httpHelper.get(100, "/AllItemList");

        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForSkill;

    }

    void OnHttpRequestForAllItemList(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else
        {
            Debug.Log(www.text);

            // >> : item 셋팅
            JsonData AllItemData = JsonMapper.ToObject(www.text);
            SetAllItems(AllItemData);
           
            // << : 
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());

            Http httpHelper = Http.Instance;
            httpHelper.OnHttpRequest += OnHttpRequestForPlayerItemList;
            httpHelper.post(100, "/ItemListByCharacterNum", data);
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForAllItemList;
    }

    void OnHttpRequestForPlayerItemList(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else
        {
            Debug.Log(www.text);

            // >> : inventory 셋팅
            JsonData PlayerItemData = JsonMapper.ToObject(www.text);
            SetPlayerItems(PlayerItemData);
            // << : 
            Http httpHelper = Http.Instance;
            httpHelper.OnHttpRequest += OnHttpRequestForMonsterList;
            httpHelper.get(100, "/MonsterList");
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForPlayerItemList;
    }

    void OnHttpRequestForMonsterList(int id, WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else
        {
            Debug.Log(www.text);

            JsonData MonsterData = JsonMapper.ToObject(www.text);
            SetMonsterList(MonsterData);
            
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForMonsterList;
    }






    // SETTINGS...

    void SetCharacterInfo(JsonData characterData)
    {
        characterInfo = new CharacterInfo();

        characterInfo.CharacterName = characterData[0]["Name"].ToString();

        characterInfo.CurrentExp = int.Parse(characterData[0]["Exp"].ToString());
        characterInfo.MaxExp = int.Parse(characterData[0]["MaxExp"].ToString());
        characterInfo.CurrentMp = int.Parse(characterData[0]["Mp"].ToString());
        characterInfo.MaxMp = int.Parse(characterData[0]["MaxMp"].ToString());
        characterInfo.CurrentHp = int.Parse(characterData[0]["Hp"].ToString());
        characterInfo.MaxHp = int.Parse(characterData[0]["MaxHp"].ToString());
        characterInfo.JobId = int.Parse(characterData[0]["Job"].ToString());
        characterInfo.skillLev[0] = int.Parse(characterData[0]["SkillLev1"].ToString());
        characterInfo.skillLev[1] = int.Parse(characterData[0]["SkillLev2"].ToString());
        characterInfo.skillLev[2] = int.Parse(characterData[0]["SkillLev3"].ToString());
        characterInfo.SkillPoint = int.Parse(characterData[0]["SkillPoint"].ToString());
        characterInfo.Money = int.Parse(characterData[0]["Money"].ToString());
        characterInfo.AttackPower = int.Parse(characterData[0]["AttackPower"].ToString());
        characterInfo.Level = int.Parse(characterData[0]["Level"].ToString());
        characterInfo.SpawnPoint = new Vector3(float.Parse(characterData[0]["X"].ToString()),
            float.Parse(characterData[0]["Y"].ToString()),
            float.Parse(characterData[0]["Z"].ToString()));
    }

    void SetJobInfo(JsonData jobData)
    {
        characterInfo.JobInfo.JobId = int.Parse(jobData[0]["JobId"].ToString());
        characterInfo.JobInfo.PrefabName = jobData[0]["PrefabName"].ToString();
        characterInfo.JobInfo.Name = jobData[0]["Name"].ToString();
        characterInfo.JobInfo.Description = jobData[0]["Description"].ToString();

        uiController.PlayerInfoSlotSetting(characterInfo.JobInfo.PrefabName, characterInfo.Level);

        CreatePlayer();
    }

    void CreatePlayer()
    {
        // >> : PrefabName을 이용해 플레이어 생성..
        GameObject itemObj = Resources.Load<GameObject>("Prefabs/Play/" + characterInfo.JobInfo.PrefabName);
        player = Instantiate(itemObj, characterInfo.SpawnPoint, Quaternion.Euler(0.0f, 180.0f, 0.0f)) as GameObject;
        player.name = "Player";

        PlayerController pc = player.GetComponent<PlayerController>();
        pc.AttackPower = characterInfo.AttackPower;

        Health health = player.GetComponent<Health>();
        health.GaugeBar = GameObject.Find("HealthBar").GetComponent<Image>();
        health.SetGauge(characterInfo.MaxHp, characterInfo.CurrentHp);
        health.isLocalPlayer = true;

        Mana mana = player.GetComponent<Mana>();
        mana.GaugeBar = GameObject.Find("ManaBar").GetComponent<Image>();
        mana.SetGauge(characterInfo.MaxMp, characterInfo.CurrentMp);

        Exp exp = player.GetComponent<Exp>();
        exp.GaugeBar = GameObject.Find("ExpBar").GetComponent<Image>();
        exp.SetGauge(characterInfo.MaxExp, characterInfo.CurrentExp);
        
        CameraController camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        camera.init();
    }

    void SetSkills(JsonData skillData)
    {
        for (int i = 0; i < skillData.Count; i++)
        {
            characterInfo.JobInfo.AddSkill(new CharacterSkill(int.Parse(skillData[i]["Id"].ToString()),
                skillData[i]["Name"].ToString(),
                skillData[i]["Description"].ToString(),
                skillData[i]["ImageName"].ToString(),
                int.Parse(skillData[i]["Skilltype"].ToString()),
                int.Parse(skillData[i]["SkillPower"].ToString()),
                int.Parse(skillData[i]["JobId"].ToString()),
                int.Parse(skillData[i]["SlotNum"].ToString()),
                int.Parse(skillData[i]["Mp"].ToString()),
                 characterInfo.skillLev[i]
                ));
        }

        // >> : 스킬 이미지 셋팅
        uiController.SkillUISetting(characterInfo.JobInfo.CharacterSkillList);
        uiController.SkillPointSetting(characterInfo.SkillPoint);
        // << :

    }

    void SetAllItems(JsonData AllItemData)
    {
        for (int i = 0; i < AllItemData.Count; i++)
        {
            allItemList.Add(new Item(int.Parse(AllItemData[i]["ItemId"].ToString()),
                AllItemData[i]["Name"].ToString(),
                AllItemData[i]["PrefabName"].ToString(),
                int.Parse(AllItemData[i]["Price"].ToString()),
                int.Parse(AllItemData[i]["ItemType"].ToString()),
                int.Parse(AllItemData[i]["ItemPower"].ToString())));
        }

        uiController.StoreUISetting(allItemList);

    }

    void SetPlayerItems(JsonData PlayerItemData)
    {
        for (int i = 0; i < PlayerItemData.Count; i++)
        {
            playerItemList.Add(new Item(int.Parse(PlayerItemData[i]["ItemId"].ToString()),
                PlayerItemData[i]["Name"].ToString(),
                PlayerItemData[i]["PrefabName"].ToString(),
                int.Parse(PlayerItemData[i]["Price"].ToString()),
                int.Parse(PlayerItemData[i]["ItemType"].ToString()),
                int.Parse(PlayerItemData[i]["ItemPower"].ToString()),
                int.Parse(PlayerItemData[i]["ItemCount"].ToString()),
                int.Parse(PlayerItemData[i]["ItemSlotNum"].ToString())
                ));
        }

        uiController.InventoryUISetting(playerItemList);
        uiController.InventoryMoneySetting(characterInfo.Money);
    }

    void SetMonsterList(JsonData MonsterData)
    {
        for(int i = 0; i < MonsterData.Count; i++)
        {
            EnemyInfoList.Add(new EnemyInfo(int.Parse(MonsterData[i]["ID"].ToString()),
                MonsterData[i]["Name"].ToString(),
                MonsterData[i]["PrefabName"].ToString(),
                int.Parse(MonsterData[i]["Exp"].ToString()),
                int.Parse(MonsterData[i]["Hp"].ToString()),
                int.Parse(MonsterData[i]["Power"].ToString())));
        }

        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        for (int i = 0;i<enemySpawners.Length;i++)
        {
            EnemySpawner es = enemySpawners[i].GetComponent<EnemySpawner>();

            for (int j = 0; j < EnemyInfoList.Count; j++)
            {
                if (EnemyInfoList[j].PrefabName == es.enemy.name)
                {
                    es.SetEnemyInfo(EnemyInfoList[j]);
                }
            }
            es.SpawnEnemies();
        }

        //EnemySpawner enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        //enemySpawner.SetEnemyInfo(EnemyInfoList[0]);
        //enemySpawner.SpawnEnemies();
        //Debug.Log(enemySpawner.enemy.name);
    }








    // USE ITEM...
    public bool UseItem(int itemId)
    {
        int findIndex = -1;

        for (int i = 0; i < playerItemList.Count; i++)
        {
            if (playerItemList[i].ItemID == itemId)
            {
                findIndex = i;
                break;
            }
        }

        if (findIndex < 0)
            return false;

        if (playerItemList[findIndex].ItemCount < 1)
            return false;

        switch (playerItemList[findIndex].ItemType)
        {
            case Item.ITEM_TYPE.HEALTH_ITEM:
                player.GetComponent<Health>().IncreaseGauge(playerItemList[findIndex].ItemPower);
                break;
            case Item.ITEM_TYPE.MANA_ITEM:
                player.GetComponent<Mana>().IncreaseGauge(playerItemList[findIndex].ItemPower);
                break;
        }

        playerItemList[findIndex].ItemCount--;
        uiController.InventoryUseUpdate(playerItemList[findIndex].ItemSlotNum);


        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());
        data.Add("ITEM_ID", playerItemList[findIndex].ItemID.ToString());
        data.Add("ITEM_COUNT", playerItemList[findIndex].ItemCount.ToString());

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
        httpHelper.post(100, "/UpdateItem", data);


        return true;
    }

    public bool UseSkill(int skillSlotNum)
    {
        Mana mana = player.GetComponent<Mana>();
        CharacterSkill skill = characterInfo.JobInfo.CharacterSkillList[skillSlotNum];

        if (mana.CurrentGauge < skill.Mp)
            return false;

        mana.IncreaseGauge(skill.Mp * -1);
       
        PlayerController pc = player.GetComponent<PlayerController>();

        switch (skill.SlotNum)
        {
            case 0:
                pc.Skill_1();
                break;
            case 1:
                pc.Skill_2();
                break;
            case 2:
                pc.Skill_3();
                break;
        }

        return true;
    }

    public void SkillLevelUp(int skillSlotNum)
    {
        if (characterInfo.SkillPoint < 1)
            return;
        
        characterInfo.skillLev[skillSlotNum]++;
        characterInfo.SkillPoint--;

        uiController.SkillLevelSetting(skillSlotNum, characterInfo.skillLev[skillSlotNum]);
        uiController.SkillPointSetting(characterInfo.SkillPoint);

        SkillChange();
    }

    public void SkillLevelDown(int skillSlotNum)
    {
        if (characterInfo.skillLev[skillSlotNum] < 1)
            return;

        characterInfo.skillLev[skillSlotNum]--;
        characterInfo.SkillPoint++;

        uiController.SkillLevelSetting(skillSlotNum, characterInfo.skillLev[skillSlotNum]);
        uiController.SkillPointSetting(characterInfo.SkillPoint);

        SkillChange();
    }

    public void SkillChange()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());
        data.Add("SKILL_LEV_1", characterInfo.skillLev[0].ToString());
        data.Add("SKILL_LEV_2", characterInfo.skillLev[1].ToString());
        data.Add("SKILL_LEV_3", characterInfo.skillLev[2].ToString());
        data.Add("SKILL_POINT", characterInfo.SkillPoint.ToString());

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
        httpHelper.post(100, "/Skill", data);
    }

    public void GetMoney(int money)
    {
        characterInfo.Money += money;
        uiController.InventoryMoneySetting(characterInfo.Money);

        // << : 
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());
        data.Add("MONEY", characterInfo.Money.ToString());

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
        httpHelper.post(100, "/Money", data);
    }

    void OnHttpRequestForCharInfoUpdate(int id, WWW www)    // : /Money, /Exp 여기로들어옴
    {
        if (www.error != null)
        {
            Debug.Log("[Error] " + www.error);
        }
        else
        {
            Debug.Log(www.text);
        }

        Http.Instance.OnHttpRequest -= OnHttpRequestForCharInfoUpdate;
    }


    public void GetItem()
    {

    }

    public void ExpUp(int currentExp)
    {
        characterInfo.CurrentExp = currentExp;

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());
        data.Add("EXP", characterInfo.CurrentExp.ToString());

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
        httpHelper.post(100, "/Exp", data);
    }

    public void LevelUp(int currentExp)
    {
        characterInfo.Level++;
        uiController.PlayerInfoSlotSetting(characterInfo.JobInfo.PrefabName, characterInfo.Level);

        // 체력높이고, 마력높이고, 공격력높이고 스킬포인트 올려주고 그런거..
        characterInfo.MaxExp += 10;
        characterInfo.CurrentExp = currentExp;
        characterInfo.MaxHp += 5;
        characterInfo.CurrentHp = characterInfo.MaxHp;
        characterInfo.MaxMp += 5;
        characterInfo.CurrentMp = characterInfo.MaxMp;
        characterInfo.SkillPoint++;
        // uiController도 업데이트 해주고..
        player.GetComponent<Health>().SetGauge(characterInfo.MaxHp, characterInfo.CurrentHp);
        player.GetComponent<Mana>().SetGauge(characterInfo.MaxMp, characterInfo.CurrentMp);
        player.GetComponent<Exp>().SetGauge(characterInfo.MaxExp, characterInfo.CurrentExp);
        uiController.SkillPointSetting(characterInfo.SkillPoint);
        
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());
        data.Add("LEV", characterInfo.Level.ToString());
        data.Add("EXP", characterInfo.CurrentExp.ToString());
        data.Add("MAX_EXP", characterInfo.MaxExp.ToString());
        data.Add("MAX_HP", characterInfo.MaxHp.ToString());
        data.Add("MAX_MP", characterInfo.MaxMp.ToString());
        data.Add("SKILL_POINT", characterInfo.SkillPoint.ToString());

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
        httpHelper.post(100, "/Lev", data);
    }

    public bool BuyItem(int itemId, int count)
    {
        int findIndexAllItemList = -1;

        for(int i = 0; i< allItemList.Count;i++)
        {
            if (allItemList[i].ItemID == itemId)
            {
                findIndexAllItemList = i;
            }
        }

        if (findIndexAllItemList < 0)
            return false;

        if (allItemList[findIndexAllItemList].Price * count > characterInfo.Money)
        {
            uiController.Warning("돈이 부족합니다.");
            return false;
        }

        int slotNum = uiController.InventoryAdd(allItemList[findIndexAllItemList], count);

        if (slotNum < 0)//슬롯꽉찬거..
        {
            uiController.Warning("아이템 슬롯이 부족합니다.");
            return false;
        }

        int findIndexPlayerItemList = -1;

        for (int i = 0; i < playerItemList.Count; i++)
        {
            if (playerItemList[i].ItemID == itemId)
                findIndexPlayerItemList = i;
        }
          
        if (findIndexPlayerItemList < 0)
        {
            playerItemList.Add(allItemList[findIndexAllItemList]);
            playerItemList[playerItemList.Count - 1].ItemCount = count;
            playerItemList[playerItemList.Count - 1].ItemSlotNum = slotNum;
            
        }

        GetMoney(allItemList[findIndexAllItemList].Price * count * -1);
        //characterInfo.Money -= allItemList[findIndexAllItemList].Price * count;
        //uiController.InventoryMoneySetting(characterInfo.Money);

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());
        data.Add("ITEM_SLOT_NUM", slotNum.ToString());

        if (findIndexPlayerItemList < 0)
        {
            data.Add("ITEM_ID", playerItemList[playerItemList.Count - 1].ItemID.ToString());
            data.Add("ITEM_COUNT", playerItemList[playerItemList.Count - 1].ItemCount.ToString());

            Http httpHelper = Http.Instance;
            httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
            httpHelper.post(100, "/AddNewItem", data);
        }
        else
        {
            data.Add("ITEM_ID", allItemList[findIndexAllItemList].ItemID.ToString());
            data.Add("ITEM_COUNT", playerItemList[findIndexPlayerItemList].ItemCount.ToString());

            Http httpHelper = Http.Instance;
            httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
            httpHelper.post(100, "/UpdateItem", data);
        }
        
        return true;
    }

    public void Exit()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("CHARACTER_NUM", GameManager.Instance.CharacterNum.ToString());

        data.Add("EXP", characterInfo.CurrentExp.ToString());
        data.Add("HP", characterInfo.CurrentHp.ToString());
        data.Add("MP", characterInfo.CurrentMp.ToString());
        data.Add("X", player.transform.position.x.ToString());
        data.Add("Y", player.transform.position.y.ToString());
        data.Add("Z", player.transform.position.z.ToString());

        Http httpHelper = Http.Instance;
        httpHelper.OnHttpRequest += OnHttpRequestForCharInfoUpdate;
        httpHelper.post(100, "/Exit", data);
    }

}
