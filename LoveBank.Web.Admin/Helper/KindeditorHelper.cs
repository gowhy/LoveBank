
using System.Collections;
using System.IO;

namespace LoveBank.Web.Admin.Helper {
    public class KindeditorHelper {
         
    }
    public class NameSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			FileInfo xInfo = new FileInfo(x.ToString());
			FileInfo yInfo = new FileInfo(y.ToString());

			return xInfo.FullName.CompareTo(yInfo.FullName);
		}
	}

	public class SizeSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			FileInfo xInfo = new FileInfo(x.ToString());
			FileInfo yInfo = new FileInfo(y.ToString());

			return xInfo.Length.CompareTo(yInfo.Length);
		}
	}

	public class TypeSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			FileInfo xInfo = new FileInfo(x.ToString());
			FileInfo yInfo = new FileInfo(y.ToString());

			return xInfo.Extension.CompareTo(yInfo.Extension);
		}
	}
}