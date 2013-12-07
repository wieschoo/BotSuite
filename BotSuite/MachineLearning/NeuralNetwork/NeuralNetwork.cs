// -----------------------------------------------------------------------
//  <copyright file="NeuralNetwork.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.MachineLearning.NeuralNetwork
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Runtime.Serialization.Formatters.Binary;

	using global::BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions;
	using global::BotSuite.MachineLearning.NeuralNetwork.LearningAlgorithms;

	/// <summary>
	///     represent an artificial feed forward neural network
	/// </summary>
	[Serializable]
	public class NeuralNetwork
	{
		/// <summary>
		///     store the layers
		/// </summary>
		protected Layer[] NetworkLayers;

		/// <summary>
		///     number of input neurons
		/// </summary>
		protected int ProtectedNumberOfInputs;

		/// <summary>
		///     the learning algorithmn to improve weights
		/// </summary>
		protected LearningAlgorithm TeacherAlgorithm;

		/// <summary>
		///     get number of input neurons
		/// </summary>
		public int NumberOfInputs
		{
			get
			{
				return this.ProtectedNumberOfInputs;
			}
		}

		/// <summary>
		///     get number of output neurons
		/// </summary>
		public int NumberOfOutputs
		{
			get
			{
				return this.NetworkLayers[this.NumberOfLayers - 1].NeuronsInLayer;
			}
		}

		/// <summary>
		///     get number of hidden layers
		/// </summary>
		public int NumberOfLayers
		{
			get
			{
				return this.NetworkLayers.Length;
			}
		}

		/// <summary>
		///     the learning algorithm
		/// </summary>
		public LearningAlgorithm Teacher
		{
			get
			{
				return this.TeacherAlgorithm;
			}
			set
			{
				this.TeacherAlgorithm = value ?? this.TeacherAlgorithm;
			}
		}

		/// <summary>
		///     pointer to hidden layer n
		/// </summary>
		/// <param name="n">
		///     index n
		/// </param>
		/// <returns>
		///     The <see cref="Layer" />.
		/// </returns>
		public Layer this[int n]
		{
			get
			{
				return this.NetworkLayers[n];
			}
		}

		/// <summary>
		///     activation function the each neuron in each layer
		/// </summary>
		public IActivationFunction ActivationFunction
		{
			set
			{
				foreach (Layer l in this.NetworkLayers)
				{
					l.ActivationFunction = value;
				}
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NeuralNetwork" /> class.
		///     initialise neural network
		/// </summary>
		/// <param name="numberOfInputs">
		///     the number of input vectors
		/// </param>
		/// <param name="layerVectorDescription">
		///     description of hidden layers
		/// </param>
		/// <param name="activationFunction">
		///     the activation function (default sigmoid)
		/// </param>
		/// <param name="learner">
		///     Learning algorithm (default: backpropagation)
		/// </param>
		public NeuralNetwork(
			int numberOfInputs,
			IList<int> layerVectorDescription,
			IActivationFunction activationFunction = null,
			LearningAlgorithm learner = null)
		{
			if (layerVectorDescription.Count < 1)
			{
				throw new Exception("NeuralNetwork -> cannot be built, since there are no neurons");
			}
			if (numberOfInputs < 1)
			{
				throw new Exception("NeuralNetwork -> cannot be built, since it need at least 1 input");
			}

			this.ProtectedNumberOfInputs = numberOfInputs;
			this.TeacherAlgorithm = learner ?? new BackPropagationLearningAlgorithm(this);

			this.NetworkLayers = new Layer[layerVectorDescription.Count];
			this.NetworkLayers[0] = new Layer(layerVectorDescription[0], this.ProtectedNumberOfInputs);
			for (int i = 1; i < layerVectorDescription.Count; i++)
			{
				this.NetworkLayers[i] = new Layer(layerVectorDescription[i], layerVectorDescription[i - 1], activationFunction);
			}
		}

		/// <summary>
		///     set ranom weights to each connection
		/// </summary>
		public void ShuffleWeight()
		{
			foreach (Layer l in this.NetworkLayers)
			{
				l.ShuffleWeight();
			}
		}

		/// <summary>
		///     set random threshold to each neuron
		/// </summary>
		public void ShuffleThreshold()
		{
			foreach (Layer l in this.NetworkLayers)
			{
				l.ShuffleThreshold();
			}
		}

		/// <summary>
		///     everything is random
		/// </summary>
		public void ShuffleBoth()
		{
			foreach (Layer l in this.NetworkLayers)
			{
				l.ShuffleBoth();
			}
		}

		/// <summary>
		///     set interval for random numbers
		/// </summary>
		/// <param name="min">
		///     left interval ending
		/// </param>
		/// <param name="max">
		///     right interval ending
		/// </param>
		public void SetIntervalForPrng(float min, float max)
		{
			foreach (Layer l in this.NetworkLayers)
			{
				l.SetIntervalForPrng(min, max);
			}
		}

		/// <summary>
		///     store the network as a binary file
		/// </summary>
		/// <code>
		/// NeuralNetwork ANN = new NeuralNetwork(...);
		/// Ann.Store("savednetwork.bin");
		/// </code>
		/// <param name="file">
		///     file name
		/// </param>
		public void Store(string file)
		{
			IFormatter binFmt = new BinaryFormatter();
			Stream s = File.Open(file, FileMode.Create);
			binFmt.Serialize(s, this);
			s.Close();
		}

		/// <summary>
		///     load the neural network from binary file
		/// </summary>
		/// <code>
		/// NeuralNetwork ANN = NeuralNetwork.Load("savednetwork.bin");
		/// </code>
		/// <param name="file">
		/// </param>
		/// <returns>
		///     The <see cref="NeuralNetwork" />.
		/// </returns>
		public static NeuralNetwork Load(string file)
		{
			NeuralNetwork result;
			try
			{
				IFormatter binFmt = new BinaryFormatter();
				Stream s = File.Open(file, FileMode.Open);
				result = (NeuralNetwork)binFmt.Deserialize(s);
				s.Close();
			}
			catch (Exception e)
			{
				throw new Exception("NeuralNetwork : Unable to load file " + file + " : " + e);
			}

			return result;
		}

		/// <summary>
		///     wrapper for direct learning
		/// </summary>
		/// <param name="inputs">
		///     input training data
		/// </param>
		/// <param name="expectedOutputs">
		///     the expected output of training data
		/// </param>
		public void Learn(float[][] inputs, float[][] expectedOutputs)
		{
			this.Teacher.Learn(inputs, expectedOutputs);
		}

		/// <summary>
		///     calculates the output for specific input
		/// </summary>
		/// <param name="input">
		///     input vector
		/// </param>
		/// <returns>
		///     output vector
		/// </returns>
		public float[] Output(float[] input)
		{
			if (input.Length != this.ProtectedNumberOfInputs)
			{
				throw new Exception("PERCEPTRON : Wrong input vector size, unable to compute output value");
			}
			float[] result = this.NetworkLayers[0].Signal(input);
			for (int i = 1; i < this.NumberOfLayers; i++)
			{
				result = this.NetworkLayers[i].Signal(result);
			}
			return result;
		}

		/// <summary>
		///     calculates the prediction error (good for cross validation)
		/// </summary>
		/// <param name="inputs">
		///     set of training data/ test data
		/// </param>
		/// <param name="expectedOutputs">
		///     the expected output
		/// </param>
		/// <returns>
		///     The <see cref="float" />.
		/// </returns>
		public float PredictionError(float[][] inputs, float[][] expectedOutputs)
		{
			return inputs.Select(this.Output).Select((result, i) => result.Select((t, j) => (t - expectedOutputs[i][j]) * (t - expectedOutputs[i][j])).Sum()).Sum();
		}
	}
}