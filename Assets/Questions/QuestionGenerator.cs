using System;

namespace FunMath
{
    public struct Question
    {
        public int First;
        public int Second;
        public int Answer;
        public int Distractor;
        public bool IsSubtraction;
        public string Text;
    }

    public static class QuestionGenerator
    {
        // Operand range (inclusive) per tier; one tier = 20 questions.
        static readonly int[] PlusMin = { 1, 6, 11, 16, 16 };
        static readonly int[] PlusMax = { 5, 10, 15, 20, 20 };
        // Largest number used in subtraction per tier.
        static readonly int[] MinusMax = { 10, 20, 30, 40, 40 };
        // Wrong-answer range for addition per tier.
        static readonly int[] PlusDistractorMin = { 2, 11, 21, 31, 31 };
        static readonly int[] PlusDistractorMax = { 10, 20, 30, 40, 40 };

        public const int TierCount = 5;

        public static int TierFor(int questionCount)
        {
            if (questionCount <= 20) return 0;
            if (questionCount <= 40) return 1;
            if (questionCount <= 60) return 2;
            if (questionCount <= 80) return 3;
            return 4;
        }

        // level 1 = addition only, level 2 = addition and subtraction.
        public static Question Generate(Random rnd, int level, int questionCount, string previousText = null)
        {
            int tier = TierFor(questionCount);
            Question q = default;
            for (int attempt = 0; attempt < 8; attempt++)
            {
                bool subtraction = level == 2 && rnd.Next(2) == 1;
                q = subtraction ? Subtraction(rnd, tier) : Addition(rnd, tier);
                if (q.Text != previousText)
                    break;
            }
            return q;
        }

        static Question Addition(Random rnd, int tier)
        {
            int a = rnd.Next(PlusMin[tier], PlusMax[tier] + 1);
            int b = rnd.Next(PlusMin[tier], PlusMax[tier] + 1);
            int answer = a + b;
            int distractor;
            do
            {
                distractor = rnd.Next(PlusDistractorMin[tier], PlusDistractorMax[tier] + 1);
            } while (distractor == answer);

            return new Question
            {
                First = a,
                Second = b,
                Answer = answer,
                Distractor = distractor,
                Text = a + " + " + b + " ",
            };
        }

        static Question Subtraction(Random rnd, int tier)
        {
            int max = MinusMax[tier];
            int small = rnd.Next(0, max);
            int big = rnd.Next(small + 1, max + 1);
            int answer = big - small;

            // Wrong answers stay close to the right one so size alone does not give it away.
            int distractor;
            do
            {
                int offset = rnd.Next(1, 4);
                distractor = rnd.Next(2) == 0 ? answer + offset : answer - offset;
            } while (distractor < 0 || distractor == answer);

            return new Question
            {
                First = small,
                Second = big,
                Answer = answer,
                Distractor = distractor,
                IsSubtraction = true,
                Text = big + " - " + small + " ",
            };
        }
    }
}
