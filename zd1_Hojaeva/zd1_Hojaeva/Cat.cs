using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zd1_Hojaeva
{
    internal class Cat
    {
        private string name;    //скрытое поле
        private double weight;    //скрытое поле
        public Cat(string CatName, double CatWeight)    //конструктор 1 со входными значениями
        {
            Name = CatName;
            Weight = CatWeight;
        }
        public Cat() { }    //кнструктор 2 без входных значений
        public string Name    //свойство Имя с проверками
        { 
            get
            {
                return name;
            }
            set
            {
                bool OnlyLetters = true;

                foreach (var ch in value)
                {
                    if (!char.IsLetter(ch))
                    {
                        OnlyLetters = false;
                    }
                }
                if (OnlyLetters)
                {
                    name = value; 
                }
                else
                {
                    Console.WriteLine($"{value} - неправильное имя!!!");
                }
            }
        }
        public double Weight    //свойство Вес с проверками
        {
            get
            {
                return weight;
            }
            set
            {
                if (value > 0 && value < 40)
                {
                    weight = value;
                }
                else
                {
                    Console.WriteLine($"{value} - неправильный вес!!!");
                }
            }
        }
        public void Meow()  //метод Мяу 
        {
            Console.WriteLine($"{name}: МЯЯЯЯУ!!!!");
        }
    
    }
}
