using System;
using System.Xml.Linq;

namespace Editor.GameData
{
	public struct BarrierEntryData
	{
		#region Fields

		public readonly BarrierType Type;

		public readonly BarrierScale Scale;

		public readonly float Weight;

		#endregion

		#region Constructors

		public BarrierEntryData(XElement element)
		{
			this.Type = (BarrierType)Enum.Parse(typeof(BarrierType), (string)element.Attribute("type"));
			this.Scale = (BarrierScale)Enum.Parse(typeof(BarrierScale), (string)element.Attribute("scale"));
			this.Weight = (float)element.Attribute("weight");
		}

		#endregion
	}
}
