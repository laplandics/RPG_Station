public interface IEnemyModSetter
{
    public string ModKey { get; set; }
    public IEnemyMod SetMod(Enemy owner);
}