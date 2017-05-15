using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager sInstance;
    public static GameManager Instance
    {
        get
        {
            if (sInstance == null)
            {
                GameObject newGameObject = new GameObject("GameManager");
                sInstance = newGameObject.AddComponent<GameManager>();
            }
            return sInstance;
        }
    }

    private string nextSceneName = "";
    public string NextSceneName
    {
        get
        {
            return nextSceneName;
        }
        set
        {
            nextSceneName = value;
        }
    }

    private string loginId = "";
    public string LoginId
    {
        get
        {
            return loginId;
        }
        set
        {
            loginId = value;
        }
    }

    private int selectSlotNum = 0;
    public int SelectSlotNum
    {
        get
        {
            return selectSlotNum;
        }
        set
        {
            selectSlotNum = value;
        }
    }

    private int characterNum = 1;
    public int CharacterNum
    {
        get
        {
            return characterNum;
        }
        set
        {
            characterNum = value;
        }
    }

    // >> : 현재 마우스 클릭 좌표에서 rayCast되는 GameObject 반환.. 
    public static GameObject MousePick
    {
        get
        {
            RaycastHit m_Hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out m_Hit, 100))
            {
                return m_Hit.collider.gameObject;
            }
            else
                return null;
        }
    }
    // << : MousePick

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
