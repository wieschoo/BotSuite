using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// The sigmoid activation function
    /// </summary>
    /// <remarks>
    /// f(x)  = (1+exp(-beta*x))^-1
    /// f'(x) = beta * f(x) * ( 1 - f(x) )      
    /// </remarks>
    [Serializable]
    public class SigmoidActivationFunction : iActivationFunction
    {
        /// <summary>
        /// parameter of sigmoid function
        /// </summary>
        protected float _beta = 1.0f;
        /// <summary>
        /// Set or Get the parameter of the sigmoid function
        /// </summary>
        public float Beta
        {
            get { return _beta; }
            set { _beta = (value > 0) ? value : 1.0f; }
        }
        /// <summary>
        /// get the value of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of function in x</returns>
        public virtual float Function(float x)
        {
            return (float)(1 / (1 + Math.Exp(-_beta * x)));
        }
        /// <summary>
        /// get the value of the derivative of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of derivative in x</returns>
        public virtual float Derivative(float x)
        {
            float y = Function(x);
            return (_beta * y * (1 - y));
        }
    }
}
