using UnityEngine;

public abstract class Billboard : BatchMonoBehaviour<Billboard>
{
	#region Messages

	protected void Start()
	{
		Process();
	}

	#endregion
}