using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class WebImageParser<T>
{
    private UnityWebRequest webRequest;
    private Sprite sprite;
    
    public void RequestParcing(string url, Action<Sprite> callBack)
    {
        Debug.Log("In order to parse image we need to send web request to our image file which is url");
        GetWebRequest(url, callBack);
    }

    private async void GetWebRequest(string imgUrl, Action<Sprite> callBack)
    {
        webRequest = UnityWebRequestTexture.GetTexture(imgUrl);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            await Task.Yield();
        }

        Debug.Log("We are waiting until unity fully proceeds a given web request\n" +
            "once it proceeded we take the result of request as an expression for switch state");

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
                var sprite = CreateSpriteFromRenderer(((DownloadHandlerTexture)webRequest.downloadHandler).texture);
                callBack.Invoke(sprite);
                Debug.Log("If webrequest was successful we can download image from the internet\n" +
                    "as a 2D texture file, then by using our custom function we convert it to sprite");
                break;
        }

    }

    private Sprite CreateSpriteFromRenderer(Texture2D texture)
    {
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

}
