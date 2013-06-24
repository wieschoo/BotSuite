using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    [Serializable]
    /// <summary>
    /// The tanh activation function
    /// </summary>
    /// <remarks>
    /// f(x)  = tanh(x)
    /// f'(x) = 1-f(x)^2
    /// </remarks>
    public class TanhActivationFunction
    {
        public virtual float Function(float x)
        {
            return (float) Math.Tanh(x);
        }
        public virtual float Derivative(float x)
        {
            float y = Function(x);
            return (1.0f - y * y);
        }
    }
}
