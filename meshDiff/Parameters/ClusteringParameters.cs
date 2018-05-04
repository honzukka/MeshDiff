// author: Jan Horesovsky

using System;
using System.Globalization;

namespace meshDiff
{
	public class ClusteringParameters
	{
		private int clusterCount;

		private int directionSignificance;
		private int magnitudeSignificance;
		private int positionSignificance;
		private int resolutionSignificance;

		// a text representation of the field which can be saved to a file
		string fileSectionName;

		const string clusterCountName = "clusterCount";
		const string directionSignificanceName = "directionSignificance";
		const string magnitudeSignificanceName = "magnitudeSignificance";
		const string positionSignificanceName = "positionSignificance";
		const string resolutionSignificanceName = "resolutionSignificance";

		public ClusteringParameters(string name)
		{
			fileSectionName = "Clustering Parameters " + name;

			// set default parameters
			clusterCount = 20;

			directionSignificance = 15;
			positionSignificance = 25;
			magnitudeSignificance = 10;
			resolutionSignificance = 0;
		}

		// no need to properly implement equality, so that's why there is a new method
		// instead of overriding Equals
		public bool HasSameValuesAs(ClusteringParameters other)
		{
			if (other == null)
			{
				return false;
			}

			return (
				clusterCount == other.clusterCount &&
				directionSignificance == other.directionSignificance &&
				positionSignificance == other.positionSignificance &&
				magnitudeSignificance == other.magnitudeSignificance &&
				resolutionSignificance == other.resolutionSignificance
			);
		}

		// when the only field that differs is the cluster count, the clustering configuration is still considered the same
		// (a new clustering doesn't have to be performed)
		public bool HasSameValuesAsExceptForClusterCount(ClusteringParameters other)
		{
			if (other == null)
			{
				return false;
			}

			return (
				directionSignificance == other.directionSignificance &&
				positionSignificance == other.positionSignificance &&
				magnitudeSignificance == other.magnitudeSignificance &&
				resolutionSignificance == other.resolutionSignificance
			);
		}

		public void CopyInto(ClusteringParameters other)
		{
			other.clusterCount = clusterCount;
			other.directionSignificance = directionSignificance;
			other.positionSignificance = positionSignificance;
			other.magnitudeSignificance = magnitudeSignificance;
			other.resolutionSignificance = resolutionSignificance;
		}

		public void ValidateValues()
		{
			if (DirectionSignificance == 0 && PositionSignificance == 0 && MagnitudeSignificance == 0 && ResolutionSignificance == 0)
			{
				throw new ArgumentException("All values cannot be zero.");
			}
		}

		public void SaveToFile(string path)
		{
			using (ParameterWriter paramWriter = new ParameterWriter(path, fileSectionName))
			{
				paramWriter.WritePair(clusterCountName, clusterCount.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(directionSignificanceName, directionSignificance.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(positionSignificanceName, positionSignificance.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(magnitudeSignificanceName, magnitudeSignificance.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(resolutionSignificanceName, resolutionSignificance.ToString(CultureInfo.InvariantCulture));
				paramWriter.WriteEmptyLine();
			}
		}

		public bool LoadFromFile(string path)
		{
			using (ParameterReader paramReader = new ParameterReader(path, fileSectionName))
			{
				Tuple<string, string> parameterPair;

				while ((parameterPair = paramReader.ReadPair()) != null)
				{
					try
					{
						switch (parameterPair.Item1)
						{
							case clusterCountName:
								ClusterCount = int.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case directionSignificanceName:
								DirectionSignificance = int.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case positionSignificanceName:
								PositionSignificance = int.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case magnitudeSignificanceName:
								MagnitudeSignificance = int.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case resolutionSignificanceName:
								ResolutionSignificance = int.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							default:
								break;
						}
					}
					catch (Exception ex) when (
						(ex is ArgumentOutOfRangeException) ||
						(ex is FormatException) ||
						(ex is OverflowException) ||
						(ex is ArgumentNullException)
					)
					{
						return false;
					}
				}
			}

			try
			{
				this.ValidateValues();
			}
			catch (ArgumentException)
			{
				return false;
			}

			return true;
		}

		public int ClusterCount
		{
			get { return clusterCount; }
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException(nameof(ClusterCount) + " cannot be lower than 1.");
				}

				clusterCount = value;
			}
		}

		/// <summary>
		/// Determines how significant the direction difference will be when computing the clustering error.
		/// </summary>
		public int DirectionSignificance
		{
			get { return directionSignificance; }
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException(nameof(DirectionSignificance) + " can only take values in [0, 100]");
				}

				directionSignificance = value;
			}
		}

		/// <summary>
		/// Determines how significant the magnitude difference will be when computing the clustering error.
		/// </summary>
		public int MagnitudeSignificance
		{
			get { return magnitudeSignificance; }
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException(nameof(MagnitudeSignificance) + " can only take values in [0, 100]");
				}

				magnitudeSignificance = value;
			}
		}

		/// <summary>
		/// Determines how significant the position difference will be when computing the clustering error.
		/// </summary>
		public int PositionSignificance
		{
			get { return positionSignificance; }
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException(nameof(PositionSignificance) + " can only take values in [0, 100]");
				}

				positionSignificance = value;
			}
		}

		/// <summary>
		/// Determines how significant the mesh resolution will be when computing the clustering error.
		/// </summary>
		public int ResolutionSignificance
		{
			get { return resolutionSignificance; }
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException(nameof(ResolutionSignificance) + " can only take values in [0, 100]");
				}

				resolutionSignificance = value;
			}
		}
	}
}
