namespace ServerOver;

internal static class ProgramArgs
{
	public static string? DbGoBackTo { get; private set; }
	
	public static void From(IEnumerable<string> args)
    {
        using var a = args.GetEnumerator();
        while (a.MoveNext())
        {
            switch (a.Current)
            {
                case string ii when string.Equals(ii, "--goback-to-ob", StringComparison.OrdinalIgnoreCase):
                case string ii1 when string.Equals(ii1, "--go-back-to-ob", StringComparison.OrdinalIgnoreCase):
                case string ii2 when string.Equals(ii2, "--back-to-ob", StringComparison.OrdinalIgnoreCase):
                    {
                        a.MoveNext();
                        DbGoBackTo = "CreateViews";
                    }
                    break;
            }
        }
    }
}