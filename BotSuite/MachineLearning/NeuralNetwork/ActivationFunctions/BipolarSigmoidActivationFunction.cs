// -----------------------------------------------------------------------
//  <copyright file="BipolarSigmoidActivationFunction.cs" company="HoovesWare">
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
	///     The bipolar sigmoid activation function
	/// </summary>
	/// <remarks>
	///     f(x)  = -1+ 2/(1+exp(-beta*x)
	///     f'(x) = beta/2*(1-f(x)*f(x))
	/// </remarks>
	[Serializable]
	public class BipolarSigmoidActivationFunction
	{
		/// <summary>
		///     inter parameter
		/// </summary>
		protected float MBeta = 1.0f;

		/// <summary>
		///     get or set parameter beta of function
		/// </summary>
		public float Beta
		{
			get
			{
				return this.MBeta;
			}
			set
			{
				this.MBeta = (value > 0) ? value : 1.0f;
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
			return (2.0f / (float)(1.0f + Math.Exp(-this.MBeta * x))) - 1;
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
			return this.MBeta * (1 - y * y) / 2.0f;
		}
	}
}