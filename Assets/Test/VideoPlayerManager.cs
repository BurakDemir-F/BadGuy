using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private VideoClip _clip;
    [SerializeField] private GameObject _videoCanvas;

    private void Start()
    {
        //_videoPlayer.Stop();
    }

    [ContextMenu("Resize Video Canvas Based On Video")]
    private void ResizeVideo()
    {
        var clipWidth = _clip.width;
        var clipHeight = _clip.height;

        Debug.Log($"clip height: {clipHeight}, clip width: {clipWidth}");
        
        var sizeVec = new Vector3(2f, 2f * clipHeight/clipWidth, 1f);
        _videoCanvas.transform.localScale = sizeVec;
    }

    [ContextMenu("Play video")]
    private void PlayVideo()
    {
        _videoPlayer.Play();
    }


    [ContextMenu("Debug frame count")]
    private void DebugFrameCount()
    {
        Debug.Log($"Frame count: {_clip.frameCount}");
    }

    [ContextMenu("Go To Frame 25")]
    private void GoToFrame25()
    {
        _videoPlayer.frame = 25;
    }
}
