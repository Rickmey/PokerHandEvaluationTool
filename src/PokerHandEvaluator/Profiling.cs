using System;

namespace PokerHandEvaluator
{

    /// <summary>
    /// This class is for test purpose only. To get decent result it had to be build in release mode and executed outside VisualStudio.
    /// </summary>
    class Profiling
    {
        static void Main()
        {
            oldVsNewTexasHoldemEvaluation();
            //exhaustiveTexasHoldem();
        }

        static void exhaustiveTexasHoldem()
        {
            var hand1 = "Ac2c".ToCardMask();
            var hand2 = "6s6h".ToCardMask();
            var hand3 = "Kd7c".ToCardMask();
            var hand4 = "9s8h".ToCardMask();
            var hand5 = "TsJh".ToCardMask();
            var hand6 = "4c3c".ToCardMask();
            var hand7 = "4s3h".ToCardMask();
            var hand8 = "TcQc".ToCardMask();
            var hand9 = "KcKh".ToCardMask();
            var board1 = "As7h7s".ToCardMask();
            var board2 = "As7h7s5s".ToCardMask();

            Console.WriteLine("+++ Texas Holdem Exhaustive +++");
            Console.WriteLine();
            Console.WriteLine("PRE FLOP");
            Profile("2 Player 2 Cards (1,712,304 trials):   ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2));
            Profile("3 Player 2 Cards (1,370,754 trials):   ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2, hand3));
            Profile("4 Player 2 Cards (1,086,008 trials):   ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2, hand3, hand4));
            Profile("5 Player 2 Cards (850,668 trials):     ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2, hand3, hand4, hand5));
            Profile("6 Player 2 Cards (658,008 trials):     ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2, hand3, hand4, hand5, hand6));
            Profile("7 Player 2 Cards (501,942 trials):     ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2, hand3, hand4, hand5, hand6, hand7));
            Profile("8 Player 2 Cards (376,992 trials):     ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2, hand3, hand4, hand5, hand6, hand7, hand8));
            Profile("9 Player 2 Cards (278,256 trials):     ", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(0, hand1, hand2, hand3, hand4, hand5, hand6, hand7, hand8, hand9));
            Console.WriteLine();
            Console.WriteLine("FLOP");
            Profile("2 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2));
            Profile("3 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2, hand3));
            Profile("4 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2, hand3, hand4));
            Profile("5 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2, hand3, hand4, hand5));
            Profile("6 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2, hand3, hand4, hand5, hand6));
            Profile("7 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2, hand3, hand4, hand5, hand6, hand7));
            Profile("8 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2, hand3, hand4, hand5, hand6, hand7, hand8));
            Profile("9 Player 5 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board1, hand1, hand2, hand3, hand4, hand5, hand6, hand7, hand8, hand9));
            Console.WriteLine();
            Console.WriteLine("TURN");
            Profile("2 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2));
            Profile("3 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2, hand3));
            Profile("4 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2, hand3, hand4));
            Profile("5 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2, hand3, hand4, hand5));
            Profile("6 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2, hand3, hand4, hand5, hand6));
            Profile("7 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2, hand3, hand4, hand5, hand6, hand7));
            Profile("8 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2, hand3, hand4, hand5, hand6, hand7, hand8));
            Profile("9 Player 6 Cards:", 1, () => TexasHoldem.Simulator.ExhaustiveNPlayer(board2, hand1, hand2, hand3, hand4, hand5, hand6, hand7, hand8, hand9));
            Console.ReadLine();
        }

        static void oldVsNewTexasHoldemEvaluation()
        {
            var count = 50 * 1000000;
            var straightflushS = "As 2s 3s 4s 5s Ts Kd".ToCardMask();
            var straightflushD = "Ad 2d 3d 4d 5d Ts Kd".ToCardMask();
            var straightflushH = "Ah 2h 3h 4h 5h Ts Kd".ToCardMask();
            var straightflushC = "Ac 2c 3c 4c 5c Ts Kd".ToCardMask();

            var quads = "As Ad Ah Ac Ts Kd 2s".ToCardMask();
            var fullhouse = "As Ad Ah 2d 2d Ts Kd".ToCardMask();
            var flush = "As 2h 3s 4s 5s Ts Kd".ToCardMask();
            var straight = "As 2d 3s 4d 5s Ts Kd".ToCardMask();
            var trips = "As Ad Ac 2d 3d Ts Kd".ToCardMask();
            var twopair = "As Ad 2c 2d 3d Ts Kd".ToCardMask();
            var onepair = "As Ad 2c 7d 3d Ts Kd".ToCardMask();
            var highcard = "As 9d 2c Jd 3d Ts Kd".ToCardMask();
            double total1 = 0;
            double total2 = 0;

            Console.WriteLine("+++ Texas Holdem Evaluation Implementation Comparison +++");
            Console.WriteLine("{0} iterations", count);
            total1 += Profile("Original  straightflush spade", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(straightflushS, 7));
            total2 += Profile("New       straightflush spade", count, () => TexasHoldem.Evaluator.GetHandValue(straightflushS, 7));
            Console.WriteLine();

            total1 += Profile("Original  straightflush diamo", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(straightflushD, 7));
            total2 += Profile("New       straightflush diamo", count, () => TexasHoldem.Evaluator.GetHandValue(straightflushD, 7));
            Console.WriteLine();

            total1 += Profile("Original  straightflush heart", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(straightflushH, 7));
            total2 += Profile("New       straightflush heart", count, () => TexasHoldem.Evaluator.GetHandValue(straightflushH, 7));
            Console.WriteLine();

            total1 += Profile("Original  straightflush clubs", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(straightflushC, 7));
            total2 += Profile("New       straightflush clubs", count, () => TexasHoldem.Evaluator.GetHandValue(straightflushC, 7));
            Console.WriteLine();

            total1 += Profile("Original  quads              ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(quads, 7));
            total2 += Profile("New       quads              ", count, () => TexasHoldem.Evaluator.GetHandValue(quads, 7));
            Console.WriteLine();

            total1 += Profile("Original  fullhouse          ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(fullhouse, 7));
            total2 += Profile("New       fullhouse          ", count, () => TexasHoldem.Evaluator.GetHandValue(fullhouse, 7));
            Console.WriteLine();

            total1 += Profile("Original  flush              ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(flush, 7));
            total2 += Profile("New       flush              ", count, () => TexasHoldem.Evaluator.GetHandValue(flush, 7));
            Console.WriteLine();

            total1 += Profile("Original  straight           ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(straight, 7));
            total2 += Profile("New       straight           ", count, () => TexasHoldem.Evaluator.GetHandValue(straight, 7));
            Console.WriteLine();

            total1 += Profile("Original  trips              ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(trips, 7));
            total2 += Profile("New       trips              ", count, () => TexasHoldem.Evaluator.GetHandValue(trips, 7));
            Console.WriteLine();

            total1 += Profile("Original  twopair            ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(twopair, 7));
            total2 += Profile("New       twopair            ", count, () => TexasHoldem.Evaluator.GetHandValue(twopair, 7));
            Console.WriteLine();

            total1 += Profile("Original  onepair            ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(onepair, 7));
            total2 += Profile("New       onepair            ", count, () => TexasHoldem.Evaluator.GetHandValue(onepair, 7));
            Console.WriteLine();

            total1 += Profile("Original  highcard           ", count, () => TexasHoldem.EvaluatorOriginal.GetHandValue(highcard, 7));
            total2 += Profile("New       highcard           ", count, () => TexasHoldem.Evaluator.GetHandValue(highcard, 7));
            Console.WriteLine();
            Console.WriteLine("Original= {0}ms", total1);
            Console.WriteLine("New =     {0}ms", total2);
            Console.WriteLine("New is {0}% fast than the original", (((total1 / total2) - 1) * 100).ToString("0.00"));
            Console.ReadLine();
        }


        #region Profiling
        // https://stackoverflow.com/questions/1047218/benchmarking-small-code-samples-in-c-can-this-implementation-be-improved
        internal static double Profile(string description, int iterations, Action action)
        {
            //Run at highest priority to minimize fluctuations caused by other processes/threads
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;

            // warm up 
            action();

            var watch = new System.Diagnostics.Stopwatch();

            // clean up
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();

            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                action();
            }
            watch.Stop();
            System.Console.Write(description);
            System.Console.WriteLine(" Time Elapsed {0} ms", watch.Elapsed.TotalMilliseconds);
            return watch.Elapsed.TotalMilliseconds;
        }
        #endregion Profiliing
    }
}
