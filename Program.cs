class Quiz
{
    static void Main(string[] args)
    {
        bool repeatQuiz = true;
        int highestScore = 0;

        while (repeatQuiz)
        {
            Console.Clear();
            Console.WriteLine("Witaj w Quizie Informatycznym! Obecny najwyższy wynik: " + highestScore);
            Console.WriteLine("Wybierz daną opcję i naciśnij Enter");
            Console.WriteLine("1. Start Quiz");
            Console.WriteLine("2. Pokaz najwyzszy wynik");
            Console.WriteLine("3. Wychodze");
            Console.Write("Twój wybór: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    int score = StartQuiz();
                    if (score > highestScore)
                    {
                        highestScore = score;
                    }
                    break;
                case 2:
                    Console.WriteLine("Najwyzszy wynik: " + highestScore);
                    break;
                case 3:
                    repeatQuiz = false;
                    break;
                default:
                    Console.WriteLine("Niepoprawny wybor. Spróbuj ponownie");
                    break;
            }

            Console.WriteLine("Nacisnij jakikolwiek przycisk, by kontynuowac...");
            Console.ReadKey();
        }
    }

    static int StartQuiz()
    {
        List<Question> questions = LoadQuestionsFromFile("quiz.txt");
        int score = 0;
        int questionNumber = 1; // Dodaj zmienną licznikową

        foreach (Question question in questions)
        {
            Console.Clear();
            Console.WriteLine("Pytanie " + questionNumber + ": " + question.Text); // Wyświetl numer pytania
            Console.WriteLine("1. " + question.Answer1);
            Console.WriteLine("2. " + question.Answer2);
            Console.WriteLine("3. " + question.Answer3);
            Console.WriteLine("4. " + question.Answer4);
            Console.Write("Podaj swoją odpowiedź (1-4): ");
            int answer = Convert.ToInt32(Console.ReadLine());
            if (answer < 1 || answer > 4)
            {
                Console.WriteLine("Błąd. Niepoprawna odpowiedź. Spróbuj ponownie.");
            }

            if (answer.ToString() == question.CorrectAnswer)
            {
                score++;
            }

            Console.WriteLine("Naciśnij dowolny przycisk, aby kontynuować...");
            Console.ReadKey();

            questionNumber++; // Zwiększ numer pytania
        }

        Console.WriteLine("Quiz zakończony!");
        Console.WriteLine("Liczba poprawnych odpowiedzi: " + score);
        return score;
    }

    static List<Question> LoadQuestionsFromFile(string filePath)
    {
        List<Question> questions = new List<Question>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] data = line.Split(';');
                int id;
                if (int.TryParse(data[0], out id))
                {
                    string text = data[1];
                    string correctAnswer = data[2];
                    string answer1 = data[3];
                    string answer2 = data[4];
                    string answer3 = data[5];
                    string answer4 = data[6];

                    Question question = new Question(id, text, correctAnswer, answer1, answer2, answer3, answer4);
                    questions.Add(question);
                }
                else
                {
                    Console.WriteLine("Error podczas ładowania pytania: The input string 'ID' was not in a correct format.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error podczas ładowania pytania: " + ex.Message);
        }

        return questions;
    }

    class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }

        public Question(int id, string text, string correctAnswer, string answer1, string answer2, string answer3, string answer4)
        {
            Id = id;
            Text = text;
            CorrectAnswer = correctAnswer;
            Answer1 = answer1;
            Answer2 = answer2;
            Answer3 = answer3;
            Answer4 = answer4;
        }
    }
}
