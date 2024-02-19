using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplilerA
{
    internal class Keyword
    {
        /*
         * fully reserved keywords
         * and array as auto begin case class const ?constructor destructor? div do downto else end event except ?extensionmethod? file
         * ?finalization? finally for foreach function goto if ?implementation? in ?inherited? ?initialization? ?interface? is label lock
         * loop mod nil not of operator or procedure program property raise record repeat sealed set sequence shl shr sizeof template then
         * to try type typeof until uses using var where while with xor
         * 
         * 
         *          
            Shortint
            Smallint
            Integer
            Longint
            Int64
            Byte
            Word
            Longword 
            Cardinal
            Uint64
            Real
            Double
            Single
            Boolean
            Char
         */
        static Dictionary<int, Dictionary<string, int>> keywords = new Dictionary<int, Dictionary<string, int>>();
        public Dictionary<int, Dictionary<string, int>> Keywords()
        {
            return keywords;
        }
        public Keyword() 
        {
            keywords[1] = new Dictionary<string, int>();
            Dictionary<string, int> tmp = new Dictionary<string, int>
            {
                //before 25 is for symbols
                ["do"] = Lexer.doKeyword,
                ["if"] = Lexer.ifKeyword,
                ["in"] = Lexer.inKeyword,
                ["of"] = Lexer.ofKeyword,
                ["or"] = Lexer.orKeyword,
                ["to"] = Lexer.toKeyword,
                ["as"] = Lexer.asKeyword,
                ["is"] = Lexer.isKeyword
            };
            keywords[2] = tmp;
            tmp = new Dictionary<string, int>
            {
                ["end"] = Lexer.endKeyword,
                ["try"] = Lexer.tryKeyword,
                ["var"] = Lexer.varKeyword,
                ["div"] = Lexer.divKeyword,
                ["and"] = Lexer.andKeyword,
                ["not"] = Lexer.notKeyword,
                ["for"] = Lexer.forKeyword,
                ["mod"] = Lexer.modKeyword,
                ["nil"] = Lexer.nilKeyword,
                ["set"] = Lexer.setKeyword,
                ["shl"] = Lexer.shlKeyword,
                ["shr"] = Lexer.shrKeyword,
                ["xor"] = Lexer.xorKeyword
            };
            keywords[3] = tmp;
            tmp = new Dictionary<string, int>
            {
                ["real"] = Lexer.realKeyword,
                ["then"] = Lexer.thenKeyword,
                ["else"] = Lexer.elseKeyword,
                ["case"] = Lexer.caseKeyword,
                ["file"] = Lexer.fileKeyword,
                ["goto"] = Lexer.gotoKeyword,
                ["type"] = Lexer.typeKeyword,
                ["with"] = Lexer.withKeyword,
                ["char"] = Lexer.charKeyword,
                ["auto"] = Lexer.autoKeyword,
                ["lock"] = Lexer.lockKeyword,
                ["loop"] = Lexer.loopKeyword,
                ["uses"] = Lexer.usesKeyword,
                ["byte"] = Lexer.byteKeyword,
                ["word"] = Lexer.wordKeyword
            };
            keywords[4] = tmp;
            tmp = new Dictionary<string, int>
            {
                ["begin"] = Lexer.beginKeyword,
                ["while"] = Lexer.whileKeyword,
                ["array"] = Lexer.arrayKeyword,
                ["const"] = Lexer.constKeyword,
                ["label"] = Lexer.labelKeyword,
                ["until"] = Lexer.untilKeyword,
                ["write"] = Lexer.writeKeyword,//Commands in here?
                ["class"] = Lexer.classKeyword,
                ["event"] = Lexer.eventKeyword,
                ["raise"] = Lexer.raiseKeyword,
                ["using"] = Lexer.usingKeyword,
                ["int64"] = Lexer.int64Keyword
            };
            keywords[5] = tmp;
            tmp = new Dictionary<string, int>
            {
                ["downto"] = Lexer.downtoKeyword,
                ["except"] = Lexer.exceptKeyword,
                ["packed"] = Lexer.packedKeyword,
                ["record"] = Lexer.recordKeyword,
                ["repeat"] = Lexer.repeatKeyword,
                ["sealed"] = Lexer.sealedKeyword,
                ["random"] = Lexer.randomKeyword, //?
                ["typeof"] = Lexer.typeofKeyword,
                ["string"] = Lexer.stringKeyword,
                ["double"] = Lexer.doubleKeyword,
                ["uint64"] = Lexer.uint64Keyword
            };
            keywords[6] = tmp;
            tmp = new Dictionary<string, int>
            {
                ["program"] = Lexer.programKeyword,
                ["integer"] = Lexer.integerKeyword,
                ["boolean"] = Lexer.booleanKeyword,
                ["finally"] = Lexer.finallyKeyword,
                ["foreach"] = Lexer.foreachKeyword,
                ["longint"] = Lexer.longintKeyword
            };
            keywords[7] = tmp;
            tmp = new Dictionary<string, int>
            {
                ["function"] = Lexer.functionKeyword,
                ["operator"] = Lexer.operatorKeyword,
                ["property"] = Lexer.propertyKeyword,
                ["sequence"] = Lexer.sequenceKeyword,
                ["template"] = Lexer.templateKeyword,
                ["shortint"] = Lexer.shortintKeyword,
                ["smallint"] = Lexer.smallintKeyword,
                ["longword"] = Lexer.longwordKeyword,
                ["cardinal"] = Lexer.cardinalKeyword
            };
            keywords[8] = tmp;
            tmp = new Dictionary<string, int>
            {
                ["procedure"] = Lexer.procedureKeyword,
                ["randomize"] = Lexer.randomizeKeyword,
                ["interface"] = Lexer.interfaceKeyword
            };
            keywords[9] = tmp;
            tmp.Clear();
        }     
    }
}
