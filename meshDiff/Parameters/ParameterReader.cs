// author: Jan Horesovsky

using System;
using System.IO;

namespace meshDiff
{
	class ParameterReader : IDisposable
	{
		StreamReader streamReader;
		string fileSectionName;
		string fileSectionDecoration;
		string fileSectionString;
		bool firstCall;
		bool endOfParams;

		/// <summary>
		/// The reader will look for a section starting with line ("{0}{1}{0}", fileSectionDecoration, fileSectionName)
		/// </summary>
		public ParameterReader(string path, string fileSectionName, string fileSectionDecoration = "---")
		{
			streamReader = new StreamReader(path);
			fileSectionString = fileSectionDecoration + fileSectionName + fileSectionDecoration;
			this.fileSectionName = fileSectionName;
			this.fileSectionDecoration = fileSectionDecoration;
			firstCall = true;
			endOfParams = false;
		}

		/// <summary>
		/// Reads a line containing parameter name/value pair of the given section.
		/// Discards all other lines.
		/// </summary>
		/// <returns>Null once the end of the section has been reached.</returns>
		public Tuple<string, string> ReadPair()
		{
			string line;

			if (endOfParams)
			{
				return null;
			}

			if (firstCall)
			{
				// repeat until section found or EOF
				while ((line = streamReader.ReadLine()) != null && line != fileSectionString)
				{
					continue;
				}

				firstCall = false;
			}

			// discard all lines not containing a parameter name/value pair
			while ((line = streamReader.ReadLine()) != null && line.Split('=').Length != 2)
			{
				// end of section was hit
				if (line.StartsWith(fileSectionDecoration))
				{
					endOfParams = true;
					return null;
				}

				continue;
			}

			// if EOF
			if (line == null)
			{
				endOfParams = true;
				return null;
			}

			// we have the desired line, split it and return the name and value
			string[] lineParts = line.Split('=');

			return new Tuple<string, string>(lineParts[0], lineParts[1]);
		}

		public void Dispose()
		{
			streamReader.Dispose();
		}
	}
}
