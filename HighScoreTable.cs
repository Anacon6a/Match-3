using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using SFML.Graphics;

namespace Match_3
{
	[DataContract]
	class HighScoreTable
	{
		private string _name;
		private int _score, _element;

		[DataMember]
		public int Element
		{
			get { return _element; }
			set { _element = value; }
		}
		[DataMember]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		[DataMember]
		public int Score
		{
			get { return _score; }
			set { _score = value; }
		}
		public HighScoreTable(string name, int score, int element)
		{
			Name = name;
			Score = score;
			Element = element;

		}

	}
}
