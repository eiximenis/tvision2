namespace Tvision2.Layouts
{
    public struct LayoutSize
    {
        public int Variable { get; private set; }
        public int Fixed { get; private set; }

        public int Dock { get; private set; }

        public static LayoutSize FixedSize(int size) => new LayoutSize() { Fixed = size };
        public static LayoutSize VariableSize(int variable) => new LayoutSize() { Variable = variable };

        public static LayoutSize FromString(string rep)
        {
            rep = rep.Trim();
            if (rep[rep.Length - 1] == '*')
            {
                return rep.Length > 1 ?
                    VariableSize(int.Parse(rep.Substring(0, rep.Length - 1))) :
                    VariableSize(1);
            }
            else
            {
                return FixedSize(int.Parse(rep));
            }
        }
    }
}
