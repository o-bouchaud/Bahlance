﻿using System;
using System.Collections.Generic;

namespace Juce.Tween
{
    public class GroupTween : Tween
    {
        private readonly List<Tween> allTweens = new List<Tween>();
        private int tweensLeftToFinish;

        protected override void SetTimeScaleInternal(float timeScale)
        {
            for (int i = 0; i < allTweens.Count; ++i)
            {
                allTweens[i].SetTimeScale(timeScale);
            }
        }

        protected override void SetEaseInternal(EaseDelegate easeFunction)
        {
            for (int i = 0; i < allTweens.Count; ++i)
            {
                allTweens[i].SetEase(easeFunction);
            }
        }

        protected override void ActivateInternal()
        {
            for (int i = 0; i < allTweens.Count; ++i)
            {
                allTweens[i].Activate();
            }
        }

        protected override void StartInternal()
        {
            tweensLeftToFinish = allTweens.Count;
        }

        protected override void CompleteInternal()
        {
            for (int i = 0; i < allTweens.Count; ++i)
            {
                Tween currTween = allTweens[i];

                if (!currTween.IsPlaying && !currTween.IsCompletedOrKilled)
                {
                    currTween.Start();
                }

                if (currTween.IsPlaying)
                {
                    currTween.Complete();
                }
            }

            tweensLeftToFinish = 0;
        }

        protected override void KillInternal()
        {
            for (int i = 0; i < allTweens.Count; ++i)
            {
                Tween currTween = allTweens[i];

                if (currTween.IsPlaying)
                {
                    currTween.Kill();
                }
            }

            tweensLeftToFinish = 0;
        }

        protected override void ResetInternal(ResetMode resetMode)
        {
            for (int i = allTweens.Count - 1; i >= 0; --i)
            {
                Tween currTween = allTweens[i];

                currTween.Reset(resetMode);

                currTween.Activate();
            }
        }

        protected override void UpdateInternal()
        {
            bool finished = TweenUtils.UpdateSimultaneous(allTweens, tweensLeftToFinish);

            if (finished)
            {
                MarkAsFinished();
            }
        }

        protected override float GetDurationInternal()
        {
            float duration = 0.0f;

            for (int i = 0; i < allTweens.Count; ++i)
            {
                Tween currTween = allTweens[i];

                float currDuration = currTween.GetDuration();

                if (currDuration > duration)
                {
                    duration = currDuration;
                }
            }

            return duration;
        }

        protected override float GetProgressInternal()
        {
            float progress = float.MaxValue;

            for (int i = 0; i < allTweens.Count; ++i)
            {
                Tween currTween = allTweens[i];

                float currProgress = currTween.GetProgress();

                if (currProgress < progress)
                {
                    progress = currProgress;
                }
            }

            return progress;
        }

        public override int GetNestedTweenChildsCount()
        {
            int nestedChilds = 0;

            for (int i = 0; i < allTweens.Count; ++i)
            {
                nestedChilds += allTweens[i].GetNestedTweenChildsCount() + 1;
            }

            return nestedChilds;
        }

        public void Add(Tween tween)
        {
            if (tween == null)
            {
                throw new ArgumentNullException($"Tried to {nameof(Add)} a null {nameof(Tween)} on {nameof(GroupTween)}");
            }

            if (IsPlaying)
            {
                return;
            }

            if (tween.IsPlaying)
            {
                return;
            }

            if (tween.IsNested)
            {
                return;
            }

            tween.IsNested = true;

            allTweens.Add(tween);
        }
    }
}