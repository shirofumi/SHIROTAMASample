using System.Collections.Specialized;

namespace Editor.GameData
{
	public struct PanelData
	{
		#region Fields

		private readonly BitVector32 code;

		private static readonly BitVector32.Section TypeSection = BitVector32.CreateSection(0xff);
		private static readonly BitVector32.Section OptionSection = BitVector32.CreateSection(0xff, TypeSection);

		#endregion

		#region Properties

		public int Code
		{
			get { return code.Data; }
		}

		public PanelType Type
		{
			get { return (PanelType)code[TypeSection]; }
		}

		public int Option
		{
			get { return code[OptionSection]; }
		}

		#endregion

		#region Constructors

		public PanelData(int code)
		{
			this.code = new BitVector32(code);
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			return (obj != null && obj.GetType() == typeof(PanelData) && this.code.Equals(((PanelData)obj).code));
		}

		public override int GetHashCode()
		{
			return this.code.GetHashCode();
		}

		public static bool operator ==(PanelData x, PanelData y)
		{
			return (x.Code == y.Code);
		}

		public static bool operator !=(PanelData x, PanelData y)
		{
			return !(x == y);
		}

		#endregion
	}
}
