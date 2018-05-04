// author: Jan Horesovsky

using System;
using System.IO;

namespace meshDiff
{
	class ParameterWriter : IDisposable
	{
		StreamWriter streamWriter;
		string fileSectionName;
		string fileSectionDecoration;
		string fileSectionString;
		bool firstCall;

		/// <summary>
		/// The writer will create a section starting with line ("{0}{1}{0}", fileSectionDecoration, fileSectionName)
		/// </summary>
		public ParameterWriter(string path, string fileSectionName, string fileSectionDecoration = "---")
		{
			streamWriter = new StreamWriter(path, true);
			fileSectionString = fileSectionDecoration + fileSectionName + fileSectionDecoration;
			this.fileSectionName = fileSectionName;
			this.fileSectionDecoration = fileSectionDecoration;
			firstCall = true;
		}

		/// <summary>
		/// Writes a line containing parameter name/value pair.
		/// Writes the section name upon the first call.
		/// </summary>
		public void WritePair(string paramName, string paramValue)
		{
			if (firstCall)
			{
				streamWriter.WriteLine(fileSectionString);
				firstCall = false;
			}

			streamWriter.WriteLine(paramName + "=" + paramValue);
		}

		public void WriteEmptyLine()
		{
			streamWriter.WriteLine();
		}

		public void Dispose()
		{
			streamWriter.Dispose();
		}
	}
}
