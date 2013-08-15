using System;

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// The gaussian activation function
    /// </summary>
    /// <remarks>
    /// using normal probability density function
    /// </remarks>
    [Serializable]
    public class GaussianActivationFunction : iActivationFunction
    {
        /// <summary>
        /// intern standard devariance
        /// </summary>
        protected float _sigma = 0.159155f;
        /// <summary>
        /// intern values for faster computation
        /// </summary>
        protected float _double_sigma_square_reziproke = 19.739194686118779769055784534436f;
        /// <summary>
        /// intern means parameter
        /// </summary>
        protected float _mu = 0f;
        /// <summary>
        /// intern factor = 1/(2*pi*_sigma)
        /// </summary>
        protected float _normalisation = 0.99999982121796440166828584582973f;
        /// <summary>
        /// get or set standard devariance
        /// </summary>
        public float Sigma
        {
            get { return _sigma; }
            set
            {
                _sigma = (value > 0) ? value : _sigma;
                _double_sigma_square_reziproke = 1 / (2 * value * value);
                _normalisation = 1 / (2 * value * value);
            }
        }
        /// <summary>
        /// set or get mean
        /// </summary>
        public float Mu
        {
            get { return _mu; }
            set { _mu = value; }
        }
        /// <summary>
        /// get the value of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of function in x</returns>
        public virtual float Function(float x)
        {
            return _normalisation * (float)Math.Exp(-(x - _mu) * (x - _mu) * _double_sigma_square_reziproke);
        }
        /// <summary>
        /// get the value of the derivative of the function in x
        /// </summary>
        /// <param name="x">parameter x</param>
        /// <returns>value of derivative in x</returns>
        public virtual float Derivative(float x)
        {
            float y = Function(x);
            return -2 * y * _double_sigma_square_reziproke * (x - _mu);
        }
    }
}
