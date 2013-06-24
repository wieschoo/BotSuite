using System;

namespace MachineLearning.NeuralNetwork.ActivationFunctions
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
        protected float _sigma = 0.159155f;
        protected float _double_sigma_square_reziproke = 19.739194686118779769055784534436f;
        protected float _mu = 0f;
        protected float _normalisation = 0.99999982121796440166828584582973f;

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
        public float Mu
        {
            get { return _mu; }
            set { _mu = value; }
        }
        public GaussianActivationFunction()
        {
        }
        public virtual float Function(float x)
        {
            return _normalisation * (float)Math.Exp(-(x - _mu) * (x - _mu) * _double_sigma_square_reziproke);
        }
        public virtual float Derivative(float x)
        {
            float y = Function(x);
            return -2 * y * _double_sigma_square_reziproke * (x - _mu);
        }
    }
}
