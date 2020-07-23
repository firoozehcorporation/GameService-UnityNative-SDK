using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using UnityEngine;

namespace Plugins.GameService.Tools.NaughtyAttributes.Scripts.Test
{
	public class SceneTest : MonoBehaviour
	{
		[Scene]
		public string scene0;

		public SceneNest1 nest1;
	}

	[System.Serializable]
	public class SceneNest1
	{
		[Scene]
		public string scene1;

		public SceneNest2 nest2;
	}

	[System.Serializable]
	public struct SceneNest2
	{
		[Scene]
		public string scene2;
	}
}
