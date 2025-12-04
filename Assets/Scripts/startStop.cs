using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Networking;

public class startStop : MonoBehaviour
{
    private VideoPlayer video;
    public Button button;
    private Dictionary<string, string> videos;
    public Text text;
    private string id;
    private string character;
    private bool waitingVideo;

    // Start is called before the first frame update
    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.Pause();
        videos = new Dictionary<string, string>(){
            {"3", "Assets/bödvarAnimationFINAL.mp4"},
            {"6", "Assets/lordVraxx_roughExtraAnimation.mp4"},
            {"7", "Assets/gnash_roughExtraAnimation.MP4"},
            {"e", "Assets/emptyVideo.mp4"}
        };
        character = "e";
        waitingVideo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && video.isPlaying == false && !waitingVideo && !string.IsNullOrEmpty(id))
        {
            waitingVideo = !waitingVideo;
            StartCoroutine(PlayVideo());
        }
    }

    public void UpdateId(string id)
    {
        // error check for id
        this.id = id;
        Debug.Log(this.id.Length);
        Debug.Log("id: " + this.id + "end");
    }

    private IEnumerator PlayVideo()
    {   
        yield return GetMostPlayedLegendById(id);

        text.text = "Selected Char: " + character;
        if (videos.ContainsKey(character))
        {
            video.url = videos[character];
        } else
        {
            video.url = videos["e"];
        }
        video.Play();
        id = "";
        character = "";
        waitingVideo = !waitingVideo;
    }

    private IEnumerator GetMostPlayedLegendById(string id)
    {
        string url = "http://localhost:8080/event/mostPlayedLegendById/" + id;
        Debug.Log(url);
        string response;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            character = request.downloadHandler.text;
            Debug.Log("response: " + character);
        }
        else
        {
            Debug.Log("error: " + request.error);
        }
    }

    public void changeStartStop() {
        if (video.isPlaying == false) {
            video.Play();
        } else {
            video.Pause();
        }
    }
}
