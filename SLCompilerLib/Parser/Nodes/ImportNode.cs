

namespace SLProject.SLCompilerLib.Parser.Nodes{
    class ImportNode{
        public enum ImportType{
            Undefined,
            Module,
            Function
        }

        public ImportType importType;
        public Accessor accessor;

        public ImportNode(ImportType type, Accessor accessor){
            this.importType = type;
            this.accessor = accessor;
        }
    }
}