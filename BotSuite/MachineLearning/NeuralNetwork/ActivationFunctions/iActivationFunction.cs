using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.MachineLearning.NeuralNetwork.ActivationFunctions
{
    public interface iActivationFunction
    {
        float Function(float x);
        float Derivative(float x);
    }
}
