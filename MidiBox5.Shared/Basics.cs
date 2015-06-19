using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;
namespace MidiBox5.Shared
{
    public class Octave
    {
        public int Index { get; private set; }

        Pitch[] pitches;
        public Pitch this[int position]
        {
            get
            {
                return pitches[position];
            }
        }
        public Pitch C { get { return pitches[0]; } }
        public Pitch Cs { get { return pitches[1]; } }
        public Pitch D { get { return pitches[2]; } }
        public Pitch Ds { get { return pitches[3]; } }
        public Pitch E { get { return pitches[4]; } }
        public Pitch F { get { return pitches[5]; } }
        public Pitch Fs { get { return pitches[6]; } }
        public Pitch G { get { return pitches[7]; } }
        public Pitch Gs { get { return pitches[8]; } }
        public Pitch A { get { return pitches[9]; } }
        public Pitch As { get { return pitches[10]; } }
        public Pitch B { get { return pitches[11]; } }

        public Octave(int octave)
        {
            Index = octave;

pitches = new Pitch[] {
       new Pitch() { Note = "C", Octave = this, Accidental = Accidental.Natural, PositionInOctave = 0 },
           new Pitch() { Note = "C", Octave = this, Accidental = Accidental.Sharp, PositionInOctave = 1 },
            new Pitch() { Note = "D", Octave = this, Accidental = Accidental.Natural, PositionInOctave = 2 },
             new Pitch() { Note = "D", Octave = this, Accidental = Accidental.Sharp, PositionInOctave = 3 },
            new Pitch() { Note = "E", Octave = this, Accidental = Accidental.Natural, PositionInOctave = 4 },
             new Pitch() { Note = "F", Octave = this, Accidental = Accidental.Natural, PositionInOctave = 5 },
            new Pitch() { Note = "F", Octave = this, Accidental = Accidental.Sharp, PositionInOctave = 6 },
            new Pitch() { Note = "G", Octave = this, Accidental = Accidental.Natural, PositionInOctave = 7 },
            new Pitch() { Note = "G", Octave = this, Accidental = Accidental.Sharp, PositionInOctave = 8 },
            new Pitch() { Note = "A", Octave = this, Accidental = Accidental.Natural, PositionInOctave = 9 },
            new Pitch() { Note = "A", Octave = this, Accidental = Accidental.Sharp, PositionInOctave = 10 },
             new Pitch() { Note = "B", Octave = this, Accidental = Accidental.Natural, PositionInOctave = 11 }};
        }
    }
    
    public static class Octaves
    {
        public static Octave At(int position)
        {
            return octaves[position];
        }
        public static Octave _0 { get { return octaves[0]; } }
        public static Octave _1 { get { return octaves[1]; } }
        public static Octave _2 { get { return octaves[2]; } }
        public static Octave _3 { get { return octaves[3]; } }
        public static Octave _4 { get { return octaves[4]; } }
        public static Octave _5 { get { return octaves[5]; } }
        public static Octave _6 { get { return octaves[6]; } }
        public static Octave _7 { get { return octaves[7]; } }
        static Octave[] octaves = new Octave[] { new Octave(0), new Octave(1), new Octave(2), new Octave(3), new Octave(4), new Octave(5), new Octave(6), new Octave(7) };
    }
    public enum Accidental
    {
        DoubleSharp=2,
        Sharp=1,
        Natural=0,
        Flat=-2,
        DoubleFlat=-2
    }
    public class Pitch
    {
        public Accidental Accidental{get;set;}
        public string Note { get; set; }
        public Octave Octave { get; set; }
        public int PositionInOctave { get; set; }
        public Pitch Transpose(int octavePostionDelta)
        {
            int positionDelta;
            int octsDelta = Math.DivRem(octavePostionDelta, 12, out positionDelta);
            return Octaves.At(Octave.Index+octsDelta)[positionDelta+(int)Accidental];
        }
        public string NoteAndAccidental
        {
            get
            {
                if (Accidental == Shared.Accidental.DoubleFlat)
                    return Note + "bb";
                if (Accidental == Shared.Accidental.DoubleSharp)
                    return Note + "##";
                if (Accidental == Shared.Accidental.Flat)
                    return Note + "b";
                if (Accidental == Shared.Accidental.Sharp)
                    return Note + "#";
                return Note;
            }
        }
    }
    public class Note : TimedElement
    {
        public Pitch Pitch {get;set;}
    }
    public abstract class TimedElement
    {
        public Duration Duration { get; set; }
        public Location Location { get; set; }
    }
    [Flags]
    public enum Duration : int
    {
        Whole = 1,
        Half = 2,
        Quarter = 4,
        Eighth = 8,
        Sixteenth = 16,
        d32nd = 32,
        d64th = 64,
        d128th = 128,
        EighthTuplet = 12
    };
    public class TimeSignature
    {
        public int NumberOfBeats { get; set; }
        public Duration LengthOfBeats { get; set; }
    }
    public class Location
    {
        public int Bar { get; set; }
        public float Beat { get; set; }
    }

    //These bits of theory are more for notation
    public class Key
    {
        public Key(int fifths)
        {
            Fifths = fifths;
        }
        public int Fifths { get; set; }
    }





    public class ScalePattern
    {
        #region Properties

        /// <summary>The name of the scale being described.</summary>
        public string Name { get { return name; } }
        public string ShortName { get; private set; }

        /// <summary>The ascent of the scale.</summary>
        /// <remarks>
        /// <para>The ascent is expressed as a series of integers, each giving a semitone
        /// distance above the tonic.  It must have at least two elements, start at zero (the
        /// tonic), be monotonically increasing, and stay below 12 (the next tonic above).</para>
        /// <para>The number of elements in the ascent tells us how many notes-per-octave in the
        /// scale.  For example, a heptatonic scale will always have seven elements in the ascent.
        /// </para>
        /// </remarks>
        public int[] Ascent { get { return ascent; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a scale pattern.
        /// </summary>
        /// <param name="name">The name of the scale pattern.</param>
        /// <param name="ascent">The ascending pattern of the scale.  See the <see cref="Ascent"/>
        /// property for a detailed description and requirements.  This parameter is copied.</param>
        /// <exception cref="ArgumentNullException">name or ascent is null.</exception>
        /// <exception cref="ArgumentException">ascent is invalid.</exception>
        public ScalePattern(string name,string shortName, int[] ascent)
        {
            if (name == null || shortName == null || ascent == null)
            {
                throw new ArgumentNullException();
            }
            // Make sure ascent is valid.
            if (!AscentIsValid(ascent))
            {
                throw new ArgumentException("ascent is invalid.");
            }
            this.name = string.Copy(name);
            ShortName = string.Copy(shortName);
            this.ascent = new int[ascent.Length];
            Array.Copy(ascent, this.ascent, ascent.Length);
        }

        #endregion

        #region Operators, Equality, Hash Codes

        /// <summary>
        /// ToString returns the pattern name.
        /// </summary>
        /// <returns>The pattern's name, such as "Major" or "Melodic Minor (ascending)".</returns>
        public override string ToString() { return name; }

        /// <summary>
        /// Equality operator does value equality.
        /// </summary>
        public static bool operator ==(ScalePattern a, ScalePattern b)
        {
            return System.Object.ReferenceEquals(a, b) || a.Equals(b);
        }

        /// <summary>
        /// Inequality operator does value inequality.
        /// </summary>
        public static bool operator !=(ScalePattern a, ScalePattern b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Value equality.
        /// </summary>
        public override bool Equals(System.Object obj)
        {
            ScalePattern other = obj as ScalePattern;
            if ((Object)other == null)
            {
                return false;
            }
            if (!this.name.Equals(other.name))
            {
                return false;
            }
            if (this.ascent.Length != other.ascent.Length)
            {
                return false;
            }
            for (int i = 0; i < this.ascent.Length; ++i)
            {
                if (this.ascent[i] != other.ascent[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Hash code.
        /// </summary>
        public override int GetHashCode()
        {
            // TODO
            return 0;
        }

        #endregion

        #region Private

        /// <summary>Returns true if ascent is valid.</summary>
        private bool AscentIsValid(int[] ascent)
        {
            // Make sure it is non-empty, starts at zero, and ends before 12.
            if (ascent.Length < 2 || ascent[0] != 0 || ascent[ascent.Length - 1] >= 12)
            {
                return false;
            }
            // Make sure it's monotonically increasing.
            for (int i = 1; i < ascent.Length; ++i)
            {
                if (ascent[i] <= ascent[i - 1])
                {
                    return false;
                }
            }
            return true;
        }

        private string name;
        private int[] ascent;

        #endregion
    }

    /// <summary>
    /// A scale based on a pattern and a tonic note.
    /// </summary>
    /// <remarks>
    /// <para>For our purposes, a scale is defined by a tonic and then the pattern that it uses to
    /// ascend up to the next tonic.  The tonic is described with a <see cref="Note"/> because it is
    /// not specific to any one octave.  The ascending pattern is provided by the
    /// <see cref="ScalePattern"/> class.
    /// </para>
    /// <para>This class comes with a collection of predefined patterns, such as
    /// <see cref="Major"/> and <see cref="Scale.HarmonicMinor"/>.</para>
    /// </remarks>
    public class Scale
    {
        #region Properties

        /// <summary>
        /// The scale's human-readable name, such as "G# Major" or "Eb Melodic Minor (ascending)".
        /// </summary>
        public string Name
        {
            get
            {
                return String.Format("{0} {1}", tonic, pattern);
            }
        }

        /// <summary>The tonic of this scale.</summary>
        public Pitch Tonic { get { return tonic; } }

        /// <summary>The pattern of this scale.</summary>
        public ScalePattern Pattern { get { return pattern; } }

        /// <summary>
        /// The sequence of notes in this scale.
        /// </summary>
        /// <remarks>
        /// <para>This sequence begins at the tonic and ascends, stopping before the next tonic.
        /// </para>
        /// </remarks>
        public Pitch[] PitchSequence { get { return pitchSequence; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a scale from its tonic and its pattern.
        /// </summary>
        /// <param name="tonic">The tonic note.</param>
        /// <param name="pattern">The scale pattern.</param>
        /// <exception cref="ArgumentNullException">tonic or pattern is null.</exception>
        public Scale(Pitch tonic, ScalePattern pattern)
        {
            if (tonic == null || pattern == null)
            {
                throw new ArgumentNullException();
            }
            this.tonic = tonic;
            this.pattern = pattern;
            this.positionInOctaveToSequenceIndex = new int[12];
            this.pitchSequence = new Pitch[pattern.Ascent.Length];
            int numAccidentals;
            Build(this.tonic, this.pattern, this.positionInOctaveToSequenceIndex, this.pitchSequence,
                out numAccidentals);
        }

        #endregion

        #region Scale/Pitch Interaction

        /// <summary>
        /// Returns true if pitch is in this scale.
        /// </summary>
        /// <param name="pitch">The pitch to test.</param>
        /// <returns>True if pitch is in this scale.</returns>
        public bool Contains(Pitch pitch)
        {
            return this.ScaleDegree(pitch) != -1;
        }

        /// <summary>
        /// Returns the scale degree of the given pitch in this scale.
        /// </summary>
        /// <param name="pitch">The pitch to test.</param>
        /// <returns>The scale degree of pitch in this scale, where 1 is the tonic.  Returns -1
        /// if pitch is not in this scale.</returns>
        public int ScaleDegree(Pitch pitch)
        {
            int result = this.positionInOctaveToSequenceIndex[pitch.PositionInOctave];
            return result == -1 ? -1 : result + 1;
        }

        #endregion

        #region Predefined Scale Patterns

        /// <summary>
        /// Pattern for Major scales.
        /// </summary>
        public static ScalePattern Major =
            new ScalePattern("Major","", new int[] { 0, 2, 4, 5, 7, 9, 11 });

        /// <summary>
        /// Pattern for Natural Minor scales.
        /// </summary>
        public static ScalePattern NaturalMinor =
            new ScalePattern("Natural Minor","m", new int[] { 0, 2, 3, 5, 7, 8, 10 });

        /// <summary>
        /// Pattern for Harmonic Minor scales.
        /// </summary>
        public static ScalePattern HarmonicMinor =
            new ScalePattern("Harmonic Minor","hm", new int[] { 0, 2, 3, 5, 7, 8, 11 });

        /// <summary>
        /// Pattern for Melodic Minor scale as it ascends.
        /// </summary>
        public static ScalePattern MelodicMinorAscending =
            new ScalePattern("Melodic Minor (ascending)","mma",
                  new int[] { 0, 2, 3, 5, 7, 9, 11 });

        /// <summary>
        /// Pattern for Melodic Minor scale as it descends.
        /// </summary>
        public static ScalePattern MelodicMinorDescending =
            new ScalePattern("Melodic Minor (descending)", "mmd",
                  new int[] { 0, 2, 3, 5, 7, 8, 10 });

        /// <summary>
        /// Pattern for Chromatic scales.
        /// </summary>
        public static ScalePattern Chromatic =
            new ScalePattern("Chromatic","",
                  new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });

        /// <summary>
        /// Array of all the built-in scale patterns.
        /// </summary>
        public static ScalePattern[] Patterns = new ScalePattern[]
        {
            Major,
            NaturalMinor,
            HarmonicMinor,
            MelodicMinorAscending,
            MelodicMinorDescending,
            Chromatic
        };

        #endregion

        #region Operators, Equality, Hash Codes

        /// <summary>
        /// ToString returns the scale's human-readable name.
        /// </summary>
        /// <returns>The scale's name, such as "G# Major" or "Eb Melodic Minor (ascending)".
        /// </returns>
        public override string ToString() { return Name; }

        public string ShortString
        { 
            get {
                return tonic.NoteAndAccidental + pattern.ShortName + " [" + String.Join(", ", PitchSequence.Select(p => p.NoteAndAccidental)) + "]";
            }
        }

        /// <summary>
        /// Equality operator does value equality because Scale is immutable.
        /// </summary>
        public static bool operator ==(Scale a, Scale b)
        {
            return System.Object.ReferenceEquals(a, b) || a.Equals(b);
        }

        /// <summary>
        /// Inequality operator does value inequality because Chord is immutable.
        /// </summary>
        public static bool operator !=(Scale a, Scale b)
        {
            return !(System.Object.ReferenceEquals(a, b) || a.Equals(b));
        }

        /// <summary>
        /// Value equality.
        /// </summary>
        public override bool Equals(System.Object obj)
        {
            Scale other = obj as Scale;
            if ((Object)other == null)
            {
                return false;
            }

            return base.Equals(obj) || (this.tonic == other.tonic && this.pattern == other.pattern);
        }

        /// <summary>
        /// Hash code.
        /// </summary>
        public override int GetHashCode()
        {
            return this.tonic.GetHashCode() + this.pattern.GetHashCode();
        }

        #endregion

        #region Private

        /// <summary>
        /// Builds a scale.
        /// </summary>
        /// <param name="tonic">The tonic.</param>
        /// <param name="pattern">The scale pattern.</param>
        /// <param name="positionInOctaveToSequenceIndex">Must have 12 elements, and is filled with
        /// the 0-indexed scale position (or -1) for each position in the octave.</param>
        /// <param name="noteSequence">Must have pattern.Ascent.Length elements, and is filled with
        /// the notes for each scale degree.</param>
        /// <param name="numAccidentals">Filled with the total number of accidentals in the built
        /// scale.</param>
        private static void Build(Pitch tonic, ScalePattern pattern,
            int[] positionInOctaveToSequenceIndex, Pitch[] pitchSequence, out int numAccidentals)
        {
            numAccidentals = 0;
            for (int i = 0; i < 12; ++i)
            {
                positionInOctaveToSequenceIndex[i] = -1;
            }
            Pitch tonicPitch = tonic;
            for (int i = 0; i < pattern.Ascent.Length; ++i)
            {
                Pitch pitch = tonicPitch.Transpose(pattern.Ascent[i]);
                pitchSequence[i] = pitch;
                positionInOctaveToSequenceIndex[pitch.PositionInOctave] = i;
            }
        }

        private Pitch tonic;
        private ScalePattern pattern;
        private int[] positionInOctaveToSequenceIndex; // for each PositionInOctave, the 0-indexed
        // position of that pitch in noteSequence,
        // or -1 if it's not in the scale.
        private Pitch[] pitchSequence; // the note sequence of the scale.

        #endregion
    }
}
