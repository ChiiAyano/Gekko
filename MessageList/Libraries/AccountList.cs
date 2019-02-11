using System;
using System.Collections.Generic;
using System.Text;

namespace Gekko.MessageList.Libraries
{
	public static class AccountList
	{
		/// <summary>
		/// 配列の順番を指定して入れ替えます。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="toIndex">移動先の配列番号</param>
		/// <param name="fromIndex">移動元の配列番号</param>
		public static void Move<T>(this List<T> list, int fromIndex, int toIndex)
		{
			T a = list[toIndex];
			list[toIndex] = list[fromIndex];
			list[fromIndex] = a;
		}
	}
}
