using System.Collections.Generic;

namespace Reign.Generic
{
    public sealed class ReignServiceDetails
    {
        // ---------------------------------------------------------------------------------------------------- //
        //                                                                                                      //
        //   DO NOT EDIT THESE FIELDS, THEY ARE STRICTLY USED TO CATEGORISE REIGN'S CURRENT DEVELOPMENT STATE.  //
        //                                                                                                      //
        // ---------------------------------------------------------------------------------------------------- //

        public const string REIGN_VERSION = "1.0.4.1";                                  // Version
        public const string RELEASE_DATE = "2026-07-30";                                // YYYY-MM-DD Format
        public static readonly Dictionary<string, string> CONTRIBUTORS = new()
        {
            { "Tureen", "[Reign Lead] Programmer, artist, documentation, etc." }
        };

        public const string REIGN_TOOLS_VERSION = "1.1.0";                              // Version of Reign Tools (Reign/Reign Tools in the editor)
    }
}