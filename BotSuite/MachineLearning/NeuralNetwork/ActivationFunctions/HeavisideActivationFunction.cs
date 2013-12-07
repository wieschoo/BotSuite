// -----------------------------------------------------------------------
//  <copyright file="HeavisideActivationFunction.cs" company="HoovesWare">
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
	///     The heaviside activation function
	/// </summary>
	/// <remarks>
	///     f(x) = 1_]0,infty[ (x)
	/// </remarks>
	[Serializable]
	public class HeavisideActivationFunction : IActivationFunction
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
			return (x > 0) ? 1.0f : 0.0f;
		}

		/// <summary>
		///     get the value of the derivative of the function in x (ok only something like that, becaus we cannot build the
		///     derivative of this funtion)
		/// </summary>
		/// <param name="x">
		///     parameter x
		/// </param>
		/// <returns>
		///     value of derivative in x
		/// </returns>
		public virtual float Derivative(float x)
		{
			return (Math.Abs(x) < 0.0001) ? float.MaxValue : 0;
		}
	}
}