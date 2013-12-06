// -----------------------------------------------------------------------
//  <copyright file="LearningAlgorithm.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.MachineLearning.NeuralNetwork.LearningAlgorithms
{
	using System;

	using global::BotSuite.MachineLearning.NeuralNetwork.LearningMonitors;

	/// <summary>
	///     The abstract class describing a learning
	///     algorithm for a neural network
	/// </summary>
	[Serializable]
	public abstract class LearningAlgorithm
	{
		/// <summary>
		///     intern error threshold
		/// </summary>
		protected float MErrorThreshhold;

		/// <summary>
		///     the maximum of epochs in gradient descent
		/// </summary>
		protected int MMaximumOfEpochs;

		/// <summary>
		///     the input training data
		/// </summary>
		protected float[][] InputTrainingData;

		/// <summary>
		///     the corresponding expected output for training data
		/// </summary>
		protected float[][] OutputTrainingData;

		/// <summary>
		///     intern variable for extern monitoring
		/// </summary>
		protected ILearningMonitor Monitor;

		/// <summary>
		///     the neural network to learn
		/// </summary>
		public NeuralNetwork ANN { get; protected set; }

		/// <summary>
		///     the mean square error in training data
		/// </summary>
		public float MeanSquareError { get; protected set; }

		/// <summary>
		///     access to the error threshold
		/// </summary>
		public float ErrorTreshold
		{
			get
			{
				return this.MErrorThreshhold;
			}
			set
			{
				this.MErrorThreshhold = (value > 0) ? value : this.MErrorThreshhold;
			}
		}

		/// <summary>
		///     extern monitor to get information about progress in learning
		/// </summary>
		public ILearningMonitor LearningMonitor
		{
			get
			{
				return this.Monitor;
			}
			set
			{
				this.Monitor = value;
			}
		}

		/// <summary>
		///     the current epoch
		/// </summary>
		public int CurrentEpoch { get; protected set; }

		/// <summary>
		///     get or set maximum of epoch
		/// </summary>
		public int MaximumOfEpochs
		{
			get
			{
				return this.MMaximumOfEpochs;
			}
			set
			{
				this.MMaximumOfEpochs = (value > 0) ? value : this.MMaximumOfEpochs;
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="LearningAlgorithm" /> class.
		///     The learning algorithm.
		/// </summary>
		/// <param name="ann">
		///     ANN to train
		/// </param>
		protected LearningAlgorithm(NeuralNetwork ann)
		{
			this.ANN = ann;
			this.ErrorTreshold = 0.005f;
			this.MaximumOfEpochs = 10000;
			this.CurrentEpoch = 0;
			this.MeanSquareError = -1.0f;
		}

		/// <summary>
		///     base function to learn data
		/// </summary>
		/// <param name="inputs">
		///     input data for training
		/// </param>
		/// <param name="expectedOutputs">
		///     th corresponding output data
		/// </param>
		public virtual void Learn(float[][] inputs, float[][] expectedOutputs)
		{
			if (expectedOutputs.Length < 1)
			{
				throw new Exception("LearningAlgorithm -> missing OutputTrainingData");
			}
			if (inputs.Length < 1)
			{
				throw new Exception("LearningAlgorithm -> missing InputTrainingData");
			}
			if (inputs.Length != expectedOutputs.Length)
			{
				throw new Exception("LearningAlgorithme -> length of input and output doesn't match ");
			}

			this.InputTrainingData = inputs;
			this.OutputTrainingData = expectedOutputs;
		}

		/// <summary>
		///     custom check for convergence
		/// </summary>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		protected bool Convergence()
		{
			// custom check, if the learning phase should be stopped
			return (this.Monitor != null) && this.Monitor.Convergence();
		}

		/// <summary>
		///     pulse in every epoch of stochastic gradient descent
		/// </summary>
		protected void Pulse()
		{
			// send current progress to learn monitor
			if (this.Monitor != null)
			{
				this.Monitor.Pulse(this.CurrentEpoch, this.MeanSquareError);
			}
		}
	}
}