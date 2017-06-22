using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PokerHandEvaluator.Razz.Tests
{
    [TestClass()]
    public class RazzTests
    {
        ulong getTestValue(string hand, int cardCount)
        {
            ulong cards = hand.ToCardMask();
            cards = Utils.ShiftAceToBottom(cards);
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
                    if (n1 != n2)
                        continue;
                    if (n1 != 7) // only 7 card hands for now
                        continue;
                    Assert.IsTrue(getTestValue(item1, n1) < getTestValue(item2, n2));
                }
            }
        }


        List<string> quads = new List<string>
        {
            "As Ad Ah Ac",
            "Ks Kd Kh Kc",
            "As Ad Ah Ac 2d",
            "As Ad Ah Ac Kd",
            "Ks Kd Kh Kc 2d",
            "Ks Kd Kh Kc Qd",
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
            "As Ad Ah Ac 2c 2d",
            "As Ad Ah Ac 2c 2d 2h",
            "As Ad Ah Ac Kc Kd Kh",
            "Ks Kd Kh Kc Qc Qd Qh",
            "Ks Kd Kh Kc 2c 2d 2h"
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
            "As Ad Ah Kd Qd",
            "Ks Kd Kh Qd Jd",
            "Ks Kd Kh 3d 2d",
            "As Ad Ah Ac 2d 3d",
            "As Ad Ah Ac Kd Qd",
            "Ks Kd Kh Kc Qd Jd",
            "Ks Kd Kh Kc 2d 3d",
        };

        List<string> twopair = new List<string>
        {
            "As Ad 2h 2d",
            "As Ad Kh Kd",
            "As Ad 2h 2d 3d",
            "As Ad Kh Kd Qd",
            "As Ad 2h 2d 3d 3c",
            "As Ad Ac 2h 2d 3d 3c",
            "As Ad Ac 2h 2d 2s 3c",
            "As Ad Ac Ah 2d 2s 3c",
            "Ks Kd Qh Qd",
            "Ks Kd Qh Qd Jd",
            "Ks Kd Qh Qd Jd Jc",
            "Ks Kd Kc Qh Qd Jd Jc",
            "Ks Kd Kc Qh Qd Qs Jc",
            "Ks Kd Kc Kh Qd Qs Jc",
            "As Ad Kh Kd Qd Qc",
            "As Ad Ac Kh Kd Qd Qc",
            "As Ad Ac Kh Kd Ks Qc",
            "As Ad Ac Ah Kd Ks Qc",
            "Ks Kd 2h 2d",
            "Ks Kd 2h 2d 3d",
            "Ks Kd 2h 2d 3d 3c",
            "Ks Kd Kc 2h 2d 3d 3c",
            "Ks Kd Kc 2h 2d 2s 3c",
            "Ks Kd Kc Kh 2d 2s 3c",
            "Ks Kd Kc 3h 3d 2s 2c",
            "Ks Kd Kc 3h 3d 2s",
            "Ks Kd 2d 2s 3c 3h",
            "Ks Kd Kc 2c 2d 2s 3c",
            "Ks Kd Kc Ks 2d 2s 3c",
        };


        List<string> onepair = new List<string>
        {
            "As Ad",
            "As Ad 2s",
            "As Ad 2s 3s",
            "As Ad 2s 3s 4s",
            "As Ad 2s 2d 3s 4s",
            "As Ad Ac 2d 3s 4s",
            "As Ad 2s 2c 3d 3s 4s",
            "As Ad Ac 2c 2d 3s 4s",
            "As Ad Ac Ah 2d 3s 4s",
            "As Ad Ks",
            "As Ad Ks Qs",
            "As Ad Ks Qs Js",
            "As Ad Ks Kd Qs Js",
            "As Ad Ac Kd Qs Js",
            "As Ad Ks Kc Qd Qs Js",
            "As Ad Ac Kc Kd Qs Js",
            "As Ad Ac Ah Kd Qs Js",
            "Ks Kd",
            "Ks Kd Qs",
            "Ks Kd Qs Js",
            "Ks Kd Qs Js Ts",
            "Ks Kd Qs Qd Js Ts",
            "Ks Kd Kc Qd Js Ts",
            "Ks Kd Qs Qc Jd Js Ts",
            "Ks Kd Kc Qc Qd Js Ts",
            "Ks Kd Kc Kh Qd Js Ts",
            "Ks Kd 2s",
            "Ks Kd 2s 3s",
            "Ks Kd 2s 3s 4s",
            "Ks Kd 2s 2d 3s 4s",
            "Ks Kd Kc 2d 3s 4s",
            "Ks Kd 2s 2c 3d 3s 4s",
            "Ks Kd Kc 2c 2d 3s 4s",
            "Ks Kd Kc Kh 2d 3s 4s",
            "Ks Kd 2d 2s 3s 3d 4s",
        };

        List<string> wheel = new List<string>
        {
            "As 2s 3s 4s 5s",
            "9s Ts Js Qs Ks",
            "As 2s 3s 4s 5s 6s",
            "As 2s 3s 4s 5s 6s 7s"
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
        public void RazzAceShiftTest()
        {
            Assert.IsTrue(Utils.ShiftAceToBottom(4096) == 1); // A
            Assert.IsTrue(Utils.ShiftAceToBottom(4097) == 3); // A2
            Assert.IsTrue(Utils.ShiftAceToBottom(2048) == 4096); // K
            Assert.IsTrue(Utils.ShiftAceToBottom(0x8004002001000) == 0x8004002001); // All Aces

            Assert.IsTrue(Utils.ShiftAceToTop(1) == 4096); // A
            Assert.IsTrue(Utils.ShiftAceToTop(3) == 4097); // A2
            Assert.IsTrue(Utils.ShiftAceToTop(4096) == 2048); // K
            Assert.IsTrue(Utils.ShiftAceToTop(0x8004002001) == 0x8004002001000); // All Aces
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
            var val1 = getTestValue("Ad 2c 3c 5c 6s 7c Kc");
            var val2 = getTestValue("2s 3h 5c 6d 7h 7s Kd");
            Assert.IsTrue(val1 < val2);
        }
    }
}