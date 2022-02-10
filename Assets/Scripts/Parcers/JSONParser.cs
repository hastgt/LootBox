using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class JSONParser<T>
{
    private UnityWebRequest _webRequest;

    public async Task<T> RequestParsingAsync(string url)
    {
        TaskCompletionSource<T> webRequestCompletion = new TaskCompletionSource<T>();
        
        RequestParsing(url, webRequestCompletion.SetResult);
        return await webRequestCompletion.Task;
    }

    private void RequestParsing(string url, Action<T> callBack)
    {
        GetWebRequest(url, callBack, _webRequest);
    }

    private async void GetWebRequest(string url, Action<T> callBack, UnityWebRequest webRequest)
    {
        webRequest = UnityWebRequest.Get(url);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            await Task.Yield();
        }

       

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log($"{UnityWebRequest.Result.Success}");
                T parcedData = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                callBack.Invoke(parcedData);
                break;
        }

    }
}

