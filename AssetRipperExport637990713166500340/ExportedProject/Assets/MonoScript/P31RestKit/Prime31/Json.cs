using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Prime31
{
	public class Json
	{
		internal class Deserializer
		{
			private enum JsonToken
			{
				None = 0,
				CurlyOpen = 1,
				CurlyClose = 2,
				SquaredOpen = 3,
				SquaredClose = 4,
				Colon = 5,
				Comma = 6,
				String = 7,
				Number = 8,
				True = 9,
				False = 10,
				Null = 11
			}

			private bool _useGenericContainers;

			private char[] charArray;

			private Deserializer(string json, bool useGenericContainers)
			{
				_useGenericContainers = useGenericContainers;
				charArray = json.ToCharArray();
			}

			public static object deserialize(string json, bool useGenericContainers)
			{
				if (json != null)
				{
					Deserializer deserializer = new Deserializer(json, useGenericContainers);
					return deserializer.deserialize();
				}
				return null;
			}

			private object deserialize()
			{
				int index = 0;
				return parseValue(charArray, ref index);
			}

			protected object parseValue(char[] json, ref int index)
			{
				switch (lookAhead(json, index))
				{
				case JsonToken.String:
					return parseString(json, ref index);
				case JsonToken.Number:
					return parseNumber(json, ref index);
				case JsonToken.CurlyOpen:
					return parseObject(json, ref index);
				case JsonToken.SquaredOpen:
					return parseArray(json, ref index);
				case JsonToken.True:
					nextToken(json, ref index);
					return bool.Parse("TRUE");
				case JsonToken.False:
					nextToken(json, ref index);
					return bool.Parse("FALSE");
				case JsonToken.Null:
					nextToken(json, ref index);
					return null;
				default:
					return null;
				}
			}

			private IDictionary parseObject(char[] json, ref int index)
			{
				IDictionary dictionary = ((!_useGenericContainers) ? ((IDictionary)new Hashtable()) : ((IDictionary)new Dictionary<string, object>()));
				nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					switch (lookAhead(json, index))
					{
					case JsonToken.None:
						return null;
					case JsonToken.Comma:
						nextToken(json, ref index);
						continue;
					case JsonToken.CurlyClose:
						nextToken(json, ref index);
						return dictionary;
					}
					string text = parseString(json, ref index);
					if (text == null)
					{
						return null;
					}
					JsonToken jsonToken = nextToken(json, ref index);
					if (jsonToken != JsonToken.Colon)
					{
						return null;
					}
					object value = parseValue(json, ref index);
					dictionary[text] = value;
				}
				return dictionary;
			}

			private IList parseArray(char[] json, ref int index)
			{
				IList list = ((!_useGenericContainers) ? ((IList)new ArrayList()) : ((IList)new List<object>()));
				nextToken(json, ref index);
				bool flag = false;
				while (!flag)
				{
					switch (lookAhead(json, index))
					{
					case JsonToken.None:
						return null;
					case JsonToken.Comma:
						nextToken(json, ref index);
						continue;
					case JsonToken.SquaredClose:
						break;
					default:
					{
						object value = parseValue(json, ref index);
						list.Add(value);
						continue;
					}
					}
					nextToken(json, ref index);
					break;
				}
				return list;
			}

			private string parseString(char[] json, ref int index)
			{
				string text = "";
				eatWhitespace(json, ref index);
				char c = json[index++];
				bool flag = false;
				while (!flag && index != json.Length)
				{
					c = json[index++];
					switch (c)
					{
					case '"':
						flag = true;
						break;
					case '\\':
					{
						if (index == json.Length)
						{
							break;
						}
						switch (json[index++])
						{
						case '"':
							text += '"';
							continue;
						case '\\':
							text += '\\';
							continue;
						case '/':
							text += '/';
							continue;
						case 'b':
							text += '\b';
							continue;
						case 'f':
							text += '\f';
							continue;
						case 'n':
							text += '\n';
							continue;
						case 'r':
							text += '\r';
							continue;
						case 't':
							text += '\t';
							continue;
						case 'u':
							break;
						default:
							continue;
						}
						int num = json.Length - index;
						if (num < 4)
						{
							break;
						}
						char[] array = new char[4];
						Array.Copy(json, index, array, 0, 4);
						uint num2 = uint.Parse(new string(array), NumberStyles.HexNumber);
						Console.WriteLine(num2);
						try
						{
							text += char.ConvertFromUtf32((int)num2);
						}
						catch (Exception)
						{
							char[] array2 = array;
							foreach (char c2 in array2)
							{
								text += c2;
							}
						}
						index += 4;
						continue;
					}
					default:
						text += c;
						continue;
					}
					break;
				}
				if (!flag)
				{
					return null;
				}
				return text;
			}

			private double parseNumber(char[] json, ref int index)
			{
				eatWhitespace(json, ref index);
				int lastIndexOfNumber = getLastIndexOfNumber(json, index);
				int num = lastIndexOfNumber - index + 1;
				char[] array = new char[num];
				Array.Copy(json, index, array, 0, num);
				index = lastIndexOfNumber + 1;
				return double.Parse(new string(array));
			}

			private int getLastIndexOfNumber(char[] json, int index)
			{
				int i;
				for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
				{
				}
				return i - 1;
			}

			private void eatWhitespace(char[] json, ref int index)
			{
				while (index < json.Length && " \t\n\r".IndexOf(json[index]) != -1)
				{
					index++;
				}
			}

			private JsonToken lookAhead(char[] json, int index)
			{
				int index2 = index;
				return nextToken(json, ref index2);
			}

			private JsonToken nextToken(char[] json, ref int index)
			{
				eatWhitespace(json, ref index);
				if (index == json.Length)
				{
					return JsonToken.None;
				}
				char c = json[index];
				index++;
				switch (c)
				{
				case '{':
					return JsonToken.CurlyOpen;
				case '}':
					return JsonToken.CurlyClose;
				case '[':
					return JsonToken.SquaredOpen;
				case ']':
					return JsonToken.SquaredClose;
				case ',':
					return JsonToken.Comma;
				case '"':
					return JsonToken.String;
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return JsonToken.Number;
				case ':':
					return JsonToken.Colon;
				default:
				{
					index--;
					int num = json.Length - index;
					if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
					{
						index += 5;
						return JsonToken.False;
					}
					if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
					{
						index += 4;
						return JsonToken.True;
					}
					if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
					{
						index += 4;
						return JsonToken.Null;
					}
					return JsonToken.None;
				}
				}
			}
		}

		internal class Serializer
		{
			private StringBuilder _builder;

			private Serializer()
			{
				_builder = new StringBuilder();
			}

			public static string serialize(object obj)
			{
				Serializer serializer = new Serializer();
				serializer.serializeObject(obj);
				return serializer._builder.ToString();
			}

			private void serializeObject(object value)
			{
				if (value == null)
				{
					_builder.Append("null");
				}
				else if (value is string)
				{
					serializeString((string)value);
				}
				else if (value is IList)
				{
					serializeIList((IList)value);
				}
				else if (value is IDictionary)
				{
					serializeIDictionary((IDictionary)value);
				}
				else if (value is bool)
				{
					_builder.Append(value.ToString().ToLower());
				}
				else if (value.GetType().IsPrimitive)
				{
					_builder.Append(value);
				}
				else if (value is DateTime)
				{
					DateTime value2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					double totalMilliseconds = ((DateTime)value).Subtract(value2).TotalMilliseconds;
					serializeString(Convert.ToString(totalMilliseconds));
				}
				else
				{
					serializeString(value.ToString());
				}
			}

			private void serializeIList(IList anArray)
			{
				_builder.Append("[");
				bool flag = true;
				for (int i = 0; i < anArray.Count; i++)
				{
					object value = anArray[i];
					if (!flag)
					{
						_builder.Append(", ");
					}
					serializeObject(value);
					flag = false;
				}
				_builder.Append("]");
			}

			private void serializeIDictionary(IDictionary dict)
			{
				_builder.Append("{");
				bool flag = true;
				IEnumerator enumerator = dict.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (!flag)
						{
							_builder.Append(", ");
						}
						serializeString(current.ToString());
						_builder.Append(":");
						serializeObject(dict[current]);
						flag = false;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = enumerator as IDisposable) != null)
					{
						disposable.Dispose();
					}
				}
				_builder.Append("}");
			}

			private void serializeString(string str)
			{
				_builder.Append("\"");
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					switch (c)
					{
					case '"':
						_builder.Append("\\\"");
						continue;
					case '\\':
						_builder.Append("\\\\");
						continue;
					case '\b':
						_builder.Append("\\b");
						continue;
					case '\f':
						_builder.Append("\\f");
						continue;
					case '\n':
						_builder.Append("\\n");
						continue;
					case '\r':
						_builder.Append("\\r");
						continue;
					case '\t':
						_builder.Append("\\t");
						continue;
					}
					int num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						_builder.Append(c);
					}
					else
					{
						_builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
					}
				}
				_builder.Append("\"");
			}
		}

		public static object jsonDecode(string json)
		{
			return jsonDecode(json, false);
		}

		public static object jsonDecode(string json, bool decodeUsingGenericContainers)
		{
			return Deserializer.deserialize(json, decodeUsingGenericContainers);
		}

		public static string jsonEncode(object obj)
		{
			return Serializer.serialize(obj);
		}
	}
}
