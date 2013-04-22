/* From http://xmlunit.sourceforge.net/ Moved here because the original library is for testing, and 
 * it is tied to nunit, which we don't want to ship in production
 */

using System;
using System.Text;
using Chorus.merge.xml.generic.xmldiff;

namespace Chorus.merge.xml.generic.xmldiff
{
    public class DiffResult {
        private bool _identical = true;
        private bool _equal = true;
        private Difference _difference;
        private StringBuilder _stringBuilder;
    	
        public DiffResult() {
            _stringBuilder = new StringBuilder();
        }
        
        public bool Identical {
            get {
                return _identical;
            }
        }
        
        public bool Equal {
            get {
                return _equal;
            }
        }
        
        public Difference Difference {
            get {
                return _difference;
            }
        }
     
        public string StringValue {
            get {
                if (_stringBuilder.Length == 0) {
                    if (Identical) {
                        _stringBuilder.Append("Identical");        			
                    } else {
                        _stringBuilder.Append("Equal");
                    }
                }
                return _stringBuilder.ToString();
            }
        }
        
        public void DifferenceFound(Chorus.merge.xml.generic.xmldiff.XmlDiff inDiff, Difference difference) {
            _identical = false;
            if (difference.MajorDifference) {
                _equal = false;
            }       
            _difference = difference;
            if (_stringBuilder.Length == 0) {
                _stringBuilder.Append(inDiff.OptionalDescription);
            }
            _stringBuilder.Append(Environment.NewLine).Append(difference);
        }        
    }
}