#region 模块信息
using System;
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             AudioManager
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : BaseManager<AudioManager>
{
    private List<AudioSource> musicList;
    public Dictionary<int, AudioSource> SoundDictionary { get; set; }
    public Dictionary<string, Dictionary<string, string>> DicDubbingCfgData;
    public Dictionary<int, List<string>> DubbingData;
    private AudioSource CurrentMusic { get; set; }
    private int CurrentSoundId { get; set; }
    private int soundID = 0;
    public float volume = 1.0f;

    protected override void Awake()
    {
        base.Awake();

        musicList = new List<AudioSource>();
        SoundDictionary = new Dictionary<int, AudioSource>();
        DubbingData = new Dictionary<int, List<string>>();
    }
    //加载配音配置
    public void SetDubbing()
    {
        if (DubbingData.Count != 0)
        {
            DubbingData.Clear();
        }
        for (int i = 1; i < DicDubbingCfgData["ID"].Count + 1; i++)
        {
            List<string> temp = new List<string>();
            temp.Add(DicDubbingCfgData["ABName"][i.ToString()]);

            temp.Add(DicDubbingCfgData["AssetName"][i.ToString()]);

            DubbingData.Add(int.Parse(DicDubbingCfgData["ID"][i.ToString()]), temp);
        }
    }

    public string[] GetDubbingName(int id)
    {
        string[] names = new string[2];
        foreach (var item in DubbingData)
        {
            if (id == item.Key)
            {
                names[0] = DubbingData[id][0];
                names[1] = DubbingData[id][1];
                return names;
            }
        }
        return names;
    }
    public void LoadMusic()
    {
        LoadMusicAB("AudioClip/A", "A", true);
        LoadMusicAB("AudioClip/C", "C", true);
        LoadMusicAB("AudioClip/C1", "C1", true);
        LoadMusicAB("AudioClip/D", "D", true);
        LoadMusicAB("AudioClip/E", "E", true);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="abName">音效的ab名字</param>
    /// <param name="loop">是否循环</param>
    /// <param name="isFade">是否淡化背景音乐</param>
    /// <returns> 返回音效id</returns>
    public void PlaySound(int id, bool loop = false, Action action = null, bool isFade = true)
    {
        if (CurrentSoundId >=39 && CurrentSoundId<=45)
        {
            return;
        }
        StopSound();
        soundID++;
        var audioSource = gameObject.AddComponent<AudioSource>();
        Debug.Log(GetDubbingName(id)[0]+"----" + GetDubbingName(id)[1]);
        AudioClip audioClip = ResourceManager.GetInstance.LoadAudioClip(GetDubbingName(id)[0], GetDubbingName(id)[1]);
        //动态添加音效
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();
        CurrentSoundId = id;
        SoundDictionary.Add(soundID, audioSource);
        if (isFade)
        {
            foreach (var item in SoundDictionary)
            {
                if (item.Key != soundID)
                {
                    item.Value.DOFade(0.2f, 1f);
                }
            }
            CurrentMusic.DOFade(0.2f, 2);
        }
        StartCoroutine(PlaySoundEndDestroy(audioSource, soundID, action, isFade));
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="abName">名字</param>
    /// <param name="loop">是否循环</param>
    /// <param name="isTrack">是否音轨</param>
    public void PlayMusic(string abName, string assetname, bool loop = true, bool isTrack = false)
    {
        if (!isTrack)
        {
            if (CurrentMusic && CurrentMusic.clip.name == assetname)
            {
                CurrentMusic.time = 0;
                CurrentMusic.Play();
            }
            else
            {
                if (musicList.Count != 0)
                {
                    foreach (var source in musicList)
                    {
                        if (source.clip.name == assetname)
                        {
                            CurrentMusic = source;
                            CurrentMusic.Play();
                        }
                        else
                        {
                            source.Stop();
                        }
                    }
                }
            }
        }
        else
        {
            foreach (var source in musicList)
            {
                if (source.clip.name == assetname)
                {
                    source.Play();
                }
                else
                {
                    source.Stop();
                }
            }
        }
    }

    private void LoadMusicAB(string abName, string assetname, bool loop)
    {
        AudioClip audioClip = ResourceManager.GetInstance.LoadAudioClip(abName, assetname);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = loop;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        musicList.Insert(0, audioSource);
    }
    public void IsPlayCOne(bool isPlay)
    {
        foreach (AudioSource item in musicList)
        {
            if (item.clip.name == "C1")
            {
                if (isPlay)
                {
                    item.time = 0;
                    item.Play();
                }
                else
                {
                    item.Pause();
                }
            }
        }
    }
    /// <summary>
    ///设置所有音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        this.volume = Mathf.Clamp01(volume);

        foreach (var source in musicList)
        {
            source.volume = this.volume;
        }
        foreach (var sound in SoundDictionary)
        {
            sound.Value.volume = this.volume;
        }
    }

    /// <summary>
    /// 停止背景音乐的播放
    /// </summary>
    public void StopAllMusic()
    {
        if (musicList.Count != 0)
        {
            foreach (var source in musicList)
            {
                source.Stop();
            }
        }
        CurrentMusic = null;
    }
    /// <summary>
    /// 停止音效的播放
    /// </summary>
    /// <param name="soundID"></param>
    public void StopSound(int soundID = 0)
    {
        if (SoundDictionary.ContainsKey(soundID))
        {
            Destroy(SoundDictionary[soundID]);
            SoundDictionary.Remove(soundID);
        }
        if (soundID == 0)
        {
            foreach (var source in SoundDictionary)
            {
                Destroy(source.Value);
            }
            SoundDictionary.Clear();
        }
        CurrentSoundId = -1;
    }
    /// <summary>
    /// 音乐自动销毁
    /// </summary>
    /// <param name="audioclip"></param>
    /// <param name="soundID"></param>
    /// <returns></returns>
    private IEnumerator PlaySoundEndDestroy(AudioSource audioclip, int soundID, Action action, bool isFade)
    {
        yield return new WaitForSeconds(audioclip.clip.length + 1);
        StopSound(soundID);
        if (isFade)
        {
            foreach (var item in SoundDictionary)
            {
                item.Value.DOFade(volume, 0.2f);
            }
            CurrentMusic.DOFade(volume, 0.2f);
        }

        if (action != null)
        {
            action();
        }

    }
    protected override void Update()
    {
        base.Update();
        if (musicList.Count > 1)
        {
            //需要维护队列
            if (CurrentMusic && !CurrentMusic.isPlaying)
            {
                CurrentMusic.Stop();
                musicList.Remove(CurrentMusic);
                Destroy(CurrentMusic);
                CurrentMusic = musicList[0];
                CurrentMusic.Play();
            }
        }
    }

}