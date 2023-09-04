using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
    internal interface IGenAlgorithm
    {
        Hero[] generatePopulation(int populationSize);

        Hero[] selection(Hero[] population);

        void mutation(Hero[] population);


    }
}
