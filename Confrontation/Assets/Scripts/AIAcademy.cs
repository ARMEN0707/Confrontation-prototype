public static class AIAcademy
{
    public static float BaseSpeed = 1f * GetCoeff();
    public static float BaseForce = 1f * GetCoeff();
    public static float BaseMilitaryReproduction = 4f * GetCoeff();
    public static float BaseArmyReproduction = 2f * GetCoeff();

    private static float GetCoeff() => (10 + LevelManager.CurrentLevel - 1) / 10f;
}