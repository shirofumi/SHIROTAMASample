using UnityEngine;

public class ContextInitializer : MonoBehaviour, IContextInitializer
{
	#region Fields

	public int Primary;

	public int Secondary;

	public int Depth;

	#endregion

	#region Methods

	public void InitializeContext()
	{
		if (!Context.Data.ContainsKey(Keys.MapID))
		{
			Context.Data[Keys.MapID] = new MapID(Primary, Secondary);
			Context.Data[Keys.Depth] = Depth;
		}
	}

	#endregion
}
