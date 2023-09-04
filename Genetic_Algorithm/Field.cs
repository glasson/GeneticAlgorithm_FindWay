using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
    internal class Field
    {
        public int[,] field;
        public Cell[,] gui_field;
        int size;

        public Field(int size) {
            this.size = (int)Math.Sqrt(size);
            field = new int[this.size, this.size];
            for (int i = 0; i < this.size; i++) {
                for (int j = 0; j < this.size; j++) {
                    field[i, j] = 0;
                }
            }
        }

        public int[,] get_matrix_field(int n) {
            return field;
        }

      


        public Cell[,] get_GUI_field(int n) {
            var field = new Cell[n, n];
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    field[i, j] = new Cell();
                }
            }
            return field;
        }

        public void setRandomObstacle() {
            var r = new Random();
            for (int i = 0; i<size; i++) {
                for (int j = 0;j < size; j++) {
                    if (r.Next(1,11) < 5) 
                        field[i, j] = (short) r.Next(-1, 1);
                }
            }
        }

        public void setEndpoint(int[] endpoint) { // координата
            if (!(endpoint[0] < size) && !(endpoint[1] < size) && !(endpoint[0] > size) && !(endpoint[1] > size)) {
                field[endpoint[0], endpoint[1]] = 1;
            }
            else  
                field[size-1,size-1] = 1; 
            
        }

        public void setObstacle(int[][] obstacle) { // массив координат
            foreach(int[] i in obstacle) {
                field[i[0], i[1]] = -1;
            }
        }

    }
}
