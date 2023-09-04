using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
     public class Cell : System.Windows.Forms.Button
     { 
        int X, Y;
        private Color color;
        public enum States {OBSTACLE = -1, FREE = 0, END = 1, Start = 2 };
        public States state;
        
        public Cell()
        {
            color = Color.Azure;
            state = States.FREE;
            this.Click += onClick;
        }

        public void onClick(object sender, EventArgs e) {
            if (state == States.FREE) 
                setObstacle();
            else if (state == States.OBSTACLE) {
                state = States.FREE;
                this.BackColor = Color.Transparent;
            }
            else {

            }

        }

        public void setColor(Color c) {
            color = c;
        }

        public void setObstacle() {
            state = States.OBSTACLE;
            this.BackColor = Color.Black;
        }

        public void setEnd() {
            state = States.END;
            this.BackColor = Color.Red;
        }

        public void setStart() {
            state = States.Start;
            this.BackColor = Color.Green;
        }

        public void setFree() {
            state = States.FREE;
            this.BackColor = Color.Transparent;
        }

    }
}
