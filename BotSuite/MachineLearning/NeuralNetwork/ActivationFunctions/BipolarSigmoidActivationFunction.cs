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
        protected float _beta = 1.0f;
        public float Beta
        {
            get { return _beta; }
            set { _beta = (value > 0) ? value : 1.0f; }
        }
        public virtual float Function(float x)
        {
            return ((2.0f / (float)(1.0f + Math.Exp(-_beta * x))) - 1);
        }
        public virtual float Derivative(float x)
        {
            float y = Function(x);
            return (_beta * (1 - y * y) / 2.0f);
        }
    }
}
