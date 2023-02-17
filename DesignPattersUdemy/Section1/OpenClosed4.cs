using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattersUdemy.Section1
{
    public enum Color
    {
        Red, Black, Blue, Green
    }
    public enum Size
    {
        Small, Medium, Large, Yuge
    }
    public enum Form
    {
        Square, Circle
    }
    public class Product
    {
        public string Name; public Color Color; public Size Size; public Form Form;
        public Product(string name, Color color, Size size, Form form)
        {
            if (name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }
            Name = name;
            Color = color;
            Size = size;
            Form = form;
        }
    }
    public class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
                if (p.Size == size)
                    yield return p;
        }
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
                if (p.Color == color)
                    yield return p;
        }
    }


    // Open-Close principle implementation

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFiler<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;
        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;
        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class FormSpecification : ISpecification<Product>
    {
        private Form form;
        public FormSpecification(Form form)
        {
            this.form = form;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Form == form;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;
        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(T t)
        {
            return this.first.IsSatisfied(t) && this.second.IsSatisfied(t);
        }
    }

    public class MultiSpecification<T> : ISpecification<T>
    {
        private List<ISpecification<T>> specifications;
        public MultiSpecification(List<ISpecification<T>> specifications)
        {
            this.specifications = specifications;
        }

        public bool IsSatisfied(T t)
        {
            bool r = true;
            foreach (var s in specifications)
            {
                r = r && s.IsSatisfied(t);
            }
            return r;
        }
    }

    public class BetterFilter : IFiler<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
                if (spec.IsSatisfied(i))
                    yield return i;
        }

    }
    internal class OpenClosed4
    {
        public void Run()
        {
            Console.WriteLine("Open Close Principle!");
            var apple = new Product("Apple", Color.Red, Size.Large, Form.Square);
            var tree = new Product("Tree", Color.Green, Size.Yuge, Form.Circle);
            var house = new Product("House", Color.Blue, Size.Large, Form.Circle);

            Product[] products = new Product[] { apple, tree, house };
            var pf = new ProductFilter();
            Console.WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            var bf = new BetterFilter();
            Console.WriteLine("Green products (new):");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            Console.WriteLine("Large blue items");
            foreach (var p in bf.Filter(products, new AndSpecification<Product>(
                new ColorSpecification(Color.Blue),
                new SizeSpecification(Size.Large))))
            {
                Console.WriteLine($" - {p.Name} is big and blue");
            }

            Console.WriteLine("Large blue items Multi filter");
            foreach (var p in bf.Filter(products, new MultiSpecification<Product>(
                new List<ISpecification<Product>>()
                {
                new ColorSpecification(Color.Blue),
                new SizeSpecification(Size.Large),
                new FormSpecification(Form.Circle)
                })
                ))
            {
                Console.WriteLine($" - {p.Name} is blue, large and circle");
            }
        }
    }
}
