using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// The linear activation function
    /// </summary>
    /// <remarks>
    /// piecewise linear: in  ]-0.5/k,0.5/k[ is A * x + 0.5 otherwise 1 or zero  
    /// </code>
    /// </remarks>
    [Serializable]
    public class LinearActivationFunction : iActivationFunction
    {
        protected float _k = 1.0f;
        protected float _hinge = 0.5f;

        public float K
        {
            get { return _k; }
            set
            {
                _k = (value > 0) ? value : 1.0f;
                _hinge = 0.5f / _k;
            }
        }
        public virtual float Function(float x)
        {
            if (x > _hinge) return 1;
            else if (x < -_hinge) return 0;
            else return _k * x + 0.5f;
        }
        public virtual float Derivative(float x)
        {
            if (x > _hinge) return 0;
            else if (x < -_hinge) return 0;
            else return _k;
        }
    }
}
