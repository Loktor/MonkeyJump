using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MonkeyJumpGameModel
{
    [DataContractAttribute]
    public class Options
    {
        /// <summary>
        /// Constructor used by the serializer
        /// </summary>
        public Options() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="backgroundMusic">defines if background music is enabled</param>
        /// <param name="gameplaySounds">defines if gamplay sound are enabled</param>
        public Options(bool backgroundMusic, bool gameplaySounds)
        {
            GamePlaySoundsEnabled = gameplaySounds;
            BackgroundMusicEnabled = backgroundMusic;
        }

        [DataMember]
        public Boolean GamePlaySoundsEnabled { get; set; }

        [DataMember]
        public Boolean BackgroundMusicEnabled { get; set; }

        [DataMember]
        public String LastTypedPlayerName { get; set; }
    }
}
