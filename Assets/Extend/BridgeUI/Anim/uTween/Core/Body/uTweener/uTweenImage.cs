﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BridgeUI.uTween
{
    [System.Serializable]
    public class uTweenImage : uTweener
    {

        [Range(0, 1)]
        public float from;
        [Range(0, 1)]
        public float to;

        float mValue;
        public float value
        {
            get { return mValue; }
            set
            {
                mValue = value;
            }
        }

        public Image mImage;
        public Image cacheImage
        {
            get
            {
                if (mImage.type != Image.Type.Filled)
                {
                    Debug.LogWarning("[uTweenImage] To use tween the image type must be [Image.Type.Filled]");
                }
                return mImage;
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = from + factor * (to - from);
            cacheImage.fillAmount = value;
        }

        public static uTweenImage Begin(Image go, float from, float to, float duration, float delay)
        {
            uTweenImage comp = Begin<uTweenImage>();
            comp.value = from;
            comp.from = from;
            comp.to = to;
            comp.delay = delay;

            if (duration <= 0)
            {
                comp.Sample(1, true);
                comp.enabled = false;
            }
            return comp;
        }
    }
}
