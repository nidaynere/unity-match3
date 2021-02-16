using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectPool", menuName = "EffectPool", order = 1)]
public class EffectPool : ScriptableObject
{
    public struct Pool
    {
        private struct Effect
        {
            private Transform main;
            private AudioPlayer[] audioPlayers;
            private ParticleSystem[] particleSystems;

            public Effect(Transform target)
            {
                main = target;
                audioPlayers = target.GetComponentsInChildren<AudioPlayer>(true);
                particleSystems = target.GetComponentsInChildren<ParticleSystem>(true);
            }

            public void Play(Vector3 position)
            {
                main.localPosition = position;

                foreach (var audio in audioPlayers)
                    audio.Play();

                foreach (var particle in particleSystems)
                    particle.Play();
            }
        }

        private int step;
        private Effect[] effects;
        private int size;

        public Pool(Transform holder, Transform t, int size)
        {
            this.size = size;

            step = 0;
            effects = new Effect[size];

            for (int i = 0; i < size; i++)
            {
                effects[i] = new Effect(Instantiate(t, holder));
            }
        }

        public void Play(Vector3 position)
        {
            effects[step].Play(position);
            step++;
            if (step > size)
                step = 0;
        }
    }

    [SerializeField] public Transform[] effects;

    [SerializeField] private int poolSize = 10;

    private Dictionary<string, Pool> pool = new Dictionary<string, Pool>();

    public void Create(Transform holder)
    {
        foreach (var e in effects)
        {
            pool.Add(e.name, new Pool(holder, e, poolSize));
        }
    }

    public void Play(string effectName, Vector3 position)
    {
        if (pool.ContainsKey(effectName))
        {
            pool[effectName].Play(position);
        }
        else
        {
            Debug.LogWarning("Effect is not found with that name => " + effectName);
        }
    }
}
