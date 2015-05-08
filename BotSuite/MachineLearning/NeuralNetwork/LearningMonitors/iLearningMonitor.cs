using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.MachineLearning.NeuralNetwork.LearningMonitors
{
    /// <summary>
    /// to send the current progress in learning to another class you can use this monitor
    /// </summary>
    public interface iLearningMonitor
    {
        /// <summary>
        /// after every epoch this function can decide whether the learning process should be stopped or not
        /// </summary>
        /// <returns></returns>
        bool Convergence();
        /// <summary>
        /// after each epoch this function is be called be the learner
        /// </summary>
        /// <param name="CurrentNumberOfIterations">number of iterations in training session</param>
        /// <param name="CurrentMeanSquareError">the current mean square error</param>
        void Pulse(int CurrentNumberOfIterations, float CurrentMeanSquareError);
    }
}
