using HERBS_PRODUKCJA;

public class Globals
{
    private Globals _Instance;
    public Globals Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new Globals();
                _Instance.KodFirmy = "";
            }
            return _Instance;
        }
    }

    private Globals()
    {
    }

    public string KodFirmy { get; set; }
    public UZYTKOWNICY User { get; set; }
    public bool UserLoged { get; set; }

    
}