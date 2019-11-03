using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorial
{
    public delegate void FactorialStateHandler(Factorial f);
    public delegate void FactorialCalculatingStateHandler(FactorialCalculating fc);
    class Program
    {
        static void Main(string[] args)
        {
            Factorial f = new Factorial(5);

            Selector selector = new Selector();
            Invalid invalid = new Invalid();
            IsZero isZero = new IsZero();
            IsSimple isSimple = new IsSimple();

            FactorialCalculating factorialCalculating = new FactorialCalculating();
            CalculatingSelector calculatingSelector = new CalculatingSelector();
            Iterator iterator = new Iterator();

            IsReady isReady = new IsReady();

            f.Initialized += selector.Select;

            selector.FactorialIsInvalid += invalid.FactorialIsInvalid;
            selector.FactorialIsSimple += isSimple.FactorialIsSimple;
            selector.FactorialIsZero += isZero.FactorialIsZero;
            selector.FactorialNeedCalculating += factorialCalculating.FactorialCalculatingPrepare;

            factorialCalculating.IsReady += calculatingSelector.Select;

            calculatingSelector.NeedIteration += iterator.Iterate;
            iterator.IsReady += calculatingSelector.Select;

            calculatingSelector.IsReady += isReady.FactorialWrite;
            isZero.IsReady += isReady.FactorialWrite;
            isSimple.IsReady += isReady.FactorialWrite;

            f.Start();
        }
    }
    public class Factorial
    {
        public event FactorialStateHandler Initialized;
        public int Number { get; set; }
        public int? Result { get; set; }
        public Factorial(int n)
        {
            Number = n;
            Result = null;
        }
        public void Start()
        {
            Initialized(this);
        }
    }
    public class Selector
    {
        public event FactorialStateHandler FactorialIsInvalid;
        public event FactorialStateHandler FactorialIsZero;
        public event FactorialStateHandler FactorialIsSimple;
        public event FactorialStateHandler FactorialNeedCalculating;
        
        public void Select(Factorial f)
        {
            if (f.Number < 0)
            {
                FactorialIsInvalid(f);
            }
            else if (f.Number == 0)
            {
                FactorialIsZero(f);
            }
            else if (f.Number > 0 && f.Number <= 2)
            {
                FactorialIsSimple(f);
            }
            else 
            {
                FactorialNeedCalculating(f);
            }
        }
    }
    public class Invalid
    {
        public void FactorialIsInvalid(Factorial f)
        {
            Console.WriteLine("Invalid value for factorial\n");
        }
    }
    public class IsZero
    {
        public event FactorialStateHandler IsReady;
        public void FactorialIsZero(Factorial f)
        {
            f.Result = 1;
            IsReady(f);
        }
    }
    public class IsSimple
    {
        public event FactorialStateHandler IsReady;
        public void FactorialIsSimple(Factorial f)
        {
            f.Result = f.Number;
            IsReady(f);
        }
    }
    public class IsReady
    {
        public void FactorialWrite(Factorial f)
        {
            Console.WriteLine("Factorial({0}) = {1}", f.Number, f.Result);
        }
    }
    
    public class FactorialCalculating
    {
        public event FactorialCalculatingStateHandler IsReady;
        public int Steps { get; set; }
        public Factorial Factorial { get; set; }
        public void FactorialCalculatingPrepare(Factorial f)
        {
            Steps = f.Number - 1;
            Factorial = f;
            f.Result = 1;
            Console.WriteLine("Calculating prepared");
            IsReady(this);
        }
    }
    public class CalculatingSelector
    {
        public event FactorialCalculatingStateHandler NeedIteration;
        public event FactorialStateHandler IsReady;
        public void Select(FactorialCalculating fc)
        {
            if (fc.Steps > 0)
            {
                Console.WriteLine("Calculating continued");
                NeedIteration(fc);
            }
            else
            {
                Console.WriteLine("Calculating finished");
                IsReady(fc.Factorial);
            }
        }
    }
    public class Iterator
    {
        public event FactorialCalculatingStateHandler IsReady;
        public void Iterate(FactorialCalculating fc)
        {
            fc.Factorial.Result *= (fc.Steps + 1);
            fc.Steps--;
            Console.WriteLine("Calculating iterated");
            IsReady(fc);
        }
    }
}
