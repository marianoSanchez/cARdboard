//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Plays the specified sound.
/// </summary>

public class UICustomPlaySound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		Custom,
		OnEnable,
		OnDisable,
	}

	public AudioClip audioClip;
	public Trigger trigger = Trigger.OnClick;

    public float delay = 0f;
	[Range(0f, 1f)] public float volume = 1f;
	[Range(0f, 2f)] public float pitch = 1f;

    [SerializeField]
    bool looped = false;

    List<AudioSource> sources = new List<AudioSource>();
    EventDelegate soundDelegate;

	bool mIsOver = false;

	bool canPlay
	{
		get
		{
			if (!enabled) return false;
			UIButton btn = GetComponent<UIButton>();
			return (btn == null || btn.isEnabled);
		}
	}

    void Start ()
    {
        soundDelegate = EventDelegate.Add(NGUITools.onSoundVolumeChange, () =>
        {
            foreach (var source in sources)
            {
                source.volume = NGUITools.soundVolume;
            }
        });
    }

    void OnDestroy ()
    {
        NGUITools.onSoundVolumeChange.Remove(soundDelegate);
    }

	void OnEnable ()
	{
		if (trigger == Trigger.OnEnable)
			Play();
	}

	void OnDisable ()
	{
		if (trigger == Trigger.OnDisable)
			Play();
	}

	void OnHover (bool isOver)
	{
		if (trigger == Trigger.OnMouseOver)
		{
			if (mIsOver == isOver) return;
			mIsOver = isOver;
		}

		if (canPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
			Play();
	}

	void OnPress (bool isPressed)
	{
		if (trigger == Trigger.OnPress)
		{
			if (mIsOver == isPressed) return;
			mIsOver = isPressed;
		}

		if (canPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
			Play();
	}

	void OnClick ()
	{
		if (canPlay && trigger == Trigger.OnClick)
			Play();
	}

	void OnSelect (bool isSelected)
	{
		if (canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
			OnHover(isSelected);
	}

    public void Stop ()
    {
        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];

            if (source.isPlaying)
                source.Stop();

            if (looped)
                Destroy(source);
        }

        if (looped)
            sources.Clear();
    }

	public void Play ()
	{
        StartCoroutine(PlaySound());
	}

    IEnumerator PlaySound()
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        sources.Add(NGUITools.PlaySound(audioClip, volume, pitch, looped));
    }
}
