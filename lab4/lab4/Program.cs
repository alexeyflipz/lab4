using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    public class Товар
    {
        public string Назва { get; set; }
        public decimal Ціна { get; set; }
        public string Опис { get; set; }
        public string Категорія { get; set; }
        public double Рейтинг { get; set; }

        public Товар(string назва, decimal ціна, string опис, string категорія, double рейтинг)
        {
            Назва = назва;
            Ціна = ціна;
            Опис = опис;
            Категорія = категорія;
            Рейтинг = рейтинг;
        }

        public override string ToString()
        {
            return $"{Назва} - {Ціна} грн, Категорія: {Категорія}, Рейтинг: {Рейтинг}";
        }
    }

    public class Користувач
    {
        public string Логін { get; set; }
        public string Пароль { get; set; }
        public List<Замовлення> ІсторіяПокупок { get; set; }

        public Користувач(string логін, string пароль)
        {
            Логін = логін;
            Пароль = пароль;
            ІсторіяПокупок = new List<Замовлення>();
        }

        public void ДодатиЗамовлення(Замовлення замовлення)
        {
            ІсторіяПокупок.Add(замовлення);
        }
    }

    public class Замовлення
    {
        public List<Товар> Товари { get; set; }
        public Dictionary<Товар, int> Кількість { get; set; }
        public decimal ЗагальнаВартість { get; private set; }
        public string Статус { get; set; }

        public Замовлення()
        {
            Товари = new List<Товар>();
            Кількість = new Dictionary<Товар, int>();
            Статус = "Очікує обробки";
        }

        public void ДодатиТовар(Товар товар, int кількість)
        {
            if (!Кількість.ContainsKey(товар))
            {
                Кількість[товар] = кількість;
                Товари.Add(товар);
            }
            else
            {
                Кількість[товар] += кількість;
            }
            ЗагальнаВартість += товар.Ціна * кількість;
        }

        public override string ToString()
        {
            return $"Замовлення: {Товари.Count} товарів, Загальна вартість: {ЗагальнаВартість} грн, Статус: {Статус}";
        }
    }

    public interface ISearchable
    {
        List<Товар> ПошукЗаЦіною(decimal мінЦіна, decimal максЦіна);
        List<Товар> ПошукЗаКатегорією(string категорія);
        List<Товар> ПошукЗаРейтингом(double мінРейтинг);
    }

    public class Магазин : ISearchable
    {
        public List<Товар> Товари { get; set; }
        public List<Користувач> Користувачі { get; set; }
        public List<Замовлення> Замовлення { get; set; }

        public Магазин()
        {
            Товари = new List<Товар>();
            Користувачі = new List<Користувач>();
            Замовлення = new List<Замовлення>();
        }

        public void ДодатиТовар(Товар товар)
        {
            Товари.Add(товар);
        }

        public void ДодатиКористувача(Користувач користувач)
        {
            Користувачі.Add(користувач);
        }

        public List<Товар> ПошукЗаЦіною(decimal мінЦіна, decimal максЦіна)
        {
            return Товари.Where(t => t.Ціна >= мінЦіна && t.Ціна <= максЦіна).ToList();
        }

        public List<Товар> ПошукЗаКатегорією(string категорія)
        {
            return Товари.Where(t => t.Категорія.Equals(категорія, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Товар> ПошукЗаРейтингом(double мінРейтинг)
        {
            return Товари.Where(t => t.Рейтинг >= мінРейтинг).ToList();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Магазин магазин = new Магазин();

            магазин.ДодатиТовар(new Товар("Ноутбук", 20000, "Сучасний ноутбук", "Електроніка", 4.8));
            магазин.ДодатиТовар(new Товар("Смартфон", 15000, "Флагманський смартфон", "Електроніка", 4.5));
            магазин.ДодатиТовар(new Товар("Кава", 200, "Свіжозмелена кава", "Продукти", 4.9));

            Console.WriteLine("Ласкаво просимо до магазину!");
            Console.WriteLine("1. Пошук товарів за ціною");
            Console.WriteLine("2. Пошук товарів за категорією");
            Console.WriteLine("3. Пошук товарів за рейтингом");
            Console.WriteLine("0. Вихід");

            int вибір;
            do
            {
                Console.Write("Ваш вибір: ");
                вибір = int.Parse(Console.ReadLine());

                switch (вибір)
                {
                    case 1:
                        Console.Write("Мінімальна ціна: ");
                        decimal мінЦіна = decimal.Parse(Console.ReadLine());
                        Console.Write("Максимальна ціна: ");
                        decimal максЦіна = decimal.Parse(Console.ReadLine());
                        var товариЗаЦіною = магазин.ПошукЗаЦіною(мінЦіна, максЦіна);
                        Console.WriteLine("Результати пошуку:");
                        товариЗаЦіною.ForEach(t => Console.WriteLine(t));
                        break;

                    case 2:
                        Console.Write("Категорія: ");
                        string категорія = Console.ReadLine();
                        var товариЗаКатегорією = магазин.ПошукЗаКатегорією(категорія);
                        Console.WriteLine("Результати пошуку:");
                        товариЗаКатегорією.ForEach(t => Console.WriteLine(t));
                        break;

                    case 3:
                        Console.Write("Мінімальний рейтинг: ");
                        double мінРейтинг = double.Parse(Console.ReadLine());
                        var товариЗаРейтингом = магазин.ПошукЗаРейтингом(мінРейтинг);
                        Console.WriteLine("Результати пошуку:");
                        товариЗаРейтингом.ForEach(t => Console.WriteLine(t));
                        break;

                    case 0:
                        Console.WriteLine("До побачення!");
                        break;

                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            } while (вибір != 0);
        }
    }
}
