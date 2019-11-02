using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorial
{
    public delegate void FactorialIsReady(Factorial f);
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

            f.FactorialInitialized += selector.Select;

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
        public delegate void FactorialStartdHandler(Factorial f);
        public event FactorialStartdHandler FactorialInitialized;
        public int Number;
        public int? Result;
        public int Current;
        public Factorial(int n)
        {
            Number = n;
            Current = Number;
            Result = null;
        }
        public void Start()
        {
            FactorialInitialized(this);
        }
    }
    public class Selector
    {
        public delegate void FactorialSelected(Factorial f);
        
        public event FactorialSelected FactorialIsInvalid;
        public event FactorialSelected FactorialIsZero;
        public event FactorialSelected FactorialIsSimple;
        public event FactorialSelected FactorialNeedCalculating;
        
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
        public event FactorialIsReady IsReady;
        public void FactorialIsZero(Factorial f)
        {
            f.Result = 1;
            IsReady(f);
        }
    }
    public class IsSimple
    {
        public event FactorialIsReady IsReady;
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
    public delegate void FactorialCalculatingHandler(FactorialCalculating fc);
    public class FactorialCalculating
    {
        public event FactorialCalculatingHandler IsReady;
        public int Steps;
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
        public event FactorialCalculatingHandler NeedIteration;
        public event FactorialIsReady IsReady;
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
        public event FactorialCalculatingHandler IsReady;
        public void Iterate(FactorialCalculating fc)
        {
            fc.Factorial.Result *= (fc.Steps + 1);
            fc.Steps--;
            Console.WriteLine("Calculating iterated");
            IsReady(fc);
        }
    }
}
