// -----------------------------------------------------------------------
//  <copyright file="GaussianActivationFunction.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
	using System;

	/// <summary>
	///     The gaussian activation function
	/// </summary>
	/// <remarks>
	///     using normal probability density function
	/// </remarks>
	[Serializable]
	public class GaussianActivationFunction : IActivationFunction
	{
		/// <summary>
		///     intern standard devariance
		/// </summary>
		protected float MSigma = 0.159155f;

		/// <summary>
		///     intern values for faster computation
		/// </summary>
		protected float DoubleSigmaSquareReziproke = 19.739194686118779769055784534436f;

		/// <summary>
		///     intern means parameter
		/// </summary>
		protected float MMu = 0f;

		/// <summary>
		///     intern factor = 1/(2*pi*_sigma)
		/// </summary>
		protected float Normalisation = 0.99999982121796440166828584582973f;

		/// <summary>
		///     get or set standard devariance
		/// </summary>
		public float Sigma
		{
			get
			{
				return this.MSigma;
			}
			set
			{
				this.MSigma = (value > 0) ? value : this.MSigma;
				this.DoubleSigmaSquareReziproke = 1 / (2 * value * value);
				this.Normalisation = 1 / (2 * value * value);
			}
		}

		/// <summary>
		///     set or get mean
		/// </summary>
		public float Mu
		{
			get
			{
				return this.MMu;
			}
			set
			{
				this.MMu = value;
			}
		}

		/// <summary>
		///     get the value of the function in x
		/// </summary>
		/// <param name="x">
		///     parameter x
		/// </param>
		/// <returns>
		///     value of function in x
		/// </returns>
		public virtual float Function(float x)
		{
			return this.Normalisation * (float)Math.Exp(-(x - this.MMu) * (x - this.MMu) * this.DoubleSigmaSquareReziproke);
		}

		/// <summary>
		///     get the value of the derivative of the function in x
		/// </summary>
		/// <param name="x">
		///     parameter x
		/// </param>
		/// <returns>
		///     value of derivative in x
		/// </returns>
		public virtual float Derivative(float x)
		{
			float y = this.Function(x);
			return -2 * y * this.DoubleSigmaSquareReziproke * (x - this.MMu);
		}
	}
}