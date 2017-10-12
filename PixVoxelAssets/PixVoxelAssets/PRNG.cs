using System;

namespace AssetsPV
{
    public class PRNG : Random
    {
        public static PRNG r = new PRNG(0x1337BEEF);
        public static uint[] rState = r.GetSnapshot(), altState = new uint[] {1111U, 2222U, 3333U, 4444U, 5555U, 6666U, 7777U, 8888U, 9999U,
            10101010U, 11111111U, 12121212U, 13131313U, 14141414U, 15151515U, 16161616U, 17171717U};

        public static uint GlobalRandomState = 31337U; //Randomize((uint)(DateTime.Now.Ticks ^ DateTime.Now.Ticks >> 32))

        public uint choice = 0U;
        public uint[] state = new uint[16];

        public PRNG()
        {
            for(int i = 0; i < 16; i++)
            {
                
                choice += (state[i] = Randomize(GlobalRandomState += 0x7F4A7C15U));
            }
        }
        public PRNG(int seed)
        {
            uint seed2 = Randomize((uint)seed * 0x7F4A7C15U);
            for(int i = 0; i < 16; i++)
            {
                choice += (state[i] = Randomize(seed2 += 0x7F4A7C15U));
            }
        }

        public PRNG(int[] seed)
        {
            if(seed == null || seed.Length <= 0) seed = new int[1];
            uint sum = 191U;
            for(int s = 0; s < seed.Length; s++)
            {
                sum += Randomize((uint)seed[s]);
                for(int i = 0; i < 16; i++)
                {
                    choice += (state[i] ^= Randomize(sum += 0x7F4A7C15U));
                }
            }
        }
        public PRNG(uint[] stateSeed, uint choiceSeed)
        {
            if(stateSeed == null || stateSeed.Length == 0)
            {
                for(int i = 0; i < 16; i++)
                {
                    choice += (state[i] = Randomize(GlobalRandomState += 0x7F4A7C15U));
                }
            }
            else if(stateSeed.Length != 16)
            {
                uint sum = 191U;
                for(int s = 0; s < stateSeed.Length; s++)
                {
                    sum += Randomize(stateSeed[s]);
                    for(int i = 0; i < 16; i++)
                    {
                        choice += (state[i] ^= Randomize(sum += 0x7F4A7C15U));
                    }
                }

            }
            else
            {
                Buffer.BlockCopy(stateSeed, 0, state, 0, 64);
                choice = choiceSeed;
            }
        }


        /// <summary>
        /// Returns a pseudo-random long, which can be positive or negative and have any 64-bit value.
        /// </summary>
        /// <returns>any int, all 64 bits are pseudo-random</returns>

        public long NextLong()
        {
            return (state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B500000000L ^
            (state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5;
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
            long threshold = (0x7fffffffffffffffL - maxValue + 1) % maxValue;
            for(;;)
            {
                long bits = ((state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B500000000L ^
            (state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5) & 0x7fffffffffffffffL;
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
            return (int)((state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5);
        }
        /// <summary>
        /// Returns a positive pseudo-random int, which can have any 31-bit positive value.
        /// </summary>
        /// <returns>any random positive int, all but the sign bit are pseudo-random</returns>
        public override int Next()
        {
            return (int)((state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5);
        }
        /// <summary>
        /// Gets a pseudo-random int that is between 0 (inclusive) and maxValue (exclusive), which can be positive or negative.
        /// </summary>
        /// <remarks>Based on code by Daniel Lemire, http://lemire.me/blog/2016/06/27/a-fast-alternative-to-the-modulo-reduction/ </remarks>
        /// <param name="maxValue"></param>
        /// <returns>an int between 0 and maxValue</returns>
        public override int Next(int maxValue)
        {
            return (int)((maxValue * (((state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5) & 0x7FFFFFFFL)) >> 31);
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
            for(int i = 0; i < buffer.Length;)
            {
                uint r = ((state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5);
                for(int n = Math.Min(buffer.Length - i, 4); n-- > 0; r >>= 8)
                    buffer[i++] = (byte)r;
            }
        }
        /// <summary>
        /// Gets a random double between 0.0 (inclusive) and 1.0 (exclusive).
        /// </summary>
        /// <returns>a pseudo-random double between 0.0 inclusive and 1.0 exclusive</returns>
        public override double NextDouble()
        {
            return (((state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B500000000L ^
            (state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5) & 0x1FFFFFFFFFFFFFL) * 1.1102230246251565E-16;
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
            return (((state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B500000000L ^
            (state[(choice += 0x9CBC276DU) & 15] += (state[choice >> 28] >> 13) + 0x5F356495) * 0x2C9277B5) & 0x1FFFFFFFFFFFFFL) * 1.1102230246251565E-16;
        }
        /// <summary>
        /// Returns a new RNG using the same algorithm and a copy of the internal state this uses.
        /// Calling the same methods on this RNG and its copy should produce the same values.
        /// </summary>
        /// <returns>a copy of this RNG</returns>
        public PRNG Copy()
        {
            return new PRNG(state, choice);
        }
        /// <summary>
        /// Gets a snapshot of the current state as a uint array. This snapshot can be used to restore the current state.
        /// </summary>
        /// <returns>a snapshot of the current state as a uint array</returns>
        public uint[] GetSnapshot()
        {
            uint[] snap = new uint[17];
            Array.Copy(state, snap, 16);
            snap[16] = choice;
            return snap;
        }

        /// <summary>
        /// Restores the state this uses internally to the one stored in snapshot, a uint array.
        /// </summary>
        /// <param name="snapshot">a uint array normally produced by GetSnapshot() called on this PRNG</param>
        public void FromSnapshot(uint[] snapshot)
        {
            if(snapshot == null)
                throw new ArgumentNullException("snapshot");
            if(snapshot.Length < 17)
            {
                uint seed2 = Randomize((uint)snapshot.Length * 0x8D265FCDU);
                for(uint i = 0; i < 16U; i++)
                {
                    state[i] = Randomize(seed2 + i * 0x7F4A7C15U);
                }
                choice = Randomize(Randomize(seed2 - 0x7F4A7C15U));

            }
            else
            {
                Array.Copy(snapshot, state, 16);
                choice = snapshot[16];
            }
        }

        public void Reseed(uint seed)
        {
            seed = Randomize(seed * 0x7F4A7C15U);
            for(int i = 0; i < 16; i++)
            {
                choice += (state[i] = Randomize(seed += 0x7F4A7C15U));
            }
        }

        public void Reseed(int seed)
        {
            uint seed2 = Randomize((uint)seed * 0x7F4A7C15U);
            for(int i = 0; i < 16; i++)
            {
                choice += (state[i] = Randomize(seed2 += 0x7F4A7C15U));
            }
        }

        /// <summary>
        /// Returns a random permutation of state; if state is the same on two calls to this, this will return the same number.
        /// </summary>
        /// <remarks>
        /// This is not the same implementation used in the rest of this class' methods; it is used by some constructors.
        /// This is expected to be called with <code>Randomize(state += 0x7F4A7C15U)</code> to generate a sequence of random numbers, or with
        /// <code>Randomize(state -= 0x7F4A7C15U)</code> to go backwards in the same sequence. Using other constants for the increment does not
        /// guarantee quality in the same way; in particular, using <code>Randomize(++state)</code> yields poor results for quality, and other
        /// very small numbers will likely also be low-quality.
        /// </remarks>
        /// <param name="state">A UInt32 that should be different every time you want a different random result; use <code>Randomize(state += 0x7F4A7C15U)</code> ideally.</param>
        /// <returns>A pseudo-random permutation of state.</returns>
        public static uint Randomize(uint state)
        {
            state = (state ^ state >> 14) * (0x2C9277B5U + (state * 0x632BE5A6U));
            return state ^ state >> 13;
        }

    }
}
