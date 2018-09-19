using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hozuki
{
    public static class MonoBehaviorExtension
    {
        /// <summary>
        /// 渡されたメソッドを指定時間後に実行する
        /// </summary>
        private static IEnumerator _DelayMethod(this MonoBehaviour obj, float t, Action act)
        {
            yield return new WaitForSeconds(t);
            act();
        }
        private static IEnumerator _DelayMethodInRealTime(this MonoBehaviour obj, float t, Action act)
        {
            yield return new WaitForSecondsRealtime(t);
            act();
        }
        public static void DelayMethod(this MonoBehaviour obj, float t, Action act)
        {
            obj.StartCoroutine(_DelayMethod(obj, t, act));
        }
        public static void DelayMethodInRealTime(this MonoBehaviour obj, float t, Action act)
        {
            obj.StartCoroutine(_DelayMethodInRealTime(obj, t, act));
        }
    }
}