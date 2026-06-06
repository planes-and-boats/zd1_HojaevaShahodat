using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zd23_Hojaeva
{
    internal class Product
    {
        public decimal Price { get; set; }      //свойство цена
        public string Name { get; set; }      //свойство название

        public Product(string Name, decimal Price)      //конструктор со входными значениями
        {
            this.Name = Name;
            this.Price = Price;
        }
        public string GetInfo()     //функция информация о продукте
        {
            return $"Наименование: {Name}; Цена: {Price} руб.";
        }
    }
}
