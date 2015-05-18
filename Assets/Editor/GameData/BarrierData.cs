using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Editor.GameData
{
	public class BarrierData
	{
		#region Fields

		public readonly int MaxCount;

		public readonly ReadOnlyCollection<BarrierEntryData> Entries;

		#endregion

		#region Constructors

		public BarrierData(XElement element)
		{
			this.MaxCount = (int)element.Attribute("count");
			this.Entries = element.Element("entry_list").Elements("entry").Select(x => new BarrierEntryData(x)).ToList().AsReadOnly();
		}

		#endregion
	}
}
