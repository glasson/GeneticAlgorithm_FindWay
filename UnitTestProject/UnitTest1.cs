using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Genetic_Algorithm;

namespace UnitTestProject {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestHeroMove() {
            int[,] field = { {0,0,0}, 
                             {0,0,0},
                             {0,0,0} };
            int[] gen = { 2, 2, 3, 3, 0, 0 };
            int[] expected_last_coordinate = { 2, 2 };
            var h = new Hero(gen);
            h.Move(field, 0, 0);
            Assert.AreEqual(expected_last_coordinate[0], h.lastCoordinate[0]);
            Assert.AreEqual(expected_last_coordinate[1], h.lastCoordinate[1]);
        }

        [TestMethod]
        public void TestMutation() {
            int[] gen = { 4, 4, 3, 3, 0, 0 };
            var h = new Hero((int[])gen.Clone());
            var g = new GeneticClass(0);
            Hero[] population = {h};
            g.mutation(population);
            CollectionAssert.AreNotEqual(gen, population[0].getGen());
        }

        [TestMethod]
        public void TestFitFunction() {
            int[] gen1 = { 2, 2, 3, 3, 0, 0 };
            int[] gen2 = { 0, 0, 0, 0, 0, 0 };
            var h1 = new Hero(gen1);
            var h2 = new Hero(gen2);
            var g = new GeneticClass(0);
            Hero[] population = new Hero[2] { h1,h2 };
            int[,] field = { {0,0,0},
                             {0,0,0},
                             {0,0,0} };
            g.endpoint = new int[2] { 2, 2 };
            population[0].Move(field, 0, 0);
            population[1].Move(field, 0, 0);
            var result = g.Fitfunction(population);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(4, result[1]);
        }

        [TestMethod]
        public void TestCrossingover() {
            GeneticClass g = new GeneticClass(0);
            int[] gen1 = { 2, 2, 3, 3, 0, 0 };
            int[] gen2 = { 0, 0, 0, 0, 0, 0 };
            int[] exp_gen1= { 2, 2, 3, 0, 0, 0 };
            int[] exp_gen2= { 0, 0, 0, 3, 0, 0 }; 
            Hero h1 = new Hero(gen1);
            var h2 = new Hero(gen2);
            Hero[] pop = { h1, h2 };
            Hero[] newpop = g.crossingover(pop);
            CollectionAssert.AreEqual (exp_gen1, newpop[0].getGen());
            CollectionAssert.AreEqual(exp_gen2, newpop[1].getGen());


        }
        [TestMethod]
        public void TestGeneratePopulation() {
            GeneticClass g = new GeneticClass(0);
            Hero[] population = g.generatePopulation(5);
            CollectionAssert.AllItemsAreInstancesOfType(population, typeof(Hero));
            Assert.AreEqual(5, population.Length);
        }

    }



}
