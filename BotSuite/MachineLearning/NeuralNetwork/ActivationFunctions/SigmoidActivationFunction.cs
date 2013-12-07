// -----------------------------------------------------------------------
//  <copyright file="SigmoidActivationFunction.cs" company="HoovesWare">
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
	///     The sigmoid activation function
	/// </summary>
	/// <remarks>
	///     f(x)  = (1+exp(-beta*x))^-1
	///     f'(x) = beta * f(x) * ( 1 - f(x) )
	/// </remarks>
	[Serializable]
	public class SigmoidActivationFunction : IActivationFunction
	{
		/// <summary>
		///     parameter of sigmoid function
		/// </summary>
		protected float MBeta = 1.0f;

		/// <summary>
		///     Set or Get the parameter of the sigmoid function
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
			return (float)(1 / (1 + Math.Exp(-this.MBeta * x)));
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
			return this.MBeta * y * (1 - y);
		}
	}
}