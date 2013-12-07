// -----------------------------------------------------------------------
//  <copyright file="TanhActivationFunction.cs" company="HoovesWare">
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
	///     The tanh activation function
	/// </summary>
	/// <remarks>
	///     f(x)  = tanh(x)
	///     f'(x) = 1-f(x)^2
	/// </remarks>
	[Serializable]
	public class TanhActivationFunction
	{
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
			return (float)Math.Tanh(x);
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
			return 1.0f - y * y;
		}
	}
}