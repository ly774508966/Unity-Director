using System.Collections.Generic;
using Tangzx.Director;
using UnityEngine;

public class DirectorPlayer : MonoBehaviour
{
    public delegate void OnPlayCompleteHandler();

    public OnPlayCompleteHandler onPlayComplete;

    private bool _isPlaying;
    private float _playTime;
    /// <summary>
    /// 当前播放头时间
    /// </summary>
    private float _currentTime;
    /// <summary>
    /// 正在播放的
    /// </summary>
    private List<DirectorEvent> _playingList = new List<DirectorEvent>();

    private IEventContainer[] _eventContainers;
    private float _totalTime;
    
    public void Play(IEventContainer[] containers, float totalTime)
    {
        if (_isPlaying == false)
        {
            _playTime = 0;
            _totalTime = totalTime;
            _playingList.Clear();
            _eventContainers = containers;
            for (int i = 0; i < _eventContainers.Length; i++)
            {
                _eventContainers[i].Sort();
            }
            _isPlaying = true;
        }
    }

    public void Pause()
    {
        _isPlaying = false;
    }

    public void Stop()
    {
        _isPlaying = false;
        _playTime = 0;
        _playingList.Clear();
    }

    void Update()
    {
        if (_isPlaying)
        {
            _playTime += Time.deltaTime;
            Process(_playTime);
        }
    }

    public void Process(float time)
    {
        Tick(time - _currentTime);
    }

    public void Tick(float dt)
    {
        float newTime = _currentTime + dt;
        float oldTime = _currentTime;

        _currentTime = newTime;
        //正播
        if (dt > 0)
        {
            if (newTime > _totalTime)
                newTime = _totalTime;

            for (int c = 0; c < _eventContainers.Length; c++)
            {
                IEventContainer ec = _eventContainers[c];
                //进入 playing list
                var e = ec.GetEnumerator();
                while (e.MoveNext())
                {
                    DirectorEvent p = e.Current;
                    if (p.time >= oldTime && p.time <= newTime)
                    {
                        p.Fire();
                        _playingList.Add(p);
                    }
                }
            }

            //处理 playing list
            for (int i = 0; i < _playingList.Count; i++)
            {
                DirectorEvent p = _playingList[i];
                if (p.isRangeEvent)
                {
                    float endTime = p.time + p.duration;
                    endTime = endTime < newTime ? endTime : newTime;
                    p.Process(endTime - p.time);
                }
                // exit
                if (p.time + p.duration <= newTime)
                {
                    p.End();
                    _playingList.RemoveAt(i);
                    i--;
                }
            }

            //结束
            if (newTime >= _totalTime)
            {
                OnPlayFrowardComplete();
            }
        }
        else //反播
        {

        }
    }

    protected virtual void OnPlayFrowardComplete()
    {
        _isPlaying = false;
        _playingList.Clear();

        if (onPlayComplete != null)
            onPlayComplete();
    }
}