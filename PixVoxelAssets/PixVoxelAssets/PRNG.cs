using System;

namespace AssetsPV
{
    public class PRNG : Random
    {
        public static PRNG r = new PRNG(0x1337BEEF);
        public static ulong rState = r.state, altState = 1337133713371337UL;

        public static ulong GlobalRandomState = 31337U; //Randomize((uint)(DateTime.Now.Ticks ^ DateTime.Now.Ticks >> 32))

        public ulong state = 0UL;
        public PRNG()
        {
            state = Randomize(GlobalRandomState += 0x6C8E9CF570932BD5UL);
        }
        public PRNG(int seed)
        {
            state = Randomize((ulong)seed * 0x6C8E9CF570932BD5UL);
        }

        public PRNG(ulong seed)
        {
            state = seed;
        }

        /// <summary>
        /// Returns a pseudo-random long, which can be positive or negative and have any 64-bit value.
        /// </summary>
        /// <returns>any int, all 64 bits are pseudo-random</returns>

        public long NextLong()
        {
            ulong z = (state += 0x6C8E9CF570932BD5UL);
            z = (z ^ (z >> 25)) * (z | 0xA529L);
            return (long)(z ^ (state >> 22));
        }
        /// <summary>
        /// Gets a pseudo-random int that is between 0 (inclusive) and maxValue (exclusive); maxValue must be
        /// positive (if it is 0 or less, this simply returns 0).
        /// </summary>
        /// <param name="maxValue">the exclusive upper bound, which should be 1 or greater</param>
        /// <returns>a pseudo-random long between 0 (inclusive) and maxValue (exclusive)</returns>

        public long NextLong(long maxValue)
        {
            if(maxValue <= 0) return 0;
            long threshold = (0x7fffffffffffffffL - maxValue + 1) % maxValue, bits;
            ulong z;
            for(;;)
            {
                z = (state += 0x6C8E9CF570932BD5UL);
                z = (z ^ (z >> 25)) * (z | 0xA529L);
                bits = (long)(z ^ (state >> 22)) & 0x7fffffffffffffffL;
                if(bits >= threshold)
                    return bits % maxValue;
            }
        }
        /// <summary>
        /// Gets a pseudo-random long that is between minValue (inclusive) and maxValue (exclusive);
        /// both should be positive and minValue should be less than maxValue.
        /// </summary>
        /// <param name="minValue">the lower bound as a long, inclusive</param>
        /// <param name="maxValue">the upper bound as a long, exclusive</param>
        /// <returns></returns>
        public long NextLong(long minValue, long maxValue)
        {
            return NextLong(maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Returns a pseudo-random int, which can be positive or negative and have any 32-bit value.
        /// </summary>
        /// <returns>any int, all 32 bits are pseudo-random</returns>
        public int NextInt()
        {

            ulong z = (state += 0x6C8E9CF570932BD5UL);
            z = (z ^ (z >> 25)) * (z | 0xA529L);
            return (int)(z ^ (state >> 22));
        }
        /// <summary>
        /// Returns a positive pseudo-random int, which can have any 31-bit positive value.
        /// </summary>
        /// <returns>any random positive int, all but the sign bit are pseudo-random</returns>
        public override int Next()
        {
            ulong z = (state += 0x6C8E9CF570932BD5UL);
            z = (z ^ (z >> 25)) * (z | 0xA529L);
            return (int)(z ^ (state >> 22)) & 0x7FFFFFFF;
        }
        /// <summary>
        /// Returns a pseudo-random int, which can have any value that uses at most the specified amount of bits.
        /// </summary>
        /// <param name="bits">the maximum number of bits to use for the result</param>
        /// <returns>an int between 0 and <code>Pow(2, bits)</code></returns>
        /// <returns>any random int using up to the specified amount of bits</returns>
        public int NextBits(int bits)
        {
            ulong z = (state += 0x6C8E9CF570932BD5UL);
            z = (z ^ (z >> 25)) * (z | 0xA529L);
            return (int)((z ^ (state >> 22)) >> (32 - bits));
        }
        /// <summary>
        /// Gets a pseudo-random int that is between 0 (inclusive) and maxValue (exclusive), which can be positive or negative.
        /// </summary>
        /// <remarks>Based on code by Daniel Lemire, http://lemire.me/blog/2016/06/27/a-fast-alternative-to-the-modulo-reduction/ </remarks>
        /// <param name="maxValue"></param>
        /// <returns>an int between 0 and maxValue</returns>
        public override int Next(int maxValue)
        {
            ulong z = (state += 0x6C8E9CF570932BD5UL);
            z = (z ^ (z >> 25)) * (z | 0xA529L);
            return (int)((maxValue * ((long)(z ^ (state >> 22)) & 0x7FFFFFFFL)) >> 31);
        }
        /// <summary>
        /// Gets a pseudo-random int that is between minValue (inclusive) and maxValue (exclusive); both can be positive or negative.
        /// </summary>
        /// <param name="minValue">the inner bound as an int, inclusive</param>
        /// <param name="maxValue">the outer bound as an int, exclusive</param>
        /// <returns>an int between minValue (inclusive) and maxValue (exclusive)</returns>
        public override int Next(int minValue, int maxValue)
        {
            return Next(maxValue - minValue) + minValue;
        }
        /// <summary>
        /// Fills buffer with random values, from its start to its end.
        /// </summary>
        /// <remarks>
        /// Based on reference code in the documentation for java.util.Random.
        /// </remarks>
        /// <param name="buffer">a non-null byte array that will be modified</param>
        public override void NextBytes(byte[] buffer)
        {
            if(buffer == null)
                throw new ArgumentNullException("buffer");
            ulong z;
            for(int i = 0; i < buffer.Length;)
            {
                z = (state += 0x6C8E9CF570932BD5UL);
                z = (z ^ (z >> 25)) * (z | 0xA529L);
                z ^= (state >> 22);
                for(int n = Math.Min(buffer.Length - i, 8); n-- > 0; z >>= 8)
                    buffer[i++] = (byte)z;
            }
        }
        /// <summary>
        /// Gets a random double between 0.0 (inclusive) and 1.0 (exclusive).
        /// </summary>
        /// <returns>a pseudo-random double between 0.0 inclusive and 1.0 exclusive</returns>
        public override double NextDouble()
        {
            ulong z = (state += 0x6C8E9CF570932BD5UL);
            z = (z ^ (z >> 25)) * (z | 0xA529L);
            return ((z ^ (state >> 22)) & 0x1FFFFFFFFFFFFFUL) * 1.1102230246251565E-16;
        }
        /// <summary>
        /// Gets a random double between 0.0 (inclusive) and 1.0 (exclusive).
        /// </summary>
        /// <remarks>
        /// The same code as NextDouble().
        /// </remarks>
        /// <returns>a pseudo-random double between 0.0 inclusive and 1.0 exclusive</returns>
        protected override double Sample()
        {
            ulong z = (state += 0x6C8E9CF570932BD5UL);
            z = (z ^ (z >> 25)) * (z | 0xA529L);
            return ((z ^ (state >> 22)) & 0x1FFFFFFFFFFFFFUL) * 1.1102230246251565E-16;
        }
        /// <summary>
        /// Returns a new RNG using the same algorithm and a copy of the internal state this uses.
        /// Calling the same methods on this RNG and its copy should produce the same values.
        /// </summary>
        /// <returns>a copy of this RNG</returns>
        public PRNG Copy()
        {
            return new PRNG(state);
        }
        /// <summary>
        /// Gets a snapshot of the current state as a uint array. This snapshot can be used to restore the current state.
        /// </summary>
        /// <returns>a snapshot of the current state as a uint array</returns>
        public ulong GetSnapshot()
        {
            return state;
        }

        /// <summary>
        /// Restores the state this uses internally to the one stored in snapshot, a uint array.
        /// </summary>
        /// <param name="snapshot">a uint array normally produced by GetSnapshot() called on this PRNG</param>
        public void FromSnapshot(ulong snapshot)
        {
            state = snapshot;
        }

        public void Reseed(ulong seed)
        {
            state = Randomize(seed * 0x6C8E9CF570932BD5UL);
        }

        public void Reseed(int seed)
        {
            state = Randomize((ulong)seed * 0x6C8E9CF570932BD5UL);
        }

        /// <summary>
        /// Returns a random permutation of state; if state is the same on two calls to this, this will return the same number.
        /// </summary>
        /// <remarks>
        /// This is not the same implementation used in the rest of this class' methods; it is used by some constructors.
        /// This is expected to be called with <code>Randomize(state += 0x6C8E9CF570932BD5UL)</code> to generate a sequence of random numbers, or with
        /// <code>Randomize(state -= 0x6C8E9CF570932BD5UL)</code> to go backwards in the same sequence. Using other constants for the increment does not
        /// guarantee quality in the same way; in particular, using <code>Randomize(++state)</code> yields poor results for quality, and other
        /// very small numbers will likely also be low-quality.
        /// </remarks>
        /// <param name="state">A UInt32 that should be different every time you want a different random result; use <code>Randomize(state += 0x6C8E9CF570932BD5UL)</code> ideally.</param>
        /// <returns>A pseudo-random permutation of state.</returns>
        public static ulong Randomize(ulong state)
        {
            return (state = (state ^ (state >> 25)) * (state | 0xA529L)) ^ (state >> 22);
            //state = (state ^ state >> 14) * (0x2C9277B5U + (state * 0x632BE5A6U));
            //return state ^ state >> 13;
        }

    }
}
