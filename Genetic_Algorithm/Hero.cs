using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_Algorithm {
    public class Hero {
        private int[] gen;
        public int[] lastCoordinate = new int[2];
        public int fitness; // приспособленность, расстояние до клетки

        public Hero(int[] gen) {
            this.gen = gen;
        }
        public int[] getGen() {
            return gen;
        }

        public void setGen(int[] gen) {
            this.gen = gen;
        }

        public void setFitness(int fitness) {
            this.fitness = fitness;
        }

        public void Move(int[,] field, int x, int y) { 
            int fs = (int)Math.Sqrt(field.Length) - 1;
            for (int i = 0; i < gen.Length; i++) {
                switch (gen[i]) {
                    case 0:
                        break;
                    case 1:
                        x--;
                        break;
                    case 2:
                        y++;
                        break;
                    case 3:
                        x++;
                        break;
                    case 4:
                        y--;
                        break;
                }
                if ((x < 0 || x > fs || y < 0 || y > fs) || (field[x, y] == -1)) {// если выйдет за пределы поля, сделать им минимальный ЛК чтоб не размножались
                    lastCoordinate[0] = 0;
                    lastCoordinate[1] = 0;
                    return;
                }
                lastCoordinate[0] = x;
                lastCoordinate[1] = y;
            }

        }

    }
}
