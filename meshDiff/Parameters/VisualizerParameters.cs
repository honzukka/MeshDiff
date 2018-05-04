// author: Jan Horesovsky

using OpenTK;
using System;
using System.Globalization;

namespace meshDiff
{
	public class VisualizerParameters
	{
		public Vector3 ArrowOutwardsColor { get; set; }
		public Vector3 ArrowInwardsColor { get; set; }
		public Vector3 ColorMetricOutwards { get; set; }
		public Vector3 ColorMetricInwards { get; set; }
		public Vector3 DisabledColor { get; set; }

		private float arrowWidthMinScale;
		private float arrowWidthMaxScale;

		private float arrowHeightMinScale;
		private float arrowHeightMaxScale;

		private float disabledThresholdLength;
		private float disabledThresholdSize;
		private float colorDiffThreshold;

		// a text representation of the field which can be saved to a file
		const string fileSectionName = "Visualizer Parameters";

		const string arrowOutwardsColorName = "arrowOutwardsColor";
		const string arrowInwardsColorName = "arrowInwardsColor";
		const string colorMetricOutwardsName = "colorMetricOutwards";
		const string colorMetricInwardsName = "colorMetricInwards";
		const string disabledColorName = "disabledColor";
		const string arrowWidthMinScaleName = "arrowWidthMinScale";
		const string arrowWidthMaxScaleName = "arrowWidthMaxScale";
		const string arrowHeightMinScaleName = "arrowHeightMinScale";
		const string arrowHeightMaxScaleName = "arrowHeightMaxScale";
		const string disabledThresholdLengthName = "disabledThresholdLength";
		const string disabledThresholdSizeName = "disabledThresholdSize";
		const string colorDiffThresholdName = "colorDiffThreshold";

		public VisualizerParameters()
		{
			// set default parameters
			ArrowOutwardsColor = new Vector3(1, 1, 0);
			ArrowInwardsColor = new Vector3(0, 0, 1);
			ColorMetricOutwards = new Vector3(1, 0, 0);
			ColorMetricInwards = new Vector3(0, 1, 0);
			DisabledColor = new Vector3(0.3f, 0.3f, 0.3f);

			arrowWidthMinScale = 0.5f;
			arrowWidthMaxScale = 5f;

			arrowHeightMinScale = 0.5f;
			arrowHeightMaxScale = 3f;

			disabledThresholdLength = 0f;
			disabledThresholdSize = 0f;
			colorDiffThreshold = 10f;
		}

		public bool HasSameValuesAs(VisualizerParameters other)
		{
			if (other == null)
			{
				return false;
			}

			return (
				ArrowOutwardsColor == other.ArrowOutwardsColor &&
				ArrowInwardsColor == other.ArrowInwardsColor &&
				ColorMetricOutwards == other.ColorMetricOutwards &&
				ColorMetricInwards == other.ColorMetricInwards &&
				DisabledColor == other.DisabledColor &&
				arrowWidthMinScale == other.arrowWidthMinScale &&
				arrowWidthMaxScale == other.arrowWidthMaxScale &&
				arrowHeightMinScale == other.arrowHeightMinScale &&
				arrowHeightMaxScale == other.arrowHeightMaxScale &&
				disabledThresholdLength == other.disabledThresholdLength &&
				disabledThresholdSize == other.disabledThresholdSize &&
				colorDiffThreshold == other.colorDiffThreshold
			);
		}

		public void CopyInto(VisualizerParameters other)
		{
			other.ArrowOutwardsColor = ArrowOutwardsColor;
			other.ArrowInwardsColor = ArrowInwardsColor;
			other.ColorMetricOutwards = ColorMetricOutwards;
			other.ColorMetricInwards = ColorMetricInwards;
			other.DisabledColor = DisabledColor;
			other.arrowWidthMinScale = arrowWidthMinScale;
			other.arrowWidthMaxScale = arrowWidthMaxScale;
			other.arrowHeightMinScale = arrowHeightMinScale;
			other.arrowHeightMaxScale = arrowHeightMaxScale;
			other.disabledThresholdLength = disabledThresholdLength;
			other.disabledThresholdSize = disabledThresholdSize;
			other.colorDiffThreshold = colorDiffThreshold;
		}

		public void SaveToFile(string path)
		{
			using (ParameterWriter paramWriter = new ParameterWriter(path, fileSectionName))
			{
				paramWriter.WritePair(arrowOutwardsColorName, Helpers.WriteColor(ArrowOutwardsColor));
				paramWriter.WritePair(arrowInwardsColorName, Helpers.WriteColor(ArrowInwardsColor));
				paramWriter.WritePair(colorMetricOutwardsName, Helpers.WriteColor(ColorMetricOutwards));
				paramWriter.WritePair(colorMetricInwardsName, Helpers.WriteColor(ColorMetricInwards));
				paramWriter.WritePair(disabledColorName, Helpers.WriteColor(DisabledColor));
				paramWriter.WritePair(arrowWidthMinScaleName, ArrowWidthMinScale.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(arrowWidthMaxScaleName, ArrowWidthMaxScale.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(arrowHeightMinScaleName, ArrowHeightMinScale.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(arrowHeightMaxScaleName, ArrowHeightMaxScale.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(disabledThresholdLengthName, DisabledThresholdLength.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(disabledThresholdSizeName, DisabledThresholdSize.ToString(CultureInfo.InvariantCulture));
				paramWriter.WritePair(colorDiffThresholdName, ColorDiffThreshold.ToString(CultureInfo.InvariantCulture));
				paramWriter.WriteEmptyLine();
			}
		}

		public static VisualizerParameters LoadFromFile(string path)
		{
			VisualizerParameters loadedParameters = new VisualizerParameters();

			using (ParameterReader paramReader = new ParameterReader(path, fileSectionName))
			{
				Tuple<string, string> parameterPair;

				while ((parameterPair = paramReader.ReadPair()) != null)
				{
					try
					{
						switch (parameterPair.Item1)
						{
							case arrowOutwardsColorName:
								loadedParameters.ArrowOutwardsColor = Helpers.ParseColor(parameterPair.Item2);
								break;
							case arrowInwardsColorName:
								loadedParameters.ArrowInwardsColor = Helpers.ParseColor(parameterPair.Item2);
								break;
							case colorMetricOutwardsName:
								loadedParameters.ColorMetricOutwards = Helpers.ParseColor(parameterPair.Item2);
								break;
							case colorMetricInwardsName:
								loadedParameters.ColorMetricInwards = Helpers.ParseColor(parameterPair.Item2);
								break;
							case disabledColorName:
								loadedParameters.DisabledColor = Helpers.ParseColor(parameterPair.Item2);
								break;
							case arrowWidthMinScaleName:
								loadedParameters.ArrowWidthMinScale = float.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case arrowWidthMaxScaleName:
								loadedParameters.ArrowWidthMaxScale = float.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case arrowHeightMinScaleName:
								loadedParameters.ArrowHeightMinScale = float.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case arrowHeightMaxScaleName:
								loadedParameters.ArrowHeightMaxScale = float.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case disabledThresholdLengthName:
								loadedParameters.DisabledThresholdLength = float.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case disabledThresholdSizeName:
								loadedParameters.DisabledThresholdSize = float.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
								break;
							case colorDiffThresholdName:
								loadedParameters.ColorDiffThreshold = float.Parse(parameterPair.Item2, CultureInfo.InvariantCulture);
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
						// just return the default
						return new VisualizerParameters();
					}
				}
			}

			try
			{
				loadedParameters.ValidateValues();
			}
			catch (ArgumentException)
			{
				// just return the default
				return new VisualizerParameters();
			}

			return loadedParameters;
		}

		public void ValidateValues()
		{
			if (arrowWidthMinScale > arrowWidthMaxScale)
			{
				throw new ArgumentException(nameof(arrowWidthMinScale) + " cannot be greater than " + nameof(arrowWidthMaxScale));
			}

			if (arrowHeightMinScale > arrowHeightMaxScale)
			{
				throw new ArgumentException(nameof(arrowHeightMinScale) + " cannot be greater than " + nameof(arrowHeightMaxScale));
			}
		}

		/// <summary>
		/// Determines the width scale of the thinnest arrow copied into the visualization.
		/// </summary>
		public float ArrowWidthMinScale
		{
			get { return arrowWidthMinScale; }
			set
			{
				if (value < 0.1 || value > 10)
				{
					throw new ArgumentOutOfRangeException(nameof(ArrowWidthMinScale) + " can only take values in [0.1, 10]");
				}

				arrowWidthMinScale = value;
			}
		}

		/// <summary>
		/// Determines the width scale of the thickest arrow copied into the visualization.
		/// </summary>
		public float ArrowWidthMaxScale
		{
			get { return arrowWidthMaxScale; }
			set
			{
				if (value < 0.1 || value > 10)
				{
					throw new ArgumentOutOfRangeException(nameof(ArrowWidthMaxScale) + " can only take values in [0.1, 10]");
				}

				arrowWidthMaxScale = value;
			}
		}

		/// <summary>
		/// Determines the height scale of the shortest arrow copied into the visualization.
		/// </summary>
		public float ArrowHeightMinScale
		{
			get { return arrowHeightMinScale; }
			set
			{
				if (value < 0.1 || value > 10)
				{
					throw new ArgumentOutOfRangeException(nameof(ArrowHeightMinScale) + " can only take values in [0.1, 10]");
				}

				arrowHeightMinScale = value;
			}
		}

		/// <summary>
		/// Determines the height scale of the longest arrow copied into the visualization.
		/// </summary>
		public float ArrowHeightMaxScale
		{
			get { return arrowHeightMaxScale; }
			set
			{
				if (value < 0.1 || value > 10)
				{
					throw new ArgumentOutOfRangeException(nameof(ArrowHeightMaxScale) + " can only take values in [0.1, 10]");
				}

				arrowHeightMaxScale = value;
			}
		}

		/// <summary>
		/// Determines the length threshold under which arrows will not be copied into the visualization.
		/// </summary>
		public float DisabledThresholdLength
		{
			get { return disabledThresholdLength; }
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(DisabledThresholdLength) + " has to be greater than or equal to 0.");
				}

				disabledThresholdLength = value;
			}
		}

		/// <summary>
		/// Determines the cluster size threshold under which those clusteres will not be represented in the visualization.
		/// </summary>
		public float DisabledThresholdSize
		{
			get { return disabledThresholdSize; }
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(DisabledThresholdSize) + " has to be greater than or equal to 0.");
				}

				disabledThresholdSize = value;
			}
		}

		/// <summary>
		/// Determines the threshold above which all differences will have the brightest possible color visualization.
		/// Color brightness scales uniformly below this threshold.
		/// </summary>
		public float ColorDiffThreshold
		{
			get { return colorDiffThreshold; }
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(ColorDiffThreshold) + " has to be greater than 0.");
				}

				colorDiffThreshold = value;
			}
		}
	}
}
