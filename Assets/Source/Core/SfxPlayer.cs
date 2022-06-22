using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawl.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace DungeonCrawl.Core
{
    public class SfxPlayer : MonoBehaviour
    {
        private GameObject _sfxObject = new GameObject();
        private AudioClip _background = Resources.Load("Background", typeof(AudioClip)) as AudioClip;
        private AudioClip _hit = Resources.Load("Weapon Blow", typeof(AudioClip)) as AudioClip;
        private AudioSource _audioSource;


        public void PlayBackground()
        {
            _audioSource = _sfxObject.AddComponent<AudioSource>();
            _audioSource.clip = _background;
            if (_background == null)
            {
                Debug.Log("null a hang");
            }
            else
            {
                _audioSource.Play();
                Debug.Log("started playing");
            }
        }

        public void PlayHit()
        {
            _audioSource = _sfxObject.AddComponent<AudioSource>();
            _audioSource.PlayOneShot(_hit);
        }
    }
}
