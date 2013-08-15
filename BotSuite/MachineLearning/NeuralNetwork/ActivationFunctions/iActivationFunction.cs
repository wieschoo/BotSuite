using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// interface for activations functions
    /// </summary>
    public interface iActivationFunction
    {
        /// <summary>
        /// calculates the value of the activation function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of function</returns>
        float Function(float x);
        /// <summary>
        /// calculates the value of the derivative of the activation function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of derivative</returns>
        float Derivative(float x);
    }
}
