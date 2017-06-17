using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PokerHandEvaluator.LowVariants.Tests
{
    [TestClass()]
    public class RazzTests
    {
        ulong getTestValue(string hand)
        {
            ulong cards = Utils.StringToHandMask(hand);
            cards = Razz.ShiftAceToBottom(cards);
            return Razz.GetHandValue(cards, (uint)Utils.BitCount(cards));
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


        List<KeyValuePair<int, string>> quads = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(4, "As Ad Ah Ac"),
            new KeyValuePair<int, string>(4, "Ks Kd Kh Kc"),
            new KeyValuePair<int, string>(5, "As Ad Ah Ac 2d"),
            new KeyValuePair<int, string>(5, "As Ad Ah Ac Kd"),
            new KeyValuePair<int, string>(5, "Ks Kd Kh Kc 2d"),
            new KeyValuePair<int, string>(5, "Ks Kd Kh Kc Qd"),
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
            new KeyValuePair<int, string>(7 , "As Ad Ah Ac 2c 2d 2h"),
            new KeyValuePair<int, string>(7 , "As Ad Ah Ac Kc Kd Kh"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kh Kc Qc Qd Qh"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kh Kc 2c 2d 2h")
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
            new KeyValuePair<int, string>(5 , "As Ad Ah Kd Qd"),
            new KeyValuePair<int, string>(5 , "Ks Kd Kh Qd Jd"),
            new KeyValuePair<int, string>(5 , "Ks Kd Kh 3d 2d"),
            new KeyValuePair<int, string>(6 , "As Ad Ah Ac 2d 3d"),
            new KeyValuePair<int, string>(6 , "As Ad Ah Ac Kd Qd"),
            new KeyValuePair<int, string>(6 , "Ks Kd Kh Kc Qd Jd"),
            new KeyValuePair<int, string>(6 , "Ks Kd Kh Kc 2d 3d"),
        };

        List<KeyValuePair<int, string>> twopair = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(4 , "As Ad 2h 2d"),
            new KeyValuePair<int, string>(4 , "As Ad Kh Kd"),
            new KeyValuePair<int, string>(5 , "As Ad 2h 2d 3d"),
            new KeyValuePair<int, string>(5 , "As Ad Kh Kd Qd"),
            new KeyValuePair<int, string>(6 , "As Ad 2h 2d 3d 3c"),
            new KeyValuePair<int, string>(7 , "As Ad Ac 2h 2d 3d 3c"),
            new KeyValuePair<int, string>(7 , "As Ad Ac 2h 2d 2s 3c"),
            new KeyValuePair<int, string>(7 , "As Ad Ac Ah 2d 2s 3c"),
            new KeyValuePair<int, string>(4 , "Ks Kd Qh Qd"),
            new KeyValuePair<int, string>(5 , "Ks Kd Qh Qd Jd"),
            new KeyValuePair<int, string>(6 , "Ks Kd Qh Qd Jd Jc"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc Qh Qd Jd Jc"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc Qh Qd Qs Jc"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc Kh Qd Qs Jc"),
            new KeyValuePair<int, string>(6 , "As Ad Kh Kd Qd Qc"),
            new KeyValuePair<int, string>(7 , "As Ad Ac Kh Kd Qd Qc"),
            new KeyValuePair<int, string>(7 , "As Ad Ac Kh Kd Ks Qc"),
            new KeyValuePair<int, string>(7 , "As Ad Ac Ah Kd Ks Qc"),
            new KeyValuePair<int, string>(4 , "Ks Kd 2h 2d"),
            new KeyValuePair<int, string>(5 , "Ks Kd 2h 2d 3d"),
            new KeyValuePair<int, string>(6 , "Ks Kd 2h 2d 3d 3c"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc 2h 2d 3d 3c"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc 2h 2d 2s 3c"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc Kh 2d 2s 3c")
        };


        List<KeyValuePair<int, string>> onepair = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(2 , "As Ad"),
            new KeyValuePair<int, string>(3 , "As Ad 2s"),
            new KeyValuePair<int, string>(4 , "As Ad 2s 3s"),
            new KeyValuePair<int, string>(5 , "As Ad 2s 3s 4s"),
            new KeyValuePair<int, string>(6 , "As Ad 2s 2d 3s 4s"),
            new KeyValuePair<int, string>(6 , "As Ad Ac 2d 3s 4s"),
            new KeyValuePair<int, string>(7 , "As Ad 2s 2c 3d 3s 4s"),
            new KeyValuePair<int, string>(7 , "As Ad Ac 2c 2d 3s 4s"),
            new KeyValuePair<int, string>(7 , "As Ad Ac Ah 2d 3s 4s"),
            new KeyValuePair<int, string>(3 , "As Ad Ks"),
            new KeyValuePair<int, string>(4 , "As Ad Ks Qs"),
            new KeyValuePair<int, string>(5 , "As Ad Ks Qs Js"),
            new KeyValuePair<int, string>(6 , "As Ad Ks Kd Qs Js"),
            new KeyValuePair<int, string>(6 , "As Ad Ac Kd Qs Js"),
            new KeyValuePair<int, string>(7 , "As Ad Ks Kc Qd Qs Js"),
            new KeyValuePair<int, string>(7 , "As Ad Ac Kc Kd Qs Js"),
            new KeyValuePair<int, string>(7 , "As Ad Ac Ah Kd Qs Js"),
            new KeyValuePair<int, string>(2 , "Ks Kd"),
            new KeyValuePair<int, string>(3 , "Ks Kd Qs"),
            new KeyValuePair<int, string>(4 , "Ks Kd Qs Js"),
            new KeyValuePair<int, string>(5 , "Ks Kd Qs Js Ts"),
            new KeyValuePair<int, string>(6 , "Ks Kd Qs Qd Js Ts"),
            new KeyValuePair<int, string>(6 , "Ks Kd Kc Qd Js Ts"),
            new KeyValuePair<int, string>(7 , "Ks Kd Qs Qc Jd Js Ts"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc Qc Qd Js Ts"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc Kh Qd Js Ts"),
            new KeyValuePair<int, string>(3 , "Ks Kd 2s"),
            new KeyValuePair<int, string>(4 , "Ks Kd 2s 3s"),
            new KeyValuePair<int, string>(5 , "Ks Kd 2s 3s 4s"),
            new KeyValuePair<int, string>(6 , "Ks Kd 2s 2d 3s 4s"),
            new KeyValuePair<int, string>(6 , "Ks Kd Kc 2d 3s 4s"),
            new KeyValuePair<int, string>(7 , "Ks Kd 2s 2c 3d 3s 4s"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc 2c 2d 3s 4s"),
            new KeyValuePair<int, string>(7 , "Ks Kd Kc Kh 2d 3s 4s")
        };

        List<KeyValuePair<int, string>> wheel = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(5 , "As 2s 3s 4s 5s"),
            new KeyValuePair<int, string>(5 , "9s Ts Js Qs Ks"),
            new KeyValuePair<int, string>(6 , "As 2s 3s 4s 5s 6s"),
            new KeyValuePair<int, string>(7 , "As 2s 3s 4s 5s 6s 7s")
        };


        [TestMethod()]
        public void AceCountAsLowTest()
        {
            Assert.AreEqual(getTestValue("As"), getTestValue("As"));
            Assert.AreEqual(getTestValue("Ad"), getTestValue("As"));
            Assert.AreEqual(getTestValue("Ac"), getTestValue("As"));
            Assert.AreEqual(getTestValue("Ah"), getTestValue("As"));

            Assert.IsTrue(getTestValue("Ah") < getTestValue("2s"));
            Assert.IsTrue(getTestValue("Ah") < getTestValue("3s"));
            Assert.IsTrue(getTestValue("Ah") < getTestValue("Qs"));
            Assert.IsTrue(getTestValue("Ah") < getTestValue("Ks"));
        }

        [TestMethod()]
        public void RazzSimpleLowHandsTest()
        {
            Assert.AreEqual(getTestValue("As"), getTestValue("Ad"));
            Assert.AreEqual(getTestValue("As 2s"), getTestValue("Ad 2d"));
            Assert.AreEqual(getTestValue("As 2s 3s"), getTestValue("Ad 2d 3d"));
            Assert.AreEqual(getTestValue("As 2s 3s 4s"), getTestValue("Ad 2d 3d 4d"));
            Assert.AreEqual(getTestValue("As 2s 3s 4s 5s"), getTestValue("Ad 2d 3d 4d 5d"));
            Assert.AreEqual(getTestValue("As 2s 3s 4s 5s 6s"), getTestValue("Ad 2d 3d 4d 5d 6d"));
            Assert.AreEqual(getTestValue("As 2s 3s 4s 5s 6s 7s"), getTestValue("Ad 2d 3d 4d 5d 6d 7d"));

            Assert.IsTrue(getTestValue("As") < getTestValue("2d"));
            Assert.IsTrue(getTestValue("As 2s") < getTestValue("Ad 3d"));
            Assert.IsTrue(getTestValue("As 2s 3s") < getTestValue("Ad 2d 4d"));
            Assert.IsTrue(getTestValue("As 2s 3s 4s") < getTestValue("Ad 2d 3d 5d"));
            Assert.IsTrue(getTestValue("As 2s 3s 4s 5s") < getTestValue("Ad 2d 3d 4d 6d"));
            Assert.IsTrue(getTestValue("As 2s 3s 4s 5s 6s") == getTestValue("Ad 2d 3d 4d 5d 7d"));
            Assert.IsTrue(getTestValue("As 2s 3s 4s 5s 6s 7s") == getTestValue("Ad 2d 3d 4d 5d 6d 8d"));
            Assert.IsTrue(getTestValue("As 2s 3s 4s 5s Ks Qs") < getTestValue("Ad 2d 3d 4d 6d 7d 8d"));

            firstIsAlwaysLower(wheel, onepair);
            firstIsAlwaysLower(wheel, twopair);
            firstIsAlwaysLower(wheel, trips);
            firstIsAlwaysLower(wheel, fullhouse);
            firstIsAlwaysLower(wheel, quads);
        }


        [TestMethod()]
        public void RazzOnePairHandsTest()
        {
            Assert.IsTrue(getTestValue("Td Js 9c Qd Kd") < getTestValue("Ad As Ks 2d 3d"));
            Assert.IsTrue(getTestValue("8d 8s 9c Qd Kd") < getTestValue("Ad 2s 3s 9d 9h"));
            firstIsAlwaysLower(onepair, twopair);
            firstIsAlwaysLower(onepair, trips);
            firstIsAlwaysLower(onepair, fullhouse);
            firstIsAlwaysLower(onepair, quads);
        }

        [TestMethod()]
        public void RazzTwoPairHandsTest()
        {
            Assert.IsTrue(getTestValue("Jd Js Qc Qd") < getTestValue("2d 2s Ks Kd"));
            firstIsAlwaysLower(twopair, trips);
            firstIsAlwaysLower(twopair, fullhouse);
            firstIsAlwaysLower(twopair, quads);
        }

        [TestMethod()]
        public void RazzTripsHandsTest()
        {
            Assert.IsTrue(getTestValue("Ad As Ac Kd Qh") < getTestValue("2d 2s 2c 3d 4h"));
            firstIsAlwaysLower(trips, quads);
            firstIsAlwaysLower(trips, fullhouse);
        }

        [TestMethod()]
        public void RazzFullHouseTest()
        {
            Assert.IsTrue(getTestValue("Ad As Ac 2d 2h") < getTestValue("Ad As Ac 3d 3h"));
            Assert.IsTrue(getTestValue("Ad As Ac 2d 2h") < getTestValue("Ah As 2c 2d 2h"));
            Assert.IsTrue(getTestValue("Ad As Ac Ah 2d 2h 2c") < getTestValue("Ad As Ac Ah 3d 3h 3c"));
            Assert.IsTrue(getTestValue("Ad As Ac Kd Kh") < getTestValue("Ah As 2c 2d 2h"));
            firstIsAlwaysLower(fullhouse, quads);
        }

        [TestMethod()]
        public void RazzQuadHandsTest()
        {
            Assert.IsTrue(getTestValue("Ad As Ac Ah 2h") < getTestValue("Ad As Ac Ah 3h"));
            Assert.IsTrue(getTestValue("Ad As Ac Ah Kh") < getTestValue("2d 2s 2c 2h 3h"));
        }

        [TestMethod()]
        public void RazzDebugTest()
        {
            //var val1 = getTestValue("Ad As Ac Ah Kh");
            //var val2 = getTestValue("2d 2s 2c 2h 3h");
            Assert.IsTrue(getTestValue("Ad As Ac Kd Qh") < getTestValue("2d 2s 2c 3d 4h"));
        }
    }
}