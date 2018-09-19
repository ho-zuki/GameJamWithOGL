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
		public static IEnumerator DelayMethod(this GameObject obj, float t, Action act)
		{
			yield return new WaitForSeconds(t);
			act();
		}
		public static IEnumerator DelayMethodInRealTime(this GameObject obj, float t, Action act)
		{
			yield return new WaitForSecondsRealtime(t);
			act();
		}
	}
}