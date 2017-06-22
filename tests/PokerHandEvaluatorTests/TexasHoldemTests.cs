using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PokerHandEvaluator.TexasHoldem.Tests
{
    [TestClass()]
    public class TexasHoldemTests
    {
        ulong getTestValue(string hand, int cardCount)
        {
            ulong cards = Utils.StringToHandMask(hand);
            return Evaluator.GetHandValue(cards, (uint)cardCount);
        }

        ulong getTestValue(string hand)
        {
            return getTestValue(hand, Utils.BitCount(hand.ToCardMask()));
        }

        void firstIsAlwaysLower(List<string> list1, List<string> list2)
        {
            foreach (var item1 in list1)
            {
                foreach (var item2 in list2)
                {
                    var n1 = Utils.BitCount(item1.ToCardMask());
                    var n2 = Utils.BitCount(item2.ToCardMask());
                    Assert.IsTrue(getTestValue(item1, n1) < getTestValue(item2, n2));
                }
            }
        }

        List<string> straightFlush = new List<string>
        {
            "As 2s 3s 4s 5s",
            "As Ks Qs Js Ts",
            "As Ks Qs Js Ts 2d",
            "As Ks Qs Js Ts 2s",
            "As Ks Qs Js Ts Ad",
            "As Ks Qs Js Ts Ad Ac",
        };

        List<string> quads = new List<string>
        {
            "As Ad Ah Ac",
            "Ks Kd Kh Kc",
            "As Ad Ah Ac 2d",
            "As Ad Ah Ac Kd",
            "Ks Kd Kh Kc 2d",
            "Ks Kd Kh Kc Qd",
            "Ks Kd Kh Kc Qd Qh",
            "2s 2d 2h 2c Qd Qh Qs",
            "2s 2d 2h 2c 4d 5h 6s",
        };

        List<string> fullhouse = new List<string>
        {
            "As Ad Ah 2c 2d",
            "As Ad Ah Kc Kd",
            "Ks Kd Kh Qc Qd",
            "Ks Kd Kh 2c 2d",
            "As Ad Ah 2c 2d 2h",
            "As Ad Ah Kc Kd Kh",
            "Ks Kd Kh Qc Qd Qh",
            "Ks Kd Kh 2c 2d 2h",
            "As Ad Ah 5c 2c 2d 3h",
            "As Ad Ah 4c Kc Kd Kh",
            "Ks Kd Kh 2c 2s 2d Qh",
            "Ks Kd Kh 2c 2s Qd Qh",
        };

        List<string> flush = new List<string>
        {
            "As 2s 3s 4s 9s",
            "As Ks Qs Js 3s",
            "As Ks Qs Js 7s 2d",
            "As Ks Qs Js 8s 2s",
            "As Ks Qs Js 9s Ad",
            "As Ks Qs Js 2s Ad Ac",
        };

        List<string> straight = new List<string>
        {
            "As 2d 3s 4s 5s",
            "As Ks Qc Jc Ts",
            "As Kh Qh Jc Ts 2d",
            "Ad Ks Qd Js Td 2s",
            "As Kd Qs Js Ts Ad",
            "As Ks Qc Jh Th Ad Ac",
        };

        List<string> trips = new List<string>
        {
            "As Ad Ah",
            "Ks Kd Kh",
            "As Ad Ah 2d",
            "As Ad Ah Kd",
            "Ks Kd Kh Qd",
            "Ks Kd Kh 2d",
            "As Ad Ah 2d 3d",
            "Ks Kd Kh 3d 2d",
        };

        List<string> twopair = new List<string>
        {
            "As Ad 2h 2d",
            "As Ad Kh Kd",
            "As Ad 2h 2d 3d",
            "As Ad Kh Kd Qd",
            "As Ad 2h 2d 3d 3c",
            "Ks Kd Qh Qd",
            "Ks Kd Qh Qd Jd",
            "Ks Kd Qh Qd Jd Jc",
            "As Ad Kh Kd Qd Qc",
            "Ks Kd 2h 2d",
            "Ks Kd 2h 2d 3d",
            "Ks Kd 2h 2d 3d 3c",
            "Ks Kd 2h 2d 3d 3c Ah",
        };


        List<string> onepair = new List<string>
        {
            "As Ad",
            "As Ad 2s",
            "As Ad 2s 3s",
            "As Ad 2s 3s 4s",
            "As Ad Ks",
            "As Ad Ks Qs",
            "As Ad Ks Qs Js",
            "Ks Kd",
            "Ks Kd Qs",
            "Ks Kd Qs Js",
            "Ks Kd Qs Js Ts",
            "Ks Kd 2s",
            "Ks Kd 2s 3s",
            "Ks Kd 2s 3s 4s",

        };

        [TestMethod()]
        public void HoldemOnePairTest()
        {
            Assert.IsTrue(getTestValue("3d 3s Ah Kh Qh 2d 4s") == getTestValue("3d 3s Ah Kh Qh Jd 9s"));

            firstIsAlwaysLower(onepair, twopair);
            firstIsAlwaysLower(onepair, trips);
            firstIsAlwaysLower(onepair, straight);
            firstIsAlwaysLower(onepair, flush);
            firstIsAlwaysLower(onepair, fullhouse);
            firstIsAlwaysLower(onepair, quads);
            firstIsAlwaysLower(onepair, straightFlush);
        }

        [TestMethod()]
        public void HoldemTwoPairTest()
        {
            Assert.IsTrue(getTestValue("3d 3s 4h 4s Ah 8d 2s") == getTestValue("3d 3s 4h 4s Ah Kd Qs"));

            firstIsAlwaysLower(twopair, trips);
            firstIsAlwaysLower(twopair, straight);
            firstIsAlwaysLower(twopair, flush);
            firstIsAlwaysLower(twopair, fullhouse);
            firstIsAlwaysLower(twopair, quads);
            firstIsAlwaysLower(twopair, straightFlush);
        }

        [TestMethod()]
        public void HoldemTripsTest()
        {
            firstIsAlwaysLower(trips, straight);
            firstIsAlwaysLower(trips, flush);
            firstIsAlwaysLower(trips, fullhouse);
            firstIsAlwaysLower(trips, quads);
            firstIsAlwaysLower(trips, straightFlush);
        }

        [TestMethod()]
        public void HoldemStraightTest()
        {
            firstIsAlwaysLower(straight, flush);
            firstIsAlwaysLower(straight, fullhouse);
            firstIsAlwaysLower(straight, quads);
            firstIsAlwaysLower(straight, straightFlush);

            Assert.IsTrue(getTestValue("Ac 5c 4c 4d 3d 3s 2c") > getTestValue("6s 6h 3d 3s 4d 5c 4c"));
        }

        [TestMethod()]
        public void HoldemFlushTest()
        {
            Assert.IsTrue(getTestValue("Ad 2d 3d 6d Td") == getTestValue("As 2s 3s 6s Ts"));
            Assert.IsTrue(getTestValue("2d 3d 4d 5d 7d") < getTestValue("2s 3s 4s 5s 8s"));
            Assert.IsTrue(getTestValue("2c 3c 4c 5c 7c") < getTestValue("2d 3d 4d 5d 8d"));
            Assert.IsTrue(getTestValue("2h 3h 4h 5h 7h") < getTestValue("2d 3d 4d 5d 8d"));
            Assert.IsTrue(getTestValue("2s 3s 4s 5s 7s") < getTestValue("2c 3c 4c 5c 8c"));
            Assert.IsTrue(getTestValue("7s Ts Js Qs Ks") < getTestValue("8c Tc Jc Qc Kc"));
            Assert.IsTrue(getTestValue("6s 3s 6h 3d 8c 5c 4c") < getTestValue("3s 3d Ac 8c 5c 4c 2c"));

            firstIsAlwaysLower(flush, fullhouse);
            firstIsAlwaysLower(flush, quads);
            firstIsAlwaysLower(flush, straightFlush);
        }

        [TestMethod()]
        public void HoldemFullHouseTest()
        {
            Assert.IsTrue(getTestValue("Ad As Ac 2d 2h") < getTestValue("Ad As Ac 3d 3h"));
            Assert.IsTrue(getTestValue("Ad As Ac 2d 2h") > getTestValue("Ah As 2c 2d 2h"));
            Assert.IsTrue(getTestValue("Ad As Ac Ah 2d 2h 2c") < getTestValue("Ad As Ac Ah 3d 3h 3c"));
            Assert.IsTrue(getTestValue("Ad As Ac Kd Kh") > getTestValue("Ah As 2c 2d 2h"));
            Assert.IsTrue(getTestValue("3d 3s 3c 2s 2d 2h") == getTestValue("3d 3s 3c 2s 2d Ah"));
            Assert.IsTrue(getTestValue("3d 3s 3c 2s 2d 2h Ah") == getTestValue("3d 3s 3c 2s 2d Kh Ah"));
            firstIsAlwaysLower(fullhouse, quads);
            firstIsAlwaysLower(fullhouse, straightFlush);
        }

        [TestMethod()]
        public void HoldemQuadsTest()
        {
            Assert.IsTrue(getTestValue("Ad As Ac Ah 2h") < getTestValue("Ad As Ac Ah 3h"));
            Assert.IsTrue(getTestValue("Ad As Ac Ah Kh") > getTestValue("2d 2s 2c 2h 3h"));
            firstIsAlwaysLower(quads, straightFlush);
        }

        [TestMethod()]
        public void HoldemStraightFlushTest()
        {
            Assert.IsTrue(getTestValue("As 2s 3s 4s 5s") < getTestValue("2s 3s 4s 5s 6s"));
            Assert.IsTrue(getTestValue("As 2s 3s 4s 5s Ad Ah") < getTestValue("2s 3s 4s 5s 6s 6h 6d"));
            Assert.IsTrue(getTestValue("3s 3d 3c 6s 6h 5c 4c") < getTestValue("3s 3d 5c 4c 3c 2c Ac"));
        }

        [TestMethod()]
        public void HoldemDebugTest()
        {
            Assert.IsTrue(getTestValue("Ac 5c 4c 4d 3d 3s 2c") > getTestValue("6s 6h 3d 3s 4d 5c 4c"));
        }
    }
}
