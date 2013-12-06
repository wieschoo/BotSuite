// -----------------------------------------------------------------------
//  <copyright file="BackPropagationLearningAlgorithm.cs" company="HoovesWare">
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

	/// <summary>
	///     stockastic gradient backpropagation learning algorithm
	/// </summary>
	/// <remarks>
	///     Neuron j in L-1
	///     o ---------> Signal f(W*S_j)
	///     Neuron i in L
	///     o ---------> Signal f(W*S_i)
	///     Neuron k in L+1
	///     for i
	///     t_i(n+1)     = t_i(n)     - \alpha*EO_i*\gamma     * ( T_i(n)   - T_i(n-1))
	///     w_{i,j}(n+1) = w_{i,j}(n) + \alpha*S_j*EO_i+\gamma * (w_[i,j}(n)-w_{i,j}(n-1))
	///     with :
	///     EO_i = f'(WSi) * (\IE Ouput of i - S_i)        for output layer
	///     EO_i = f'(WSi) * \sum_k  w_{k,i} EO_k  )       for others
	/// </remarks>
	[Serializable]
	public class BackPropagationLearningAlgorithm : LearningAlgorithm
	{
		/// <summary>
		///     parameters of learning algorithm
		/// </summary>
		protected float Alpha;

		/// <summary>
		///     parameters of learning algorithm
		/// </summary>
		protected float Gamma;

		/// <summary>
		///     errors in iteration
		/// </summary>
		protected float[] ErrorVector;

		/// <summary>
		///     set or get learning rate (high values fast progress but no convergence, low values -> slow learning)
		/// </summary>
		public float LearningRate
		{
			get
			{
				return this.Alpha;
			}
			set
			{
				this.Alpha = (value <= 0.0f) ? this.Alpha : value;
			}
		}

		/// <summary>
		///     set or get the rumelhart coefficient
		/// </summary>
		public float RumelhartCoefficient
		{
			get
			{
				return this.Gamma;
			}
			set
			{
				this.Gamma = (value > 0) ? value : this.Gamma;
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="BackPropagationLearningAlgorithm" /> class.
		///     default BackPropagation (alpha = 0.5 ; gamma = 0.2)
		/// </summary>
		/// <param name="ann">
		///     network to train
		/// </param>
		public BackPropagationLearningAlgorithm(NeuralNetwork ann)
			: base(ann)
		{
			this.LearningRate = 0.5f;
			this.RumelhartCoefficient = 0.2f;
		}

		/// <summary>
		///     backpropagation learning algorithm
		/// </summary>
		/// <param name="inputs">
		///     the input
		/// </param>
		/// <param name="expectedOutputs">
		///     the expected output
		/// </param>
		public override void Learn(float[][] inputs, float[][] expectedOutputs)
		{
			base.Learn(inputs, expectedOutputs);
			this.CurrentEpoch = 0;

			// for each epoch
			do
			{
				// currently no error
				this.MeanSquareError = 0f;
				this.ErrorVector = new float[this.ANN.NumberOfOutputs];

				// run through set of training data
				for (int i = 0; i < this.InputTrainingData.Length; i++)
				{
					float meanSquareError2 = 0f;
					float[] neuronOutput = this.ANN.Output(inputs[i]);
					for (int j = 0; j < neuronOutput.Length; j++)
					{
						this.ErrorVector[j] = this.OutputTrainingData[i][j] - neuronOutput[j];

						// update current error
						meanSquareError2 += this.ErrorVector[j] * this.ErrorVector[j];
					}

					meanSquareError2 /= 2f;
					this.MeanSquareError += meanSquareError2;
					this.CalculateUsefulVariable(i);

					// update weight
					this.SetWeight(i);
				}

				this.CurrentEpoch++;

				// we aren't dead
				this.Pulse();

				// custom check if we have to run further
				if (this.Convergence())
				{
					break;
				}
			}
			while (this.CurrentEpoch < this.MMaximumOfEpochs && this.MeanSquareError > this.MErrorThreshhold);
		}

		/// <summary>
		///     to speed up the process of weight updating we use this variable
		/// </summary>
		/// <param name="i">
		/// </param>
		protected void CalculateUsefulVariable(int i)
		{
			int l = this.ANN.NumberOfLayers - 1;

			// last layer
			for (int j = 0; j < this.ANN[l].NeuronsInLayer; j++)
			{
				this.ANN[l][j].UsefulVariable = this.ANN[l][j].Derivative * this.ErrorVector[j];
			}

			// other layers before
			for (l--; l >= 0; l--)
			{
				for (int j = 0; j < this.ANN[l].NeuronsInLayer; j++)
				{
					float sK = 0f;
					for (int k = 0; k < this.ANN[l + 1].NeuronsInLayer; k++)
					{
						sK += this.ANN[l + 1][k].UsefulVariable * this.ANN[l + 1][k][j];
					}

					this.ANN[l][j].UsefulVariable = this.ANN[l][j].Derivative * sK;
				}
			}
		}

		/// <summary>
		///     update the weight (see delta rule and chain rule for derivatives) and update affecting neurons
		/// </summary>
		/// <param name="newWeight">
		///     new weight to set
		/// </param>
		protected void SetWeight(int newWeight)
		{
			for (int j = 0; j < this.ANN.NumberOfLayers; j++)
			{
				float[] lin = j == 0 ? this.InputTrainingData[newWeight] : this.ANN[j - 1].GetLastOuput;
				for (int n = 0; n < this.ANN[j].NeuronsInLayer; n++)
				{
					for (int k = 0; k < this.ANN[j][n].NumberOfNeurons; k++)
					{
						this.ANN[j][n][k] += this.Alpha * lin[k] * this.ANN[j][n].UsefulVariable
											+ this.Gamma * (this.ANN[j][n][k] - this.ANN[j][n].LastWeight[k]);
					}
					this.ANN[j][n].Threshold -= this.Alpha * this.ANN[j][n].UsefulVariable
												+ this.Gamma * (this.ANN[j][n].Threshold - this.ANN[j][n].LastThreshold);
				}
			}
		}
	}
}