﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Sprites;
using UnityEngine.Scripting;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Assertions.Must;
using UnityEngine.Assertions.Comparers;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace CommonWidget
{
    public class ScrollbarCreater : ElementCreater
    {
        public override GameObject CreateInstence(WidgetItem info)
        {
            var ok = EditorApplication.ExecuteMenuItem("GameObject/UI/Scrollbar");
            if (ok)
            {
                var obj = Selection.activeGameObject;
                var bar = obj.GetComponent<Scrollbar>();
                var dic = info.spriteDic;

                if (dic.ContainsKey(KeyWord.background))
                {
                    var image = bar.GetComponent<Image>();
                    image.sprite = dic[KeyWord.background];
                    image.type = Image.Type.Simple;
                    if (image.sprite.rect.width > image.sprite.rect.height)
                    {
                        bar.SetDirection(Scrollbar.Direction.LeftToRight, true);
                    }
                    else
                    {
                        bar.SetDirection(Scrollbar.Direction.BottomToTop, true);
                    }
                    image.SetNativeSize();
                }

                if (dic.ContainsKey(KeyWord.handle))
                {
                    bar.handleRect.GetComponent<Image>().sprite = dic[KeyWord.handle];
                }

                return obj;
            }
            return null;
        }

        public override List<Sprite> GetPreviewList(WidgetItem info)
        {
            var list = new List<Sprite>();
            var dic = info.spriteDic;
            if (dic != null)
            {
                if (dic.ContainsKey(KeyWord.background))
                {
                    list.Add(dic[KeyWord.background]);
                }

                else if (dic.ContainsKey(KeyWord.handle))
                {
                    list.Add(dic[KeyWord.handle]);
                }
            }
            return list;
        }

        protected override List<string> CreateDefultList()
        {
            return new List<string>() { KeyWord.background,KeyWord.handle };
        }
    }
}
