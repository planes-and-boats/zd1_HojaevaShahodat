using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zd23_Hojaeva
{
    class Shop
    {
        private Dictionary<Product, int> products;      //словарь с продуктами
        private decimal profit = 0;
        public decimal GetProfit()      //метод для получения прибыли
        {
            return profit;
        }

        //метод достижения желаемой прибыли, возвращает истино/ложь и недостающую сумму
        public bool GetProfit(decimal targetProfit, out decimal amountLeft)
        {
            if (profit >= targetProfit)
            {
                amountLeft = 0;
                return true;
            }
            amountLeft = targetProfit - profit;
            return false;
        }
        public Shop()       //конструктов без входных значений
        {
            products = new Dictionary<Product, int>();
        }
        public void AddProduct(Product product, int count)      //метод для добавления продукта
        {
            products.Add(product, count);
        }

        public void CreateProduct(string name, decimal price, int count)    //метод для создания продукта
        {
            //проверка, есть ли уже товар с таким именем и ценой
            Product existing = FindProduct(name, price);
            if (existing != null)
            {
                products[existing] += count; // если есть, просто добавляем количество
            }
            else
            {
                products.Add(new Product(name, price), count); // если нет, добавляем как новый
            }
        }

        public void WriteAllProducts()      //метод для вывода всех продуктов
        {
            Console.WriteLine("Список продуктов: ");
            foreach (var product in products)
            {
                Console.WriteLine(product.Key.GetInfo() + "; Количество: " + product.Value);
            }
        }
        public string Sell(Product product)       //метод для поиска и продажи продукта
        {
            if (products.ContainsKey(product))
            {
                if (products[product] == 0)
                {
                    return "Нет в наличии!";
                }
                else
                {
                    products[product]--;
                    profit += product.Price;     // увеличение прибыли
                    return $"Успешно продано: {product.Name}. Цена: {product.Price} руб.";
                }
            }
            else
            {
                return "Товар не найден!";
            }
        }
        public string Sell(string ProductName)       //метод для продажи продукта
        {
            Product ToSell = FindByName(ProductName);
            if (ToSell != null)
            {
                return this.Sell(ToSell);
            }
            else
            {
                return "Товар не найден!";
            }
        }
        public string Sell(string productName, decimal price, int count) //метод для продажи нескольких штук по названию И цене
        {
            Product toSell = FindProduct(productName, price);    //поиск с учетом цены
            if (toSell != null)
            {
                return Sell(toSell, count);
            }
            return "Товар не найден!";
        }
        public Product FindByName(string name)      //метод для поиска продукта
        {
            foreach (var product in products.Keys)
            {
                if (product.Name == name)
                {
                    return product;
                }
            }
            return null;
        }
        public Product FindProduct(string name, decimal price)      //метод для поиска продукта с ценой
        {
            foreach (var product in products.Keys)
            {
                if (product.Name == name && product.Price == price)
                {
                    return product;
                }
            }
            return null;
        }
        public string Sell(Product product, int count)      //продажа нескольких штук продукта
        {
            if (products.ContainsKey(product))
            {
                if (products[product] < count)
                {
                    return $"Недостаточно товара! В наличии: {products[product]} шт.";
                }
                else
                {
                    products[product] -= count;
                    decimal totalCost = product.Price * count;
                    profit += totalCost;        // увеличение прибыли
                    return $"Успешно продано {count} шт. на сумму {totalCost} руб.";
                }
            }
            return "Товар не найден!";
        }
        public string Sell(string productName, int count)       //продажа нескольких штук по названию продукта
        {
            Product toSell = FindByName(productName);
            if (toSell != null)
            {
                return Sell(toSell, count);
            }
            return "Товар не найден!";
        }
        public List<string> GetProductsList()       //получение списка продуктов
        {
            List<string> list = new List<string>();
            foreach (var product in products)
            {
                list.Add($"{product.Key.Name} - {product.Key.Price} руб. (Остаток: {product.Value} шт.)");
            }
            return list;
        }
        public string ClearEmptyProducts()       //метод для удаления товаров с нулевым остатком
        {
            List<Product> toRemove = new List<Product>();
            foreach (var pair in products)
            {
                if (pair.Value == 0)  //все продукты, количество которых равно 0
                {
                    toRemove.Add(pair.Key);
                }
            }
            foreach (var product in toRemove)
            {
                products.Remove(product);  // удаление их из словаря
            }

            return $"Убрано с витрины товаров: {toRemove.Count} шт.";
        }
    }
}
