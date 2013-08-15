using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// The linear activation function
    /// </summary>
    /// <remarks>
    /// piecewise linear: in  ]-0.5/k,0.5/k[ is A * x + 0.5 otherwise 1 or zero  
    /// </remarks>
    [Serializable]
    public class LinearActivationFunction : iActivationFunction
    {
        /// <summary>
        /// private variable for intervall
        /// </summary>
        protected float _k = 1.0f;
        /// <summary>
        /// hinge point
        /// </summary>
        protected float _hinge = 0.5f;

        /// <summary>
        /// set or get parameter K to manipulate the piecewise linear function
        /// </summary>
        public float K
        {
            get { return _k; }
            set
            {
                _k = (value > 0) ? value : 1.0f;
                _hinge = 0.5f / _k;
            }
        }
        /// <summary>
        /// get the value of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of function in x</returns>
        public virtual float Function(float x)
        {
            if (x > _hinge) return 1;
            else if (x < -_hinge) return 0;
            else return _k * x + 0.5f;
        }
        /// <summary>
        /// get the value of the derivative of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of derivative in x</returns>
        public virtual float Derivative(float x)
        {
            if (x > _hinge) return 0;
            else if (x < -_hinge) return 0;
            else return _k;
        }
    }
}
