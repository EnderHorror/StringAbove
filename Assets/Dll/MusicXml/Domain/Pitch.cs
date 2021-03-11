using System;

namespace MusicXml.Domain
{
	public class Pitch
	{
		internal Pitch()
		{
			Alter = 0;
			Octave = 0;
			Step = string.Empty;
		}

		public Pitch(int octave,string step)
		{
			Octave = octave;
			Step = step;
		}

		public int Alter { get; internal set; }

		public int Octave { get; internal set; }

		public string Step { get; internal set; }
		public override string ToString()
		{
			return $"Alter:{Alter} Octave{Octave} Step{Step}";
		}
	}
}