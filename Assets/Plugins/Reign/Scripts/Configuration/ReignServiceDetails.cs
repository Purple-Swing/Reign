using System.Collections.ObjectModel;

namespace Reign.Generic
{
    public sealed class ReignServiceDetails
    {
        // ---------------------------------------------------------------------------------------------------- //
        //                                                                                                      //
        //   DO NOT EDIT THESE FIELDS, THEY ARE STRICTLY USED TO CATEGORISE REIGN'S CURRENT DEVELOPMENT STATE.  //
        //                                                                                                      //
        // ---------------------------------------------------------------------------------------------------- //

        public const string REIGN_VERSION = "1.0.3.3";                                  // Version
        public const string RELEASE_DATE = "2026-07-17";                                // YYYY-MM-DD Format
        public static readonly ReadOnlyCollection<string> CONTRIBUTORS = new            // Contributors to Reign since conception
        (
            new[]
            {
                "Tureen"
            }
        );

        public const string REIGN_TOOLS_VERSION = "1.1.0";                              // Version of Reign Tools (Reign/Reign Tools in the editor)
    }
}