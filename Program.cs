using System;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Media;
using System.IO;
class Game
{
    static char[,] maze = new char[10, 10]

    {
            { '╔', '═', '═', '═', '═', '═', '═', '═', '═', '╗' },
            { '║', '.', '.', '!', '.', '#', '.', '.', '.', '║' },
            { '║', '.', '!', '.', '.', '!', '.', '.', '#', '║' },
            { '║', '!', '.', '#', '#', '.', '.', '!', '.', '║' },
            { '║', '.', '.', '!', '.', '!', '#', '.', '#', '║' },
            { '║', '!', '#', '#', '#', '.', '.', '!', '.', '║' },
            { '║', '.', '!', '.', '.', '!', '#', '.', '!', '║' },
            { '║', '#', '.', '#', '!', '.', '.', '!', '.', '║' },
            { '║', '.', '.', '.', '#', '.', '!', '.', '.', '║' },
            { '╚', '═', '═', '═', '═', '═', '═', '═', '═', '╝' }
    };
    static int playerX = 1;
    static int playerY = 1;

    static int exitX = 8;
    static int exitY = 8;

    static Random random = new Random();
    static Stopwatch stopwatch = new Stopwatch();
    static void DrawMaze()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
                if (i == playerX && j == playerY)
                {
                    Console.Write('$'); // Vị trí bắt đầu
                }
                else if (i == exitX && j == exitY)
                {
                    Console.Write('E'); // Vị trí kết thúc
                }
                else
                {
                    Console.Write(maze[i, j]);
                }
            Console.WriteLine();
        }
    }

    static void MovePlayer(ConsoleKey key)
    {
        Console.OutputEncoding = Encoding.UTF8;
        int newX = playerX;
        int newY = playerY;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                newX = playerX - 1;
                break;
            case ConsoleKey.DownArrow:
                newX = playerX + 1;
                break;
            case ConsoleKey.LeftArrow:
                newY = playerY - 1;
                break;
            case ConsoleKey.RightArrow:
                newY = playerY + 1;
                break;
        }

        if ((maze[newX, newY] != '#') && (maze[newX, newY] != '║') && (maze[newX, newY] != '═'))
        {
            if (maze[newX, newY] == '!')
            {
                if (AskQuestion())
                {
                    maze[playerX, playerY] = '.';
                    playerX = newX;
                    playerY = newY;
                    maze[playerX, playerY] = '$';
                }
                else
                {
                    maze[newX, newY] = '#'; // Trả lời sai, ký tự '!' trở thành '#'
                }
            }
            else
            {
                maze[playerX, playerY] = '.';
                playerX = newX;
                playerY = newY;
                maze[playerX, playerY] = '$';
            }
        }
    }
    static bool AskQuestion()
    {

        string[,] questions = new string[,]
        {
        { "Bỏ ngoài nướng trong, ăn ngoài bỏ trong là gì?", "nướng ngô", "nướng trứng", "nướng khoai" },
        { "Con gì mang được miếng gỗ lớn nhưng không mang được hòn sỏi?", "con sông", "con đường", "con đèo" },
        { "Cái gì ở giữa bầu trời và trái đất", "và", "mây", "gió" },
        { "Thủ đô của Anh là", "london", "paris", "berlin" },
        { "Mồm bò không phải mồm bò mà lại là mồm bò là gì?", "ốc sên", "con bò", "con ngựa" },
        { "Thủ đô của Việt Nam là?", "Hà Nội","Hồ Chí Minh",  "Đà Nẵng" },
        { "Tỉnh thành nào nằm ở cực Bắc của Việt Nam?", "Hà Giang", "Sơn La", "Lạng Sơn" },
        { "Tỉnh thành nào là điểm đất liền xa nhất về phía Đông của Việt Nam?", "Cà Mau", "Bạc Liêu", "Trà Vinh" },
        { "Đâu là tỉnh thành không giáp biển?", "Lào Cai", "Quảng Bình", "Ninh Bình" },
        { "Tỉnh thành nào nổi tiếng với vịnh Hạ Long?", "Quảng Ninh", "Bắc Ninh", "Hải Phòng" }
        };

        int index = random.Next(questions.GetLength(0));
        string question = questions[index, 0];
        string correctAnswer = questions[index, 1];
        string wrongAnswer1 = questions[index, 2];
        string wrongAnswer2 = questions[index, 3];

        // Hoán đổi vị trí các câu trả lời ngẫu nhiên
        string[] answerOptions = new string[] { correctAnswer, wrongAnswer1, wrongAnswer2 };
        ShuffleArray(answerOptions);

        Console.WriteLine(question);
        Console.WriteLine("A. " + answerOptions[0]);
        Console.WriteLine("B. " + answerOptions[1]);
        Console.WriteLine("C. " + answerOptions[2]);

        Console.Write("Your answer (A, B, or C): ");
        string userInput = Console.ReadLine().ToUpper();

        string userAnswer;
        if (userInput == "A")
        {
            userAnswer = answerOptions[0];
        }
        else if (userInput == "B")
        {
            userAnswer = answerOptions[1];
        }
        else if (userInput == "C")
        {
            userAnswer = answerOptions[2];
        }
        else
        {
            Console.WriteLine("Invalid input!");
            Console.WriteLine("Please try again");
            return false;
        }

        if (userAnswer == correctAnswer)
        {
            Console.WriteLine("Correct!");
            return true;
        }
        else
        {
            Console.WriteLine("Wrong answer!");
            Console.WriteLine("Please try again");
            return false;
        }
    }

    static void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int randomIndex = i + random.Next(n - i);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    static bool CheckWin()
    {
        return (playerX == exitX && playerY == exitY);
    }
    static void Main(string[] args)
    {
        try
        {
            SoundPlayer soundPlayer = new SoundPlayer(@"C:\Users\Hi\Music\Seven-Jung-Kook.wav");
            soundPlayer.Play();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Tệp âm thanh không tồn tại.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Đã xảy ra lỗi: " + ex.Message);
        }
        Console.OutputEncoding = Encoding.UTF8;
        //Console.Write("Nhập tên của bạn: ");
        //string playerName = Console.ReadLine();
        //Console.WriteLine("Xin chào, " + playerName + "! Bắt đầu chơi!");
        //Console.WriteLine("Game: thoát khỏi mê cung");
        bool showMenu = true;

        while (showMenu)
        {
            showMenu = MainMenu();
        }
    }
    static void ToggleSound()
    {
        if (isSoundPlaying)
        {
            SoundPlayer soundPlayer = new SoundPlayer(@"C:\Users\Hi\Music\Seven-Jung-Kook.wav");
            soundPlayer.Stop(); // Tắt âm thanh
            isSoundPlaying = false;
            Console.WriteLine("Đã tắt âm thanh.");
        }
        else
        {
            SoundPlayer soundPlayer = new SoundPlayer(@"C:\Users\Hi\Music\Seven-Jung-Kook.wav");
            soundPlayer.Play(); // Bật âm thanh
            isSoundPlaying = true;
            Console.WriteLine("Đã bật âm thanh.");
        }
    }
    static bool MainMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;                                                 // Đặt màu chữ là màu vàng
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.SetCursorPosition((Console.WindowWidth - 35) / 2, Console.CursorTop + 10);
        Console.WriteLine("---- TRÒ CHƠI \"THOÁT KHỎI MẬT THẤT\" ----");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop + 1);
        Console.WriteLine("1. Hướng dẫn chơi    ");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("2. Hiển thị bảng menu");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("3. Bắt đầu trò chơi  ");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("4. Thoát GAME        ");
        Console.ResetColor();
        Console.WriteLine("\nMời bạn chọn từ 1-4");
        ConsoleKeyInfo userInput = Console.ReadKey();
        switch (userInput.KeyChar)
        {
            case '1':
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;                                                 // Đặt màu chữ là màu vàng
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition((Console.WindowWidth - 30) / 2, Console.CursorTop + 10);
                Console.WriteLine("--- Chào mừng bạn đến với trò chơi!!! ---\n");
                Console.ResetColor();
                Console.WriteLine("             Bạn sẽ vào vai một nhà thám hiểm, trên đường khám phá bạn bị lạc vào một mê cung vòng vo.\n" +
                    "             Điều kỳ lạ là trong mê cung hiện lên những câu hỏi hóc búa buộc bạn phải vắt óc giải mã \n             để tìm thấy lời giải đáp thoát khỏi mê cung.\n" +
                    "             Hãy dùng cái phím mũi tên để điều khiển nhân vật của bạn khám phá các ngóc ngách của mê cung.\n"+ "\n             => Nhấn phím \"Enter\" để chuẩn bị đến với trò chơi nhé!");
         
                Console.ReadLine();
                return true;
            case '2':
                ShowMenu();
                return true;
            case '3':
                StartGame();
                return true;
            case '4':
                bool exitConfirmed = false;
                do
                {
                    Console.WriteLine("Bạn có chắc chắn muốn thoát game không? (Y/N)");
                    string choice = Console.ReadLine();
                    if (choice.ToUpper() == "Y")
                    {
                        return false;
                    }
                    else if (choice.ToUpper() == "N")
                    {
                        Console.WriteLine("Để tiếp tục lựa chọn mời bạn nhấn phím \"Enter\"");
                        Console.ReadLine();
                        exitConfirmed = true;
                    }
                    else
                    {
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn Y hoặc N.");
                    }
                } while (!exitConfirmed);
                return true;

            default:
                return true;
        }
    }
    static bool isSoundPlaying = true; // Biến theo dõi trạng thái âm thanh
    static void ShowMenu()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;                                                 // Đặt màu chữ là màu vàng
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.SetCursorPosition((Console.WindowWidth - 13) / 2, Console.CursorTop + 10);
        Console.WriteLine("----- BẢNG MENU -----");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop + 1);
        Console.WriteLine("1. Kỷ lục            ");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("2. Dừng tạm thời     ");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("3. Tiếp tục          ");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("4. Bắt đầu lại game  ");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("5. Quay lại          ");
        Console.SetCursorPosition((Console.WindowWidth - 14) / 2, Console.CursorTop);
        Console.WriteLine("6. Tắt/Bật âm thanh  "); // Thêm lựa chọn tắt/bật âm thanh vào menu
        Console.ResetColor();
        Console.WriteLine("\nMời bạn chọn từ 1-6");
        string userInput = Console.ReadLine();
        switch (userInput)
        {
            case "1":
                Console.Clear();
                string showkyluc = File.ReadAllText("kyluc.txt");
                Console.WriteLine(showkyluc);
                Console.WriteLine(showkyluc.GetType().ToString());
                Console.ReadLine();
                break;

            case "2":
                // Handle option 2
                Console.WriteLine("Đang xử lý tùy chọn 2...");

                ShowMenu(); // Re-run the menu
                break;
            case "3":
                // Handle option 3
                Console.WriteLine("Quay lại menu chính...");

                MainMenu(); // Return to the main menu
                break;
            case "4":
                // Handle option 4
                Console.WriteLine("Bắt đầu lại game...");

                StartGame(); // Start the game
                break;
            case "5":
                // Handle option 5
                Console.WriteLine("Quay lại menu chính...");

                MainMenu(); // Return to the main menu
                break;
            case "6":
                ToggleSound(); // Gọi hàm để tắt/bật âm thanh
                break;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");

                ShowMenu(); // Re-run the menu
                break;
        }
    }
    static void StartGame()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Clear();

        //TÊN GAME
        string gameName = "GAME THOÁT KHỎI MẬT THẤT";                                                   // TẠO TÊN CHO GAME
        Console.ForegroundColor = ConsoleColor.DarkRed;                                                 // Đặt màu chữ là màu 
        Console.BackgroundColor = ConsoleColor.Gray;                                                    // Đặt màu nền là màu 
        PrintCenteredText("------ " + gameName + " ------");
        Console.ResetColor();                                                                           // Đặt lại màu chữ và màu nền mặc định

        // THỜI GIAN    
        DateTime startTime = DateTime.Now;                                                              // Lấy thời điểm bắt đầu chơi game

        // Hiển thị ngày và giờ bắt đầu chơi game
        Console.WriteLine("\n" + "--------------------------------");
        Console.WriteLine("day: " + startTime.ToShortDateString());
        Console.WriteLine("time: " + startTime.ToString("hh\\:mm\\:ss"));

        Console.ReadKey();


        Console.WriteLine();
        stopwatch = new Stopwatch();
        stopwatch.Start();
        int score = 0;
        while (true)
        {

            Console.Clear();
            DrawMaze();
            ConsoleKey key = Console.ReadKey().Key;
            MovePlayer(key);

            if (CheckWin())
            {
                Console.Clear();
                DrawMaze();
                Console.WriteLine("Chúc mừng bạn đã thắng level 1");
                Console.ReadKey(true);
                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;
                Console.WriteLine("thời gian chơi: " + elapsedTime.ToString("mm\\:ss"));
                if (elapsedTime < TimeSpan.FromSeconds(30))
                { score += 100; }
                else if (elapsedTime < TimeSpan.FromMinutes(1))
                { score += 50; }
                else { score += 25; }
                Console.WriteLine("điểm của bạn là: " + score);
                while (true)
                {
                    Console.WriteLine("Bạn có muốn tiếp tục trò chơi không^^");
                    Console.WriteLine("1. Tiếp tục đến với level 2");
                    Console.WriteLine("2. Dừng lại tại đây thôi!");
                    Console.WriteLine("Mời nhập lựa chọn của bạn: ");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        // Code để qua màn 2
                        Console.WriteLine("Bạn đã qua màn 2");
                        break;
                    }
                    else if (choice == "2")
                    {
                        // Code để thoát game
                        MainMenu();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                    }
                }
                
                break;
            }

            if (CheckLose())
            {
                Console.Clear();
                DrawMaze();
                Console.WriteLine("Bạn đã thua cuộc!");
                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;
                Console.WriteLine("thời gian chơi: " + elapsedTime.ToString("mm\\:ss"));
                break;
            }
        }
        Console.ReadLine();
        ConsoleKey Key = Console.ReadKey().Key;
        MovePlayer(Key);
        stopwatch.Stop();

        bool CheckLose()
        {
            // Kiểm tra các điều kiện để xác định người chơi đã thua cuộc
            //...

            return false; // Thay đổi giá trị trả về thành true nếu người chơi thua cuộc
        }
        using (FileStream kyluc = new FileStream("kyluc.txt", FileMode.Append, FileAccess.Write))            // xử lý ngoại lệ (th đặc biệt của try-catch-family)
        {
            Console.InputEncoding = Encoding.UTF8;
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;
            int leftMargin = (consoleWidth - 100) / 2;
            int topMargin = (consoleHeight - 1) / 2 - 8;
            TimeSpan elapsedTime = stopwatch.Elapsed;
            Console.SetCursorPosition(leftMargin, topMargin + 1);             // Ghi tên
            Console.WriteLine();
            Console.WriteLine("Enter your name:");
            StreamWriter hoten = new StreamWriter(kyluc);
            hoten.Write("--------------------------------" + "\n" + Console.ReadLine() + "\n");
            hoten.Flush();
            StreamWriter diem = new StreamWriter(kyluc);
            diem.Write("score:" + score + "\n");
            diem.Flush();
            StreamWriter thoigianchoi = new StreamWriter(kyluc);
            thoigianchoi.WriteLine("thời gian chơi kỷ lục của bạn là: " + elapsedTime.ToString("mm\\:ss"));
            thoigianchoi.Flush();
        }
        DateTime startTimes = DateTime.Now;                                                              // Lấy thời điểm bắt đầu chơi game

        // Hiển thị ngày và giờ bắt đầu chơi game
        Console.WriteLine("\n" + "--------------------------------");
        Console.WriteLine("day: " + startTime.ToShortDateString());
        Console.WriteLine("time: " + startTime.ToString("hh\\:mm\\:ss"));
        DateTime endTime = DateTime.Now;                                                                // Lấy thời điểm kết thúc chơi game  
        // Hiển thị thời gian chơi game


        using (FileStream kyluc = new FileStream("kyluc.txt", FileMode.Append, FileAccess.Write))   // Ghi thời gian bắt đầu, kết thúc vào file.
        {
            StreamWriter thoigian = new StreamWriter(kyluc);
            thoigian.WriteLine("start'time: " + startTime.ToString("hh\\:mm\\:ss") + ", " + startTime.ToShortDateString() + "\n");
            thoigian.Flush();

        }
    }
    static void PrintCenteredText(string text)                                //TẠO HIỆU ỨNG RƠI CHẬM CHO TÊN GAME
    {
        Console.InputEncoding = Encoding.UTF8;
        int consoleWidth = Console.WindowWidth;
        int consoleHeight = Console.WindowHeight;
        int leftMargin = (consoleWidth - text.Length) / 2;
        int topMargin = (consoleHeight - 1) / 2 - 8;
        Console.SetCursorPosition(leftMargin, topMargin);

        for (int i = 0; i < text.Length; i++)
        {
            Console.SetCursorPosition(leftMargin + i, Console.CursorTop);
            Console.Write(text[i]);
            System.Threading.Thread.Sleep(60);                                // Đợi 50 milliseconds giữa mỗi ký tự
        }
        using (FileStream kyluc = new FileStream("kyluc.txt", FileMode.Append, FileAccess.Write))            // xử lý ngoại lệ (th đặc biệt của try-catch-family)
        {
            Console.SetCursorPosition(leftMargin, topMargin + 1);             // Ghi tên
            Console.WriteLine();
            Console.WriteLine("Enter your name:");
            StreamWriter hoten = new StreamWriter(kyluc);
            hoten.Write("--------------------------------" + "\n" + Console.ReadLine() + "\n");
            hoten.Flush();
        }
    }
}
