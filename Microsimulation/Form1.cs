using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsimulation.Entities;
using System.IO;

namespace Microsimulation
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        List<int> MalePopulation = new List<int>();
        List<int> FemalePopulation = new List<int>();

        Random rng = new Random(1234);

        public Form1()
        {
            InitializeComponent();

            BirthProbabilities = GetBirthProbabilities(@"C:\temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\temp\halál.csv");

            //dataGridView1.DataSource = DeathProbabilities;
        }

        public void Simulation()
        {
            for (int year = 2005; year <= numericUpDown1.Value; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.isAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.isAlive
                                    select x).Count();

                MalePopulation.Add(nbrOfMales);
                FemalePopulation.Add(nbrOfFemales);

                Console.WriteLine(
                    string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }

            DisplayResults();
        }

        public void SimStep(int year, Person person)
        {
            if (!person.isAlive) return;

            int age = year - person.BirthYear;

            double deathProb = (from x in DeathProbabilities
                                where x.Gender == person.Gender && x.Age == age
                                select x.P).FirstOrDefault();
            if(rng.NextDouble() <= deathProb)
            {
                person.isAlive = false;
            }

            if(person.isAlive && person.Gender == Gender.Female)
            {
                double birthProb = (from x in BirthProbabilities
                                    where x.Age == age
                                    select x.P).FirstOrDefault();

                if(rng.NextDouble() <= birthProb)
                {
                    Person newborn = new Person()
                    {
                        BirthYear = year,
                        nbrOfChildren = 0,
                        Gender = (Gender)(rng.Next(1, 3))
                    };

                    Population.Add(newborn);

                }
            }
        }

        void DisplayResults()
        {
            for(int i = 0; i < MalePopulation.Count; i++)
            {
                richTextBox1.AppendText("Szimulációs év: " + (i + 2005) + "\n");
                richTextBox1.AppendText("\t Fiúk: " + MalePopulation[i] + "\n");
                richTextBox1.AppendText("\t Lányok: " + FemalePopulation[i] + "\n");
                richTextBox1.AppendText("\n");
            }
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using(StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while(!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        nbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> population = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        P = double.Parse(line[2]),
                        nbrOfChildren = int.Parse(line[1])
                    });
                }
            }

            return population;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> population = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new DeathProbability()
                    {
                        Age = int.Parse(line[1]),
                        P = double.Parse(line[2]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0])
                    });
                }
            }

            return population;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            MalePopulation.Clear();
            FemalePopulation.Clear();

            Population = GetPopulation(textBox1.Text);
            Simulation();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }
    }
}
