// -----------------------------------------------------------------------
//  <copyright file="Layer.cs" company="HoovesWare">
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

	using global::BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions;

	/// <summary>
	///     organize the neurons
	/// </summary>
	[Serializable]
	public class Layer
	{
		/// <summary>
		///     number of neurons in layers
		/// </summary>
		protected int MNeuronsInLayer;

		/// <summary>
		///     number of inputs
		/// </summary>
		protected int MInputsOfLayer;

		/// <summary>
		///     internal array of neurons
		/// </summary>
		protected Neuron[] Neurons;

		/// <summary>
		///     the output ofthe layer
		/// </summary>
		protected float[] Output;

		/// <summary>
		///     set activation function for all neurons in layer
		/// </summary>
		public IActivationFunction ActivationFunction
		{
			set
			{
				foreach (Neuron n in this.Neurons)
				{
					n.F = value;
				}
			}
		}

		/// <summary>
		///     get number of neurons in current layer
		/// </summary>
		public int NeuronsInLayer
		{
			get
			{
				return this.MNeuronsInLayer;
			}
		}

		/// <summary>
		///     get number of inputs in current layer
		/// </summary>
		public int InputsOfLayer
		{
			get
			{
				return this.MInputsOfLayer;
			}
		}

		/// <summary>
		///     pointer to specific neuron
		/// </summary>
		/// <param name="neuronIdx">
		///     index of neuron
		/// </param>
		/// <returns>
		///     The <see cref="Neuron" />.
		/// </returns>
		public Neuron this[int neuronIdx]
		{
			get
			{
				return this.Neurons[neuronIdx];
			}
		}

		/// <summary>
		///     get last output of the neurons in this layer
		/// </summary>
		public float[] GetLastOuput
		{
			get
			{
				return this.Output;
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Layer" /> class.
		///     initialise a layer containing x inputs and n neurons
		/// </summary>
		/// <param name="numberOfInputs">
		///     how many inputs do we have
		/// </param>
		/// <param name="numberOfNeurons">
		///     how many neurons should this layer contain
		/// </param>
		/// <param name="f">
		///     activation function foreach neuron in this layer
		/// </param>
		public Layer(int numberOfInputs, int numberOfNeurons, IActivationFunction f = null)
		{
			this.MNeuronsInLayer = numberOfInputs;
			this.MInputsOfLayer = numberOfNeurons;
			this.Neurons = new Neuron[this.MNeuronsInLayer];
			this.Output = new float[this.MNeuronsInLayer];
			for (int i = 0; i < numberOfInputs; i++)
			{
				this.Neurons[i] = new Neuron(numberOfNeurons, f);
			}
		}

		/// <summary>
		///     set random weights to each neuron
		/// </summary>
		public void ShuffleWeight()
		{
			foreach (Neuron n in this.Neurons)
			{
				n.ShuffleWeight();
			}
		}

		/// <summary>
		///     set random thresholds to each neuron
		/// </summary>
		public void ShuffleThreshold()
		{
			foreach (Neuron n in this.Neurons)
			{
				n.ShuffleThreshold();
			}
		}

		/// <summary>
		///     everything is random in this layer
		/// </summary>
		public void ShuffleBoth()
		{
			this.ShuffleThreshold();
			this.ShuffleWeight();
		}

		/// <summary>
		///     Set the randomization interval for all neurons
		/// </summary>
		/// <param name="min">
		///     the minimum value
		/// </param>
		/// <param name="max">
		///     the maximum value
		/// </param>
		public void SetIntervalForPrng(float min, float max)
		{
			foreach (Neuron n in this.Neurons)
			{
				n.RandomizationLeftIntervall = min;
				n.RandomizationRightIntervall = max;
			}
		}

		/// <summary>
		///     get the signal from each neuron as a vector
		/// </summary>
		/// <param name="input">
		///     input vector
		/// </param>
		/// <returns>
		///     vector of neuron output
		/// </returns>
		public float[] Signal(float[] input)
		{
			if (input.Length != this.MInputsOfLayer)
			{
				throw new Exception("LAYER : Wrong input vector size, unable to compute output value");
			}
			for (int i = 0; i < this.MNeuronsInLayer; i++)
			{
				this.Output[i] = this.Neurons[i].Signal(input);
			}
			return this.Output;
		}
	}
}