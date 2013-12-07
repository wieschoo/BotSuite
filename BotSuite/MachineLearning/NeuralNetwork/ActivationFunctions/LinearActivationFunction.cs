// -----------------------------------------------------------------------
//  <copyright file="LinearActivationFunction.cs" company="HoovesWare">
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
	///     The linear activation function
	/// </summary>
	/// <remarks>
	///     piecewise linear: in  ]-0.5/k,0.5/k[ is A * x + 0.5 otherwise 1 or zero
	/// </remarks>
	[Serializable]
	public class LinearActivationFunction : IActivationFunction
	{
		/// <summary>
		///     private variable for intervall
		/// </summary>
		protected float Mk = 1.0f;

		/// <summary>
		///     hinge point
		/// </summary>
		protected float Hinge = 0.5f;

		/// <summary>
		///     set or get parameter K to manipulate the piecewise linear function
		/// </summary>
		public float K
		{
			get
			{
				return this.Mk;
			}
			set
			{
				this.Mk = (value > 0) ? value : 1.0f;
				this.Hinge = 0.5f / this.Mk;
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
			if (x > this.Hinge)
			{
				return 1;
			}
			if (x < -this.Hinge)
			{
				return 0;
			}
			return this.Mk * x + 0.5f;
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
			if (x > this.Hinge)
			{
				return 0;
			}
			if (x < -this.Hinge)
			{
				return 0;
			}
			return this.Mk;
		}
	}
}