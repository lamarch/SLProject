

namespace SLProject.SLCompilerLib.Parser.Nodes{
    class ImportNode{
        public enum ImportType{
            Undefined,
            Module,
            Function
        }

        public ImportType importType;
        public string[] accessors;

        public ImportNode(ImportType type, params string[] accessors){
            this.importType = type;
            this.accessors = accessors;
        }
    }
}