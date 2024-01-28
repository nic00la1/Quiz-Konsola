/********************************
klasa: Quiz
opis: Główna klasa programu quizowego, zarządza logiką quizu i interakcją z użytkownikiem.
pola: repeatQuiz, highestScore
autor: Nicola
********************************/

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
            Console.WriteLine("3. Pokaz listę wyników");
            Console.WriteLine("4. Wychodze");
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
                    ShowScores();
                    break;
                case 4:
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

    /********************************
    funkcja: ShowQuizResults
    opis: Wyświetla pytania i odpowiedzi po zakończeniu quizu.
    parametry: questions - lista pytań, answeredQuestions - lista indeksów odpowiedzi użytkownika
    autor: Nicola
    ********************************/
    static void ShowQuizResults(List<Question> questions, List<int> answeredQuestions)
    {
        Console.WriteLine("Pytania i odpowiedzi:");

        for (int i = 0; i < questions.Count; i++)
        {
            Console.WriteLine($"Pytanie {i + 1}: {questions[i].Text}");

            Console.WriteLine($"1. {questions[i].Answer1} {(answeredQuestions[i] == 0 ? "(Twoja odpowiedź)" : "")}");
            Console.WriteLine($"2. {questions[i].Answer2} {(answeredQuestions[i] == 1 ? "(Twoja odpowiedź)" : "")}");
            Console.WriteLine($"3. {questions[i].Answer3} {(answeredQuestions[i] == 2 ? "(Twoja odpowiedź)" : "")}");
            Console.WriteLine($"4. {questions[i].Answer4} {(answeredQuestions[i] == 3 ? "(Twoja odpowiedź)" : "")}");

            Console.WriteLine($"Poprawna odpowiedź: {GetAnswerLabel(questions[i], int.Parse(questions[i].CorrectAnswer))}");

            Console.WriteLine();
        }
    }


    /********************************
    funkcja: StartQuiz
    opis: Rozpoczyna nową grę quizową, przeprowadzając użytkownika przez pytania.
    zwraca: Liczbę poprawnych odpowiedzi.
    autor: Nicola
    ********************************/
    static int StartQuiz()
    {
        List<Question> questions = LoadQuestionsFromFile("quiz.txt");
        List<int> answeredQuestions = new List<int>();  // Lista indeksów pytań, na które udzielono odpowiedzi

        int score = 0;
        int questionNumber = 1;

        // Przetasowanie pytań
        Przetasowanie(questions);

        Question question = null;  // Linia dodana

        foreach (Question q in questions)
        {
            question = q;  // Linia dodana
            Console.Clear();
            Console.WriteLine("Pytanie " + questionNumber + ": " + question.Text);
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

            // Zapisz indeks odpowiedzi w liście answeredQuestions
            answeredQuestions.Add(answer - 1); // Subtract 1 to get the index (0-3) corresponding to options 1-4

            Console.WriteLine("Naciśnij dowolny przycisk, aby kontynuować...");
            Console.ReadKey();

            questionNumber++;
        }

        Console.WriteLine("Quiz zakończony!");
        Console.WriteLine("Liczba poprawnych odpowiedzi: " + score);
        Console.ReadKey(); // Linia dodana dla wygody testowania
        Console.Clear();

        // Wyświetlenie pytań i odpowiedzi po zakończeniu quizu
        ShowQuizResults(questions, answeredQuestions);

        // Pobranie imienia gracza
        Console.Write("Podaj swoje imię: ");
        string playerName = Console.ReadLine();

        // Zapis wyniku do pliku
        SaveScoreToFile(playerName, score);

        return score;
    }

    // Poprawiona sygnatura funkcji GetAnswerLabel
    static string GetAnswerLabel(Question question, int answerIndex)
    {
        switch (answerIndex)
        {
            case 1: return question.Answer1;
            case 2: return question.Answer2;
            case 3: return question.Answer3;
            case 4: return question.Answer4;
            default: return "Nie udzielono odpowiedzi";
        }
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

    static void ShowScores()
    {
        try
        {
            string[] scores = File.ReadAllLines("rekord.txt");

            Console.WriteLine("Lista wyników:");
            foreach (string score in scores)
            {
                Console.WriteLine(score);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error podczas ładowania wyników: " + ex.Message);
        }
    }

    static void SaveScoreToFile(string playerName, int score)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("rekord.txt", true))
            {
                writer.WriteLine(playerName + ": " + score);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error podczas zapisywania wyniku: " + ex.Message);
        }
    }

    static void Przetasowanie<T>(List<T> list) // Przetasowanie Fishera-Yatesa
    {
        Random random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    /********************************
    klasa: Question
    opis: Klasa reprezentująca pojedyncze pytanie w quizie.
    pola: Id, Text, CorrectAnswer, Answer1, Answer2, Answer3, Answer4
    autor: Nicola
    ********************************/
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

