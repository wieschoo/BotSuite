using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// The tanh activation function
    /// </summary>
    /// <remarks>
    /// f(x)  = tanh(x)
    /// f'(x) = 1-f(x)^2
    /// </remarks>
    [Serializable]
    public class TanhActivationFunction
    {
        /// <summary>
        /// get the value of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of function in x</returns>
        public virtual float Function(float x)
        {
            return (float) Math.Tanh(x);
        }
        /// <summary>
        /// get the value of the derivative of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of derivative in x</returns>
        public virtual float Derivative(float x)
        {
            float y = Function(x);
            return (1.0f - y * y);
        }
    }
}
