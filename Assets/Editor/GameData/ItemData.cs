
namespace Editor.GameData
{
	public struct ItemData
	{
		#region Fields

		private readonly int code;

		#endregion

		#region Properties

		public int Code
		{
			get { return code; }
		}

		public ItemType Type
		{
			get { return (ItemType)((code & 0xff00) >> 8); }
		}

		public int Option
		{
			get { return (code & 0xff); }
		}

		public bool SmallLeftTop
		{
			get { return (code & 0x01) != 0; }
		}

		public bool SmallRightTop
		{
			get { return (code & 0x02) != 0; }
		}

		public bool SmallLeftBottom
		{
			get { return (code & 0x04) != 0; }
		}

		public bool SmallRightBottom
		{
			get { return (code & 0x08) != 0; }
		}

		#endregion

		#region Constructors

		public ItemData(int code)
		{
			this.code = code;
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			return (obj != null && obj.GetType() == typeof(ItemData) && this.code == ((ItemData)obj).code); 
		}

		public override int GetHashCode()
		{
			return this.code.GetHashCode();
		}

		public static bool operator ==(ItemData x, ItemData y)
		{
			return (x.code == y.code);
		}

		public static bool operator !=(ItemData x, ItemData y)
		{
			return !(x == y);
		}

		#endregion
	}
}
