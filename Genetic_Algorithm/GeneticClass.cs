using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Genetic_Algorithm
{
    public class GeneticClass : IGenAlgorithm
    {
        int population_size = 16; // необходимо не менее 6 для нормальной работы
        private int gen_size;
        private Hero[] population;
        private Random randomizer = new Random();
        int mutationChance = 1;
        public int[] endpoint;
        int endCondition = 0;


        public GeneticClass(int field_size, int[] endpoint, int populationSize) {
            this.gen_size = field_size * 2;
            this.population_size = populationSize;
            this.endpoint = new int[2] {endpoint[0], endpoint[1]};
        }

        public GeneticClass(int field_size) {
            gen_size = field_size * 2;
        }

        public GeneticClass(int gen_size, int population_size, int[] endpoint) {
            this.population_size = population_size;
            this.gen_size = gen_size;
            this.endpoint = endpoint;
        }

        public Hero[] generatePopulation(int populationSize) {
            Hero[] heroes = new Hero[populationSize];
            for (int i = 0; i < populationSize; i++) {
                heroes[i] = new Hero(this.generateGen());
            }
            this.population = heroes;
            return heroes;
        }

        public void mutation(Hero[] population) {
            for (int i = 0;i < population.Length;i++) {
                var index = randomizer.Next(gen_size);
                var gen = population[i].getGen();
                gen[index] = randomizer.Next(0, 5);
                population[i].setGen(gen);
            }
                
            
        }

        public Hero[] selection(Hero[] population) {
            Hero[] new_population = new Hero[population.Length];
            for (int i = 0; i<population_size; i++) {
                int i1=-1, i2=-1, i3=-1;
                while (i1==i2 || i2 == i3 || i1 == i3) { 
                    i1 = randomizer.Next(0,population_size);
                    i2 = randomizer.Next(0,population_size);
                    i3 = randomizer.Next(0,population_size);
                }
                Hero obj1 = population[i1];
                Hero obj2 = population[i2];
                Hero obj3 = population[i3];
                Hero bestObj = obj1;
                if (obj2.fitness < bestObj.fitness) {
                    bestObj = obj2;
                }
                if (obj3.fitness < bestObj.fitness) {
                    bestObj = obj3;
                }
                new_population[i] = bestObj;
            }
            return new_population;
        }

        public int[] Fitfunction(Hero[] population) {// вернет массив, в котором значение соответствует значению приспособленности каждого героя
            var distances = new int[population.Length];
            for (int i = 0;i < population.Length;i++) {
                distances[i] = Math.Abs(endpoint[0] - population[i].lastCoordinate[0]) + Math.Abs(endpoint[1] - population[i].lastCoordinate[1]);
            }
            return distances;
        }

        public int Fitfunction(Hero hero) {// вернет массив, в котором значение соответствует значению приспособленности каждого героя
            return Math.Abs(endpoint[0] - hero.lastCoordinate[0]) + Math.Abs(endpoint[1] - hero.lastCoordinate[1]);
        }

        public int[] generateGen() {
            int[] gen = new int[gen_size];   
            for (int i=0; i<gen_size; i++)
                gen[i] = randomizer.Next(0, 5);// если 0 то клетка не движется
            return gen;
        }

        public Hero[] crossingover(Hero[] population) {
            var newpopulation = new Hero[population.Length];
            if (population.Length % 2 == 1) { throw new Exception("не может быть 1 родитель, размер массива нечетный"); }
            int gen_size = population[0].getGen().Length;
            var new_gen1 = new int[gen_size];
            var new_gen2 = new int[gen_size];
            var COpoint = gen_size / 2;//randomizer.Next(2, gen_size - 2)
            // цикл по особям, 1 итерация - 2 особи
            for (int i = 0; i<population.Length; i+=2) {
                // цикл по генам
                for (int j = 0; j < COpoint; j++) {
                    new_gen1[j] = population[i].getGen()[j];// заполнение гена первого потомка от первого родителя до точки скрещивания
                    new_gen2[j] = population[i+1].getGen()[j];// заполнение гена второго потомка от второго родителя до точки скрещивания
                }
                for (int j = COpoint; j < gen_size; j++) {
                    new_gen1[j] = population[i+1].getGen()[j];// заполнение гена первого потомка от второго родителя от точки скрещивания
                    new_gen2[j] = population[i].getGen()[j];// заполнение гена второго потомка от первого родителя от точки скрещивания
                }
                newpopulation[i] = new Hero(new_gen1);
                newpopulation[i+1] = new Hero(new_gen2);
            }
            
            
            return newpopulation;
        }
    }

    


}
