using zd1_Hojaeva;

List<Cat> cats = new List<Cat>();   //список котов
while (true)    //цикл
{
    Console.WriteLine("\nМеню:");
    Console.WriteLine("1. Добавить кота(ов).\n2. Вывести информацию о котах.\n3. Количество котов.\n4. Выход.\n");
    Console.Write("Выберите действие: ");
    string answer = Console.ReadLine();
    Console.WriteLine();
    if (answer == "1")
    {
        //добавление котов в список
        Console.WriteLine("--- Добавление ---");
        int count = 0;  //переменная для количества котов
        do
        {
            Console.WriteLine("Сколько хотите добавить котов: ");
            if (int.TryParse(Console.ReadLine(), out int num))
            {
                count = num;
            }
            else
            {
                Console.WriteLine("Введите целое число!");
            }
        }
        while (count <= 0);
        for (int i = 0; i < count; i++)     //цикл для для добавления котов
        {
            Console.WriteLine($"> Введите данные для {i + 1} кота");
            Cat cat = new Cat();
            do {
                Console.Write("Введите имя кота: ");
                cat.Name = Console.ReadLine();
            } while(cat.Name == null);
            do
            {
                Console.Write("Введите вес кота: ");
                if (double.TryParse(Console.ReadLine(), out double ves))
                {
                    cat.Weight = ves;
                }
                else
                {
                    Console.WriteLine("Введите числовое значение!");
                    continue;
                }
            } while (cat.Weight == 0);
            cat.Meow();
            Console.WriteLine("\tКот добавлен!");
            cats.Add(cat);  //именно тут происходит добавление
        }
    }
    else if (answer == "2")
    {
        //вывод списка котов (если список не пуст)
        if (cats.Count > 0)
        {
            Console.WriteLine($"--- Список котов - {cats.Count} ---");
            for (int i = 0; i < cats.Count; i++)
            {
                Console.WriteLine($"Кот {i + 1}: имя - {cats[i].Name}, вес - {cats[i].Weight}");
            }
        }
        else
        {
            Console.WriteLine("-- Список пуст ---");
        }
    }
    else if (answer == "3")
    {
        Console.WriteLine($"--- Количество котов: {cats.Count} ---");
    }
    else if (answer == "4")
    {
        Console.WriteLine("--- Выход ---");
        break;  //выход из цикла
    }
    else
    {
        continue;   //продолжение без выхода
    }
}