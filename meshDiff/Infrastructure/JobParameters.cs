// author: Jan Horesovsky

using Scene3D;

namespace meshDiff
{
	class JobParameters
	{
		/// <summary>
		/// A DiffAlgo instance which will take care of clustering for the arrow visualization.
		/// </summary>
		public IDiffAlgo DiffForArrows { get; }

		/// <summary>
		/// A DiffAlgo instance which will take care of clustering for the color visualization.
		/// </summary>
		public IDiffAlgo DiffForColors { get; }

		/// <summary>
		/// A metric to be used to obtain arrows from the two input scenes.
		/// </summary>
		public MetricType Metric { get; }

		/// <summary>
		/// Clustering parameters for color visualization.
		/// </summary>
		public ClusteringParameters ClusteringParametersColors { get; }

		/// <summary>
		/// Clustering parameters for arrow visualization.
		/// </summary>
		public ClusteringParameters ClusteringParametersArrows { get; }

		/// <summary>
		/// Visualizer parameters.
		/// </summary>
		public VisualizerParameters VisualizerParameters { get; }

		/// <summary>
		/// An object which generates a color visualization.
		/// </summary>
		public IVisualizer VisualizerColor { get; }

		/// <summary>
		/// An object which generates an arrow visualization.
		/// </summary>
		public IVisualizer VisualizerArrow { get; }

		/// <summary>
		/// The left input scene and also the scene where the color visualization will be added.
		/// </summary>
		public SceneBrep Scene1Color { get; }

		/// <summary>
		/// The right input scene and also the scene where the color visualization will be added.
		/// </summary>
		public SceneBrep Scene2Color { get; }

		/// <summary>
		/// The scene where the arrow visualization for the left model will be stored.
		/// </summary>
		public SceneBrep Scene1Arrows { get; }

		/// <summary>
		/// The scene where the arrow visualization for the right model will be stored.
		/// </summary>
		public SceneBrep Scene2Arrows { get; }

		/// <param name="diffForArrows">A DiffAlgo instance which will take care of clustering for the arrow visualization.</param>
		/// <param name="diffForColors">A DiffAlgo instance which will take care of clustering for the color visualization.</param>
		/// <param name="metric">A metric to be used to obtain arrows from the two input scenes.</param>
		/// <param name="clusteringParametersColor">Clustering parameters for color visualization.</param>
		/// <param name="clusteringParametersArrows">Clustering parameters for arrow visualization.</param>
		/// <param name="visualizerParameters">Visualizer parameters.</param>
		/// <param name="visualizerColor">An object which generates a color visualization.</param>
		/// <param name="visualizerArrow">An object which generates an arrow visualization.</param>
		/// <param name="scene1Color">The left input scene and also the scene where the color visualization will be added.</param>
		/// <param name="scene2Color">The right input scene and also the scene where the color visualization will be added.</param>
		/// <param name="scene1Arrows">The scene where the arrow visualization for the left model will be stored.</param>
		/// <param name="scene2Arrows">The scene where the arrow visualization for the right model will be stored.</param>
		public JobParameters(IDiffAlgo diffForArrows, IDiffAlgo diffForColors, MetricType metric, 
			ClusteringParameters clusteringParametersColor, ClusteringParameters clusteringParametersArrows,
			VisualizerParameters visualizerParameters, IVisualizer visualizerColor, IVisualizer visualizerArrow,
			SceneBrep scene1Color, SceneBrep scene2Color, 
			SceneBrep scene1Arrows, SceneBrep scene2Arrows)
		{
			DiffForArrows = diffForArrows;
			DiffForColors = diffForColors;
			Metric = metric;
			ClusteringParametersColors = clusteringParametersColor;
			ClusteringParametersArrows = clusteringParametersArrows;
			VisualizerParameters = visualizerParameters;
			VisualizerColor = visualizerColor;
			VisualizerArrow = visualizerArrow;
			Scene1Color = scene1Color;
			Scene2Color = scene2Color;
			Scene1Arrows = scene1Arrows;
			Scene2Arrows = scene2Arrows;
		}
	}
}
