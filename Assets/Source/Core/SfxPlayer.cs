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
        private AudioClip _hit = Resources.Load("Hit", typeof(AudioClip)) as AudioClip;
        private AudioClip _unlock = Resources.Load("Key Jiggle", typeof(AudioClip)) as AudioClip;
        private AudioClip _walk = Resources.Load("Walk", typeof(AudioClip)) as AudioClip;
        private AudioClip _pick = Resources.Load("Pick", typeof(AudioClip)) as AudioClip;
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
                _audioSource.PlayOneShot(_background);
                Debug.Log("started playing");
            }
        }

        public void PlayHit()
        {
            _audioSource = _sfxObject.AddComponent<AudioSource>();
            _audioSource.PlayOneShot(_hit, 0.35f);
        }
        public void PlayUnlock()
        {
            _audioSource = _sfxObject.AddComponent<AudioSource>();
            _audioSource.PlayOneShot(_unlock, 0.5f);
        }
        public void PlayWalk()
        {
            _audioSource = _sfxObject.AddComponent<AudioSource>();
            _audioSource.PlayOneShot(_walk, 0.3f);
        }
        public void PlayPick()
        {
            _audioSource = _sfxObject.AddComponent<AudioSource>();
            _audioSource.PlayOneShot(_pick, 0.3f);
        }

    }
}
