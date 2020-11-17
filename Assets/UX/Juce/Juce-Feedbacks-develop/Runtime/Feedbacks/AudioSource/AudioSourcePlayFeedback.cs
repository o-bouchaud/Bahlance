﻿using Juce.Tween;
using System.Collections.Generic;
using UnityEngine;

namespace Juce.Feedbacks
{
    [FeedbackIdentifier("Play", "AudioSource/")]
    public class AudioSourcePlayFeedback : Feedback
    {
        [Header(FeedbackSectionsUtils.TargetSection)]
        [SerializeField] private AudioSource target = default;

        [Header(FeedbackSectionsUtils.ValuesSection)]
        [SerializeField] private AudioClip audioClip = default;

        [SerializeField] private bool oneShot = default;

        [Header(FeedbackSectionsUtils.TimingSection)]
        [SerializeField] [Min(0)] private float delay = default;

        [Header(FeedbackSectionsUtils.LoopSection)]
        [SerializeField] private LoopProperty looping = default;

        public AudioSource Target => target;
        public AudioClip AudioClip { get => audioClip; set => audioClip = value; }
        public bool OneShot { get => oneShot; set => oneShot = value; }
        public float Delay { get => delay; set => delay = Mathf.Max(0, value); }
        public LoopProperty Looping => looping;

        public override bool GetFeedbackErrors(out string errors)
        {
            if (target == null)
            {
                errors = ErrorUtils.TargetNullErrorMessage;
                return true;
            }

            errors = string.Empty;
            return false;
        }

        public override string GetFeedbackTargetInfo()
        {
            return target != null ? target.gameObject.name : string.Empty;
        }

        public override void GetFeedbackInfo(ref List<string> infoList)
        {
            InfoUtils.GetTimingInfo(ref infoList, delay);
        }

        public override ExecuteResult OnExecute(FlowContext context, SequenceTween sequenceTween)
        {
            if (target == null)
            {
                return null;
            }

            Tween.Tween delayTween = null;

            if (delay > 0)
            {
                delayTween = new WaitTimeTween(delay);
                sequenceTween.Append(delayTween);
            }

            sequenceTween.AppendCallback(() =>
            {
                if (!oneShot)
                {
                    target.clip = audioClip;

                    target.Play();
                }
                else
                {
                    target.PlayOneShot(audioClip);
                }
            });

            LoopUtils.SetLoop(sequenceTween, looping);

            ExecuteResult result = new ExecuteResult();
            result.DelayTween = delayTween;

            return result;
        }
    }
}