using GuessTheAnimal.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheAnimal
{
    class Program
    {
        private static readonly DataHelper _dataHelper = new DataHelper();
        private static string _dataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data.xml");

        static void Main(string[] args)
        {
            var dt = _dataHelper.GetData(_dataPath);
            var animals = _dataHelper.GetAnimals(dt);

            Console.WriteLine("Please choose from one of the following animals:");

            foreach (var a in animals)
            {
                Console.WriteLine(a.Name);
            }

            Console.WriteLine("Remember the animal but shh don't tell me!");
            Console.WriteLine("Press any key when you have selected an animal.");
            Console.ReadKey(true);

            GetAnswer(animals);

            AddAnimals();

            Console.WriteLine("Exiting, press any key...");
            Console.ReadKey();
        }

        private static string AskQuestions(List<Animal> animals)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Please answer the following with either Yes (Y) or No (N)");

            foreach (var a in animals)
            {
                var questions = a.Questions.Split('|');
                var correctAnswers = 0;

                foreach (var q in questions)
                {
                    Console.WriteLine(q);
                    var input = Console.ReadKey(true).KeyChar.ToString();

                    input = ValidateInput(input);

                    if (input.ToLower() == "y")
                    {
                        correctAnswers++;

                        if (correctAnswers == questions.Length)
                        {
                            return a.Name;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return string.Empty;
        }

        private static void GetAnswer(List<Animal> animals)
        {
            var answer = string.Empty;

            try
            {
                answer = AskQuestions(animals);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("press any key...");
                Console.ReadKey();

                return;
            }

            if (!string.IsNullOrWhiteSpace(answer))
            {
                Console.WriteLine($"The animal your thinking of is the {answer}");
            }
            else
            {
                Console.WriteLine("Unable to determine the animal your thinking of... You win!");
            }
        }

        private static void AddAnimals()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Would you like to add animals to the game?");
            var input = Console.ReadKey(true).KeyChar.ToString();

            input = ValidateInput(input);

            if (input == "y")
            {
                var questions = new List<string>();
                Console.WriteLine("Please enter an animal name and press enter");
                var name = Console.ReadLine();

                var anotherQuestion = string.Empty;

                do
                {
                    Console.WriteLine("Please enter a question for the animal and press enter");
                    questions.Add($"{Console.ReadLine()}");

                    Console.WriteLine("Would you like to add another question?");
                    anotherQuestion = ValidateInput(Console.ReadKey(true).KeyChar.ToString());
                } while (anotherQuestion == "y");

                var questionsToAdd = string.Join("|", questions);

                _dataHelper.AddAnimal(name, questionsToAdd, _dataPath);

                Console.WriteLine("Thank you for adding animals!");
            }
        }

        private static string ValidateInput(string input)
        {
            var answerCount = 0;

            while (answerCount <= 3)
            {
                if (input.ToLower() == "y")
                {
                    return input;
                }
                else if (input.ToLower() == "n")
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Please enter Yes (Y) or No (N)");
                    answerCount++;
                }

                if (answerCount == 3)
                {
                    throw new Exception("To many incorrect inputs, exiting...");
                }

                input = Console.ReadKey(true).KeyChar.ToString();
            }

            return string.Empty; // should never get here..
        }
    }
}
