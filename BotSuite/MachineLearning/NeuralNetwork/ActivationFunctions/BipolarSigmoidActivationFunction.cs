using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// The bipolar sigmoid activation function
    /// </summary>
    /// <remarks>
    /// f(x)  = -1+ 2/(1+exp(-beta*x)
    /// f'(x) = beta/2*(1-f(x)*f(x))   
    /// </remarks>
    [Serializable]
    public class BipolarSigmoidActivationFunction
    {
        /// <summary>
        /// inter parameter
        /// </summary>
        protected float _beta = 1.0f;
        /// <summary>
        /// get or set parameter beta of function
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
            return ((2.0f / (float)(1.0f + Math.Exp(-_beta * x))) - 1);
        }
        /// <summary>
        /// get the value of the derivative of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of derivative in x</returns>
        public virtual float Derivative(float x)
        {
            float y = Function(x);
            return (_beta * (1 - y * y) / 2.0f);
        }
    }
}
