using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Plugins.GameService.Tools.NaughtyAttributes.Scripts.Test
{
	public class ShowNativePropertyTest : MonoBehaviour
	{
		[ShowNativeProperty]
		private Transform Transform
		{
			get
			{
				return transform;
			}
		}

		[ShowNativeProperty]
		private Transform ParentTransform
		{
			get
			{
				return transform.parent;
			}
		}
	}
}
