using System;
using NUnit.Framework;

namespace FunMath
{
    public class QuestionGeneratorTests
    {
        const int Samples = 5000;

        [TestCase(0, 0)]
        [TestCase(20, 0)]
        [TestCase(21, 1)]
        [TestCase(60, 2)]
        [TestCase(80, 3)]
        [TestCase(81, 4)]
        public void TierBoundaries(int count, int tier)
        {
            Assert.AreEqual(tier, QuestionGenerator.TierFor(count));
        }

        [Test]
        public void AnswersAreConsistentAndDistractorsDiffer()
        {
            var rnd = new Random(1);
            for (int level = 1; level <= 2; level++)
            {
                for (int count = 0; count <= 100; count++)
                {
                    for (int i = 0; i < Samples / 100; i++)
                    {
                        Question q = QuestionGenerator.Generate(rnd, level, count);
                        int expected = q.IsSubtraction ? q.Second - q.First : q.First + q.Second;
                        Assert.AreEqual(expected, q.Answer, q.Text);
                        Assert.AreNotEqual(q.Answer, q.Distractor, q.Text);
                        Assert.GreaterOrEqual(q.Answer, 0, q.Text);
                        Assert.GreaterOrEqual(q.Distractor, 0, q.Text);
                    }
                }
            }
        }

        [Test]
        public void LevelOneIsAdditionOnly()
        {
            var rnd = new Random(2);
            for (int i = 0; i < Samples; i++)
                Assert.IsFalse(QuestionGenerator.Generate(rnd, 1, i % 100).IsSubtraction);
        }

        [Test]
        public void LevelTwoProducesBothOperations()
        {
            var rnd = new Random(3);
            bool plus = false, minus = false;
            for (int i = 0; i < 200; i++)
            {
                Question q = QuestionGenerator.Generate(rnd, 2, 10);
                plus |= !q.IsSubtraction;
                minus |= q.IsSubtraction;
            }
            Assert.IsTrue(plus && minus);
        }

        [Test]
        public void SubtractionIsPositiveAndWithinTierLimit()
        {
            var rnd = new Random(4);
            int[] limits = { 10, 20, 30, 40, 40 };
            for (int count = 0; count <= 100; count++)
            {
                int tier = QuestionGenerator.TierFor(count);
                for (int i = 0; i < 100; i++)
                {
                    Question q = QuestionGenerator.Generate(rnd, 2, count);
                    if (!q.IsSubtraction) continue;
                    Assert.Greater(q.Answer, 0, q.Text);
                    Assert.LessOrEqual(q.Second, limits[tier], q.Text);
                }
            }
        }

        [Test]
        public void DoesNotRepeatPreviousQuestion()
        {
            var rnd = new Random(5);
            string previous = null;
            int repeats = 0;
            for (int i = 0; i < Samples; i++)
            {
                Question q = QuestionGenerator.Generate(rnd, 2, 5, previous);
                if (q.Text == previous) repeats++;
                previous = q.Text;
            }
            Assert.AreEqual(0, repeats);
        }
    }
}
