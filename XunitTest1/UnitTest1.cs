using Xunit;
using Genetic_Algorithm;

namespace XunitTest1 {
    public class UnitTest1 {
        [Fact]
        public void TestHeroMove() {
                int[,] field = { {0,0,0},
                             {0,0,0},
                             {0,0,0} };
                int[] gen = { 2, 2, 3, 3, 0, 0 };
                int[] expected_last_coordinate = { 2, 2 };
                var h = new Hero(gen);
                h.Move(field, 0, 0);
                Assert.Equal(expected_last_coordinate[0], h.lastCoordinate[0]);
                Assert.Equal(expected_last_coordinate[1], h.lastCoordinate[1]);
            }

            [Fact]
            public void TestMutation() {
                int[] gen = { 4, 4, 3, 3, 0, 0 };
                var h = new Hero((int[])gen.Clone());
                var g = new GeneticClass(0);
                Hero[] population = { h };
                g.mutation(population);
                Assert.NotEqual(gen, population[0].getGen());
            }

        [Fact]
        public void TestFitFunction() {
                int[] gen1 = { 2, 2, 3, 3, 0, 0 };
                int[] gen2 = { 0, 0, 0, 0, 0, 0 };
                var h1 = new Hero(gen1);
                var h2 = new Hero(gen2);
                var g = new GeneticClass(0);
                Hero[] population = new Hero[2] { h1, h2 };
                int[,] field = { {0,0,0},
                             {0,0,0},
                             {0,0,0} };
                g.endpoint = new int[2] { 2, 2 };
                population[0].Move(field, 0, 0);
                population[1].Move(field, 0, 0);
                var result = g.Fitfunction(population);
                Assert.Equal(0, result[0]);
                Assert.Equal(4, result[1]);
            }

        [Fact]
        public void TestCrossingover() {
                GeneticClass g = new GeneticClass(0);
                int[] gen1 = { 2, 2, 3, 3, 0, 0 };
                int[] gen2 = { 0, 0, 0, 0, 0, 0 };
                int[] exp_gen1 = { 2, 2, 3, 0, 0, 0 };
                int[] exp_gen2 = { 0, 0, 0, 3, 0, 0 };
                Hero h1 = new Hero(gen1);
                var h2 = new Hero(gen2);
                Hero[] pop = { h1, h2 };
                Hero[] newpop = g.crossingover(pop);
                Assert.Equal(exp_gen1, newpop[0].getGen());
                Assert.Equal(exp_gen2, newpop[1].getGen());


            }
        [Fact]
        public void TestGeneratePopulation() {
                GeneticClass g = new GeneticClass(0);
                Hero[] population = g.generatePopulation(5);
                Assert.All(population, p => Assert.IsType<Hero>(p));
                Assert.Equal(5, population.Length);
            }

       
    } 
}