using System;

//instrukcje najwyższego poziomu - top-level statements
//pozwalają na pisanie kodu bez konieczności definiowania klasy i metody Main
//wszystko w pliku z instrukcjami najwyższego poziomu jest traktowane jako ciało metody Main
//tylko jeden plik w projekcie może zawierać instrukcje najwyższego poziomu
Console.WriteLine("Hello, World!");
DoSth();
Nullable();

    //funckja w funkcji Main - nie może mieć modyfikatora dostępu
    static void DoSth()
{
    System.Console.WriteLine("Doing something...");
    
}

static void Nullable()
{
    int? a = null;
    //int b = null;

    string c = "a";
    string? d = null;

    ToUpper(c);
    ToUpper(d);

    string ToUpper(string s)
    {
        /*if(s is null)
            return string.Empty;*/
        return s.ToUpper();
    }

}