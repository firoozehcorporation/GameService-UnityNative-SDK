using System.Collections.Generic;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Plugins.GameService.Tools.NaughtyAttributes.Scripts.Test
{
	public class ReorderableListTest : MonoBehaviour
	{
		[ReorderableList]
		public int[] intArray;

		[ReorderableList]
		public List<Vector3> vectorList;

		[ReorderableList]
		public List<SomeStruct> structList;
	}

	[System.Serializable]
	public struct SomeStruct
	{
		public int Int;
		public float Float;
		public Vector3 Vector;
	}
}
