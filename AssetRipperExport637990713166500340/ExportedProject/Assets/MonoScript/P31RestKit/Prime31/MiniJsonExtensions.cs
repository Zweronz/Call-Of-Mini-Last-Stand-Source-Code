using System.Collections;

namespace Prime31
{
	public static class MiniJsonExtensions
	{
		public static string toJson(this IDictionary obj)
		{
			return Json.jsonEncode(obj);
		}

		public static Hashtable hashtableFromJson(this string json)
		{
			return Json.jsonDecode(json) as Hashtable;
		}
	}
}
