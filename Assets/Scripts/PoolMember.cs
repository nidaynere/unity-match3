using UnityEngine;
using System.Collections.Generic;

public class Pool
{
    private Dictionary<string, PoolMember> poolMembers;

    public Pool (Transform holder, GameMember[] poolObjects, int poolSize) {
        poolMembers = new Dictionary<string, PoolMember>();

        int count = poolObjects.Length;
        for (int i=0; i<count; i++) {
            poolMembers.Add(poolObjects[i].name, new PoolMember(holder, poolObjects[i], poolSize));
        }
    }

    public GameMember GetFromPool (string Id) {
        if (poolMembers.ContainsKey(Id)) {
            return poolMembers[Id].Get();
        }

        return null;
    }

    public class PoolMember
    {
        private GameMember[] poolObjects;
        private int currentStep;
        private int size;

        public PoolMember(Transform holder, GameMember poolObject, int poolSize)
        {
            size = poolSize;
            poolObjects = new GameMember[poolSize];

            for (int i = 0; i < poolSize; i++)
            {
                poolObjects[i] = Object.Instantiate(poolObject, holder);
                poolObjects[i].gameObject.SetActive(false);
            }

            currentStep = 0;
        }

        public void Dispose()
        {
            for (int i = 0; i < size; i++)
            {
                Object.Destroy(poolObjects[i].gameObject);
            }

            poolObjects = null;
        }

        public GameMember Get()
        {
            var target = poolObjects[currentStep];
            if (++currentStep >= size)
                currentStep = 0;
            return target;
        }

        public void Reset()
        {
            for (int i = 0; i < size; i++)
            {
                poolObjects[i].gameObject.SetActive(false);
            }

            currentStep = 0;
        }
    }
}
