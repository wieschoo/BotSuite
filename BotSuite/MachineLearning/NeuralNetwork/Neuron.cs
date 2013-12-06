// -----------------------------------------------------------------------
//  <copyright file="Neuron.cs" company="HoovesWare">
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
	///     this class represents a neuron
	/// </summary>
	/// <remarks>
	///     output = f( threshold* \sum_{j=1}^{N_Inputs} s_{t-1,j}w_j )
	/// </remarks>
	[Serializable]
	public class Neuron
	{
		/// <summary>
		///     intern instance of a PseudoRandomNumberGenerator
		/// </summary>
		protected static Random Prng = new Random();

		/// <summary>
		///     intern interval for randomisation
		/// </summary>
		protected float IntervallMin = -1.0f;

		/// <summary>
		///     intern interval for randomisation
		/// </summary>
		protected float IntervallMax = 1.0f;

		/// <summary>
		///     intern weights of inputs
		/// </summary>
		protected float[] Weight;

		/// <summary>
		///     intern weights of inputs
		/// </summary>
		protected float[] RememberWeight;

		/// <summary>
		///     intern theshold variable
		/// </summary>
		protected float Threshhold = 0f;

		/// <summary>
		///     intern theshold variable
		/// </summary>
		protected float RememberThreshold = 0f;

		/// <summary>
		///     intern handle of the activation function
		/// </summary>
		protected IActivationFunction ActivationFunction = null;

		/// <summary>
		///     the output
		/// </summary>
		protected float NeuronOutput = 0f;

		/// <summary>
		///     values for faster computation
		/// </summary>
		protected float SynapsevalueWithThreshold = 0f;

		/// <summary>
		///     values for faster computation
		/// </summary>
		protected float UsefullVariable;

		/// <summary>
		///     returns number of neurons which affect this neuron
		/// </summary>
		public int NumberOfNeurons
		{
			get
			{
				return this.Weight.Length;
			}
		}

		/// <summary>
		///     pointer to all neurons that affect this neuron
		/// </summary>
		/// <param name="neuronIdx">
		/// </param>
		/// <returns>
		///     The <see cref="float" />.
		/// </returns>
		public float this[int neuronIdx]
		{
			get
			{
				return this.Weight[neuronIdx];
			}
			set
			{
				this.RememberWeight[neuronIdx] = this.Weight[neuronIdx];
				this.Weight[neuronIdx] = value;
			}
		}

		/// <summary>
		///     thresholding
		/// </summary>
		public float Threshold
		{
			get
			{
				return this.Threshhold;
			}
			set
			{
				this.RememberThreshold = this.Threshhold;
				this.Threshhold = value;
			}
		}

		/// <summary>
		///     remember last threshold
		/// </summary>
		public float LastThreshold
		{
			get
			{
				return this.RememberThreshold;
			}
		}

		/// <summary>
		///     get the output of this neuron
		/// </summary>
		public float Output
		{
			get
			{
				return this.NeuronOutput;
			}
		}

		/// <summary>
		///     get gradient value for stochastic gradient descent
		/// </summary>
		public float Derivative
		{
			get
			{
				return this.ActivationFunction.Derivative(this.SynapsevalueWithThreshold);
			}
		}

		/// <summary>
		///     this is useful for calculations
		/// </summary>
		public float LastSumOfInputsWithTreshold
		{
			get
			{
				return this.SynapsevalueWithThreshold;
			}
		}

		/// <summary>
		///     set or get the activationfunction
		/// </summary>
		public IActivationFunction F
		{
			get
			{
				return this.ActivationFunction;
			}
			set
			{
				this.ActivationFunction = value;
			}
		}

		/// <summary>
		///     simplify the calculation
		/// </summary>
		public float UsefulVariable
		{
			get
			{
				return this.UsefullVariable;
			}
			set
			{
				this.UsefullVariable = value;
			}
		}

		/// <summary>
		///     remember last weights
		/// </summary>
		public float[] LastWeight
		{
			get
			{
				return this.RememberWeight;
			}
		}

		/// <summary>
		///     Get or set the minimum value for randomisation of weights and threshold
		/// </summary>
		public float RandomizationLeftIntervall
		{
			get
			{
				return this.IntervallMin;
			}
			set
			{
				this.IntervallMin = value;
			}
		}

		/// <summary>
		///     get or set the right end of interval value for randomization
		/// </summary>
		public float RandomizationRightIntervall
		{
			get
			{
				return this.IntervallMax;
			}
			set
			{
				this.IntervallMax = value;
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Neuron" /> class.
		///     initialise the neuron
		/// </summary>
		/// <param name="numberOfInputs">
		///     number of inputs in layer before
		/// </param>
		/// <param name="activationFunction">
		///     activation function for current neuron (default: sigmoid function)
		/// </param>
		public Neuron(int numberOfInputs, IActivationFunction activationFunction = null)
		{
			this.RememberWeight = new float[numberOfInputs];
			this.Weight = new float[numberOfInputs];
			this.ActivationFunction = activationFunction ?? new SigmoidActivationFunction();
		}

		/// <summary>
		///     set random weights
		/// </summary>
		public void ShuffleWeight()
		{
			for (int i = 0; i < this.NumberOfNeurons; i++)
			{
				this.RememberWeight[i] = 0f;
				this.Weight[i] = this.IntervallMin
								+ (Prng.Next(1000) / 1000f) * (this.IntervallMax - this.IntervallMin);
			}
		}

		/// <summary>
		///     set random threshold
		/// </summary>
		public void ShuffleThreshold()
		{
			this.Threshhold = this.IntervallMin
							+ (Prng.Next(1000) / 1000f) * (this.IntervallMax - this.IntervallMin);
		}

		/// <summary>
		///     set all random
		/// </summary>
		public void ShuffleBoth()
		{
			this.ShuffleThreshold();
			this.ShuffleWeight();
		}

		/// <summary>
		///     get the signal from this neuron
		/// </summary>
		/// <param name="input">
		/// </param>
		/// <returns>
		///     The <see cref="float" />.
		/// </returns>
		public float Signal(float[] input)
		{
			if (input.Length != this.NumberOfNeurons)
			{
				throw new Exception(
					"Network -> neuron-> wrong size of input vector " + input.Length + " should be "
					+ this.NumberOfNeurons);
			}

			this.SynapsevalueWithThreshold = 0;

			for (int i = 0; i < this.NumberOfNeurons; i++)
			{
				this.SynapsevalueWithThreshold += this.Weight[i] * input[i];
			}
			this.SynapsevalueWithThreshold -= this.Threshhold;

			this.NeuronOutput = this.ActivationFunction != null ? this.ActivationFunction.Function(this.SynapsevalueWithThreshold) : this.SynapsevalueWithThreshold;

			return this.NeuronOutput;
		}
	}
}