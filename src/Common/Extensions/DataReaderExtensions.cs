using System.Data;

namespace Numaka.Common.Extensions
{
	/// <summary>
	/// DataReader extensions
	/// </summary>
	public static class DataReaderExtensions
	{
		/// <summary>
		///     Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataReader">The data reader.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <returns>T.</returns>
		public static T GetValue<T>(this IDataReader dataReader, string columnName)
		{
			if (dataReader.IsDBNull(dataReader.GetOrdinal(columnName)))
			{
				return default;
			}

			return (T)dataReader[columnName];
		}
	}
}