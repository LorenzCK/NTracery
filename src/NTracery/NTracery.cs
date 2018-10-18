using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NTracery.Modifiers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NTracery {

    /// <summary>
    /// Represents a Tracery text expansion grammar.
    /// </summary>
    public class NTracery {

        public NTracery() {
            _modifiers = new Dictionary<string, IModifier> {
                { "capitalize", new Capitalize() },
                { "inQuotes", new InQuotes() }
            };
        }

        private Dictionary<string, IModifier> _modifiers;

        /// <summary>
        /// Gets a map of active modifiers.
        /// </summary>
        public IDictionary<string, IModifier> Modifiers {
            get {
                return _modifiers;
            }
        }

        /// <summary>
        /// Adds a modifier to the Tracery engine.
        /// </summary>
        /// <param name="keyword">Keyword that invokes the modifier.</param>
        /// <param name="modifier">Modifier to invoke when the keyword is encountered in an expansion.</param>
        public void AddModifier(string keyword, IModifier modifier) {
            _modifiers.Add(keyword, modifier);
        }

        private Dictionary<string, string[]> _grammar;

        /// <summary>
        /// Loads a grammar definition from a JSON string.
        /// </summary>
        /// <param name="json">JSON string representing a Tracery grammar.</param>
        public void LoadGrammar(string json) {
            _grammar = new Dictionary<string, string[]>();

            var jGrammar = JObject.Parse(json);
            foreach(var p in jGrammar.Properties()) {
                string propName = p.Name;
                if (p.Value.Type == JTokenType.String) {
                    _grammar[propName] = new string[] { (string)p.Value };
                }
                else if (p.Value.Type == JTokenType.Array) {
                    _grammar[propName] = (from e in ((JArray)p.Value).Values()
                                          let eType = e.Type
                                          where eType == JTokenType.String
                                          select (string)e).ToArray();
                }
                else {
                    throw new ArgumentException("Grammar rules must be strings or string arrays");
                }
            }
        }

        /// <summary>
        /// Loads a grammar definition from a stream.
        /// </summary>
        /// <param name="json">Stream containing JSON representing a Tracery grammar.</param>
        public void LoadGrammar(Stream stream) {
            using (var sr = new StreamReader(stream)) {
                LoadGrammar(sr.ReadToEnd());
            }
        }

    }

}
