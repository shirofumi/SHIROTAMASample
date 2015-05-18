using System.Collections.Specialized;

namespace Editor.GameData
{
	public struct GroundData
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

		public GroundType Type
		{
			get { return (GroundType)code[TypeSection]; }
		}

		public int Option
		{
			get { return code[OptionSection]; }
		}

		#endregion

		#region Constructors

		public GroundData(int code)
		{
			this.code = new BitVector32(code);
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			return (obj != null && obj.GetType() == typeof(GroundData) && this.code.Equals(((GroundData)obj).code));
		}

		public override int GetHashCode()
		{
			return this.code.GetHashCode();
		}

		public static bool operator ==(GroundData x, GroundData y)
		{
			return (x.Code == y.Code);
		}

		public static bool operator !=(GroundData x, GroundData y)
		{
			return !(x == y);
		}

		#endregion
	}
}
