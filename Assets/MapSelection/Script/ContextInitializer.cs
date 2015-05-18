using UnityEngine;

namespace MapSelection
{
	public class ContextInitializer : MonoBehaviour, IContextInitializer
	{
		#region Fields

		public int Level;

		#endregion

		#region Methods

		public void InitializeContext()
		{
			if (!Context.Data.ContainsKey(Keys.Level))
			{
				Context.Data[Keys.Level] = Level;
			}
		}

		#endregion
	}
}