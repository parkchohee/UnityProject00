using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Http : MonoBehaviour
{

    public delegate void HttpRequestDelegate(int id, WWW www);
    public event HttpRequestDelegate OnHttpRequest;
    private int requestId;
    private string ServerURL = "http://52.78.172.107";
    //private string ServerURL = "http://localhost:3003";

    static Http current = null;
    static GameObject container = null;
    public static Http Instance
    {
        get
        {
            if (current == null)
            {
                container = new GameObject();
                container.name = "Http";
                current = container.AddComponent(typeof(Http)) as Http;
            }
            return current;
        }
    }

    public void get(int id, string url)
    {
        WWW www = new WWW(ServerURL + url);
        StartCoroutine(WaitForRequest(id, www));
    }

    public void post(int id, string url, IDictionary<string, string> data)
    {
        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> post_arg in data)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }

        WWW www = new WWW(ServerURL + url, form);
        StartCoroutine(WaitForRequest(id, www));
    }

    private IEnumerator WaitForRequest(int id, WWW www)
    {
        // 응답이 올떄까지 기다림
        yield return www;

        // 응답이 왔다면, 이벤트 리스너에 응답 결과 전달
        bool hasCompleteListener = (OnHttpRequest != null);

        if (hasCompleteListener)
        {
            OnHttpRequest(id, www);
        }

        // 통신 해제
        www.Dispose();
    }

}
