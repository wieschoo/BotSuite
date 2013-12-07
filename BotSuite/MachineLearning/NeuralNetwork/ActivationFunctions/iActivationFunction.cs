// -----------------------------------------------------------------------
//  <copyright file="iActivationFunction.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
	/// <summary>
	///     interface for activations functions
	/// </summary>
	public interface IActivationFunction
	{
		/// <summary>
		///     calculates the value of the activation function in x
		/// </summary>
		/// <param name="x">
		///     parameter x
		/// </param>
		/// <returns>
		///     value of function
		/// </returns>
		float Function(float x);

		/// <summary>
		///     calculates the value of the derivative of the activation function in x
		/// </summary>
		/// <param name="x">
		///     parameter x
		/// </param>
		/// <returns>
		///     value of derivative
		/// </returns>
		float Derivative(float x);
	}
}