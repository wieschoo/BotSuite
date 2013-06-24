using System;

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// The heaviside activation function
    /// </summary>
    /// <remarks>
    /// f(x) = 1_]0,infty[ (x)
    /// </remarks>
    [Serializable]
    public class HeavisideActivationFunction : iActivationFunction
    {
        public virtual float Function(float x)
        {
            return (x > 0) ? 1.0f : 0.0f;
        }
        public virtual float Derivative(float x)
        {
            return (Math.Abs(x) < 0.0001) ? float.MaxValue : 0;
        }
    }
}
