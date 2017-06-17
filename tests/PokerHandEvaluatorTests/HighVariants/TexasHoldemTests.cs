using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PokerHandEvaluator.HighVariants.Tests
{
    [TestClass()]
    public class TexasHoldemTests
    {
        ulong getTestValue(string hand)
        {
            ulong cards = Utils.StringToHandMask(hand);
            return TexasHoldem.GetHandValue(cards, (uint)Utils.BitCount(cards));
        }

        void firstIsAlwaysLower(List<KeyValuePair<int, string>> list1, List<KeyValuePair<int, string>> list2)
        {
            foreach (var item1 in list1)
            {
                foreach (var item2 in list2)
                {
                    if (item1.Key != item2.Key)
                        continue;
                    Assert.IsTrue(getTestValue(item1.Value) < getTestValue(item2.Value));
                }
            }
        }

        List<KeyValuePair<int, string>> straightFlush = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(5, "As 2s 3s 4s 5s"),
            new KeyValuePair<int, string>(5, "As Ks Qs Js Ts"),
            new KeyValuePair<int, string>(6, "As Ks Qs Js Ts 2d"),
            new KeyValuePair<int, string>(6, "As Ks Qs Js Ts 2s"),
            new KeyValuePair<int, string>(6, "As Ks Qs Js Ts Ad"),
            new KeyValuePair<int, string>(7, "As Ks Qs Js Ts Ad Ac"),
        };

        List<KeyValuePair<int, string>> quads = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(4, "As Ad Ah Ac"),
            new KeyValuePair<int, string>(4, "Ks Kd Kh Kc"),
            new KeyValuePair<int, string>(5, "As Ad Ah Ac 2d"),
            new KeyValuePair<int, string>(5, "As Ad Ah Ac Kd"),
            new KeyValuePair<int, string>(5, "Ks Kd Kh Kc 2d"),
            new KeyValuePair<int, string>(5, "Ks Kd Kh Kc Qd"),
            new KeyValuePair<int, string>(6, "Ks Kd Kh Kc Qd Qh"),
            new KeyValuePair<int, string>(7, "2s 2d 2h 2c Qd Qh Qs"),
            new KeyValuePair<int, string>(7, "2s 2d 2h 2c 4d 5h 6s"),
        };

        List<KeyValuePair<int, string>> fullhouse = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(5 , "As Ad Ah 2c 2d"),
            new KeyValuePair<int, string>(5 , "As Ad Ah Kc Kd"),
            new KeyValuePair<int, string>(5 , "Ks Kd Kh Qc Qd"),
            new KeyValuePair<int, string>(5 , "Ks Kd Kh 2c 2d"),
            new KeyValuePair<int, string>(6 , "As Ad Ah 2c 2d 2h"),
            new KeyValuePair<int, string>(6 , "As Ad Ah Kc Kd Kh"),
            new KeyValuePair<int, string>(6 , "Ks Kd Kh Qc Qd Qh"),
            new KeyValuePair<int, string>(6 , "Ks Kd Kh 2c 2d 2h"),
            new KeyValuePair<int, string>(7 , "As Ad Ah 5c 2c 2d 3h"),
            new KeyValuePair<int, string>(7 , "As Ad Ah 4c Kc Kd Kh"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kh 2c 2s 2d Qh"),
        };

        List<KeyValuePair<int, string>> flush = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(5, "As 2s 3s 4s 9s"),
            new KeyValuePair<int, string>(5, "As Ks Qs Js 3s"),
            new KeyValuePair<int, string>(6, "As Ks Qs Js 7s 2d"),
            new KeyValuePair<int, string>(6, "As Ks Qs Js 8s 2s"),
            new KeyValuePair<int, string>(6, "As Ks Qs Js 9s Ad"),
            new KeyValuePair<int, string>(7, "As Ks Qs Js 2s Ad Ac"),
        };

        List<KeyValuePair<int, string>> straight = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(5, "As 2d 3s 4s 5s"),
            new KeyValuePair<int, string>(5, "As Ks Qc Jc Ts"),
            new KeyValuePair<int, string>(6, "As Kh Qh Jc Ts 2d"),
            new KeyValuePair<int, string>(6, "Ad Ks Qd Js Td 2s"),
            new KeyValuePair<int, string>(6, "As Kd Qs Js Ts Ad"),
            new KeyValuePair<int, string>(7, "As Ks Qc Jh Th Ad Ac"),
        };

        List<KeyValuePair<int, string>> trips = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(3 , "As Ad Ah"),
            new KeyValuePair<int, string>(3 , "Ks Kd Kh"),
            new KeyValuePair<int, string>(4 , "As Ad Ah 2d"),
            new KeyValuePair<int, string>(4 , "As Ad Ah Kd"),
            new KeyValuePair<int, string>(4 , "Ks Kd Kh Qd"),
            new KeyValuePair<int, string>(4 , "Ks Kd Kh 2d"),
            new KeyValuePair<int, string>(5 , "As Ad Ah 2d 3d"),
            new KeyValuePair<int, string>(5 , "Ks Kd Kh 3d 2d"),
        };

        List<KeyValuePair<int, string>> twopair = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(4 , "As Ad 2h 2d"),
            new KeyValuePair<int, string>(4 , "As Ad Kh Kd"),
            new KeyValuePair<int, string>(5 , "As Ad 2h 2d 3d"),
            new KeyValuePair<int, string>(5 , "As Ad Kh Kd Qd"),
            new KeyValuePair<int, string>(6 , "As Ad 2h 2d 3d 3c"),
            new KeyValuePair<int, string>(4 , "Ks Kd Qh Qd"),
            new KeyValuePair<int, string>(5 , "Ks Kd Qh Qd Jd"),
            new KeyValuePair<int, string>(6 , "Ks Kd Qh Qd Jd Jc"),
            new KeyValuePair<int, string>(6 , "As Ad Kh Kd Qd Qc"),
            new KeyValuePair<int, string>(4 , "Ks Kd 2h 2d"),
            new KeyValuePair<int, string>(5 , "Ks Kd 2h 2d 3d"),
            new KeyValuePair<int, string>(6 , "Ks Kd 2h 2d 3d 3c"),
            new KeyValuePair<int, string>(7 , "Ks Kd 2h 2d 3d 3c Ah"),
        };


        List<KeyValuePair<int, string>> onepair = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(2 , "As Ad"),
            new KeyValuePair<int, string>(3 , "As Ad 2s"),
            new KeyValuePair<int, string>(4 , "As Ad 2s 3s"),
            new KeyValuePair<int, string>(5 , "As Ad 2s 3s 4s"),
            new KeyValuePair<int, string>(3 , "As Ad Ks"),
            new KeyValuePair<int, string>(4 , "As Ad Ks Qs"),
            new KeyValuePair<int, string>(5 , "As Ad Ks Qs Js"),
            new KeyValuePair<int, string>(2 , "Ks Kd"),
            new KeyValuePair<int, string>(3 , "Ks Kd Qs"),
            new KeyValuePair<int, string>(4 , "Ks Kd Qs Js"),
            new KeyValuePair<int, string>(5 , "Ks Kd Qs Js Ts"),
            new KeyValuePair<int, string>(3 , "Ks Kd 2s"),
            new KeyValuePair<int, string>(4 , "Ks Kd 2s 3s"),
            new KeyValuePair<int, string>(5 , "Ks Kd 2s 3s 4s"),

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
        }

        [TestMethod()]
        public void HoldemDebugTest()
        {
            Assert.IsTrue(getTestValue("Ad As 2h 2s") < getTestValue("Ad As Ah 2s"));
        }
    }
}
