// -----------------------------------------------------------------------
//  <copyright file="iLearningMonitor.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.MachineLearning.NeuralNetwork.LearningMonitors
{
	/// <summary>
	///     to send the current progress in learning to another class you can use this monitor
	/// </summary>
	public interface ILearningMonitor
	{
		/// <summary>
		///     after every epoch this function can decide whether the learning process should be stopped or not
		/// </summary>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		bool Convergence();

		/// <summary>
		///     after each epoch this function is be called be the learner
		/// </summary>
		/// <param name="currentNumberOfIterations">
		///     number of iterations in training session
		/// </param>
		/// <param name="currentMeanSquareError">
		///     the current mean square error
		/// </param>
		void Pulse(int currentNumberOfIterations, float currentMeanSquareError);
	}
}