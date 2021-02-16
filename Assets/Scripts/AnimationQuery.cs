﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public struct AnimationQuery
{
    private AnimationSettings gameSettings;
    private List<BaseAction> Query;

    public AnimationQuery(AnimationSettings _gameSettings)
    {
        gameSettings = _gameSettings;
        Query = new List<BaseAction>();
    }

    public void AddToQuery(BaseAction animation)
    {
        Query.Add(animation);
    }

    public IEnumerator DoQuery (Action onCompleted)
    {
        if (Query.Count == 0)
        {
            Debug.Log("[CombineAnimationQuery->DoQuery => Query is empty!");
            yield break;
        }

        Debug.Log("DoQuery");
        bool isPlaying = false;
        while (true)
        {
            while (isPlaying)
            {
                yield return new WaitForSeconds(gameSettings.AnimationDelay);
            }

            if (Query.Count == 0)
            {// Done
                Debug.Log("Animation query => completed.");

                onCompleted?.Invoke();
                yield break;
            }

            isPlaying = true;

            Debug.Log("Animation query => started anim.");

            Query[0].Trigger(gameSettings, () => {
                Debug.Log("Animation query => finished anim.");
                isPlaying = false;
            });

            Query.RemoveAt(0);
        }
    }


    public class BaseAction
    {
        protected GameMember gameMember;
        protected Action onTriggered;

        public BaseAction(GameMember gameMember)
        {
            this.gameMember = gameMember;
        }

        public virtual void Trigger(AnimationSettings gameSettings, Action onCompleted) {
            onTriggered?.Invoke();
        }
    }

    public class MovementAction : BaseAction
    {
        private int X, Y;

        public MovementAction(GameMember gameMember, int _X, int _Y) : base (gameMember)
        {
            X = _X;
            Y = _Y;
        }

        public override void Trigger(AnimationSettings gameSettings, Action onCompleted)
        {
            gameMember.SetTransition(X, Y, () => { onCompleted?.Invoke(); });
        }
    }

    public class DestroyAction : BaseAction
    {
        public DestroyAction(GameMember gameMember) : base(gameMember)
        {
        
        }

        public override void Trigger(AnimationSettings gameSettings, Action onCompleted)
        {
            gameMember.Kill();
            onCompleted?.Invoke();
        }
    }
}
